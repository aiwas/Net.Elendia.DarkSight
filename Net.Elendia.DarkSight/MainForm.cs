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
    /// 1�Q�[���̍ő�l��
    /// </summary>
    private const int NUM_ALL = 18;

    [GeneratedRegex("(?<=[^A-Za-z0-9]+)\\s*(?=[^A-Za-z0-9]+)")]
    private static partial Regex _regexSpace();

    [GeneratedRegex("(?<Killer>[A-Za-z0-9]+)\\s*�E���ꂽ\\s*(?<Killed>[A-Za-z0-9]+)")]
    private static partial Regex _regexNames();

    private readonly Processor _processor;

    /// <summary>
    /// ���S���O
    /// </summary>
    private readonly List<DeathLog> _logs = new();

    /// <summary>
    /// �v���C���[�ꗗ
    /// </summary>
    private readonly List<Player> _players = new();

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public MainForm() {
        InitializeComponent();

        _processor = GetProcessor();
    }

    /// <summary>
    /// �����N���X�擾
    /// </summary>
    /// <returns></returns>
    private static Processor GetProcessor() {
        // �ݒ�t�@�C�����擾
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("appsettings.json");
        IConfiguration config = configurationBuilder.Build();

        // �L���v�`���̈�̈ʒu�ƃT�C�Y���擾
        int getValue(string key) => int.Parse(config[$"CaptureSetting:{key}"] ?? "");
        int left = getValue("Left");
        int top = getValue("Top");
        int width = getValue("Width");
        int height = getValue("Height");

        return new Processor(new Point(left, top), width, height);
    }

    private async void TimerExecOcr_Tick(object sender, EventArgs e) {
        // OCR���ʂ��擾
        var result = await _processor.Execute();

        StringBuilder sb = new();
        sb.Append($"[{DateTime.Now:G}]");

        for (int i = 0; i < result.Lines.Count; i++) {
            string text = _regexSpace().Replace(result.Lines[i].Text, string.Empty);

            var matchResult = _regexNames().Match(text);
            if (matchResult.Success == false) {
                // �v���C���[�̎��S���O�ȊO�͖���
                continue;
            }

            // �v���C���[�̎��S���O�̏ꍇ
            string killer = matchResult.Groups["Killer"].Value;
            string killed = matchResult.Groups["Killed"].Value;
            if (_logs.Any(x => x.Killed == killed) == false) {
                // �V���Ɏ��񂾐l�Ȃ�ǉ�
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

        // �l���J�E���g�X�V
        UpdateLabelCount();
        
        // �v���C���[�ꗗ�X�V
        UpdateTextboxPlayers();
        
        // ���O�X�V
        textBoxLog.AppendText(sb.ToString());
    }

    /// <summary>
    /// �ꎞ��~�{�^�� �N���b�N�C�x���g
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
    /// ���Z�b�g�{�^�� �N���b�N�C�x���g
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
    /// �l���J�E���g���x���̍X�V
    /// </summary>
    private void UpdateLabelCount() {
        int numAlive = NUM_ALL - _logs.Count;
        labelCount.Text = $"{numAlive} / {NUM_ALL} �l";

        if (numAlive <= 6) {
            labelCount.ForeColor = Color.Red;
        }
    }

    /// <summary>
    /// �v���C���[�ꗗ�̍X�V
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
/// ���S���O
/// </summary>
/// <param name="Killer">�E�����l</param>
/// <param name="Killed">�E���ꂽ�l</param>
public record DeathLog(string Killer, string Killed);

/// <summary>
/// �v���C���[
/// </summary>
/// <param name="Name">���O</param>
/// <param name="IsAlive">������</param>
public record Player(string Name, bool IsAlive);