﻿namespace IndianBridgeScorer
{
    partial class PairsScorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PairsScorer));
            this.websiteAddress_textBox = new System.Windows.Forms.TextBox();
            this.eventInfo_textBox = new System.Windows.Forms.TextBox();
            this.selectedResultsFile_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.publishResults = new System.Windows.Forms.Button();
            this.loadResults = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectSummaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.operationStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.operationCancelButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // websiteAddress_textBox
            // 
            this.websiteAddress_textBox.Location = new System.Drawing.Point(312, 134);
            this.websiteAddress_textBox.Name = "websiteAddress_textBox";
            this.websiteAddress_textBox.Size = new System.Drawing.Size(417, 20);
            this.websiteAddress_textBox.TabIndex = 17;
            // 
            // eventInfo_textBox
            // 
            this.eventInfo_textBox.Location = new System.Drawing.Point(312, 37);
            this.eventInfo_textBox.Multiline = true;
            this.eventInfo_textBox.Name = "eventInfo_textBox";
            this.eventInfo_textBox.ReadOnly = true;
            this.eventInfo_textBox.Size = new System.Drawing.Size(417, 38);
            this.eventInfo_textBox.TabIndex = 16;
            // 
            // selectedResultsFile_textBox
            // 
            this.selectedResultsFile_textBox.Location = new System.Drawing.Point(312, 11);
            this.selectedResultsFile_textBox.Name = "selectedResultsFile_textBox";
            this.selectedResultsFile_textBox.ReadOnly = true;
            this.selectedResultsFile_textBox.Size = new System.Drawing.Size(417, 20);
            this.selectedResultsFile_textBox.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Event Info from Results file :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Selected Results File : ";
            // 
            // publishResults
            // 
            this.publishResults.BackColor = System.Drawing.Color.Red;
            this.publishResults.Enabled = false;
            this.publishResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publishResults.ForeColor = System.Drawing.Color.White;
            this.publishResults.Location = new System.Drawing.Point(12, 120);
            this.publishResults.Name = "publishResults";
            this.publishResults.Size = new System.Drawing.Size(149, 47);
            this.publishResults.TabIndex = 12;
            this.publishResults.Text = "Publish Results Online";
            this.publishResults.UseVisualStyleBackColor = false;
            this.publishResults.Click += new System.EventHandler(this.publishResults_Click);
            // 
            // loadResults
            // 
            this.loadResults.BackColor = System.Drawing.Color.Red;
            this.loadResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadResults.ForeColor = System.Drawing.Color.White;
            this.loadResults.Location = new System.Drawing.Point(12, 14);
            this.loadResults.Name = "loadResults";
            this.loadResults.Size = new System.Drawing.Size(149, 47);
            this.loadResults.TabIndex = 11;
            this.loadResults.Text = "Load Results File";
            this.loadResults.UseVisualStyleBackColor = false;
            this.loadResults.Click += new System.EventHandler(this.loadResults_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Website Address :";
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationStatus,
            this.operationProgressBar,
            this.operationCancelButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 185);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(791, 23);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.publishResults);
            this.panel1.Controls.Add(this.loadResults);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.websiteAddress_textBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.eventInfo_textBox);
            this.panel1.Controls.Add(this.selectedResultsFile_textBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 185);
            this.panel1.TabIndex = 20;
            // 
            // operationStatus
            // 
            this.operationStatus.Name = "operationStatus";
            this.operationStatus.Size = new System.Drawing.Size(104, 18);
            this.operationStatus.Text = "Operation Status : ";
            // 
            // operationProgressBar
            // 
            this.operationProgressBar.Name = "operationProgressBar";
            this.operationProgressBar.Size = new System.Drawing.Size(100, 17);
            // 
            // operationCancelButton
            // 
            this.operationCancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.operationCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("operationCancelButton.Image")));
            this.operationCancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.operationCancelButton.Name = "operationCancelButton";
            this.operationCancelButton.Size = new System.Drawing.Size(103, 21);
            this.operationCancelButton.Text = "Cancel Operation";
            // 
            // PairsScorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 208);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "PairsScorer";
            this.Text = "PairsScorer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox websiteAddress_textBox;
        private System.Windows.Forms.TextBox eventInfo_textBox;
        private System.Windows.Forms.TextBox selectedResultsFile_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button publishResults;
        private System.Windows.Forms.Button loadResults;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog SelectSummaryFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel operationStatus;
        private System.Windows.Forms.ToolStripProgressBar operationProgressBar;
        private System.Windows.Forms.ToolStripButton operationCancelButton;
    }
}