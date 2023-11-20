namespace BombazoForm {
    partial class Form1 {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            toolStrip1 = new ToolStrip();
            gameIndicator = new ToolStripLabel();
            timeLabel = new ToolStripLabel();
            killedEnemiesLabel = new ToolStripLabel();
            gameStatusStripLabel = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            toolStripLabel2 = new ToolStripLabel();
            menuStrip2 = new MenuStrip();
            gameToolStripMenuItem = new ToolStripMenuItem();
            openMapToolStrip = new ToolStripMenuItem();
            pauseToolStripMenuItem = new ToolStripMenuItem();
            kilépésToolStripMenuItem = new ToolStripMenuItem();
            sugóToolStripMenuItem = new ToolStripMenuItem();
            színekJelentéseToolStripMenuItem = new ToolStripMenuItem();
            pályaKészítéseToolStripMenuItem = new ToolStripMenuItem();
            gameTableFlowLayout = new FlowLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            richTextBox1 = new RichTextBox();
            toolStrip1.SuspendLayout();
            menuStrip2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.Bottom;
            toolStrip1.ImageScalingSize = new Size(32, 32);
            toolStrip1.Items.AddRange(new ToolStripItem[] { gameIndicator, timeLabel, killedEnemiesLabel, gameStatusStripLabel, toolStripSeparator1, toolStripLabel1, toolStripLabel2 });
            toolStrip1.Location = new Point(0, 791);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1688, 38);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // gameIndicator
            // 
            gameIndicator.Name = "gameIndicator";
            gameIndicator.Size = new Size(0, 32);
            // 
            // timeLabel
            // 
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(0, 32);
            // 
            // killedEnemiesLabel
            // 
            killedEnemiesLabel.Name = "killedEnemiesLabel";
            killedEnemiesLabel.Size = new Size(0, 32);
            // 
            // gameStatusStripLabel
            // 
            gameStatusStripLabel.Name = "gameStatusStripLabel";
            gameStatusStripLabel.Size = new Size(267, 32);
            gameStatusStripLabel.Text = "Importálj be egy pályát!";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 38);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(175, 32);
            toolStripLabel1.Text = "Mozgás: Nyilak";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(164, 32);
            toolStripLabel2.Text = "Bomba: Space";
            // 
            // menuStrip2
            // 
            menuStrip2.ImageScalingSize = new Size(32, 32);
            menuStrip2.Items.AddRange(new ToolStripItem[] { gameToolStripMenuItem, sugóToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(1688, 42);
            menuStrip2.TabIndex = 5;
            menuStrip2.Text = "menuStrip2";
            // 
            // gameToolStripMenuItem
            // 
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openMapToolStrip, pauseToolStripMenuItem, kilépésToolStripMenuItem });
            gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            gameToolStripMenuItem.Size = new Size(88, 38);
            gameToolStripMenuItem.Text = "Játék";
            // 
            // openMapToolStrip
            // 
            openMapToolStrip.Name = "openMapToolStrip";
            openMapToolStrip.Size = new Size(359, 44);
            openMapToolStrip.Text = "Pálya megnyitása";
            openMapToolStrip.Click += openMapToolStripMenuItem_Click;
            // 
            // pauseToolStripMenuItem
            // 
            pauseToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            pauseToolStripMenuItem.Size = new Size(359, 44);
            pauseToolStripMenuItem.Text = "Megállítás";
            pauseToolStripMenuItem.Click += pauseToolStripMenuItem_Click;
            // 
            // kilépésToolStripMenuItem
            // 
            kilépésToolStripMenuItem.Name = "kilépésToolStripMenuItem";
            kilépésToolStripMenuItem.Size = new Size(359, 44);
            kilépésToolStripMenuItem.Text = "Kilépés";
            kilépésToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // sugóToolStripMenuItem
            // 
            sugóToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { színekJelentéseToolStripMenuItem, pályaKészítéseToolStripMenuItem });
            sugóToolStripMenuItem.Name = "sugóToolStripMenuItem";
            sugóToolStripMenuItem.Size = new Size(89, 38);
            sugóToolStripMenuItem.Text = "Sugó";
            // 
            // színekJelentéseToolStripMenuItem
            // 
            színekJelentéseToolStripMenuItem.Name = "színekJelentéseToolStripMenuItem";
            színekJelentéseToolStripMenuItem.Size = new Size(318, 44);
            színekJelentéseToolStripMenuItem.Text = "Színek jelentése";
            színekJelentéseToolStripMenuItem.Click += színekJelentéseToolStripMenuItem_Click;
            // 
            // pályaKészítéseToolStripMenuItem
            // 
            pályaKészítéseToolStripMenuItem.Name = "pályaKészítéseToolStripMenuItem";
            pályaKészítéseToolStripMenuItem.Size = new Size(318, 44);
            pályaKészítéseToolStripMenuItem.Text = "Pálya készítése";
            pályaKészítéseToolStripMenuItem.Click += pályaKészítéseToolStripMenuItem_Click;
            // 
            // gameTableFlowLayout
            // 
            gameTableFlowLayout.Enabled = false;
            gameTableFlowLayout.Location = new Point(52, 96);
            gameTableFlowLayout.Name = "gameTableFlowLayout";
            gameTableFlowLayout.Size = new Size(1038, 635);
            gameTableFlowLayout.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(richTextBox1);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(1274, 42);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(414, 749);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.None;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Enabled = false;
            richTextBox1.Font = new Font("Segoe UI", 10.1F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox1.Location = new Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(397, 748);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1688, 829);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(gameTableFlowLayout);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip2);
            Name = "Form1";
            KeyDown += PlayerInteraction;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem openMapToolStrip;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private ToolStripMenuItem kilépésToolStripMenuItem;
        private ToolStripLabel gameIndicator;
        private ToolStripLabel timeLabel;
        private ToolStripLabel killedEnemiesLabel;
        private FlowLayoutPanel gameTableFlowLayout;
        private ToolStripLabel gameStatusStripLabel;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private RichTextBox richTextBox1;
        private ToolStripMenuItem sugóToolStripMenuItem;
        private ToolStripMenuItem színekJelentéseToolStripMenuItem;
        private ToolStripMenuItem pályaKészítéseToolStripMenuItem;
    }
}