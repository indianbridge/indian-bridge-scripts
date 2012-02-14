namespace IndianBridge.Applications
{
    partial class PairsScorerAllTabs
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Summary = new System.Windows.Forms.TextBox();
            this.Summary_Label = new System.Windows.Forms.Label();
            this.SelectSummaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SelectSummaryFileButton = new System.Windows.Forms.Button();
            this.ControlTabs = new System.Windows.Forms.TabControl();
            this.LoadSummaryTab = new System.Windows.Forms.TabPage();
            this.LoadSummaryButton = new System.Windows.Forms.Button();
            this.CreateDatabaseTab = new System.Windows.Forms.TabPage();
            this.CD_StatusLabel = new System.Windows.Forms.Label();
            this.CD_Status = new System.Windows.Forms.TextBox();
            this.CreateDatabaseButton = new System.Windows.Forms.Button();
            this.ChangeDatabaseButton = new System.Windows.Forms.Button();
            this.DatabaseFileNameLabel = new System.Windows.Forms.Label();
            this.CD_DatabaseFileName = new System.Windows.Forms.TextBox();
            this.LoadDatabaseTab = new System.Windows.Forms.TabPage();
            this.LoadDatabaseButton = new System.Windows.Forms.Button();
            this.SelectDatabaseFileButton = new System.Windows.Forms.Button();
            this.LoadDatabaseLabel = new System.Windows.Forms.Label();
            this.LoadDatabaseFileName = new System.Windows.Forms.TextBox();
            this.CreateWebpagesTab = new System.Windows.Forms.TabPage();
            this.CW_StatusLabel = new System.Windows.Forms.Label();
            this.CW_Status = new System.Windows.Forms.TextBox();
            this.CW_CreateButton = new System.Windows.Forms.Button();
            this.CW_ChangeButton = new System.Windows.Forms.Button();
            this.CW_RootFolderLabel = new System.Windows.Forms.Label();
            this.CW_RootFolder = new System.Windows.Forms.TextBox();
            this.FindCalendarEventTab = new System.Windows.Forms.TabPage();
            this.calendarGetEvents_SearchTextbox = new System.Windows.Forms.TextBox();
            this.queryString_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_Status = new System.Windows.Forms.TextBox();
            this.calendarGetEvents_Button = new System.Windows.Forms.Button();
            this.toDate_Label = new System.Windows.Forms.Label();
            this.fromDate_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_endDate = new System.Windows.Forms.DateTimePicker();
            this.calendarGetEvents_startDate = new System.Windows.Forms.DateTimePicker();
            this.UploadWebpagesTab = new System.Windows.Forms.TabPage();
            this.UW_webpagesDirectory = new System.Windows.Forms.TextBox();
            this.UW_Status = new System.Windows.Forms.TextBox();
            this.UW_RootPath = new System.Windows.Forms.TextBox();
            this.UploadToSites = new System.Windows.Forms.Button();
            this.UW_SiteName = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DatabaseChangeDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.LoadDatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ControlTabs.SuspendLayout();
            this.LoadSummaryTab.SuspendLayout();
            this.CreateDatabaseTab.SuspendLayout();
            this.LoadDatabaseTab.SuspendLayout();
            this.CreateWebpagesTab.SuspendLayout();
            this.FindCalendarEventTab.SuspendLayout();
            this.UploadWebpagesTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Summary
            // 
            this.Summary.Location = new System.Drawing.Point(6, 23);
            this.Summary.Multiline = true;
            this.Summary.Name = "Summary";
            this.Summary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Summary.Size = new System.Drawing.Size(717, 236);
            this.Summary.TabIndex = 1;
            // 
            // Summary_Label
            // 
            this.Summary_Label.AutoSize = true;
            this.Summary_Label.Location = new System.Drawing.Point(3, 7);
            this.Summary_Label.Name = "Summary_Label";
            this.Summary_Label.Size = new System.Drawing.Size(50, 13);
            this.Summary_Label.TabIndex = 3;
            this.Summary_Label.Text = "Summary";
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // SelectSummaryFileButton
            // 
            this.SelectSummaryFileButton.AutoSize = true;
            this.SelectSummaryFileButton.Location = new System.Drawing.Point(730, 23);
            this.SelectSummaryFileButton.Name = "SelectSummaryFileButton";
            this.SelectSummaryFileButton.Size = new System.Drawing.Size(112, 23);
            this.SelectSummaryFileButton.TabIndex = 9;
            this.SelectSummaryFileButton.Text = "Select Summary File";
            this.SelectSummaryFileButton.UseVisualStyleBackColor = true;
            this.SelectSummaryFileButton.Click += new System.EventHandler(this.SelectSummaryFileButton_Click);
            // 
            // ControlTabs
            // 
            this.ControlTabs.Controls.Add(this.LoadSummaryTab);
            this.ControlTabs.Controls.Add(this.CreateDatabaseTab);
            this.ControlTabs.Controls.Add(this.LoadDatabaseTab);
            this.ControlTabs.Controls.Add(this.CreateWebpagesTab);
            this.ControlTabs.Controls.Add(this.FindCalendarEventTab);
            this.ControlTabs.Controls.Add(this.UploadWebpagesTab);
            this.ControlTabs.Controls.Add(this.tabPage1);
            this.ControlTabs.Location = new System.Drawing.Point(9, 11);
            this.ControlTabs.Name = "ControlTabs";
            this.ControlTabs.SelectedIndex = 0;
            this.ControlTabs.Size = new System.Drawing.Size(877, 367);
            this.ControlTabs.TabIndex = 10;
            // 
            // LoadSummaryTab
            // 
            this.LoadSummaryTab.Controls.Add(this.LoadSummaryButton);
            this.LoadSummaryTab.Controls.Add(this.Summary);
            this.LoadSummaryTab.Controls.Add(this.SelectSummaryFileButton);
            this.LoadSummaryTab.Controls.Add(this.Summary_Label);
            this.LoadSummaryTab.Location = new System.Drawing.Point(4, 22);
            this.LoadSummaryTab.Name = "LoadSummaryTab";
            this.LoadSummaryTab.Padding = new System.Windows.Forms.Padding(3);
            this.LoadSummaryTab.Size = new System.Drawing.Size(869, 341);
            this.LoadSummaryTab.TabIndex = 0;
            this.LoadSummaryTab.Text = "Load Summary";
            this.LoadSummaryTab.UseVisualStyleBackColor = true;
            // 
            // LoadSummaryButton
            // 
            this.LoadSummaryButton.Location = new System.Drawing.Point(6, 278);
            this.LoadSummaryButton.Name = "LoadSummaryButton";
            this.LoadSummaryButton.Size = new System.Drawing.Size(835, 43);
            this.LoadSummaryButton.TabIndex = 10;
            this.LoadSummaryButton.Text = "Load Summary";
            this.LoadSummaryButton.UseVisualStyleBackColor = true;
            this.LoadSummaryButton.Click += new System.EventHandler(this.LoadSummaryButton_Click);
            // 
            // CreateDatabaseTab
            // 
            this.CreateDatabaseTab.Controls.Add(this.CD_StatusLabel);
            this.CreateDatabaseTab.Controls.Add(this.CD_Status);
            this.CreateDatabaseTab.Controls.Add(this.CreateDatabaseButton);
            this.CreateDatabaseTab.Controls.Add(this.ChangeDatabaseButton);
            this.CreateDatabaseTab.Controls.Add(this.DatabaseFileNameLabel);
            this.CreateDatabaseTab.Controls.Add(this.CD_DatabaseFileName);
            this.CreateDatabaseTab.Location = new System.Drawing.Point(4, 22);
            this.CreateDatabaseTab.Name = "CreateDatabaseTab";
            this.CreateDatabaseTab.Size = new System.Drawing.Size(869, 341);
            this.CreateDatabaseTab.TabIndex = 5;
            this.CreateDatabaseTab.Text = "Create Database";
            this.CreateDatabaseTab.UseVisualStyleBackColor = true;
            // 
            // CD_StatusLabel
            // 
            this.CD_StatusLabel.AutoSize = true;
            this.CD_StatusLabel.Location = new System.Drawing.Point(5, 116);
            this.CD_StatusLabel.Name = "CD_StatusLabel";
            this.CD_StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.CD_StatusLabel.TabIndex = 0;
            this.CD_StatusLabel.Text = "Status";
            // 
            // CD_Status
            // 
            this.CD_Status.Location = new System.Drawing.Point(8, 136);
            this.CD_Status.Multiline = true;
            this.CD_Status.Name = "CD_Status";
            this.CD_Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CD_Status.Size = new System.Drawing.Size(778, 199);
            this.CD_Status.TabIndex = 19;
            // 
            // CreateDatabaseButton
            // 
            this.CreateDatabaseButton.Location = new System.Drawing.Point(8, 70);
            this.CreateDatabaseButton.Name = "CreateDatabaseButton";
            this.CreateDatabaseButton.Size = new System.Drawing.Size(835, 43);
            this.CreateDatabaseButton.TabIndex = 18;
            this.CreateDatabaseButton.Text = "Create Database";
            this.CreateDatabaseButton.UseVisualStyleBackColor = true;
            this.CreateDatabaseButton.Click += new System.EventHandler(this.CreateDatabaseButton_Click);
            // 
            // ChangeDatabaseButton
            // 
            this.ChangeDatabaseButton.AutoSize = true;
            this.ChangeDatabaseButton.Location = new System.Drawing.Point(545, 3);
            this.ChangeDatabaseButton.Name = "ChangeDatabaseButton";
            this.ChangeDatabaseButton.Size = new System.Drawing.Size(83, 28);
            this.ChangeDatabaseButton.TabIndex = 17;
            this.ChangeDatabaseButton.Text = "Change";
            this.ChangeDatabaseButton.UseVisualStyleBackColor = true;
            this.ChangeDatabaseButton.Click += new System.EventHandler(this.ChangeDatabaseButton_Click);
            // 
            // DatabaseFileNameLabel
            // 
            this.DatabaseFileNameLabel.AutoSize = true;
            this.DatabaseFileNameLabel.Location = new System.Drawing.Point(5, 3);
            this.DatabaseFileNameLabel.Name = "DatabaseFileNameLabel";
            this.DatabaseFileNameLabel.Size = new System.Drawing.Size(112, 13);
            this.DatabaseFileNameLabel.TabIndex = 16;
            this.DatabaseFileNameLabel.Text = "Database File Name : ";
            // 
            // CD_DatabaseFileName
            // 
            this.CD_DatabaseFileName.Location = new System.Drawing.Point(121, 3);
            this.CD_DatabaseFileName.Multiline = true;
            this.CD_DatabaseFileName.Name = "CD_DatabaseFileName";
            this.CD_DatabaseFileName.Size = new System.Drawing.Size(418, 61);
            this.CD_DatabaseFileName.TabIndex = 15;
            // 
            // LoadDatabaseTab
            // 
            this.LoadDatabaseTab.Controls.Add(this.LoadDatabaseButton);
            this.LoadDatabaseTab.Controls.Add(this.SelectDatabaseFileButton);
            this.LoadDatabaseTab.Controls.Add(this.LoadDatabaseLabel);
            this.LoadDatabaseTab.Controls.Add(this.LoadDatabaseFileName);
            this.LoadDatabaseTab.Location = new System.Drawing.Point(4, 22);
            this.LoadDatabaseTab.Name = "LoadDatabaseTab";
            this.LoadDatabaseTab.Padding = new System.Windows.Forms.Padding(3);
            this.LoadDatabaseTab.Size = new System.Drawing.Size(869, 341);
            this.LoadDatabaseTab.TabIndex = 1;
            this.LoadDatabaseTab.Text = "Load Database";
            this.LoadDatabaseTab.UseVisualStyleBackColor = true;
            // 
            // LoadDatabaseButton
            // 
            this.LoadDatabaseButton.Location = new System.Drawing.Point(8, 73);
            this.LoadDatabaseButton.Name = "LoadDatabaseButton";
            this.LoadDatabaseButton.Size = new System.Drawing.Size(835, 43);
            this.LoadDatabaseButton.TabIndex = 21;
            this.LoadDatabaseButton.Text = "Load Database";
            this.LoadDatabaseButton.UseVisualStyleBackColor = true;
            this.LoadDatabaseButton.Click += new System.EventHandler(this.LoadDatabaseButton_Click);
            // 
            // SelectDatabaseFileButton
            // 
            this.SelectDatabaseFileButton.AutoSize = true;
            this.SelectDatabaseFileButton.Location = new System.Drawing.Point(545, 6);
            this.SelectDatabaseFileButton.Name = "SelectDatabaseFileButton";
            this.SelectDatabaseFileButton.Size = new System.Drawing.Size(115, 28);
            this.SelectDatabaseFileButton.TabIndex = 20;
            this.SelectDatabaseFileButton.Text = "Select Database File";
            this.SelectDatabaseFileButton.UseVisualStyleBackColor = true;
            this.SelectDatabaseFileButton.Click += new System.EventHandler(this.SelectDatabaseFileButton_Click);
            // 
            // LoadDatabaseLabel
            // 
            this.LoadDatabaseLabel.AutoSize = true;
            this.LoadDatabaseLabel.Location = new System.Drawing.Point(5, 6);
            this.LoadDatabaseLabel.Name = "LoadDatabaseLabel";
            this.LoadDatabaseLabel.Size = new System.Drawing.Size(112, 13);
            this.LoadDatabaseLabel.TabIndex = 19;
            this.LoadDatabaseLabel.Text = "Database File Name : ";
            // 
            // LoadDatabaseFileName
            // 
            this.LoadDatabaseFileName.Location = new System.Drawing.Point(121, 6);
            this.LoadDatabaseFileName.Multiline = true;
            this.LoadDatabaseFileName.Name = "LoadDatabaseFileName";
            this.LoadDatabaseFileName.Size = new System.Drawing.Size(418, 61);
            this.LoadDatabaseFileName.TabIndex = 18;
            // 
            // CreateWebpagesTab
            // 
            this.CreateWebpagesTab.Controls.Add(this.CW_StatusLabel);
            this.CreateWebpagesTab.Controls.Add(this.CW_Status);
            this.CreateWebpagesTab.Controls.Add(this.CW_CreateButton);
            this.CreateWebpagesTab.Controls.Add(this.CW_ChangeButton);
            this.CreateWebpagesTab.Controls.Add(this.CW_RootFolderLabel);
            this.CreateWebpagesTab.Controls.Add(this.CW_RootFolder);
            this.CreateWebpagesTab.Location = new System.Drawing.Point(4, 22);
            this.CreateWebpagesTab.Name = "CreateWebpagesTab";
            this.CreateWebpagesTab.Padding = new System.Windows.Forms.Padding(3);
            this.CreateWebpagesTab.Size = new System.Drawing.Size(869, 341);
            this.CreateWebpagesTab.TabIndex = 2;
            this.CreateWebpagesTab.Text = "Create Webpages";
            this.CreateWebpagesTab.UseVisualStyleBackColor = true;
            // 
            // CW_StatusLabel
            // 
            this.CW_StatusLabel.AutoSize = true;
            this.CW_StatusLabel.Location = new System.Drawing.Point(4, 120);
            this.CW_StatusLabel.Name = "CW_StatusLabel";
            this.CW_StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.CW_StatusLabel.TabIndex = 25;
            this.CW_StatusLabel.Text = "Status";
            // 
            // CW_Status
            // 
            this.CW_Status.Location = new System.Drawing.Point(7, 140);
            this.CW_Status.Multiline = true;
            this.CW_Status.Name = "CW_Status";
            this.CW_Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CW_Status.Size = new System.Drawing.Size(778, 199);
            this.CW_Status.TabIndex = 26;
            // 
            // CW_CreateButton
            // 
            this.CW_CreateButton.Location = new System.Drawing.Point(7, 73);
            this.CW_CreateButton.Name = "CW_CreateButton";
            this.CW_CreateButton.Size = new System.Drawing.Size(835, 43);
            this.CW_CreateButton.TabIndex = 24;
            this.CW_CreateButton.Text = "Create Webpages";
            this.CW_CreateButton.UseVisualStyleBackColor = true;
            this.CW_CreateButton.Click += new System.EventHandler(this.CW_CreateButton_Click);
            // 
            // CW_ChangeButton
            // 
            this.CW_ChangeButton.AutoSize = true;
            this.CW_ChangeButton.Location = new System.Drawing.Point(555, 6);
            this.CW_ChangeButton.Name = "CW_ChangeButton";
            this.CW_ChangeButton.Size = new System.Drawing.Size(115, 28);
            this.CW_ChangeButton.TabIndex = 23;
            this.CW_ChangeButton.Text = "Change";
            this.CW_ChangeButton.UseVisualStyleBackColor = true;
            this.CW_ChangeButton.Click += new System.EventHandler(this.CW_ChangeButton_Click);
            // 
            // CW_RootFolderLabel
            // 
            this.CW_RootFolderLabel.AutoSize = true;
            this.CW_RootFolderLabel.Location = new System.Drawing.Point(4, 6);
            this.CW_RootFolderLabel.Name = "CW_RootFolderLabel";
            this.CW_RootFolderLabel.Size = new System.Drawing.Size(126, 13);
            this.CW_RootFolderLabel.TabIndex = 22;
            this.CW_RootFolderLabel.Text = "Webpages Root Folder : ";
            // 
            // CW_RootFolder
            // 
            this.CW_RootFolder.Location = new System.Drawing.Point(131, 6);
            this.CW_RootFolder.Multiline = true;
            this.CW_RootFolder.Name = "CW_RootFolder";
            this.CW_RootFolder.Size = new System.Drawing.Size(418, 61);
            this.CW_RootFolder.TabIndex = 21;
            // 
            // FindCalendarEventTab
            // 
            this.FindCalendarEventTab.Controls.Add(this.calendarGetEvents_SearchTextbox);
            this.FindCalendarEventTab.Controls.Add(this.queryString_Label);
            this.FindCalendarEventTab.Controls.Add(this.calendarGetEvents_Status);
            this.FindCalendarEventTab.Controls.Add(this.calendarGetEvents_Button);
            this.FindCalendarEventTab.Controls.Add(this.toDate_Label);
            this.FindCalendarEventTab.Controls.Add(this.fromDate_Label);
            this.FindCalendarEventTab.Controls.Add(this.calendarGetEvents_endDate);
            this.FindCalendarEventTab.Controls.Add(this.calendarGetEvents_startDate);
            this.FindCalendarEventTab.Location = new System.Drawing.Point(4, 22);
            this.FindCalendarEventTab.Name = "FindCalendarEventTab";
            this.FindCalendarEventTab.Size = new System.Drawing.Size(869, 341);
            this.FindCalendarEventTab.TabIndex = 3;
            this.FindCalendarEventTab.Text = "Find Calendar Event";
            this.FindCalendarEventTab.UseVisualStyleBackColor = true;
            // 
            // calendarGetEvents_SearchTextbox
            // 
            this.calendarGetEvents_SearchTextbox.Location = new System.Drawing.Point(84, 59);
            this.calendarGetEvents_SearchTextbox.Name = "calendarGetEvents_SearchTextbox";
            this.calendarGetEvents_SearchTextbox.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_SearchTextbox.TabIndex = 23;
            // 
            // queryString_Label
            // 
            this.queryString_Label.AutoSize = true;
            this.queryString_Label.Location = new System.Drawing.Point(8, 59);
            this.queryString_Label.Name = "queryString_Label";
            this.queryString_Label.Size = new System.Drawing.Size(65, 13);
            this.queryString_Label.TabIndex = 22;
            this.queryString_Label.Text = "Search Text";
            // 
            // calendarGetEvents_Status
            // 
            this.calendarGetEvents_Status.Location = new System.Drawing.Point(11, 115);
            this.calendarGetEvents_Status.Multiline = true;
            this.calendarGetEvents_Status.Name = "calendarGetEvents_Status";
            this.calendarGetEvents_Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.calendarGetEvents_Status.Size = new System.Drawing.Size(273, 223);
            this.calendarGetEvents_Status.TabIndex = 20;
            // 
            // calendarGetEvents_Button
            // 
            this.calendarGetEvents_Button.Location = new System.Drawing.Point(11, 86);
            this.calendarGetEvents_Button.Name = "calendarGetEvents_Button";
            this.calendarGetEvents_Button.Size = new System.Drawing.Size(273, 23);
            this.calendarGetEvents_Button.TabIndex = 4;
            this.calendarGetEvents_Button.Text = "Get Events";
            this.calendarGetEvents_Button.UseVisualStyleBackColor = true;
            this.calendarGetEvents_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // toDate_Label
            // 
            this.toDate_Label.AutoSize = true;
            this.toDate_Label.Location = new System.Drawing.Point(8, 29);
            this.toDate_Label.Name = "toDate_Label";
            this.toDate_Label.Size = new System.Drawing.Size(46, 13);
            this.toDate_Label.TabIndex = 3;
            this.toDate_Label.Text = "To Date";
            // 
            // fromDate_Label
            // 
            this.fromDate_Label.AutoSize = true;
            this.fromDate_Label.Location = new System.Drawing.Point(8, 3);
            this.fromDate_Label.Name = "fromDate_Label";
            this.fromDate_Label.Size = new System.Drawing.Size(53, 13);
            this.fromDate_Label.TabIndex = 2;
            this.fromDate_Label.Text = "FromDate";
            // 
            // calendarGetEvents_endDate
            // 
            this.calendarGetEvents_endDate.Location = new System.Drawing.Point(84, 29);
            this.calendarGetEvents_endDate.Name = "calendarGetEvents_endDate";
            this.calendarGetEvents_endDate.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_endDate.TabIndex = 1;
            // 
            // calendarGetEvents_startDate
            // 
            this.calendarGetEvents_startDate.Location = new System.Drawing.Point(84, 3);
            this.calendarGetEvents_startDate.Name = "calendarGetEvents_startDate";
            this.calendarGetEvents_startDate.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_startDate.TabIndex = 0;
            this.calendarGetEvents_startDate.Value = new System.DateTime(2012, 2, 3, 11, 12, 0, 0);
            // 
            // UploadWebpagesTab
            // 
            this.UploadWebpagesTab.Controls.Add(this.UW_webpagesDirectory);
            this.UploadWebpagesTab.Controls.Add(this.UW_Status);
            this.UploadWebpagesTab.Controls.Add(this.UW_RootPath);
            this.UploadWebpagesTab.Controls.Add(this.UploadToSites);
            this.UploadWebpagesTab.Controls.Add(this.UW_SiteName);
            this.UploadWebpagesTab.Location = new System.Drawing.Point(4, 22);
            this.UploadWebpagesTab.Name = "UploadWebpagesTab";
            this.UploadWebpagesTab.Size = new System.Drawing.Size(869, 341);
            this.UploadWebpagesTab.TabIndex = 4;
            this.UploadWebpagesTab.Text = "Upload Webpages";
            this.UploadWebpagesTab.UseVisualStyleBackColor = true;
            // 
            // UW_webpagesDirectory
            // 
            this.UW_webpagesDirectory.Location = new System.Drawing.Point(51, 68);
            this.UW_webpagesDirectory.Name = "UW_webpagesDirectory";
            this.UW_webpagesDirectory.Size = new System.Drawing.Size(338, 20);
            this.UW_webpagesDirectory.TabIndex = 4;
            // 
            // UW_Status
            // 
            this.UW_Status.Location = new System.Drawing.Point(63, 113);
            this.UW_Status.Multiline = true;
            this.UW_Status.Name = "UW_Status";
            this.UW_Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.UW_Status.Size = new System.Drawing.Size(778, 199);
            this.UW_Status.TabIndex = 3;
            // 
            // UW_RootPath
            // 
            this.UW_RootPath.Location = new System.Drawing.Point(51, 42);
            this.UW_RootPath.Name = "UW_RootPath";
            this.UW_RootPath.Size = new System.Drawing.Size(338, 20);
            this.UW_RootPath.TabIndex = 2;
            this.UW_RootPath.Text = "/pairs_test";
            // 
            // UploadToSites
            // 
            this.UploadToSites.Location = new System.Drawing.Point(445, 21);
            this.UploadToSites.Name = "UploadToSites";
            this.UploadToSites.Size = new System.Drawing.Size(147, 41);
            this.UploadToSites.TabIndex = 1;
            this.UploadToSites.Text = "Upload to Site";
            this.UploadToSites.UseVisualStyleBackColor = true;
            this.UploadToSites.Click += new System.EventHandler(this.UploadToSites_Click);
            // 
            // UW_SiteName
            // 
            this.UW_SiteName.Location = new System.Drawing.Point(51, 13);
            this.UW_SiteName.Name = "UW_SiteName";
            this.UW_SiteName.Size = new System.Drawing.Size(338, 20);
            this.UW_SiteName.TabIndex = 0;
            this.UW_SiteName.Text = "srirambridgetest";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(869, 341);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(36, 22);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(827, 195);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // LoadDatabaseFileDialog
            // 
            this.LoadDatabaseFileDialog.Title = "Select a database to load";
            // 
            // PairsScorerAllTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 384);
            this.Controls.Add(this.ControlTabs);
            this.Name = "PairsScorerAllTabs";
            this.Text = "Pairs Results Submission";
            this.ControlTabs.ResumeLayout(false);
            this.LoadSummaryTab.ResumeLayout(false);
            this.LoadSummaryTab.PerformLayout();
            this.CreateDatabaseTab.ResumeLayout(false);
            this.CreateDatabaseTab.PerformLayout();
            this.LoadDatabaseTab.ResumeLayout(false);
            this.LoadDatabaseTab.PerformLayout();
            this.CreateWebpagesTab.ResumeLayout(false);
            this.CreateWebpagesTab.PerformLayout();
            this.FindCalendarEventTab.ResumeLayout(false);
            this.FindCalendarEventTab.PerformLayout();
            this.UploadWebpagesTab.ResumeLayout(false);
            this.UploadWebpagesTab.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Summary;
        private System.Windows.Forms.Label Summary_Label;
        private System.Windows.Forms.OpenFileDialog SelectSummaryFileDialog;
        private System.Windows.Forms.Button SelectSummaryFileButton;
        private System.Windows.Forms.TabControl ControlTabs;
        private System.Windows.Forms.TabPage LoadSummaryTab;
        private System.Windows.Forms.TabPage LoadDatabaseTab;
        private System.Windows.Forms.TabPage CreateWebpagesTab;
        private System.Windows.Forms.TabPage FindCalendarEventTab;
        private System.Windows.Forms.TabPage UploadWebpagesTab;
        private System.Windows.Forms.Button LoadSummaryButton;
        private System.Windows.Forms.TabPage CreateDatabaseTab;
        private System.Windows.Forms.Button ChangeDatabaseButton;
        private System.Windows.Forms.Label DatabaseFileNameLabel;
        private System.Windows.Forms.TextBox CD_DatabaseFileName;
        private System.Windows.Forms.Button CreateDatabaseButton;
        private System.Windows.Forms.Button LoadDatabaseButton;
        private System.Windows.Forms.Button SelectDatabaseFileButton;
        private System.Windows.Forms.Label LoadDatabaseLabel;
        private System.Windows.Forms.TextBox LoadDatabaseFileName;
        private System.Windows.Forms.FolderBrowserDialog DatabaseChangeDialog;
        private System.Windows.Forms.OpenFileDialog LoadDatabaseFileDialog;
        private System.Windows.Forms.Button CW_CreateButton;
        private System.Windows.Forms.Button CW_ChangeButton;
        private System.Windows.Forms.Label CW_RootFolderLabel;
        private System.Windows.Forms.TextBox CW_RootFolder;
        private System.Windows.Forms.TextBox UW_RootPath;
        private System.Windows.Forms.Button UploadToSites;
        private System.Windows.Forms.TextBox UW_SiteName;
        private System.Windows.Forms.TextBox UW_Status;
        private System.Windows.Forms.TextBox UW_webpagesDirectory;
        private System.Windows.Forms.TextBox CD_Status;
        private System.Windows.Forms.Label CD_StatusLabel;
        private System.Windows.Forms.Label CW_StatusLabel;
        private System.Windows.Forms.TextBox CW_Status;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_startDate;
        private System.Windows.Forms.Label fromDate_Label;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_endDate;
        private System.Windows.Forms.Label toDate_Label;
        private System.Windows.Forms.TextBox calendarGetEvents_Status;
        private System.Windows.Forms.Button calendarGetEvents_Button;
        private System.Windows.Forms.TextBox calendarGetEvents_SearchTextbox;
        private System.Windows.Forms.Label queryString_Label;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

