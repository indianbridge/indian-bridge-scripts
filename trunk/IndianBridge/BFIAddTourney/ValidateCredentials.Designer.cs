namespace BFIAddTourney
{
    partial class ValidateCredentials
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
			this.passwordTextbox = new System.Windows.Forms.TextBox();
			this.usernameTextbox = new System.Windows.Forms.TextBox();
			this.loadingPicture = new System.Windows.Forms.PictureBox();
			this.loginPanel = new System.Windows.Forms.Panel();
			this.okButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).BeginInit();
			this.loginPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// passwordTextbox
			// 
			this.passwordTextbox.Location = new System.Drawing.Point(77, 30);
			this.passwordTextbox.Name = "passwordTextbox";
			this.passwordTextbox.PasswordChar = '*';
			this.passwordTextbox.Size = new System.Drawing.Size(251, 20);
			this.passwordTextbox.TabIndex = 9;
			// 
			// usernameTextbox
			// 
			this.usernameTextbox.Location = new System.Drawing.Point(77, 4);
			this.usernameTextbox.Name = "usernameTextbox";
			this.usernameTextbox.Size = new System.Drawing.Size(251, 20);
			this.usernameTextbox.TabIndex = 8;
			// 
			// loadingPicture
			// 
			this.loadingPicture.Location = new System.Drawing.Point(131, 12);
			this.loadingPicture.Name = "loadingPicture";
			this.loadingPicture.Size = new System.Drawing.Size(67, 51);
			this.loadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.loadingPicture.TabIndex = 7;
			this.loadingPicture.TabStop = false;
			this.loadingPicture.Visible = false;
			// 
			// loginPanel
			// 
			this.loginPanel.Controls.Add(this.passwordTextbox);
			this.loginPanel.Controls.Add(this.usernameTextbox);
			this.loginPanel.Controls.Add(this.loadingPicture);
			this.loginPanel.Controls.Add(this.okButton);
			this.loginPanel.Controls.Add(this.label1);
			this.loginPanel.Controls.Add(this.cancelButton);
			this.loginPanel.Controls.Add(this.label2);
			this.loginPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.loginPanel.Location = new System.Drawing.Point(0, 0);
			this.loginPanel.Name = "loginPanel";
			this.loginPanel.Size = new System.Drawing.Size(339, 96);
			this.loginPanel.TabIndex = 7;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(172, 61);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Username : ";
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(253, 61);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Password : ";
			// 
			// ValidateCredentials
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 96);
			this.Controls.Add(this.loginPanel);
			this.Name = "ValidateCredentials";
			this.Text = "ValidateCredentials";
			((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).EndInit();
			this.loginPanel.ResumeLayout(false);
			this.loginPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.PictureBox loadingPicture;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
    }
}