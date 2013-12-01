namespace CSVParser
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
			this.tabPublishBulletin = new System.Windows.Forms.TabPage();
			this.pnlBulletin = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.tabPublishResults = new System.Windows.Forms.TabPage();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.publishResultsStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.publishResultsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.cancelPublishResultsButton = new System.Windows.Forms.ToolStripButton();
			this.label4 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnPublish = new System.Windows.Forms.Button();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.tourneyYearCombobox = new System.Windows.Forms.ComboBox();
			this.tourneyNamesCombobox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabSaveResults = new System.Windows.Forms.TabPage();
			this.lblPreview = new System.Windows.Forms.Label();
			this.txtPreview = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnSelectFile = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.btnSaveHtml = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.cmbStyling = new System.Windows.Forms.ComboBox();
			this.tabCredentials = new System.Windows.Forms.TabPage();
			this.loadingPicture = new System.Windows.Forms.PictureBox();
			this.label8 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnRootFolder = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPublishBulletin.SuspendLayout();
			this.pnlBulletin.SuspendLayout();
			this.tabPublishResults.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.tabSaveResults.SuspendLayout();
			this.tabCredentials.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPublishBulletin
			// 
			this.tabPublishBulletin.Controls.Add(this.pnlBulletin);
			this.tabPublishBulletin.Location = new System.Drawing.Point(4, 22);
			this.tabPublishBulletin.Name = "tabPublishBulletin";
			this.tabPublishBulletin.Padding = new System.Windows.Forms.Padding(3);
			this.tabPublishBulletin.Size = new System.Drawing.Size(760, 539);
			this.tabPublishBulletin.TabIndex = 1;
			this.tabPublishBulletin.Text = "Publish Bulletin";
			this.tabPublishBulletin.UseVisualStyleBackColor = true;
			// 
			// pnlBulletin
			// 
			this.pnlBulletin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBulletin.Controls.Add(this.button2);
			this.pnlBulletin.Controls.Add(this.label6);
			this.pnlBulletin.Controls.Add(this.button3);
			this.pnlBulletin.Controls.Add(this.textBox3);
			this.pnlBulletin.Location = new System.Drawing.Point(54, 38);
			this.pnlBulletin.Name = "pnlBulletin";
			this.pnlBulletin.Size = new System.Drawing.Size(397, 297);
			this.pnlBulletin.TabIndex = 17;
			this.pnlBulletin.Visible = false;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(256, 57);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(85, 162);
			this.button2.TabIndex = 13;
			this.button2.Text = "Publish";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 87);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(70, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Bulletins path";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(27, 34);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(96, 32);
			this.button3.TabIndex = 0;
			this.button3.Text = "Select File";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(27, 108);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(192, 20);
			this.textBox3.TabIndex = 11;
			// 
			// tabPublishResults
			// 
			this.tabPublishResults.Controls.Add(this.statusStrip1);
			this.tabPublishResults.Controls.Add(this.label4);
			this.tabPublishResults.Controls.Add(this.lblStatus);
			this.tabPublishResults.Controls.Add(this.btnPublish);
			this.tabPublishResults.Controls.Add(this.txtStatus);
			this.tabPublishResults.Controls.Add(this.txtPath);
			this.tabPublishResults.Controls.Add(this.tourneyYearCombobox);
			this.tabPublishResults.Controls.Add(this.tourneyNamesCombobox);
			this.tabPublishResults.Controls.Add(this.label2);
			this.tabPublishResults.Controls.Add(this.label5);
			this.tabPublishResults.Location = new System.Drawing.Point(4, 22);
			this.tabPublishResults.Name = "tabPublishResults";
			this.tabPublishResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabPublishResults.Size = new System.Drawing.Size(760, 539);
			this.tabPublishResults.TabIndex = 3;
			this.tabPublishResults.Text = "Publish Results";
			this.tabPublishResults.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.publishResultsStatus,
            this.publishResultsProgressBar,
            this.cancelPublishResultsButton});
			this.statusStrip1.Location = new System.Drawing.Point(3, 513);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(754, 23);
			this.statusStrip1.TabIndex = 40;
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
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(97, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(114, 17);
			this.label4.TabIndex = 26;
			this.label4.Text = "Tourney Name : ";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(97, 236);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(96, 17);
			this.lblStatus.TabIndex = 21;
			this.lblStatus.Text = "Publish status";
			this.lblStatus.Visible = false;
			// 
			// btnPublish
			// 
			this.btnPublish.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPublish.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.btnPublish.Location = new System.Drawing.Point(196, 154);
			this.btnPublish.Name = "btnPublish";
			this.btnPublish.Size = new System.Drawing.Size(343, 53);
			this.btnPublish.TabIndex = 24;
			this.btnPublish.Text = "Publish";
			this.btnPublish.UseVisualStyleBackColor = false;
			this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click_1);
			// 
			// txtStatus
			// 
			this.txtStatus.Location = new System.Drawing.Point(100, 264);
			this.txtStatus.Multiline = true;
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(439, 201);
			this.txtStatus.TabIndex = 20;
			this.txtStatus.Visible = false;
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(503, 103);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(192, 20);
			this.txtPath.TabIndex = 22;
			// 
			// tourneyYearCombobox
			// 
			this.tourneyYearCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tourneyYearCombobox.FormattingEnabled = true;
			this.tourneyYearCombobox.Location = new System.Drawing.Point(228, 102);
			this.tourneyYearCombobox.Name = "tourneyYearCombobox";
			this.tourneyYearCombobox.Size = new System.Drawing.Size(92, 21);
			this.tourneyYearCombobox.TabIndex = 28;
			// 
			// tourneyNamesCombobox
			// 
			this.tourneyNamesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tourneyNamesCombobox.FormattingEnabled = true;
			this.tourneyNamesCombobox.Location = new System.Drawing.Point(228, 70);
			this.tourneyNamesCombobox.Name = "tourneyNamesCombobox";
			this.tourneyNamesCombobox.Size = new System.Drawing.Size(254, 21);
			this.tourneyNamesCombobox.TabIndex = 25;
			this.tourneyNamesCombobox.SelectedIndexChanged += new System.EventHandler(this.tourneyNamesCombobox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(559, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 23;
			this.label2.Text = "Path on web site";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(97, 103);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(107, 17);
			this.label5.TabIndex = 27;
			this.label5.Text = "Tourney Year : ";
			// 
			// tabSaveResults
			// 
			this.tabSaveResults.Controls.Add(this.lblPreview);
			this.tabSaveResults.Controls.Add(this.txtPreview);
			this.tabSaveResults.Controls.Add(this.label7);
			this.tabSaveResults.Controls.Add(this.btnSelectFile);
			this.tabSaveResults.Controls.Add(this.lblTitle);
			this.tabSaveResults.Controls.Add(this.txtTitle);
			this.tabSaveResults.Controls.Add(this.btnSaveHtml);
			this.tabSaveResults.Controls.Add(this.label3);
			this.tabSaveResults.Controls.Add(this.label1);
			this.tabSaveResults.Controls.Add(this.txtFileName);
			this.tabSaveResults.Controls.Add(this.cmbStyling);
			this.tabSaveResults.Location = new System.Drawing.Point(4, 22);
			this.tabSaveResults.Name = "tabSaveResults";
			this.tabSaveResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabSaveResults.Size = new System.Drawing.Size(760, 539);
			this.tabSaveResults.TabIndex = 0;
			this.tabSaveResults.Text = "Save Results";
			this.tabSaveResults.UseVisualStyleBackColor = true;
			// 
			// lblPreview
			// 
			this.lblPreview.AutoSize = true;
			this.lblPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPreview.Location = new System.Drawing.Point(356, 33);
			this.lblPreview.Name = "lblPreview";
			this.lblPreview.Size = new System.Drawing.Size(57, 17);
			this.lblPreview.TabIndex = 38;
			this.lblPreview.Text = "Preview";
			this.lblPreview.Visible = false;
			// 
			// txtPreview
			// 
			this.txtPreview.Location = new System.Drawing.Point(359, 55);
			this.txtPreview.Multiline = true;
			this.txtPreview.Name = "txtPreview";
			this.txtPreview.Size = new System.Drawing.Size(343, 344);
			this.txtPreview.TabIndex = 37;
			this.txtPreview.Visible = false;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(26, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(155, 17);
			this.label7.TabIndex = 36;
			this.label7.Text = "Select Results file (csv)";
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Location = new System.Drawing.Point(186, 48);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(91, 32);
			this.btnSelectFile.TabIndex = 28;
			this.btnSelectFile.Text = "Browse";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click_1);
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(26, 99);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(212, 17);
			this.lblTitle.TabIndex = 31;
			this.lblTitle.Text = "HTML Document Title (Optional)";
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(29, 118);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(192, 20);
			this.txtTitle.TabIndex = 30;
			// 
			// btnSaveHtml
			// 
			this.btnSaveHtml.Location = new System.Drawing.Point(51, 441);
			this.btnSaveHtml.Name = "btnSaveHtml";
			this.btnSaveHtml.Size = new System.Drawing.Size(631, 46);
			this.btnSaveHtml.TabIndex = 29;
			this.btnSaveHtml.Text = "Save Results";
			this.btnSaveHtml.UseVisualStyleBackColor = true;
			this.btnSaveHtml.Visible = false;
			this.btnSaveHtml.Click += new System.EventHandler(this.btnSaveHtml_Click_1);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(26, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 17);
			this.label3.TabIndex = 35;
			this.label3.Text = "Table styling";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(26, 156);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(170, 17);
			this.label1.TabIndex = 33;
			this.label1.Text = "Save As (File Name Only)";
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(29, 176);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(192, 20);
			this.txtFileName.TabIndex = 32;
			this.txtFileName.Click += new System.EventHandler(this.txtFileName_Click);
			this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
			// 
			// cmbStyling
			// 
			this.cmbStyling.FormattingEnabled = true;
			this.cmbStyling.Items.AddRange(new object[] {
            "table-blue",
            "table-red",
            "datagrid"});
			this.cmbStyling.Location = new System.Drawing.Point(29, 227);
			this.cmbStyling.Name = "cmbStyling";
			this.cmbStyling.Size = new System.Drawing.Size(121, 21);
			this.cmbStyling.TabIndex = 34;
			// 
			// tabCredentials
			// 
			this.tabCredentials.Controls.Add(this.loadingPicture);
			this.tabCredentials.Controls.Add(this.label8);
			this.tabCredentials.Controls.Add(this.button1);
			this.tabCredentials.Controls.Add(this.lblUserName);
			this.tabCredentials.Controls.Add(this.txtPassword);
			this.tabCredentials.Controls.Add(this.txtUserName);
			this.tabCredentials.Controls.Add(this.lblPassword);
			this.tabCredentials.Controls.Add(this.btnRootFolder);
			this.tabCredentials.Location = new System.Drawing.Point(4, 22);
			this.tabCredentials.Name = "tabCredentials";
			this.tabCredentials.Padding = new System.Windows.Forms.Padding(3);
			this.tabCredentials.Size = new System.Drawing.Size(760, 539);
			this.tabCredentials.TabIndex = 2;
			this.tabCredentials.Text = "Enter Credentials";
			this.tabCredentials.UseVisualStyleBackColor = true;
			// 
			// loadingPicture
			// 
			this.loadingPicture.Image = ((System.Drawing.Image)(resources.GetObject("loadingPicture.Image")));
			this.loadingPicture.Location = new System.Drawing.Point(209, 105);
			this.loadingPicture.Name = "loadingPicture";
			this.loadingPicture.Size = new System.Drawing.Size(36, 31);
			this.loadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.loadingPicture.TabIndex = 28;
			this.loadingPicture.TabStop = false;
			this.loadingPicture.Visible = false;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(110, 172);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(107, 17);
			this.label8.TabIndex = 27;
			this.label8.Text = "Results folder : ";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(113, 230);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(270, 69);
			this.button1.TabIndex = 26;
			this.button1.Text = "Save Settings";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUserName.Location = new System.Drawing.Point(110, 75);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(85, 17);
			this.lblUserName.TabIndex = 22;
			this.lblUserName.Text = "Username : ";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(226, 116);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(157, 20);
			this.txtPassword.TabIndex = 25;
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(226, 75);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(157, 20);
			this.txtUserName.TabIndex = 24;
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPassword.Location = new System.Drawing.Point(110, 119);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(81, 17);
			this.lblPassword.TabIndex = 23;
			this.lblPassword.Text = "Password : ";
			// 
			// btnRootFolder
			// 
			this.btnRootFolder.Location = new System.Drawing.Point(226, 162);
			this.btnRootFolder.Name = "btnRootFolder";
			this.btnRootFolder.Size = new System.Drawing.Size(157, 32);
			this.btnRootFolder.TabIndex = 21;
			this.btnRootFolder.Text = "Browse";
			this.btnRootFolder.UseVisualStyleBackColor = true;
			this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click_1);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabCredentials);
			this.tabControl1.Controls.Add(this.tabSaveResults);
			this.tabControl1.Controls.Add(this.tabPublishResults);
			this.tabControl1.Controls.Add(this.tabPublishBulletin);
			this.tabControl1.Location = new System.Drawing.Point(30, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(768, 565);
			this.tabControl1.TabIndex = 27;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1150, 611);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.tabPublishBulletin.ResumeLayout(false);
			this.pnlBulletin.ResumeLayout(false);
			this.pnlBulletin.PerformLayout();
			this.tabPublishResults.ResumeLayout(false);
			this.tabPublishResults.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabSaveResults.ResumeLayout(false);
			this.tabSaveResults.PerformLayout();
			this.tabCredentials.ResumeLayout(false);
			this.tabCredentials.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
		private System.Windows.Forms.TabPage tabPublishBulletin;
		private System.Windows.Forms.Panel pnlBulletin;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TabPage tabPublishResults;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnPublish;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.ComboBox tourneyYearCombobox;
		private System.Windows.Forms.ComboBox tourneyNamesCombobox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TabPage tabSaveResults;
		private System.Windows.Forms.TabPage tabCredentials;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button btnRootFolder;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Label lblPreview;
		private System.Windows.Forms.TextBox txtPreview;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnSelectFile;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Button btnSaveHtml;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.ComboBox cmbStyling;
		private System.Windows.Forms.PictureBox loadingPicture;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel publishResultsStatus;
		private System.Windows.Forms.ToolStripProgressBar publishResultsProgressBar;
		private System.Windows.Forms.ToolStripButton cancelPublishResultsButton;
	}
}

