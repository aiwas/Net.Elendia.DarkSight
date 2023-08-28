using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Net.Elendia.DarkSight;

public partial class MainForm : Form {

    /// <summary>
    /// 1ゲームの最大人数
    /// </summary>
    private const int NUM_ALL = 18;

    [GeneratedRegex("(?<=[^A-Za-z0-9]+)\\s*(?=[^A-Za-z0-9]+)")]
    private static partial Regex _regexSpace();

    [GeneratedRegex("(?<Killer>[A-Za-z0-9]+)\\s*殺された\\s*(?<Killed>[A-Za-z0-9]+)")]
    private static partial Regex _regexNames();

    private readonly Processor _processor;

    /// <summary>
    /// 死亡ログ
    /// </summary>
    private readonly List<DeathLog> _logs = new();

    /// <summary>
    /// プレイヤー一覧
    /// </summary>
    private readonly List<Player> _players = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainForm() {
        InitializeComponent();

        _processor = GetProcessor();
    }

    /// <summary>
    /// 処理クラス取得
    /// </summary>
    /// <returns></returns>
    private static Processor GetProcessor() {
        // 設定ファイルを取得
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("appsettings.json");
        IConfiguration config = configurationBuilder.Build();

        // キャプチャ領域の位置とサイズを取得
        int getValue(string key) => int.Parse(config[$"CaptureSetting:{key}"] ?? "");
        int left = getValue("Left");
        int top = getValue("Top");
        int width = getValue("Width");
        int height = getValue("Height");

        return new Processor(new Point(left, top), width, height);
    }

    private async void TimerExecOcr_Tick(object sender, EventArgs e) {
        // OCR結果を取得
        var result = await _processor.Execute();

        StringBuilder sb = new();
        sb.Append($"[{DateTime.Now:G}]");

        for (int i = 0; i < result.Lines.Count; i++) {
            string text = _regexSpace().Replace(result.Lines[i].Text, string.Empty);

            var matchResult = _regexNames().Match(text);
            if (matchResult.Success == false) {
                // プレイヤーの死亡ログ以外は無視
                continue;
            }

            // プレイヤーの死亡ログの場合
            string killer = matchResult.Groups["Killer"].Value;
            string killed = matchResult.Groups["Killed"].Value;
            if (_logs.Any(x => x.Killed == killed) == false) {
                // 新たに死んだ人なら追加
                var log = new DeathLog(killer, killed);
                _logs.Add(log);
                if (_players.Any(p => p.Name == log.Killer) == false) {
                    _players.Add(new Player(log.Killer, true));
                }
                if (_players.Any(p => p.Name == log.Killed) == false) {
                    _players.Add(new Player(log.Killed, false));
                }
                sb.AppendLine($"'{text}' matched : '{log.Killed} was killed by {log.Killer}'");
            }
        }

        // 人数カウント更新
        UpdateLabelCount();
        
        // プレイヤー一覧更新
        UpdateTextboxPlayers();
        
        // ログ更新
        textBoxLog.AppendText(sb.ToString());
    }

    /// <summary>
    /// 一時停止ボタン クリックイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonSwitch_Click(object sender, EventArgs e) {
        if (timerExecOcr.Enabled) {
            timerExecOcr.Enabled = false;
            buttonSwitch.Text = "Resume";
        } else {
            timerExecOcr.Enabled = true;
            buttonSwitch.Text = "Pause";
        }
    }

    /// <summary>
    /// リセットボタン クリックイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonReset_Click(object sender, EventArgs e) {
        _logs.Clear();
        _players.Clear();
        textBoxLog.Text = string.Empty;
        textBoxPlayers.Text = string.Empty;
    }

    /// <summary>
    /// 人数カウントラベルの更新
    /// </summary>
    private void UpdateLabelCount() {
        int numAlive = NUM_ALL - _logs.Count;
        labelCount.Text = $"{numAlive} / {NUM_ALL} 人";

        if (numAlive <= 6) {
            labelCount.ForeColor = Color.Red;
        }
    }

    /// <summary>
    /// プレイヤー一覧の更新
    /// </summary>
    private void UpdateTextboxPlayers() {
        StringBuilder sb = new();
        int count = 0;
        foreach (var player in _players) {
            sb.Append($"[{player.Name}] ");
            count++;
            if (count % 6 == 0) {
                sb.AppendLine();
            }
        }
        textBoxPlayers.Text = sb.ToString();
    }
}

/// <summary>
/// 死亡ログ
/// </summary>
/// <param name="Killer">殺した人</param>
/// <param name="Killed">殺された人</param>
public record DeathLog(string Killer, string Killed);

/// <summary>
/// プレイヤー
/// </summary>
/// <param name="Name">名前</param>
/// <param name="IsAlive">生存ｓ</param>
public record Player(string Name, bool IsAlive);