namespace IndianBridge.Applications
{
    partial class EventInformationDisplay
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
            this.EventName = new System.Windows.Forms.TextBox();
            this.EventNameLabel = new System.Windows.Forms.Label();
            this.EventDateLabel = new System.Windows.Forms.Label();
            this.EventDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ACBLSummaryLabel = new System.Windows.Forms.Label();
            this.ACBLSummary = new System.Windows.Forms.TextBox();
            this.ScoringType = new System.Windows.Forms.TextBox();
            this.NumberOfDirections = new System.Windows.Forms.TextBox();
            this.ScoringTypeLabel = new System.Windows.Forms.Label();
            this.NumberOfDirectionsLabel = new System.Windows.Forms.Label();
            this.UpdateEventInformationButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DatabaseFileName = new System.Windows.Forms.TextBox();
            this.DatabaseFileNameLabel = new System.Windows.Forms.Label();
            this.SelectDatabaseFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ChangeDatabaseButton = new System.Windows.Forms.Button();
            this.WebpagesDirectoryChangeButton = new System.Windows.Forms.Button();
            this.WebpagesDirectoryLabel = new System.Windows.Forms.Label();
            this.WebpagesDirectory = new System.Windows.Forms.TextBox();
            this.SelectWebpagesDirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // EventName
            // 
            this.EventName.Location = new System.Drawing.Point(128, 16);
            this.EventName.Name = "EventName";
            this.EventName.Size = new System.Drawing.Size(515, 20);
            this.EventName.TabIndex = 0;
            // 
            // EventNameLabel
            // 
            this.EventNameLabel.AutoSize = true;
            this.EventNameLabel.Location = new System.Drawing.Point(12, 16);
            this.EventNameLabel.Name = "EventNameLabel";
            this.EventNameLabel.Size = new System.Drawing.Size(75, 13);
            this.EventNameLabel.TabIndex = 1;
            this.EventNameLabel.Text = "Event Name : ";
            // 
            // EventDateLabel
            // 
            this.EventDateLabel.AutoSize = true;
            this.EventDateLabel.Location = new System.Drawing.Point(12, 43);
            this.EventDateLabel.Name = "EventDateLabel";
            this.EventDateLabel.Size = new System.Drawing.Size(70, 13);
            this.EventDateLabel.TabIndex = 2;
            this.EventDateLabel.Text = "Event Date : ";
            // 
            // EventDatePicker
            // 
            this.EventDatePicker.Location = new System.Drawing.Point(128, 43);
            this.EventDatePicker.Name = "EventDatePicker";
            this.EventDatePicker.Size = new System.Drawing.Size(200, 20);
            this.EventDatePicker.TabIndex = 3;
            // 
            // ACBLSummaryLabel
            // 
            this.ACBLSummaryLabel.AutoSize = true;
            this.ACBLSummaryLabel.Location = new System.Drawing.Point(12, 205);
            this.ACBLSummaryLabel.Name = "ACBLSummaryLabel";
            this.ACBLSummaryLabel.Size = new System.Drawing.Size(92, 13);
            this.ACBLSummaryLabel.TabIndex = 4;
            this.ACBLSummaryLabel.Text = "ACBL Summary ? ";
            // 
            // ACBLSummary
            // 
            this.ACBLSummary.Location = new System.Drawing.Point(128, 205);
            this.ACBLSummary.Name = "ACBLSummary";
            this.ACBLSummary.ReadOnly = true;
            this.ACBLSummary.Size = new System.Drawing.Size(70, 20);
            this.ACBLSummary.TabIndex = 5;
            // 
            // ScoringType
            // 
            this.ScoringType.Location = new System.Drawing.Point(128, 232);
            this.ScoringType.Name = "ScoringType";
            this.ScoringType.ReadOnly = true;
            this.ScoringType.Size = new System.Drawing.Size(70, 20);
            this.ScoringType.TabIndex = 6;
            // 
            // NumberOfDirections
            // 
            this.NumberOfDirections.Location = new System.Drawing.Point(128, 259);
            this.NumberOfDirections.Name = "NumberOfDirections";
            this.NumberOfDirections.ReadOnly = true;
            this.NumberOfDirections.Size = new System.Drawing.Size(70, 20);
            this.NumberOfDirections.TabIndex = 7;
            // 
            // ScoringTypeLabel
            // 
            this.ScoringTypeLabel.AutoSize = true;
            this.ScoringTypeLabel.Location = new System.Drawing.Point(12, 232);
            this.ScoringTypeLabel.Name = "ScoringTypeLabel";
            this.ScoringTypeLabel.Size = new System.Drawing.Size(52, 13);
            this.ScoringTypeLabel.TabIndex = 8;
            this.ScoringTypeLabel.Text = "Scoring : ";
            // 
            // NumberOfDirectionsLabel
            // 
            this.NumberOfDirectionsLabel.AutoSize = true;
            this.NumberOfDirectionsLabel.Location = new System.Drawing.Point(12, 259);
            this.NumberOfDirectionsLabel.Name = "NumberOfDirectionsLabel";
            this.NumberOfDirectionsLabel.Size = new System.Drawing.Size(117, 13);
            this.NumberOfDirectionsLabel.TabIndex = 9;
            this.NumberOfDirectionsLabel.Text = "Number Of Directions : ";
            // 
            // UpdateEventInformationButton
            // 
            this.UpdateEventInformationButton.Location = new System.Drawing.Point(15, 295);
            this.UpdateEventInformationButton.Name = "UpdateEventInformationButton";
            this.UpdateEventInformationButton.Size = new System.Drawing.Size(628, 31);
            this.UpdateEventInformationButton.TabIndex = 10;
            this.UpdateEventInformationButton.Text = "Update/Accept Event Information";
            this.UpdateEventInformationButton.UseVisualStyleBackColor = true;
            this.UpdateEventInformationButton.Click += new System.EventHandler(this.UpdateEventInformationButton_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(207, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 67);
            this.label1.TabIndex = 11;
            this.label1.Text = "Information to the left was automatically detected from the summary text and cann" +
                "ot be changed by the user.";
            // 
            // DatabaseFileName
            // 
            this.DatabaseFileName.Location = new System.Drawing.Point(128, 69);
            this.DatabaseFileName.Multiline = true;
            this.DatabaseFileName.Name = "DatabaseFileName";
            this.DatabaseFileName.Size = new System.Drawing.Size(418, 61);
            this.DatabaseFileName.TabIndex = 12;
            // 
            // DatabaseFileNameLabel
            // 
            this.DatabaseFileNameLabel.AutoSize = true;
            this.DatabaseFileNameLabel.Location = new System.Drawing.Point(12, 69);
            this.DatabaseFileNameLabel.Name = "DatabaseFileNameLabel";
            this.DatabaseFileNameLabel.Size = new System.Drawing.Size(112, 13);
            this.DatabaseFileNameLabel.TabIndex = 13;
            this.DatabaseFileNameLabel.Text = "Database File Name : ";
            // 
            // ChangeDatabaseButton
            // 
            this.ChangeDatabaseButton.AutoSize = true;
            this.ChangeDatabaseButton.Location = new System.Drawing.Point(552, 69);
            this.ChangeDatabaseButton.Name = "ChangeDatabaseButton";
            this.ChangeDatabaseButton.Size = new System.Drawing.Size(83, 28);
            this.ChangeDatabaseButton.TabIndex = 14;
            this.ChangeDatabaseButton.Text = "Change";
            this.ChangeDatabaseButton.UseVisualStyleBackColor = true;
            this.ChangeDatabaseButton.Click += new System.EventHandler(this.ChangeDatabaseButton_Click);
            // 
            // WebpagesDirectoryChangeButton
            // 
            this.WebpagesDirectoryChangeButton.AutoSize = true;
            this.WebpagesDirectoryChangeButton.Location = new System.Drawing.Point(552, 136);
            this.WebpagesDirectoryChangeButton.Name = "WebpagesDirectoryChangeButton";
            this.WebpagesDirectoryChangeButton.Size = new System.Drawing.Size(83, 28);
            this.WebpagesDirectoryChangeButton.TabIndex = 17;
            this.WebpagesDirectoryChangeButton.Text = "Change";
            this.WebpagesDirectoryChangeButton.UseVisualStyleBackColor = true;
            this.WebpagesDirectoryChangeButton.Click += new System.EventHandler(this.WebpagesDirectoryChangeButton_Click);
            // 
            // WebpagesDirectoryLabel
            // 
            this.WebpagesDirectoryLabel.AutoSize = true;
            this.WebpagesDirectoryLabel.Location = new System.Drawing.Point(12, 136);
            this.WebpagesDirectoryLabel.Name = "WebpagesDirectoryLabel";
            this.WebpagesDirectoryLabel.Size = new System.Drawing.Size(115, 13);
            this.WebpagesDirectoryLabel.TabIndex = 16;
            this.WebpagesDirectoryLabel.Text = "Webpages Directiory : ";
            // 
            // WebpagesDirectory
            // 
            this.WebpagesDirectory.Location = new System.Drawing.Point(128, 136);
            this.WebpagesDirectory.Multiline = true;
            this.WebpagesDirectory.Name = "WebpagesDirectory";
            this.WebpagesDirectory.Size = new System.Drawing.Size(418, 61);
            this.WebpagesDirectory.TabIndex = 15;
            // 
            // EventInformationDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 334);
            this.Controls.Add(this.WebpagesDirectoryChangeButton);
            this.Controls.Add(this.WebpagesDirectoryLabel);
            this.Controls.Add(this.WebpagesDirectory);
            this.Controls.Add(this.ChangeDatabaseButton);
            this.Controls.Add(this.DatabaseFileNameLabel);
            this.Controls.Add(this.DatabaseFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateEventInformationButton);
            this.Controls.Add(this.NumberOfDirectionsLabel);
            this.Controls.Add(this.ScoringTypeLabel);
            this.Controls.Add(this.NumberOfDirections);
            this.Controls.Add(this.ScoringType);
            this.Controls.Add(this.ACBLSummary);
            this.Controls.Add(this.ACBLSummaryLabel);
            this.Controls.Add(this.EventDatePicker);
            this.Controls.Add(this.EventDateLabel);
            this.Controls.Add(this.EventNameLabel);
            this.Controls.Add(this.EventName);
            this.Name = "EventInformationDisplay";
            this.Text = "Event Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EventName;
        private System.Windows.Forms.Label EventNameLabel;
        private System.Windows.Forms.Label EventDateLabel;
        private System.Windows.Forms.DateTimePicker EventDatePicker;
        private System.Windows.Forms.Label ACBLSummaryLabel;
        private System.Windows.Forms.TextBox ACBLSummary;
        private System.Windows.Forms.TextBox ScoringType;
        private System.Windows.Forms.TextBox NumberOfDirections;
        private System.Windows.Forms.Label ScoringTypeLabel;
        private System.Windows.Forms.Label NumberOfDirectionsLabel;
        private System.Windows.Forms.Button UpdateEventInformationButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DatabaseFileName;
        private System.Windows.Forms.Label DatabaseFileNameLabel;
        private System.Windows.Forms.SaveFileDialog SelectDatabaseFileDialog;
        private System.Windows.Forms.Button ChangeDatabaseButton;
        private System.Windows.Forms.Button WebpagesDirectoryChangeButton;
        private System.Windows.Forms.Label WebpagesDirectoryLabel;
        private System.Windows.Forms.TextBox WebpagesDirectory;
        private System.Windows.Forms.FolderBrowserDialog SelectWebpagesDirectoryDialog;
    }
}