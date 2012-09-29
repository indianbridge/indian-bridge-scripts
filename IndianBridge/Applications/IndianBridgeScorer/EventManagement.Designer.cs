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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.eventsDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tourneyInfoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.saveTourneyInfoButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.addNewPDEventButton = new System.Windows.Forms.Button();
            this.addNewTeamEventButton = new System.Windows.Forms.Button();
            this.addNewPairEventButton = new System.Windows.Forms.Button();
            this.addKnockoutButton = new System.Windows.Forms.Button();
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.eventsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.eventsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
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
            this.tableLayoutPanel2.Controls.Add(this.addKnockoutButton, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.addNewPDEventButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.addNewTeamEventButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.addNewPairEventButton, 1, 0);
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
            this.addNewPDEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPDEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPDEventButton.Location = new System.Drawing.Point(551, 3);
            this.addNewPDEventButton.Name = "addNewPDEventButton";
            this.addNewPDEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPDEventButton.TabIndex = 11;
            this.addNewPDEventButton.Text = "Add New PD Event";
            this.addNewPDEventButton.UseVisualStyleBackColor = false;
            this.addNewPDEventButton.Click += new System.EventHandler(this.addNewPDEventButton_Click);
            // 
            // addNewTeamEventButton
            // 
            this.addNewTeamEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewTeamEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewTeamEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewTeamEventButton.Location = new System.Drawing.Point(3, 3);
            this.addNewTeamEventButton.Name = "addNewTeamEventButton";
            this.addNewTeamEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewTeamEventButton.TabIndex = 4;
            this.addNewTeamEventButton.Text = "Add New Team Event";
            this.addNewTeamEventButton.UseVisualStyleBackColor = false;
            this.addNewTeamEventButton.Click += new System.EventHandler(this.addNewTeamEventButton_Click);
            // 
            // addNewPairEventButton
            // 
            this.addNewPairEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewPairEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPairEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPairEventButton.Location = new System.Drawing.Point(277, 3);
            this.addNewPairEventButton.Name = "addNewPairEventButton";
            this.addNewPairEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPairEventButton.TabIndex = 10;
            this.addNewPairEventButton.Text = "Add New Pair Event";
            this.addNewPairEventButton.UseVisualStyleBackColor = false;
            this.addNewPairEventButton.Click += new System.EventHandler(this.addNewPairEventButton_Click);
            // 
            // addKnockoutButton
            // 
            this.addKnockoutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addKnockoutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addKnockoutButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addKnockoutButton.Location = new System.Drawing.Point(3, 53);
            this.addKnockoutButton.Name = "addKnockoutButton";
            this.addKnockoutButton.Size = new System.Drawing.Size(257, 43);
            this.addKnockoutButton.TabIndex = 12;
            this.addKnockoutButton.Text = "Add New Knockout Event";
            this.addKnockoutButton.UseVisualStyleBackColor = false;
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
        private System.Windows.Forms.Button addNewTeamEventButton;
        private System.Windows.Forms.Button addNewPairEventButton;
        private System.Windows.Forms.PropertyGrid tourneyInfoPropertyGrid;
        private System.Windows.Forms.Button addKnockoutButton;


    }
}