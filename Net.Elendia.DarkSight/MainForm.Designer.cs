namespace Net.Elendia.DarkSight;

partial class MainForm {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        components = new System.ComponentModel.Container();
        textBoxLog = new System.Windows.Forms.TextBox();
        timerExecOcr = new System.Windows.Forms.Timer(components);
        buttonSwitch = new System.Windows.Forms.Button();
        labelCount = new System.Windows.Forms.Label();
        textBoxPlayers = new System.Windows.Forms.TextBox();
        buttonReset = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // textBoxLog
        // 
        textBoxLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        textBoxLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
        textBoxLog.Location = new System.Drawing.Point(12, 258);
        textBoxLog.Multiline = true;
        textBoxLog.Name = "textBoxLog";
        textBoxLog.ReadOnly = true;
        textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBoxLog.Size = new System.Drawing.Size(776, 180);
        textBoxLog.TabIndex = 0;
        // 
        // timerExecOcr
        // 
        timerExecOcr.Enabled = true;
        timerExecOcr.Interval = 5000;
        timerExecOcr.Tick += TimerExecOcr_Tick;
        // 
        // buttonSwitch
        // 
        buttonSwitch.BackColor = System.Drawing.SystemColors.Control;
        buttonSwitch.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        buttonSwitch.Location = new System.Drawing.Point(382, 9);
        buttonSwitch.Name = "buttonSwitch";
        buttonSwitch.Size = new System.Drawing.Size(200, 50);
        buttonSwitch.TabIndex = 1;
        buttonSwitch.Text = "停止する";
        buttonSwitch.UseVisualStyleBackColor = false;
        buttonSwitch.Click += ButtonSwitch_Click;
        // 
        // labelCount
        // 
        labelCount.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        labelCount.Location = new System.Drawing.Point(12, 9);
        labelCount.Name = "labelCount";
        labelCount.Size = new System.Drawing.Size(200, 50);
        labelCount.TabIndex = 2;
        labelCount.Text = "0 / 18 人";
        labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // textBoxPlayers
        // 
        textBoxPlayers.Location = new System.Drawing.Point(12, 65);
        textBoxPlayers.Multiline = true;
        textBoxPlayers.Name = "textBoxPlayers";
        textBoxPlayers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBoxPlayers.Size = new System.Drawing.Size(776, 187);
        textBoxPlayers.TabIndex = 3;
        // 
        // buttonReset
        // 
        buttonReset.BackColor = System.Drawing.SystemColors.Control;
        buttonReset.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        buttonReset.Location = new System.Drawing.Point(588, 9);
        buttonReset.Name = "buttonReset";
        buttonReset.Size = new System.Drawing.Size(200, 50);
        buttonReset.TabIndex = 4;
        buttonReset.Text = "リセット";
        buttonReset.UseVisualStyleBackColor = false;
        buttonReset.Click += ButtonReset_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(buttonReset);
        Controls.Add(textBoxPlayers);
        Controls.Add(labelCount);
        Controls.Add(buttonSwitch);
        Controls.Add(textBoxLog);
        Name = "MainForm";
        Text = "DarkSight";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TextBox textBoxLog;
    private System.Windows.Forms.Timer timerExecOcr;
    private System.Windows.Forms.Button buttonSwitch;
    private System.Windows.Forms.Label labelCount;
    private System.Windows.Forms.TextBox textBoxPlayers;
    private System.Windows.Forms.Button buttonReset;
}
