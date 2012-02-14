namespace IndianBridge.Applications
{
    partial class WinterNationals2011PairsScorer
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
            this.LoadSummaryButton = new System.Windows.Forms.Button();
            this.Summary = new System.Windows.Forms.TextBox();
            this.SelectSummaryFileButton = new System.Windows.Forms.Button();
            this.Summary_Label = new System.Windows.Forms.Label();
            this.SelectSummaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.GoogleSitePageLabel = new System.Windows.Forms.Label();
            this.PairsEventName = new System.Windows.Forms.ComboBox();
            this.GooglePage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgressReport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Steps = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadSummaryButton
            // 
            this.LoadSummaryButton.Location = new System.Drawing.Point(15, 204);
            this.LoadSummaryButton.Name = "LoadSummaryButton";
            this.LoadSummaryButton.Size = new System.Drawing.Size(835, 43);
            this.LoadSummaryButton.TabIndex = 14;
            this.LoadSummaryButton.Text = "Run";
            this.LoadSummaryButton.UseVisualStyleBackColor = true;
            this.LoadSummaryButton.Click += new System.EventHandler(this.LoadSummaryButton_Click);
            // 
            // Summary
            // 
            this.Summary.Location = new System.Drawing.Point(15, 25);
            this.Summary.Multiline = true;
            this.Summary.Name = "Summary";
            this.Summary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Summary.Size = new System.Drawing.Size(717, 114);
            this.Summary.TabIndex = 11;
            this.Summary.TextChanged += new System.EventHandler(this.Summary_TextChanged);
            // 
            // SelectSummaryFileButton
            // 
            this.SelectSummaryFileButton.AutoSize = true;
            this.SelectSummaryFileButton.Location = new System.Drawing.Point(738, 25);
            this.SelectSummaryFileButton.Name = "SelectSummaryFileButton";
            this.SelectSummaryFileButton.Size = new System.Drawing.Size(112, 23);
            this.SelectSummaryFileButton.TabIndex = 13;
            this.SelectSummaryFileButton.Text = "Select Summary File";
            this.SelectSummaryFileButton.UseVisualStyleBackColor = true;
            this.SelectSummaryFileButton.Click += new System.EventHandler(this.SelectSummaryFileButton_Click);
            // 
            // Summary_Label
            // 
            this.Summary_Label.AutoSize = true;
            this.Summary_Label.Location = new System.Drawing.Point(12, 9);
            this.Summary_Label.Name = "Summary_Label";
            this.Summary_Label.Size = new System.Drawing.Size(50, 13);
            this.Summary_Label.TabIndex = 12;
            this.Summary_Label.Text = "Summary";
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.FileName = "*.txt";
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // GoogleSitePageLabel
            // 
            this.GoogleSitePageLabel.AutoSize = true;
            this.GoogleSitePageLabel.Location = new System.Drawing.Point(12, 145);
            this.GoogleSitePageLabel.Name = "GoogleSitePageLabel";
            this.GoogleSitePageLabel.Size = new System.Drawing.Size(68, 13);
            this.GoogleSitePageLabel.TabIndex = 16;
            this.GoogleSitePageLabel.Text = "Select Event";
            // 
            // PairsEventName
            // 
            this.PairsEventName.FormattingEnabled = true;
            this.PairsEventName.Location = new System.Drawing.Point(15, 161);
            this.PairsEventName.Name = "PairsEventName";
            this.PairsEventName.Size = new System.Drawing.Size(250, 21);
            this.PairsEventName.TabIndex = 17;
            this.PairsEventName.SelectedIndexChanged += new System.EventHandler(this.PairsEventName_SelectedIndexChanged);
            // 
            // GooglePage
            // 
            this.GooglePage.Location = new System.Drawing.Point(271, 161);
            this.GooglePage.Multiline = true;
            this.GooglePage.Name = "GooglePage";
            this.GooglePage.ReadOnly = true;
            this.GooglePage.Size = new System.Drawing.Size(579, 37);
            this.GooglePage.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Results will be uploaded to";
            // 
            // ProgressReport
            // 
            this.ProgressReport.Location = new System.Drawing.Point(15, 341);
            this.ProgressReport.Multiline = true;
            this.ProgressReport.Name = "ProgressReport";
            this.ProgressReport.ReadOnly = true;
            this.ProgressReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ProgressReport.Size = new System.Drawing.Size(834, 250);
            this.ProgressReport.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Progress Messages";
            // 
            // Steps
            // 
            this.Steps.FormattingEnabled = true;
            this.Steps.Items.AddRange(new object[] {
            "Load Summary Into Database",
            "Create Local Webpages from Database",
            "Upload Local Webpages to Google Sites"});
            this.Steps.Location = new System.Drawing.Point(15, 272);
            this.Steps.Name = "Steps";
            this.Steps.Size = new System.Drawing.Size(260, 49);
            this.Steps.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Progress Steps";
            // 
            // WinterNationals2011PairsScorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 603);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Steps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProgressReport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GooglePage);
            this.Controls.Add(this.PairsEventName);
            this.Controls.Add(this.GoogleSitePageLabel);
            this.Controls.Add(this.LoadSummaryButton);
            this.Controls.Add(this.Summary);
            this.Controls.Add(this.SelectSummaryFileButton);
            this.Controls.Add(this.Summary_Label);
            this.Name = "WinterNationals2011PairsScorer";
            this.Text = "Winter Nationals 2011 Pairs Results Creator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadSummaryButton;
        private System.Windows.Forms.TextBox Summary;
        private System.Windows.Forms.Button SelectSummaryFileButton;
        private System.Windows.Forms.Label Summary_Label;
        private System.Windows.Forms.OpenFileDialog SelectSummaryFileDialog;
        private System.Windows.Forms.Label GoogleSitePageLabel;
        private System.Windows.Forms.ComboBox PairsEventName;
        private System.Windows.Forms.TextBox GooglePage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ProgressReport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox Steps;
        private System.Windows.Forms.Label label3;
    }
}

