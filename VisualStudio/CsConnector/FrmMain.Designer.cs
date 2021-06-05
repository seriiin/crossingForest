namespace CsConnector
{
    partial class FrmMain
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
            this.LstStatus = new System.Windows.Forms.ListBox();
            this.BtSend = new System.Windows.Forms.Button();
            this.TbSend = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LstStatus
            // 
            this.LstStatus.FormattingEnabled = true;
            this.LstStatus.ItemHeight = 12;
            this.LstStatus.Location = new System.Drawing.Point(12, 12);
            this.LstStatus.Name = "LstStatus";
            this.LstStatus.Size = new System.Drawing.Size(468, 316);
            this.LstStatus.TabIndex = 1;
            // 
            // BtSend
            // 
            this.BtSend.Location = new System.Drawing.Point(405, 334);
            this.BtSend.Name = "BtSend";
            this.BtSend.Size = new System.Drawing.Size(75, 23);
            this.BtSend.TabIndex = 2;
            this.BtSend.Text = "Send";
            this.BtSend.UseVisualStyleBackColor = true;
            this.BtSend.Click += new System.EventHandler(this.BtSend_Click);
            // 
            // TbSend
            // 
            this.TbSend.Location = new System.Drawing.Point(12, 334);
            this.TbSend.Name = "TbSend";
            this.TbSend.Size = new System.Drawing.Size(387, 21);
            this.TbSend.TabIndex = 3;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 369);
            this.Controls.Add(this.TbSend);
            this.Controls.Add(this.BtSend);
            this.Controls.Add(this.LstStatus);
            this.Name = "FrmMain";
            this.Text = "Admin_notice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox LstStatus;
        private System.Windows.Forms.Button BtSend;
        private System.Windows.Forms.TextBox TbSend;
    }
}