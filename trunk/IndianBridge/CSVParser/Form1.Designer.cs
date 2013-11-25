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
			this.btnSelectFile = new System.Windows.Forms.Button();
			this.lblContents = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.btnSaveHtml = new System.Windows.Forms.Button();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbStyling = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
			this.btnPublish = new System.Windows.Forms.Button();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.btnRootFolder = new System.Windows.Forms.Button();
			this.txtFileContents = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.tourneyYearCombobox = new System.Windows.Forms.ComboBox();
			this.tourneyNamesCombobox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnUploadResults = new System.Windows.Forms.Button();
			this.btnUploadBulletin = new System.Windows.Forms.Button();
			this.btnMainMenu = new System.Windows.Forms.Button();
			this.pnlMenu = new System.Windows.Forms.Panel();
			this.pnlBulletin = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.publishResultsStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.publishResultsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.cancelPublishResultsButton = new System.Windows.Forms.ToolStripButton();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.pnlResults.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.pnlMenu.SuspendLayout();
			this.pnlBulletin.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Location = new System.Drawing.Point(42, 45);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(192, 32);
			this.btnSelectFile.TabIndex = 0;
			this.btnSelectFile.Text = "Select CSV File";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
			// 
			// lblContents
			// 
			this.lblContents.AutoSize = true;
			this.lblContents.Location = new System.Drawing.Point(14, 205);
			this.lblContents.Name = "lblContents";
			this.lblContents.Size = new System.Drawing.Size(72, 13);
			this.lblContents.TabIndex = 3;
			this.lblContents.Text = "Upload status";
			this.lblContents.Visible = false;
			// 
			// btnSaveHtml
			// 
			this.btnSaveHtml.Location = new System.Drawing.Point(42, 267);
			this.btnSaveHtml.Name = "btnSaveHtml";
			this.btnSaveHtml.Size = new System.Drawing.Size(192, 32);
			this.btnSaveHtml.TabIndex = 6;
			this.btnSaveHtml.Text = "Save HTML";
			this.btnSaveHtml.UseVisualStyleBackColor = true;
			this.btnSaveHtml.Visible = false;
			this.btnSaveHtml.Click += new System.EventHandler(this.btnSaveHtml_Click);
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(42, 113);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(192, 20);
			this.txtTitle.TabIndex = 7;
			this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(39, 96);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(160, 13);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "HTML Document Title (Optional)";
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(42, 170);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(192, 20);
			this.txtFileName.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(39, 153);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Save As (File Name Only)";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(168, 106);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(192, 20);
			this.txtPath.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(224, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Path on web site";
			// 
			// cmbStyling
			// 
			this.cmbStyling.FormattingEnabled = true;
			this.cmbStyling.Items.AddRange(new object[] {
            "table-blue",
            "table-red",
            "datagrid"});
			this.cmbStyling.Location = new System.Drawing.Point(42, 221);
			this.cmbStyling.Name = "cmbStyling";
			this.cmbStyling.Size = new System.Drawing.Size(121, 21);
			this.cmbStyling.TabIndex = 14;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(39, 205);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Table styling";
			// 
			// btnPublish
			// 
			this.btnPublish.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.btnPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPublish.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.btnPublish.Location = new System.Drawing.Point(17, 139);
			this.btnPublish.Name = "btnPublish";
			this.btnPublish.Size = new System.Drawing.Size(343, 53);
			this.btnPublish.TabIndex = 13;
			this.btnPublish.Text = "Publish";
			this.btnPublish.UseVisualStyleBackColor = false;
			this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
			// 
			// pnlResults
			// 
			this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlResults.Controls.Add(this.label8);
			this.pnlResults.Controls.Add(this.label7);
			this.pnlResults.Controls.Add(this.btnRootFolder);
			this.pnlResults.Controls.Add(this.panel1);
			this.pnlResults.Controls.Add(this.panel2);
			this.pnlResults.Location = new System.Drawing.Point(181, 50);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Size = new System.Drawing.Size(738, 491);
			this.pnlResults.TabIndex = 16;
			this.pnlResults.Visible = false;
			// 
			// btnRootFolder
			// 
			this.btnRootFolder.Location = new System.Drawing.Point(18, 11);
			this.btnRootFolder.Name = "btnRootFolder";
			this.btnRootFolder.Size = new System.Drawing.Size(695, 32);
			this.btnRootFolder.TabIndex = 20;
			this.btnRootFolder.Text = "Select Results folder";
			this.btnRootFolder.UseVisualStyleBackColor = true;
			this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
			// 
			// txtFileContents
			// 
			this.txtFileContents.Location = new System.Drawing.Point(17, 221);
			this.txtFileContents.Multiline = true;
			this.txtFileContents.Name = "txtFileContents";
			this.txtFileContents.Size = new System.Drawing.Size(343, 167);
			this.txtFileContents.TabIndex = 2;
			this.txtFileContents.Visible = false;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnSelectFile);
			this.panel1.Controls.Add(this.lblTitle);
			this.panel1.Controls.Add(this.txtTitle);
			this.panel1.Controls.Add(this.btnSaveHtml);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.txtFileName);
			this.panel1.Controls.Add(this.cmbStyling);
			this.panel1.Location = new System.Drawing.Point(19, 67);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(294, 404);
			this.panel1.TabIndex = 21;
			this.panel1.Visible = false;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.lblContents);
			this.panel2.Controls.Add(this.btnPublish);
			this.panel2.Controls.Add(this.txtFileContents);
			this.panel2.Controls.Add(this.tourneyYearCombobox);
			this.panel2.Controls.Add(this.txtPath);
			this.panel2.Controls.Add(this.tourneyNamesCombobox);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Location = new System.Drawing.Point(328, 67);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(385, 404);
			this.panel2.TabIndex = 22;
			this.panel2.Visible = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Tourney Name : ";
			// 
			// tourneyYearCombobox
			// 
			this.tourneyYearCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tourneyYearCombobox.FormattingEnabled = true;
			this.tourneyYearCombobox.Location = new System.Drawing.Point(106, 78);
			this.tourneyYearCombobox.Name = "tourneyYearCombobox";
			this.tourneyYearCombobox.Size = new System.Drawing.Size(92, 21);
			this.tourneyYearCombobox.TabIndex = 19;
			// 
			// tourneyNamesCombobox
			// 
			this.tourneyNamesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tourneyNamesCombobox.FormattingEnabled = true;
			this.tourneyNamesCombobox.Location = new System.Drawing.Point(106, 45);
			this.tourneyNamesCombobox.Name = "tourneyNamesCombobox";
			this.tourneyNamesCombobox.Size = new System.Drawing.Size(254, 21);
			this.tourneyNamesCombobox.TabIndex = 16;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 81);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Tourney Year : ";
			// 
			// btnUploadResults
			// 
			this.btnUploadResults.Location = new System.Drawing.Point(17, 8);
			this.btnUploadResults.Name = "btnUploadResults";
			this.btnUploadResults.Size = new System.Drawing.Size(119, 54);
			this.btnUploadResults.TabIndex = 0;
			this.btnUploadResults.Text = "Upload Results";
			this.btnUploadResults.UseVisualStyleBackColor = true;
			this.btnUploadResults.Click += new System.EventHandler(this.btnUploadResults_Click);
			// 
			// btnUploadBulletin
			// 
			this.btnUploadBulletin.Location = new System.Drawing.Point(17, 79);
			this.btnUploadBulletin.Name = "btnUploadBulletin";
			this.btnUploadBulletin.Size = new System.Drawing.Size(119, 54);
			this.btnUploadBulletin.TabIndex = 1;
			this.btnUploadBulletin.Text = "Upload Bulletin";
			this.btnUploadBulletin.UseVisualStyleBackColor = true;
			this.btnUploadBulletin.Click += new System.EventHandler(this.btnUploadBulletin_Click);
			// 
			// btnMainMenu
			// 
			this.btnMainMenu.Location = new System.Drawing.Point(29, 38);
			this.btnMainMenu.Name = "btnMainMenu";
			this.btnMainMenu.Size = new System.Drawing.Size(119, 54);
			this.btnMainMenu.TabIndex = 17;
			this.btnMainMenu.Text = "Menu";
			this.btnMainMenu.UseVisualStyleBackColor = true;
			this.btnMainMenu.Click += new System.EventHandler(this.btnMainMenu_Click);
			// 
			// pnlMenu
			// 
			this.pnlMenu.Controls.Add(this.btnUploadResults);
			this.pnlMenu.Controls.Add(this.btnUploadBulletin);
			this.pnlMenu.Location = new System.Drawing.Point(12, 98);
			this.pnlMenu.Name = "pnlMenu";
			this.pnlMenu.Size = new System.Drawing.Size(163, 143);
			this.pnlMenu.TabIndex = 16;
			// 
			// pnlBulletin
			// 
			this.pnlBulletin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBulletin.Controls.Add(this.button2);
			this.pnlBulletin.Controls.Add(this.label6);
			this.pnlBulletin.Controls.Add(this.button3);
			this.pnlBulletin.Controls.Add(this.textBox3);
			this.pnlBulletin.Location = new System.Drawing.Point(244, 31);
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
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.publishResultsStatus,
            this.publishResultsProgressBar,
            this.cancelPublishResultsButton});
			this.statusStrip1.Location = new System.Drawing.Point(0, 554);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(931, 23);
			this.statusStrip1.TabIndex = 26;
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
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(23, 55);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(144, 17);
			this.label7.TabIndex = 16;
			this.label7.Text = "Save results to HTML";
			this.label7.Visible = false;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(332, 55);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(172, 17);
			this.label8.TabIndex = 23;
			this.label8.Text = "Publish results to web-site";
			this.label8.Visible = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(931, 577);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.pnlResults);
			this.Controls.Add(this.btnMainMenu);
			this.Controls.Add(this.pnlMenu);
			this.Controls.Add(this.pnlBulletin);
			this.Name = "Form1";
			this.Text = "Form1";
			this.pnlResults.ResumeLayout(false);
			this.pnlResults.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.pnlMenu.ResumeLayout(false);
			this.pnlBulletin.ResumeLayout(false);
			this.pnlBulletin.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnSelectFile;
		private System.Windows.Forms.Label lblContents;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button btnSaveHtml;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbStyling;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
		private System.Windows.Forms.Button btnPublish;
		private System.Windows.Forms.Panel pnlResults;
		private System.Windows.Forms.Button btnUploadBulletin;
		private System.Windows.Forms.Button btnUploadResults;
		private System.Windows.Forms.Button btnMainMenu;
		private System.Windows.Forms.Panel pnlMenu;
		private System.Windows.Forms.Panel pnlBulletin;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox txtFileContents;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox tourneyYearCombobox;
		private System.Windows.Forms.ComboBox tourneyNamesCombobox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel publishResultsStatus;
		private System.Windows.Forms.ToolStripProgressBar publishResultsProgressBar;
		private System.Windows.Forms.ToolStripButton cancelPublishResultsButton;
		private System.Windows.Forms.Button btnRootFolder;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
	}
}

