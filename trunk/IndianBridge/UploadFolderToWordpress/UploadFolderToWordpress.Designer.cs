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
            this.uploadStatusTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.createHTML = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.loadCSVButton = new System.Windows.Forms.Button();
            this.htmlBrowser = new System.Windows.Forms.WebBrowser();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.savedAsLabel = new System.Windows.Forms.Label();
            this.saveAsHTMLButton = new System.Windows.Forms.Button();
            this.uploadFiles = new System.Windows.Forms.TabPage();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.createHTML.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.uploadFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectFolderDialog
            // 
            this.selectFolderDialog.ShowNewFolderButton = false;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Location = new System.Drawing.Point(5, 6);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(64, 13);
            this.username.TabIndex = 13;
            this.username.Text = "Username : ";
            // 
            // wordpressPathTextbox
            // 
            this.wordpressPathTextbox.Location = new System.Drawing.Point(76, 267);
            this.wordpressPathTextbox.Name = "wordpressPathTextbox";
            this.wordpressPathTextbox.Size = new System.Drawing.Size(336, 20);
            this.wordpressPathTextbox.TabIndex = 24;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Location = new System.Drawing.Point(5, 38);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(62, 13);
            this.password.TabIndex = 14;
            this.password.Text = "Password : ";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 27);
            this.label3.TabIndex = 23;
            this.label3.Text = "Wordpress Path : ";
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(76, 6);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(216, 20);
            this.usernameTextbox.TabIndex = 15;
            // 
            // uploadButton
            // 
            this.uploadButton.BackColor = System.Drawing.Color.Green;
            this.uploadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.uploadButton.Location = new System.Drawing.Point(8, 297);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(404, 68);
            this.uploadButton.TabIndex = 22;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = false;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(76, 35);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.PasswordChar = '*';
            this.passwordTextbox.Size = new System.Drawing.Size(216, 20);
            this.passwordTextbox.TabIndex = 16;
            // 
            // wordpressURLTextbox
            // 
            this.wordpressURLTextbox.Location = new System.Drawing.Point(76, 230);
            this.wordpressURLTextbox.Name = "wordpressURLTextbox";
            this.wordpressURLTextbox.Size = new System.Drawing.Size(336, 20);
            this.wordpressURLTextbox.TabIndex = 21;
            this.wordpressURLTextbox.Text = "http://127.0.0.1/bfi";
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(8, 159);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(173, 37);
            this.selectFolderButton.TabIndex = 17;
            this.selectFolderButton.Text = "Select Folder To Upload";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click_1);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Wordpress URL : ";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 58);
            this.label1.TabIndex = 18;
            this.label1.Text = "Selected File or Folder : ";
            // 
            // selectedFolderTextbox
            // 
            this.selectedFolderTextbox.Location = new System.Drawing.Point(76, 61);
            this.selectedFolderTextbox.Multiline = true;
            this.selectedFolderTextbox.Name = "selectedFolderTextbox";
            this.selectedFolderTextbox.Size = new System.Drawing.Size(332, 92);
            this.selectedFolderTextbox.TabIndex = 19;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.publishResultsStatus,
            this.publishResultsProgressBar,
            this.cancelPublishResultsButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 403);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(876, 23);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
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
            this.selectFileToUploadButton.Location = new System.Drawing.Point(235, 159);
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
            this.useTourneyTemplateCheckbox.Location = new System.Drawing.Point(8, 206);
            this.useTourneyTemplateCheckbox.Name = "useTourneyTemplateCheckbox";
            this.useTourneyTemplateCheckbox.Size = new System.Drawing.Size(134, 17);
            this.useTourneyTemplateCheckbox.TabIndex = 27;
            this.useTourneyTemplateCheckbox.Text = "Use Tourney Template";
            this.useTourneyTemplateCheckbox.UseVisualStyleBackColor = true;
            // 
            // uploadStatusTextbox
            // 
            this.uploadStatusTextbox.Location = new System.Drawing.Point(430, 22);
            this.uploadStatusTextbox.Multiline = true;
            this.uploadStatusTextbox.Name = "uploadStatusTextbox";
            this.uploadStatusTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uploadStatusTextbox.Size = new System.Drawing.Size(427, 337);
            this.uploadStatusTextbox.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(427, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Upload Status";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.createHTML);
            this.tabControl1.Controls.Add(this.uploadFiles);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(876, 426);
            this.tabControl1.TabIndex = 30;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // createHTML
            // 
            this.createHTML.Controls.Add(this.splitContainer1);
            this.createHTML.Location = new System.Drawing.Point(4, 22);
            this.createHTML.Name = "createHTML";
            this.createHTML.Padding = new System.Windows.Forms.Padding(3);
            this.createHTML.Size = new System.Drawing.Size(868, 400);
            this.createHTML.TabIndex = 0;
            this.createHTML.Text = "Convert CSV To HTML";
            this.createHTML.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(862, 394);
            this.splitContainer1.SplitterDistance = 304;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.loadCSVButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.htmlBrowser);
            this.splitContainer2.Size = new System.Drawing.Size(862, 304);
            this.splitContainer2.SplitterDistance = 80;
            this.splitContainer2.TabIndex = 0;
            // 
            // loadCSVButton
            // 
            this.loadCSVButton.BackColor = System.Drawing.Color.Green;
            this.loadCSVButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadCSVButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadCSVButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.loadCSVButton.Location = new System.Drawing.Point(0, 0);
            this.loadCSVButton.Name = "loadCSVButton";
            this.loadCSVButton.Size = new System.Drawing.Size(862, 80);
            this.loadCSVButton.TabIndex = 23;
            this.loadCSVButton.Text = "Load CSV File";
            this.loadCSVButton.UseVisualStyleBackColor = false;
            this.loadCSVButton.Click += new System.EventHandler(this.loadCSVButton_Click);
            // 
            // htmlBrowser
            // 
            this.htmlBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlBrowser.Location = new System.Drawing.Point(0, 0);
            this.htmlBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.htmlBrowser.Name = "htmlBrowser";
            this.htmlBrowser.Size = new System.Drawing.Size(862, 220);
            this.htmlBrowser.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.savedAsLabel);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.saveAsHTMLButton);
            this.splitContainer3.Size = new System.Drawing.Size(862, 86);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 0;
            // 
            // savedAsLabel
            // 
            this.savedAsLabel.AutoSize = true;
            this.savedAsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savedAsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savedAsLabel.Location = new System.Drawing.Point(0, 0);
            this.savedAsLabel.Name = "savedAsLabel";
            this.savedAsLabel.Size = new System.Drawing.Size(89, 20);
            this.savedAsLabel.TabIndex = 0;
            this.savedAsLabel.Text = "Saved As : ";
            // 
            // saveAsHTMLButton
            // 
            this.saveAsHTMLButton.BackColor = System.Drawing.Color.Green;
            this.saveAsHTMLButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveAsHTMLButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAsHTMLButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveAsHTMLButton.Location = new System.Drawing.Point(0, 0);
            this.saveAsHTMLButton.Name = "saveAsHTMLButton";
            this.saveAsHTMLButton.Size = new System.Drawing.Size(858, 53);
            this.saveAsHTMLButton.TabIndex = 24;
            this.saveAsHTMLButton.Text = "Save with different filename";
            this.saveAsHTMLButton.UseVisualStyleBackColor = false;
            this.saveAsHTMLButton.Click += new System.EventHandler(this.saveAsHTMLButton_Click);
            // 
            // uploadFiles
            // 
            this.uploadFiles.Controls.Add(this.uploadStatusTextbox);
            this.uploadFiles.Controls.Add(this.label4);
            this.uploadFiles.Controls.Add(this.selectedFolderTextbox);
            this.uploadFiles.Controls.Add(this.label1);
            this.uploadFiles.Controls.Add(this.useTourneyTemplateCheckbox);
            this.uploadFiles.Controls.Add(this.label2);
            this.uploadFiles.Controls.Add(this.selectFileToUploadButton);
            this.uploadFiles.Controls.Add(this.selectFolderButton);
            this.uploadFiles.Controls.Add(this.wordpressURLTextbox);
            this.uploadFiles.Controls.Add(this.username);
            this.uploadFiles.Controls.Add(this.passwordTextbox);
            this.uploadFiles.Controls.Add(this.wordpressPathTextbox);
            this.uploadFiles.Controls.Add(this.uploadButton);
            this.uploadFiles.Controls.Add(this.password);
            this.uploadFiles.Controls.Add(this.usernameTextbox);
            this.uploadFiles.Controls.Add(this.label3);
            this.uploadFiles.Location = new System.Drawing.Point(4, 22);
            this.uploadFiles.Name = "uploadFiles";
            this.uploadFiles.Padding = new System.Windows.Forms.Padding(3);
            this.uploadFiles.Size = new System.Drawing.Size(868, 400);
            this.uploadFiles.TabIndex = 1;
            this.uploadFiles.Text = "Upload File/Folder To Website";
            this.uploadFiles.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "html";
            this.saveFileDialog1.Filter = "Html Files | *.html";
            this.saveFileDialog1.Title = "Save As Different HTML file";
            // 
            // UploadFolderToWordpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 426);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "UploadFolderToWordpress";
            this.Text = "Upload Folder to Wordpress";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.createHTML.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.uploadFiles.ResumeLayout(false);
            this.uploadFiles.PerformLayout();
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
        private System.Windows.Forms.TextBox uploadStatusTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage createHTML;
        private System.Windows.Forms.TabPage uploadFiles;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button loadCSVButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.WebBrowser htmlBrowser;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label savedAsLabel;
        private System.Windows.Forms.Button saveAsHTMLButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

