namespace IndianBridgeScorer
{
    partial class EventManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.eventsDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tourneyInfoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.saveTourneyInfoButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.addNewPDEventButton = new System.Windows.Forms.Button();
            this.addNewSwissTeamEventButton = new System.Windows.Forms.Button();
            this.addNewPairEventButton = new System.Windows.Forms.Button();
            this.generateFollowOnKnockout = new System.Windows.Forms.Button();
            this.addNewKnockoutButton = new System.Windows.Forms.Button();
            this.generateFollowOnSwissButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eventsDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventsDataGridView
            // 
            this.eventsDataGridView.AllowUserToAddRows = false;
            this.eventsDataGridView.AllowUserToDeleteRows = false;
            this.eventsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.eventsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.eventsDataGridView.DefaultCellStyle = dataGridViewCellStyle8;
            this.eventsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsDataGridView.Location = new System.Drawing.Point(3, 16);
            this.eventsDataGridView.Name = "eventsDataGridView";
            this.eventsDataGridView.ReadOnly = true;
            this.eventsDataGridView.Size = new System.Drawing.Size(823, 268);
            this.eventsDataGridView.TabIndex = 6;
            this.eventsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.eventsDataGridView_CellContentClick);
            this.eventsDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.eventsDataGridView_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tourneyInfoPropertyGrid);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(829, 141);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tourney Info";
            // 
            // tourneyInfoPropertyGrid
            // 
            this.tourneyInfoPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tourneyInfoPropertyGrid.HelpVisible = false;
            this.tourneyInfoPropertyGrid.Location = new System.Drawing.Point(3, 16);
            this.tourneyInfoPropertyGrid.Name = "tourneyInfoPropertyGrid";
            this.tourneyInfoPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.tourneyInfoPropertyGrid.Size = new System.Drawing.Size(823, 72);
            this.tourneyInfoPropertyGrid.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.saveTourneyInfoButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(823, 50);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // saveTourneyInfoButton
            // 
            this.saveTourneyInfoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.saveTourneyInfoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveTourneyInfoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTourneyInfoButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.saveTourneyInfoButton.Location = new System.Drawing.Point(3, 3);
            this.saveTourneyInfoButton.Name = "saveTourneyInfoButton";
            this.saveTourneyInfoButton.Size = new System.Drawing.Size(405, 44);
            this.saveTourneyInfoButton.TabIndex = 6;
            this.saveTourneyInfoButton.Text = "Save Tourney Info to Database";
            this.saveTourneyInfoButton.UseVisualStyleBackColor = false;
            this.saveTourneyInfoButton.Click += new System.EventHandler(this.saveTourneyInfoButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cancelButton.Location = new System.Drawing.Point(414, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(406, 44);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Reload Tourney Info from Database";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.eventsDataGridView);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(829, 387);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tourney Events";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.generateFollowOnSwissButton, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.addNewSwissTeamEventButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.generateFollowOnKnockout, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.addNewPDEventButton, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.addNewPairEventButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.addNewKnockoutButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 284);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(823, 100);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // addNewPDEventButton
            // 
            this.addNewPDEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewPDEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPDEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPDEventButton.Location = new System.Drawing.Point(551, 53);
            this.addNewPDEventButton.Name = "addNewPDEventButton";
            this.addNewPDEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPDEventButton.TabIndex = 11;
            this.addNewPDEventButton.Text = "Add New PD Event";
            this.addNewPDEventButton.UseVisualStyleBackColor = false;
            this.addNewPDEventButton.Click += new System.EventHandler(this.addNewPDEventButton_Click);
            // 
            // addNewSwissTeamEventButton
            // 
            this.addNewSwissTeamEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewSwissTeamEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewSwissTeamEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewSwissTeamEventButton.Location = new System.Drawing.Point(3, 3);
            this.addNewSwissTeamEventButton.Name = "addNewSwissTeamEventButton";
            this.addNewSwissTeamEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewSwissTeamEventButton.TabIndex = 4;
            this.addNewSwissTeamEventButton.Text = "Add New Swiss Teams";
            this.addNewSwissTeamEventButton.UseVisualStyleBackColor = false;
            this.addNewSwissTeamEventButton.Click += new System.EventHandler(this.addNewTeamEventButton_Click);
            // 
            // addNewPairEventButton
            // 
            this.addNewPairEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewPairEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPairEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPairEventButton.Location = new System.Drawing.Point(551, 3);
            this.addNewPairEventButton.Name = "addNewPairEventButton";
            this.addNewPairEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPairEventButton.TabIndex = 10;
            this.addNewPairEventButton.Text = "Add New Pair Event";
            this.addNewPairEventButton.UseVisualStyleBackColor = false;
            this.addNewPairEventButton.Click += new System.EventHandler(this.addNewPairEventButton_Click);
            // 
            // generateFollowOnKnockout
            // 
            this.generateFollowOnKnockout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.generateFollowOnKnockout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateFollowOnKnockout.ForeColor = System.Drawing.SystemColors.WindowText;
            this.generateFollowOnKnockout.Location = new System.Drawing.Point(277, 53);
            this.generateFollowOnKnockout.Name = "generateFollowOnKnockout";
            this.generateFollowOnKnockout.Size = new System.Drawing.Size(257, 43);
            this.generateFollowOnKnockout.TabIndex = 13;
            this.generateFollowOnKnockout.Text = "Generate Follow On Knockout Event";
            this.generateFollowOnKnockout.UseVisualStyleBackColor = false;
            this.generateFollowOnKnockout.Click += new System.EventHandler(this.generateFollowOnKnockout_Click);
            // 
            // addNewKnockoutButton
            // 
            this.addNewKnockoutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewKnockoutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewKnockoutButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewKnockoutButton.Location = new System.Drawing.Point(277, 3);
            this.addNewKnockoutButton.Name = "addNewKnockoutButton";
            this.addNewKnockoutButton.Size = new System.Drawing.Size(257, 43);
            this.addNewKnockoutButton.TabIndex = 14;
            this.addNewKnockoutButton.Text = "Add New Knockout Event";
            this.addNewKnockoutButton.UseVisualStyleBackColor = false;
            this.addNewKnockoutButton.Click += new System.EventHandler(this.addNewKnockoutButton_Click);
            // 
            // generateFollowOnSwissButton
            // 
            this.generateFollowOnSwissButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.generateFollowOnSwissButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateFollowOnSwissButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.generateFollowOnSwissButton.Location = new System.Drawing.Point(3, 53);
            this.generateFollowOnSwissButton.Name = "generateFollowOnSwissButton";
            this.generateFollowOnSwissButton.Size = new System.Drawing.Size(257, 43);
            this.generateFollowOnSwissButton.TabIndex = 15;
            this.generateFollowOnSwissButton.Text = "Generate Follow On Swiss Teams";
            this.generateFollowOnSwissButton.UseVisualStyleBackColor = false;
            this.generateFollowOnSwissButton.Click += new System.EventHandler(this.generateFollowOnSwissButton_Click);
            // 
            // EventManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(829, 528);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "EventManagement";
            this.Text = "Tourney Name : ";
            ((System.ComponentModel.ISupportInitialize)(this.eventsDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView eventsDataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button saveTourneyInfoButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button addNewPDEventButton;
        private System.Windows.Forms.Button addNewSwissTeamEventButton;
        private System.Windows.Forms.Button addNewPairEventButton;
        private System.Windows.Forms.PropertyGrid tourneyInfoPropertyGrid;
        private System.Windows.Forms.Button generateFollowOnSwissButton;
        private System.Windows.Forms.Button generateFollowOnKnockout;
        private System.Windows.Forms.Button addNewKnockoutButton;


    }
}