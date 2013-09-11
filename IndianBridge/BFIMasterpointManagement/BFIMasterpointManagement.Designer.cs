namespace BFIMasterpointManagement
{
    partial class BFIMasterpointManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BFIMasterpointManagement));
            this.tournamentTypesTextbox = new System.Windows.Forms.TextBox();
            this.addUsersButton = new System.Windows.Forms.Button();
            this.addMasterpointsButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripUsername = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tournamentLevelMaster = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ttmDataGridView = new System.Windows.Forms.DataGridView();
            this.addTournamentLevelButton = new System.Windows.Forms.Button();
            this.tournamentMaster = new System.Windows.Forms.TabPage();
            this.eventMaster = new System.Windows.Forms.TabPage();
            this.addUsers = new System.Windows.Forms.TabPage();
            this.addMasterpoints = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tmDataGridView = new System.Windows.Forms.DataGridView();
            this.addTournamentButton = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.emDataGridView = new System.Windows.Forms.DataGridView();
            this.addEventButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ttmLoadingPicture = new System.Windows.Forms.PictureBox();
            this.toolStripLoginButton = new System.Windows.Forms.ToolStripSplitButton();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tournamentLevelMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ttmDataGridView)).BeginInit();
            this.tournamentMaster.SuspendLayout();
            this.eventMaster.SuspendLayout();
            this.addUsers.SuspendLayout();
            this.addMasterpoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tmDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.emDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ttmLoadingPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // tournamentTypesTextbox
            // 
            this.tournamentTypesTextbox.Location = new System.Drawing.Point(52, 25);
            this.tournamentTypesTextbox.Multiline = true;
            this.tournamentTypesTextbox.Name = "tournamentTypesTextbox";
            this.tournamentTypesTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tournamentTypesTextbox.Size = new System.Drawing.Size(465, 238);
            this.tournamentTypesTextbox.TabIndex = 1;
            // 
            // addUsersButton
            // 
            this.addUsersButton.Location = new System.Drawing.Point(56, 89);
            this.addUsersButton.Name = "addUsersButton";
            this.addUsersButton.Size = new System.Drawing.Size(203, 49);
            this.addUsersButton.TabIndex = 5;
            this.addUsersButton.Text = "Add Users";
            this.addUsersButton.UseVisualStyleBackColor = true;
            this.addUsersButton.Click += new System.EventHandler(this.addUsersButton_Click);
            // 
            // addMasterpointsButton
            // 
            this.addMasterpointsButton.Location = new System.Drawing.Point(128, 363);
            this.addMasterpointsButton.Name = "addMasterpointsButton";
            this.addMasterpointsButton.Size = new System.Drawing.Size(203, 49);
            this.addMasterpointsButton.TabIndex = 6;
            this.addMasterpointsButton.Text = "Add Masterpoints";
            this.addMasterpointsButton.UseVisualStyleBackColor = true;
            this.addMasterpointsButton.Click += new System.EventHandler(this.addMasterpointsButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripUsername,
            this.toolStripStatusLabel1,
            this.toolStripLoginButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 498);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(891, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripUsername
            // 
            this.toolStripUsername.Name = "toolStripUsername";
            this.toolStripUsername.Size = new System.Drawing.Size(135, 17);
            this.toolStripUsername.Text = "Logged in as : username";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tournamentLevelMaster);
            this.tabControl1.Controls.Add(this.tournamentMaster);
            this.tabControl1.Controls.Add(this.eventMaster);
            this.tabControl1.Controls.Add(this.addUsers);
            this.tabControl1.Controls.Add(this.addMasterpoints);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(891, 498);
            this.tabControl1.TabIndex = 8;
            // 
            // tournamentLevelMaster
            // 
            this.tournamentLevelMaster.Controls.Add(this.splitContainer1);
            this.tournamentLevelMaster.Location = new System.Drawing.Point(4, 22);
            this.tournamentLevelMaster.Name = "tournamentLevelMaster";
            this.tournamentLevelMaster.Size = new System.Drawing.Size(883, 472);
            this.tournamentLevelMaster.TabIndex = 2;
            this.tournamentLevelMaster.Text = "Tournament Level Master";
            this.tournamentLevelMaster.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ttmDataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.ttmLoadingPicture);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.addTournamentLevelButton);
            this.splitContainer1.Size = new System.Drawing.Size(883, 472);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 9;
            // 
            // ttmDataGridView
            // 
            this.ttmDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ttmDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ttmDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ttmDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ttmDataGridView.Name = "ttmDataGridView";
            this.ttmDataGridView.ReadOnly = true;
            this.ttmDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ttmDataGridView.ShowCellToolTips = false;
            this.ttmDataGridView.Size = new System.Drawing.Size(883, 401);
            this.ttmDataGridView.TabIndex = 0;
            this.toolTip1.SetToolTip(this.ttmDataGridView, "Double Click on any element to create a new tourney at that level");
            this.ttmDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ttmDataGridView_CellMouseDoubleClick);
            // 
            // addTournamentLevelButton
            // 
            this.addTournamentLevelButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.addTournamentLevelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addTournamentLevelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTournamentLevelButton.Location = new System.Drawing.Point(0, 0);
            this.addTournamentLevelButton.Name = "addTournamentLevelButton";
            this.addTournamentLevelButton.Size = new System.Drawing.Size(883, 67);
            this.addTournamentLevelButton.TabIndex = 0;
            this.addTournamentLevelButton.Text = "Add New Tournament Level";
            this.toolTip1.SetToolTip(this.addTournamentLevelButton, "Testing tooltip");
            this.addTournamentLevelButton.UseVisualStyleBackColor = false;
            this.addTournamentLevelButton.Click += new System.EventHandler(this.addTournamentLevelButton_Click);
            // 
            // tournamentMaster
            // 
            this.tournamentMaster.Controls.Add(this.splitContainer2);
            this.tournamentMaster.Location = new System.Drawing.Point(4, 22);
            this.tournamentMaster.Name = "tournamentMaster";
            this.tournamentMaster.Size = new System.Drawing.Size(883, 472);
            this.tournamentMaster.TabIndex = 3;
            this.tournamentMaster.Text = "Tournament Master";
            this.tournamentMaster.UseVisualStyleBackColor = true;
            // 
            // eventMaster
            // 
            this.eventMaster.Controls.Add(this.splitContainer3);
            this.eventMaster.Location = new System.Drawing.Point(4, 22);
            this.eventMaster.Name = "eventMaster";
            this.eventMaster.Size = new System.Drawing.Size(883, 472);
            this.eventMaster.TabIndex = 4;
            this.eventMaster.Text = "Event Master";
            this.eventMaster.UseVisualStyleBackColor = true;
            // 
            // addUsers
            // 
            this.addUsers.Controls.Add(this.addUsersButton);
            this.addUsers.Location = new System.Drawing.Point(4, 22);
            this.addUsers.Name = "addUsers";
            this.addUsers.Padding = new System.Windows.Forms.Padding(3);
            this.addUsers.Size = new System.Drawing.Size(883, 472);
            this.addUsers.TabIndex = 0;
            this.addUsers.Text = "Add Users";
            this.addUsers.UseVisualStyleBackColor = true;
            // 
            // addMasterpoints
            // 
            this.addMasterpoints.Controls.Add(this.addMasterpointsButton);
            this.addMasterpoints.Controls.Add(this.tournamentTypesTextbox);
            this.addMasterpoints.Location = new System.Drawing.Point(4, 22);
            this.addMasterpoints.Name = "addMasterpoints";
            this.addMasterpoints.Padding = new System.Windows.Forms.Padding(3);
            this.addMasterpoints.Size = new System.Drawing.Size(883, 472);
            this.addMasterpoints.TabIndex = 1;
            this.addMasterpoints.Text = "addMasterpoints";
            this.addMasterpoints.UseVisualStyleBackColor = true;
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
            this.splitContainer2.Panel1.Controls.Add(this.tmDataGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.addTournamentButton);
            this.splitContainer2.Size = new System.Drawing.Size(883, 472);
            this.splitContainer2.SplitterDistance = 401;
            this.splitContainer2.TabIndex = 10;
            // 
            // tmDataGridView
            // 
            this.tmDataGridView.AllowUserToAddRows = false;
            this.tmDataGridView.AllowUserToDeleteRows = false;
            this.tmDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tmDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tmDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmDataGridView.Location = new System.Drawing.Point(0, 0);
            this.tmDataGridView.Name = "tmDataGridView";
            this.tmDataGridView.ReadOnly = true;
            this.tmDataGridView.Size = new System.Drawing.Size(883, 401);
            this.tmDataGridView.TabIndex = 0;
            // 
            // addTournamentButton
            // 
            this.addTournamentButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.addTournamentButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addTournamentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTournamentButton.Location = new System.Drawing.Point(0, 0);
            this.addTournamentButton.Name = "addTournamentButton";
            this.addTournamentButton.Size = new System.Drawing.Size(883, 67);
            this.addTournamentButton.TabIndex = 0;
            this.addTournamentButton.Text = "Add New Tournament";
            this.addTournamentButton.UseVisualStyleBackColor = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.emDataGridView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.addEventButton);
            this.splitContainer3.Size = new System.Drawing.Size(883, 472);
            this.splitContainer3.SplitterDistance = 401;
            this.splitContainer3.TabIndex = 11;
            // 
            // emDataGridView
            // 
            this.emDataGridView.AllowUserToAddRows = false;
            this.emDataGridView.AllowUserToDeleteRows = false;
            this.emDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.emDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.emDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emDataGridView.Location = new System.Drawing.Point(0, 0);
            this.emDataGridView.Name = "emDataGridView";
            this.emDataGridView.ReadOnly = true;
            this.emDataGridView.Size = new System.Drawing.Size(883, 401);
            this.emDataGridView.TabIndex = 0;
            // 
            // addEventButton
            // 
            this.addEventButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.addEventButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEventButton.Location = new System.Drawing.Point(0, 0);
            this.addEventButton.Name = "addEventButton";
            this.addEventButton.Size = new System.Drawing.Size(883, 67);
            this.addEventButton.TabIndex = 0;
            this.addEventButton.Text = "Add New Event";
            this.addEventButton.UseVisualStyleBackColor = false;
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.Red;
            this.toolTip1.ForeColor = System.Drawing.SystemColors.Window;
            this.toolTip1.IsBalloon = true;
            // 
            // ttmLoadingPicture
            // 
            this.ttmLoadingPicture.Image = global::BFIMasterpointManagement.Properties.Resources.loading;
            this.ttmLoadingPicture.Location = new System.Drawing.Point(411, 214);
            this.ttmLoadingPicture.Name = "ttmLoadingPicture";
            this.ttmLoadingPicture.Size = new System.Drawing.Size(67, 51);
            this.ttmLoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ttmLoadingPicture.TabIndex = 8;
            this.ttmLoadingPicture.TabStop = false;
            this.ttmLoadingPicture.Visible = false;
            // 
            // toolStripLoginButton
            // 
            this.toolStripLoginButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLoginButton.DropDownButtonWidth = 0;
            this.toolStripLoginButton.Enabled = false;
            this.toolStripLoginButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLoginButton.Image")));
            this.toolStripLoginButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLoginButton.Name = "toolStripLoginButton";
            this.toolStripLoginButton.Size = new System.Drawing.Size(133, 19);
            this.toolStripLoginButton.Text = "Login As Different User";
            this.toolStripLoginButton.ButtonClick += new System.EventHandler(this.toolStripLoginButton_ButtonClick);
            // 
            // BFIMasterpointManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 520);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "BFIMasterpointManagement";
            this.Text = "BFI Masterpoint Management";
            this.Shown += new System.EventHandler(this.BFIMasterpointManagement_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tournamentLevelMaster.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ttmDataGridView)).EndInit();
            this.tournamentMaster.ResumeLayout(false);
            this.eventMaster.ResumeLayout(false);
            this.addUsers.ResumeLayout(false);
            this.addMasterpoints.ResumeLayout(false);
            this.addMasterpoints.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tmDataGridView)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.emDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ttmLoadingPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tournamentTypesTextbox;
        private System.Windows.Forms.Button addUsersButton;
        private System.Windows.Forms.Button addMasterpointsButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripUsername;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSplitButton toolStripLoginButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tournamentLevelMaster;
        private System.Windows.Forms.TabPage tournamentMaster;
        private System.Windows.Forms.TabPage eventMaster;
        private System.Windows.Forms.TabPage addUsers;
        private System.Windows.Forms.TabPage addMasterpoints;
        private System.Windows.Forms.DataGridView ttmDataGridView;
        private System.Windows.Forms.PictureBox ttmLoadingPicture;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button addTournamentLevelButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView tmDataGridView;
        private System.Windows.Forms.Button addTournamentButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.DataGridView emDataGridView;
        private System.Windows.Forms.Button addEventButton;
    }
}

