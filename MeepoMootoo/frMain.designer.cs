namespace ts
{
    partial class frMain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnJigsaw = new System.Windows.Forms.RadioButton();
            this.btnRubik = new System.Windows.Forms.RadioButton();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 115);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 353);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnJigsaw);
            this.panel2.Controls.Add(this.btnRubik);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(230, 97);
            this.panel2.TabIndex = 3;
            // 
            // btnJigsaw
            // 
            this.btnJigsaw.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnJigsaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnJigsaw.Location = new System.Drawing.Point(25, 10);
            this.btnJigsaw.Name = "btnJigsaw";
            this.btnJigsaw.Size = new System.Drawing.Size(85, 71);
            this.btnJigsaw.TabIndex = 4;
            this.btnJigsaw.Text = "Jigsaw";
            this.btnJigsaw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnJigsaw.UseVisualStyleBackColor = true;
            this.btnJigsaw.Click += new System.EventHandler(this.btnJigsaw_Click);
            // 
            // btnRubik
            // 
            this.btnRubik.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRubik.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnRubik.Location = new System.Drawing.Point(120, 10);
            this.btnRubik.Name = "btnRubik";
            this.btnRubik.Size = new System.Drawing.Size(85, 72);
            this.btnRubik.TabIndex = 3;
            this.btnRubik.Text = "Rubik";
            this.btnRubik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRubik.UseVisualStyleBackColor = true;
            this.btnRubik.Click += new System.EventHandler(this.btnRubik_Click);
            // 
            // frMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 480);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frMain";
            this.Text = "TS2 by Mootoo, the enemy Helper";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton btnJigsaw;
        private System.Windows.Forms.RadioButton btnRubik;

    }
}