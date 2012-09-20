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
            this.tourneyNameLabel = new System.Windows.Forms.Label();
            this.addNewTeamEventButton = new System.Windows.Forms.Button();
            this.eventsList = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultsWebsiteTextBox = new System.Windows.Forms.TextBox();
            this.tourneyNameTextBox = new System.Windows.Forms.TextBox();
            this.addNewPairEventButton = new System.Windows.Forms.Button();
            this.addNewPDEventButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tourneyNameLabel
            // 
            this.tourneyNameLabel.AutoSize = true;
            this.tourneyNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tourneyNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourneyNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tourneyNameLabel.Location = new System.Drawing.Point(30, 9);
            this.tourneyNameLabel.Name = "tourneyNameLabel";
            this.tourneyNameLabel.Size = new System.Drawing.Size(188, 26);
            this.tourneyNameLabel.TabIndex = 2;
            this.tourneyNameLabel.Text = "Tourney Name : ";
            // 
            // addNewTeamEventButton
            // 
            this.addNewTeamEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewTeamEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewTeamEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewTeamEventButton.Location = new System.Drawing.Point(12, 303);
            this.addNewTeamEventButton.Name = "addNewTeamEventButton";
            this.addNewTeamEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewTeamEventButton.TabIndex = 4;
            this.addNewTeamEventButton.Text = "Add New Team Event";
            this.addNewTeamEventButton.UseVisualStyleBackColor = false;
            this.addNewTeamEventButton.Click += new System.EventHandler(this.addNewTeamEventButton_Click);
            // 
            // eventsList
            // 
            this.eventsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsList.AutoScroll = true;
            this.eventsList.AutoSize = true;
            this.eventsList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.eventsList.ColumnCount = 4;
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventsList.Location = new System.Drawing.Point(6, 30);
            this.eventsList.Name = "eventsList";
            this.eventsList.RowCount = 1;
            this.eventsList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.eventsList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.eventsList.Size = new System.Drawing.Size(771, 171);
            this.eventsList.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.eventsList);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(783, 215);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Events in Tourney";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Results Website : ";
            // 
            // resultsWebsiteTextBox
            // 
            this.resultsWebsiteTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsWebsiteTextBox.Location = new System.Drawing.Point(224, 44);
            this.resultsWebsiteTextBox.Name = "resultsWebsiteTextBox";
            this.resultsWebsiteTextBox.ReadOnly = true;
            this.resultsWebsiteTextBox.Size = new System.Drawing.Size(571, 26);
            this.resultsWebsiteTextBox.TabIndex = 8;
            // 
            // tourneyNameTextBox
            // 
            this.tourneyNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourneyNameTextBox.Location = new System.Drawing.Point(224, 10);
            this.tourneyNameTextBox.Name = "tourneyNameTextBox";
            this.tourneyNameTextBox.ReadOnly = true;
            this.tourneyNameTextBox.Size = new System.Drawing.Size(571, 26);
            this.tourneyNameTextBox.TabIndex = 9;
            // 
            // addNewPairEventButton
            // 
            this.addNewPairEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewPairEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPairEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPairEventButton.Location = new System.Drawing.Point(275, 303);
            this.addNewPairEventButton.Name = "addNewPairEventButton";
            this.addNewPairEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPairEventButton.TabIndex = 10;
            this.addNewPairEventButton.Text = "Add New Pair Event";
            this.addNewPairEventButton.UseVisualStyleBackColor = false;
            this.addNewPairEventButton.Click += new System.EventHandler(this.addNewPairEventButton_Click);
            // 
            // addNewPDEventButton
            // 
            this.addNewPDEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewPDEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewPDEventButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.addNewPDEventButton.Location = new System.Drawing.Point(538, 303);
            this.addNewPDEventButton.Name = "addNewPDEventButton";
            this.addNewPDEventButton.Size = new System.Drawing.Size(257, 43);
            this.addNewPDEventButton.TabIndex = 11;
            this.addNewPDEventButton.Text = "Add New PD Event";
            this.addNewPDEventButton.UseVisualStyleBackColor = false;
            this.addNewPDEventButton.Click += new System.EventHandler(this.addNewPDEventButton_Click);
            // 
            // EventManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(808, 360);
            this.Controls.Add(this.addNewPDEventButton);
            this.Controls.Add(this.addNewPairEventButton);
            this.Controls.Add(this.tourneyNameTextBox);
            this.Controls.Add(this.resultsWebsiteTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.addNewTeamEventButton);
            this.Controls.Add(this.tourneyNameLabel);
            this.Name = "EventManagement";
            this.Text = "Tourney Name : ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tourneyNameLabel;
        private System.Windows.Forms.Button addNewTeamEventButton;
        private System.Windows.Forms.TableLayoutPanel eventsList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox resultsWebsiteTextBox;
        private System.Windows.Forms.TextBox tourneyNameTextBox;
        private System.Windows.Forms.Button addNewPairEventButton;
        private System.Windows.Forms.Button addNewPDEventButton;

    }
}