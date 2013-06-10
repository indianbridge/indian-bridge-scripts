namespace UploadFolderToWordpress
{
    partial class UploadFolderToWordpress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadFolderToWordpress));
            this.selectFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.username = new System.Windows.Forms.Label();
            this.wordpressPathTextbox = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.uploadButton = new System.Windows.Forms.Button();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.wordpressURLTextbox = new System.Windows.Forms.TextBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedFolderTextbox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.publishResultsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.publishResultsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.cancelPublishResultsButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectFolderDialog
            // 
            this.selectFolderDialog.ShowNewFolderButton = false;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Location = new System.Drawing.Point(12, 9);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(64, 13);
            this.username.TabIndex = 13;
            this.username.Text = "Username : ";
            // 
            // wordpressPathTextbox
            // 
            this.wordpressPathTextbox.Location = new System.Drawing.Point(83, 253);
            this.wordpressPathTextbox.Name = "wordpressPathTextbox";
            this.wordpressPathTextbox.Size = new System.Drawing.Size(336, 20);
            this.wordpressPathTextbox.TabIndex = 24;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Location = new System.Drawing.Point(12, 41);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(62, 13);
            this.password.TabIndex = 14;
            this.password.Text = "Password : ";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 27);
            this.label3.TabIndex = 23;
            this.label3.Text = "Wordpress Path : ";
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(83, 9);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(216, 20);
            this.usernameTextbox.TabIndex = 15;
            // 
            // uploadButton
            // 
            this.uploadButton.BackColor = System.Drawing.Color.Green;
            this.uploadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.uploadButton.Location = new System.Drawing.Point(15, 283);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(404, 68);
            this.uploadButton.TabIndex = 22;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = false;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(83, 38);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.PasswordChar = '*';
            this.passwordTextbox.Size = new System.Drawing.Size(216, 20);
            this.passwordTextbox.TabIndex = 16;
            // 
            // wordpressURLTextbox
            // 
            this.wordpressURLTextbox.Location = new System.Drawing.Point(83, 216);
            this.wordpressURLTextbox.Name = "wordpressURLTextbox";
            this.wordpressURLTextbox.Size = new System.Drawing.Size(336, 20);
            this.wordpressURLTextbox.TabIndex = 21;
            this.wordpressURLTextbox.Text = "http://127.0.0.1/bfitest";
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(15, 162);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(404, 37);
            this.selectFolderButton.TabIndex = 17;
            this.selectFolderButton.Text = "Select Folder To Upload";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click_1);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Wordpress URL : ";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 30);
            this.label1.TabIndex = 18;
            this.label1.Text = "Selected Folder : ";
            // 
            // selectedFolderTextbox
            // 
            this.selectedFolderTextbox.Location = new System.Drawing.Point(83, 64);
            this.selectedFolderTextbox.Multiline = true;
            this.selectedFolderTextbox.Name = "selectedFolderTextbox";
            this.selectedFolderTextbox.ReadOnly = true;
            this.selectedFolderTextbox.Size = new System.Drawing.Size(332, 92);
            this.selectedFolderTextbox.TabIndex = 19;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.publishResultsStatus,
            this.publishResultsProgressBar,
            this.cancelPublishResultsButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 359);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(425, 23);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // publishResultsStatus
            // 
            this.publishResultsStatus.Name = "publishResultsStatus";
            this.publishResultsStatus.Size = new System.Drawing.Size(0, 18);
            // 
            // publishResultsProgressBar
            // 
            this.publishResultsProgressBar.Name = "publishResultsProgressBar";
            this.publishResultsProgressBar.Size = new System.Drawing.Size(100, 17);
            // 
            // cancelPublishResultsButton
            // 
            this.cancelPublishResultsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cancelPublishResultsButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelPublishResultsButton.Image")));
            this.cancelPublishResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelPublishResultsButton.Name = "cancelPublishResultsButton";
            this.cancelPublishResultsButton.Size = new System.Drawing.Size(88, 21);
            this.cancelPublishResultsButton.Text = "Cancel Upload";
            // 
            // UploadFolderToWordpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 382);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.username);
            this.Controls.Add(this.wordpressPathTextbox);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.usernameTextbox);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.wordpressURLTextbox);
            this.Controls.Add(this.selectFolderButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectedFolderTextbox);
            this.Name = "UploadFolderToWordpress";
            this.Text = "Upload Folder to Wordpress";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog selectFolderDialog;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.TextBox wordpressPathTextbox;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.TextBox wordpressURLTextbox;
        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox selectedFolderTextbox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel publishResultsStatus;
        private System.Windows.Forms.ToolStripProgressBar publishResultsProgressBar;
        private System.Windows.Forms.ToolStripButton cancelPublishResultsButton;
    }
}

