namespace BFIMasterpointManagement
{
    partial class UploadProgress
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
            this.uploadProgressBar = new System.Windows.Forms.ProgressBar();
            this.statusMessageTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // uploadProgressBar
            // 
            this.uploadProgressBar.Location = new System.Drawing.Point(12, 12);
            this.uploadProgressBar.Name = "uploadProgressBar";
            this.uploadProgressBar.Size = new System.Drawing.Size(490, 23);
            this.uploadProgressBar.TabIndex = 0;
            // 
            // statusMessageTextbox
            // 
            this.statusMessageTextbox.Location = new System.Drawing.Point(12, 41);
            this.statusMessageTextbox.Multiline = true;
            this.statusMessageTextbox.Name = "statusMessageTextbox";
            this.statusMessageTextbox.ReadOnly = true;
            this.statusMessageTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.statusMessageTextbox.Size = new System.Drawing.Size(490, 334);
            this.statusMessageTextbox.TabIndex = 1;
            // 
            // UploadProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 387);
            this.ControlBox = false;
            this.Controls.Add(this.statusMessageTextbox);
            this.Controls.Add(this.uploadProgressBar);
            this.Name = "UploadProgress";
            this.Text = "UploadProgress";
            this.Load += new System.EventHandler(this.UploadProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar uploadProgressBar;
        private System.Windows.Forms.TextBox statusMessageTextbox;
    }
}