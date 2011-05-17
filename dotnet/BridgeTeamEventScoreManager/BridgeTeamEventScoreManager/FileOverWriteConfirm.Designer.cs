namespace BridgeTeamEventScoreManager
{
    partial class FileOverWriteConfirm
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
            this.overwriteCancelButton = new System.Windows.Forms.Button();
            this.overwriteMessageTextBox = new System.Windows.Forms.TextBox();
            this.overwriteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // overwriteCancelButton
            // 
            this.overwriteCancelButton.Location = new System.Drawing.Point(149, 168);
            this.overwriteCancelButton.Name = "overwriteCancelButton";
            this.overwriteCancelButton.Size = new System.Drawing.Size(127, 32);
            this.overwriteCancelButton.TabIndex = 0;
            this.overwriteCancelButton.Text = "Cancel";
            this.overwriteCancelButton.UseVisualStyleBackColor = true;
            this.overwriteCancelButton.Click += new System.EventHandler(this.overwriteCancelButton_Click);
            // 
            // overwriteMessageTextBox
            // 
            this.overwriteMessageTextBox.Location = new System.Drawing.Point(11, 18);
            this.overwriteMessageTextBox.Multiline = true;
            this.overwriteMessageTextBox.Name = "overwriteMessageTextBox";
            this.overwriteMessageTextBox.ReadOnly = true;
            this.overwriteMessageTextBox.Size = new System.Drawing.Size(265, 124);
            this.overwriteMessageTextBox.TabIndex = 1;
            // 
            // overwriteButton
            // 
            this.overwriteButton.Location = new System.Drawing.Point(11, 168);
            this.overwriteButton.Name = "overwriteButton";
            this.overwriteButton.Size = new System.Drawing.Size(132, 32);
            this.overwriteButton.TabIndex = 2;
            this.overwriteButton.Text = "OverWrite";
            this.overwriteButton.UseVisualStyleBackColor = true;
            this.overwriteButton.Click += new System.EventHandler(this.overwriteButton_Click);
            // 
            // FileOverWriteConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 212);
            this.Controls.Add(this.overwriteButton);
            this.Controls.Add(this.overwriteMessageTextBox);
            this.Controls.Add(this.overwriteCancelButton);
            this.Name = "FileOverWriteConfirm";
            this.Text = "OverWrite File?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button overwriteCancelButton;
        private System.Windows.Forms.TextBox overwriteMessageTextBox;
        private System.Windows.Forms.Button overwriteButton;
    }
}