namespace RotationPuzzleFrame
{
    partial class frRotatePuzzle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.rb33 = new System.Windows.Forms.RadioButton();
            this.rb44 = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tbScore = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbForceEmptySlot = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDuration = new System.Windows.Forms.NumericUpDown();
            this.cbForceDoubleClick = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(30, 105);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // rb33
            // 
            this.rb33.AutoSize = true;
            this.rb33.Checked = true;
            this.rb33.Location = new System.Drawing.Point(50, 12);
            this.rb33.Name = "rb33";
            this.rb33.Size = new System.Drawing.Size(41, 17);
            this.rb33.TabIndex = 1;
            this.rb33.TabStop = true;
            this.rb33.Text = "3*3";
            this.rb33.UseVisualStyleBackColor = true;
            this.rb33.CheckedChanged += new System.EventHandler(this.rb33_CheckedChanged);
            // 
            // rb44
            // 
            this.rb44.AutoSize = true;
            this.rb44.Location = new System.Drawing.Point(109, 12);
            this.rb44.Name = "rb44";
            this.rb44.Size = new System.Drawing.Size(41, 17);
            this.rb44.TabIndex = 2;
            this.rb44.Text = "4*4";
            this.rb44.UseVisualStyleBackColor = true;
            this.rb44.Click += new System.EventHandler(this.rb33_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(263, 58);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(250, 250);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Location = new System.Drawing.Point(532, 58);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(250, 250);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Perfect Image :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(529, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Puzzle Image :";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(423, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Show Images";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tbScore
            // 
            this.tbScore.Location = new System.Drawing.Point(363, 13);
            this.tbScore.Mask = "0.00";
            this.tbScore.Name = "tbScore";
            this.tbScore.Size = new System.Drawing.Size(54, 20);
            this.tbScore.TabIndex = 11;
            this.tbScore.Text = "085";
            this.tbScore.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Empty Position Score :";
            this.label3.Visible = false;
            // 
            // cbForceEmptySlot
            // 
            this.cbForceEmptySlot.Checked = true;
            this.cbForceEmptySlot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbForceEmptySlot.Location = new System.Drawing.Point(19, 361);
            this.cbForceEmptySlot.Name = "cbForceEmptySlot";
            this.cbForceEmptySlot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbForceEmptySlot.Size = new System.Drawing.Size(173, 23);
            this.cbForceEmptySlot.TabIndex = 17;
            this.cbForceEmptySlot.Text = ": กำหนดช่องว่างเอง";
            this.cbForceEmptySlot.UseVisualStyleBackColor = true;
            this.cbForceEmptySlot.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(135, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 26);
            this.label6.TabIndex = 18;
            this.label6.Text = "Use \"Spacebar\"\r\nto stop the Process !!";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(158, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "sec";
            // 
            // tbDuration
            // 
            this.tbDuration.DecimalPlaces = 2;
            this.tbDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tbDuration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.tbDuration.Location = new System.Drawing.Point(109, 34);
            this.tbDuration.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.Size = new System.Drawing.Size(50, 23);
            this.tbDuration.TabIndex = 19;
            this.tbDuration.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // cbForceDoubleClick
            // 
            this.cbForceDoubleClick.AutoSize = true;
            this.cbForceDoubleClick.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbForceDoubleClick.Checked = true;
            this.cbForceDoubleClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbForceDoubleClick.Location = new System.Drawing.Point(12, 68);
            this.cbForceDoubleClick.Name = "cbForceDoubleClick";
            this.cbForceDoubleClick.Size = new System.Drawing.Size(176, 17);
            this.cbForceDoubleClick.TabIndex = 21;
            this.cbForceDoubleClick.Text = "Force double click in Last step :";
            this.cbForceDoubleClick.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(20, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "Click Delay :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "label4";
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(9, 215);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(234, 60);
            this.webBrowser1.TabIndex = 25;
            this.webBrowser1.TabStop = false;
            this.webBrowser1.Url = new System.Uri("http://216.237.113.122/mootoomeepo/ads.php", System.UriKind.Absolute);
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frRotatePuzzle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 282);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbForceDoubleClick);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDuration);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbForceEmptySlot);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbScore);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.rb44);
            this.Controls.Add(this.rb33);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frRotatePuzzle";
            this.Text = "Main Page";
            this.Load += new System.EventHandler(this.frRotatePuzzle_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frJigsaw_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RadioButton rb33;
        private System.Windows.Forms.RadioButton rb44;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MaskedTextBox tbScore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbForceEmptySlot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown tbDuration;
        private System.Windows.Forms.CheckBox cbForceDoubleClick;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

