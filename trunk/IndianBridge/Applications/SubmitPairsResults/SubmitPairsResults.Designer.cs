namespace SubmitPairsResults
{
    partial class SubmitPairsResults
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
            this.loadResults = new System.Windows.Forms.Button();
            this.selectCalendarEvent = new System.Windows.Forms.Button();
            this.publishResults = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selectedResultsFile_textBox = new System.Windows.Forms.TextBox();
            this.eventInfo_textBox = new System.Windows.Forms.TextBox();
            this.selectedCalendarEvent_textBox = new System.Windows.Forms.TextBox();
            this.websiteAddress_textBox = new System.Windows.Forms.TextBox();
            this.SelectSummaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // loadResults
            // 
            this.loadResults.BackColor = System.Drawing.Color.Red;
            this.loadResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadResults.ForeColor = System.Drawing.Color.White;
            this.loadResults.Location = new System.Drawing.Point(12, 12);
            this.loadResults.Name = "loadResults";
            this.loadResults.Size = new System.Drawing.Size(149, 47);
            this.loadResults.TabIndex = 0;
            this.loadResults.Text = "Load Results File";
            this.loadResults.UseVisualStyleBackColor = false;
            this.loadResults.Click += new System.EventHandler(this.loadResults_Click);
            // 
            // selectCalendarEvent
            // 
            this.selectCalendarEvent.BackColor = System.Drawing.Color.Red;
            this.selectCalendarEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectCalendarEvent.ForeColor = System.Drawing.Color.White;
            this.selectCalendarEvent.Location = new System.Drawing.Point(12, 65);
            this.selectCalendarEvent.Name = "selectCalendarEvent";
            this.selectCalendarEvent.Size = new System.Drawing.Size(149, 47);
            this.selectCalendarEvent.TabIndex = 1;
            this.selectCalendarEvent.Text = "Select Event from Calendar";
            this.selectCalendarEvent.UseVisualStyleBackColor = false;
            this.selectCalendarEvent.Click += new System.EventHandler(this.selectCalendarEvent_Click);
            // 
            // publishResults
            // 
            this.publishResults.BackColor = System.Drawing.Color.Red;
            this.publishResults.Enabled = false;
            this.publishResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publishResults.ForeColor = System.Drawing.Color.White;
            this.publishResults.Location = new System.Drawing.Point(12, 118);
            this.publishResults.Name = "publishResults";
            this.publishResults.Size = new System.Drawing.Size(149, 47);
            this.publishResults.TabIndex = 2;
            this.publishResults.Text = "Publish Results Online";
            this.publishResults.UseVisualStyleBackColor = false;
            this.publishResults.Click += new System.EventHandler(this.publishResults_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected Results File : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Event Info from Results file :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Selected Calendar Event : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Website Address :";
            // 
            // selectedResultsFile_textBox
            // 
            this.selectedResultsFile_textBox.Location = new System.Drawing.Point(312, 9);
            this.selectedResultsFile_textBox.Name = "selectedResultsFile_textBox";
            this.selectedResultsFile_textBox.ReadOnly = true;
            this.selectedResultsFile_textBox.Size = new System.Drawing.Size(417, 20);
            this.selectedResultsFile_textBox.TabIndex = 7;
            // 
            // eventInfo_textBox
            // 
            this.eventInfo_textBox.Location = new System.Drawing.Point(312, 35);
            this.eventInfo_textBox.Multiline = true;
            this.eventInfo_textBox.Name = "eventInfo_textBox";
            this.eventInfo_textBox.ReadOnly = true;
            this.eventInfo_textBox.Size = new System.Drawing.Size(417, 38);
            this.eventInfo_textBox.TabIndex = 8;
            // 
            // selectedCalendarEvent_textBox
            // 
            this.selectedCalendarEvent_textBox.Location = new System.Drawing.Point(312, 79);
            this.selectedCalendarEvent_textBox.Multiline = true;
            this.selectedCalendarEvent_textBox.Name = "selectedCalendarEvent_textBox";
            this.selectedCalendarEvent_textBox.ReadOnly = true;
            this.selectedCalendarEvent_textBox.Size = new System.Drawing.Size(417, 47);
            this.selectedCalendarEvent_textBox.TabIndex = 9;
            // 
            // websiteAddress_textBox
            // 
            this.websiteAddress_textBox.Location = new System.Drawing.Point(312, 132);
            this.websiteAddress_textBox.Name = "websiteAddress_textBox";
            this.websiteAddress_textBox.ReadOnly = true;
            this.websiteAddress_textBox.Size = new System.Drawing.Size(417, 20);
            this.websiteAddress_textBox.TabIndex = 10;
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // SubmitPairsResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 183);
            this.Controls.Add(this.websiteAddress_textBox);
            this.Controls.Add(this.selectedCalendarEvent_textBox);
            this.Controls.Add(this.eventInfo_textBox);
            this.Controls.Add(this.selectedResultsFile_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.publishResults);
            this.Controls.Add(this.selectCalendarEvent);
            this.Controls.Add(this.loadResults);
            this.Name = "SubmitPairsResults";
            this.Text = "SubmitPairsResults";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadResults;
        private System.Windows.Forms.Button selectCalendarEvent;
        private System.Windows.Forms.Button publishResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox selectedResultsFile_textBox;
        private System.Windows.Forms.TextBox eventInfo_textBox;
        private System.Windows.Forms.TextBox selectedCalendarEvent_textBox;
        private System.Windows.Forms.TextBox websiteAddress_textBox;
        private System.Windows.Forms.OpenFileDialog SelectSummaryFileDialog;
    }
}