namespace ts
{
    partial class frRubik
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbStep = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbAutoDecrement = new System.Windows.Forms.CheckBox();
            this.cbForceDoubleClick = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTimeLimit = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDuration = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.tbStep)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(43, 206);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(104, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(35, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Steps :";
            // 
            // tbStep
            // 
            this.tbStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tbStep.Location = new System.Drawing.Point(90, 40);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new System.Drawing.Size(34, 23);
            this.tbStep.TabIndex = 6;
            this.tbStep.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbAutoDecrement);
            this.groupBox1.Controls.Add(this.cbForceDoubleClick);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbTimeLimit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbDuration);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbStep);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 179);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Panel";
            // 
            // cbAutoDecrement
            // 
            this.cbAutoDecrement.AutoSize = true;
            this.cbAutoDecrement.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAutoDecrement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cbAutoDecrement.Location = new System.Drawing.Point(36, 150);
            this.cbAutoDecrement.Name = "cbAutoDecrement";
            this.cbAutoDecrement.Size = new System.Drawing.Size(109, 17);
            this.cbAutoDecrement.TabIndex = 23;
            this.cbAutoDecrement.Text = "Auto Decrement :";
            this.cbAutoDecrement.UseVisualStyleBackColor = true;
            // 
            // cbForceDoubleClick
            // 
            this.cbForceDoubleClick.AutoSize = true;
            this.cbForceDoubleClick.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbForceDoubleClick.Checked = true;
            this.cbForceDoubleClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbForceDoubleClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cbForceDoubleClick.Location = new System.Drawing.Point(1, 127);
            this.cbForceDoubleClick.Name = "cbForceDoubleClick";
            this.cbForceDoubleClick.Size = new System.Drawing.Size(176, 17);
            this.cbForceDoubleClick.TabIndex = 22;
            this.cbForceDoubleClick.Text = "Force double click in Last step :";
            this.cbForceDoubleClick.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(140, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "sec";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(140, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "sec";
            // 
            // tbTimeLimit
            // 
            this.tbTimeLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tbTimeLimit.Location = new System.Drawing.Point(90, 91);
            this.tbTimeLimit.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.tbTimeLimit.Name = "tbTimeLimit";
            this.tbTimeLimit.Size = new System.Drawing.Size(51, 23);
            this.tbTimeLimit.TabIndex = 10;
            this.tbTimeLimit.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(2, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Time Limits :";
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
            this.tbDuration.Location = new System.Drawing.Point(90, 65);
            this.tbDuration.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.Size = new System.Drawing.Size(51, 23);
            this.tbDuration.TabIndex = 8;
            this.tbDuration.Value = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(3, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Click Delay :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(16, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 26);
            this.label1.TabIndex = 19;
            this.label1.Text = "Use \"Right Click\" or \"Spacebar\"\r\nto stop the Process !!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(6, 247);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(234, 60);
            this.webBrowser1.TabIndex = 26;
            this.webBrowser1.TabStop = false;
            this.webBrowser1.Url = new System.Uri("http://216.237.113.122/mootoomeepo/ads.php", System.UriKind.Absolute);
            this.webBrowser1.Visible = false;
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frRubik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 317);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.Name = "frRubik";
            this.Text = "Rubik";
            this.Load += new System.EventHandler(this.frRubik_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frRubik_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.tbStep)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.NumericUpDown tbStep;  
        private System.Windows.Forms.NumericUpDown tbDuration;
        private System.Windows.Forms.NumericUpDown tbTimeLimit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbForceDoubleClick;
        private System.Windows.Forms.CheckBox cbAutoDecrement;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}