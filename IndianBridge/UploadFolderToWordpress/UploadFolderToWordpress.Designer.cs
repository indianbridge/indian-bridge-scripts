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
            this.selectFileOrFolderDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectFileToUploadButton = new System.Windows.Forms.Button();
            this.useTourneyTemplateCheckbox = new System.Windows.Forms.CheckBox();
            this.csvContentTextbox = new System.Windows.Forms.TextBox();
            this.postMasterpointsButton = new System.Windows.Forms.Button();
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
            this.wordpressPathTextbox.Location = new System.Drawing.Point(83, 270);
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
            this.label3.Location = new System.Drawing.Point(12, 270);
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
            this.uploadButton.Location = new System.Drawing.Point(15, 300);
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
            this.wordpressURLTextbox.Location = new System.Drawing.Point(83, 233);
            this.wordpressURLTextbox.Name = "wordpressURLTextbox";
            this.wordpressURLTextbox.Size = new System.Drawing.Size(336, 20);
            this.wordpressURLTextbox.TabIndex = 21;
            this.wordpressURLTextbox.Text = "http://127.0.0.1/bfitest";
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(15, 162);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(173, 37);
            this.selectFolderButton.TabIndex = 17;
            this.selectFolderButton.Text = "Select Folder To Upload";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click_1);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Wordpress URL : ";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 58);
            this.label1.TabIndex = 18;
            this.label1.Text = "Selected File or Folder : ";
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 374);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(955, 23);
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
            // selectFileToUploadButton
            // 
            this.selectFileToUploadButton.Location = new System.Drawing.Point(242, 162);
            this.selectFileToUploadButton.Name = "selectFileToUploadButton";
            this.selectFileToUploadButton.Size = new System.Drawing.Size(173, 37);
            this.selectFileToUploadButton.TabIndex = 26;
            this.selectFileToUploadButton.Text = "Select File To Upload";
            this.selectFileToUploadButton.UseVisualStyleBackColor = true;
            this.selectFileToUploadButton.Click += new System.EventHandler(this.selectFileToUploadButton_Click);
            // 
            // useTourneyTemplateCheckbox
            // 
            this.useTourneyTemplateCheckbox.AutoSize = true;
            this.useTourneyTemplateCheckbox.Checked = true;
            this.useTourneyTemplateCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTourneyTemplateCheckbox.Location = new System.Drawing.Point(15, 209);
            this.useTourneyTemplateCheckbox.Name = "useTourneyTemplateCheckbox";
            this.useTourneyTemplateCheckbox.Size = new System.Drawing.Size(134, 17);
            this.useTourneyTemplateCheckbox.TabIndex = 27;
            this.useTourneyTemplateCheckbox.Text = "Use Tourney Template";
            this.useTourneyTemplateCheckbox.UseVisualStyleBackColor = true;
            // 
            // csvContentTextbox
            // 
            this.csvContentTextbox.Location = new System.Drawing.Point(425, 12);
            this.csvContentTextbox.Multiline = true;
            this.csvContentTextbox.Name = "csvContentTextbox";
            this.csvContentTextbox.Size = new System.Drawing.Size(508, 144);
            this.csvContentTextbox.TabIndex = 28;
            // 
            // postMasterpointsButton
            // 
            this.postMasterpointsButton.BackColor = System.Drawing.Color.Green;
            this.postMasterpointsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postMasterpointsButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.postMasterpointsButton.Location = new System.Drawing.Point(425, 162);
            this.postMasterpointsButton.Name = "postMasterpointsButton";
            this.postMasterpointsButton.Size = new System.Drawing.Size(404, 68);
            this.postMasterpointsButton.TabIndex = 29;
            this.postMasterpointsButton.Text = "Post Masterpoints";
            this.postMasterpointsButton.UseVisualStyleBackColor = false;
            this.postMasterpointsButton.Click += new System.EventHandler(this.postMasterpointsButton_Click);
            // 
            // UploadFolderToWordpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 397);
            this.Controls.Add(this.postMasterpointsButton);
            this.Controls.Add(this.csvContentTextbox);
            this.Controls.Add(this.useTourneyTemplateCheckbox);
            this.Controls.Add(this.selectFileToUploadButton);
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
        private System.Windows.Forms.OpenFileDialog selectFileOrFolderDialog;
        private System.Windows.Forms.Button selectFileToUploadButton;
        private System.Windows.Forms.CheckBox useTourneyTemplateCheckbox;
        private System.Windows.Forms.TextBox csvContentTextbox;
        private System.Windows.Forms.Button postMasterpointsButton;
    }
}

