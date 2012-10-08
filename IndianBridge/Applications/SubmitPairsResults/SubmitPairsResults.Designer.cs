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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitPairsResults));
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.publishStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.publishProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.publishCancelButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.statusStrip3 = new System.Windows.Forms.StatusStrip();
            this.loadSummaryStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.loadSummaryProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.loadSummaryCancelButton = new System.Windows.Forms.ToolStripButton();
            this.createWebpagesStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.createWebpagesProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.createWebpagesCancelButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.statusStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadResults
            // 
            this.loadResults.BackColor = System.Drawing.Color.Red;
            this.loadResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadResults.ForeColor = System.Drawing.Color.White;
            this.loadResults.Location = new System.Drawing.Point(6, 8);
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
            this.selectCalendarEvent.Location = new System.Drawing.Point(6, 61);
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
            this.publishResults.Location = new System.Drawing.Point(6, 114);
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
            this.label1.Location = new System.Drawing.Point(185, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected Results File : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Event Info from Results file :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(166, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Selected Calendar Event : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Website Address :";
            // 
            // selectedResultsFile_textBox
            // 
            this.selectedResultsFile_textBox.Location = new System.Drawing.Point(306, 5);
            this.selectedResultsFile_textBox.Name = "selectedResultsFile_textBox";
            this.selectedResultsFile_textBox.ReadOnly = true;
            this.selectedResultsFile_textBox.Size = new System.Drawing.Size(417, 20);
            this.selectedResultsFile_textBox.TabIndex = 7;
            // 
            // eventInfo_textBox
            // 
            this.eventInfo_textBox.Location = new System.Drawing.Point(306, 31);
            this.eventInfo_textBox.Multiline = true;
            this.eventInfo_textBox.Name = "eventInfo_textBox";
            this.eventInfo_textBox.ReadOnly = true;
            this.eventInfo_textBox.Size = new System.Drawing.Size(417, 38);
            this.eventInfo_textBox.TabIndex = 8;
            // 
            // selectedCalendarEvent_textBox
            // 
            this.selectedCalendarEvent_textBox.Location = new System.Drawing.Point(306, 75);
            this.selectedCalendarEvent_textBox.Multiline = true;
            this.selectedCalendarEvent_textBox.Name = "selectedCalendarEvent_textBox";
            this.selectedCalendarEvent_textBox.ReadOnly = true;
            this.selectedCalendarEvent_textBox.Size = new System.Drawing.Size(417, 47);
            this.selectedCalendarEvent_textBox.TabIndex = 9;
            // 
            // websiteAddress_textBox
            // 
            this.websiteAddress_textBox.Location = new System.Drawing.Point(306, 128);
            this.websiteAddress_textBox.Name = "websiteAddress_textBox";
            this.websiteAddress_textBox.Size = new System.Drawing.Size(417, 20);
            this.websiteAddress_textBox.TabIndex = 10;
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.publishStatus,
            this.publishProgressBar,
            this.publishCancelButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 211);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(738, 23);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.loadResults);
            this.panel1.Controls.Add(this.websiteAddress_textBox);
            this.panel1.Controls.Add(this.selectCalendarEvent);
            this.panel1.Controls.Add(this.selectedCalendarEvent_textBox);
            this.panel1.Controls.Add(this.publishResults);
            this.panel1.Controls.Add(this.eventInfo_textBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.selectedResultsFile_textBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(738, 188);
            this.panel1.TabIndex = 12;
            // 
            // publishStatus
            // 
            this.publishStatus.Name = "publishStatus";
            this.publishStatus.Size = new System.Drawing.Size(87, 18);
            this.publishStatus.Text = "Publish Status :";
            // 
            // publishProgressBar
            // 
            this.publishProgressBar.Name = "publishProgressBar";
            this.publishProgressBar.Size = new System.Drawing.Size(50, 17);
            // 
            // publishCancelButton
            // 
            this.publishCancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.publishCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("publishCancelButton.Image")));
            this.publishCancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.publishCancelButton.Name = "publishCancelButton";
            this.publishCancelButton.Size = new System.Drawing.Size(89, 21);
            this.publishCancelButton.Text = "Cancel Publish";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createWebpagesStatus,
            this.createWebpagesProgressBar,
            this.createWebpagesCancelButton});
            this.statusStrip2.Location = new System.Drawing.Point(0, 188);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(738, 23);
            this.statusStrip2.TabIndex = 13;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // statusStrip3
            // 
            this.statusStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSummaryStatus,
            this.loadSummaryProgressBar,
            this.loadSummaryCancelButton});
            this.statusStrip3.Location = new System.Drawing.Point(0, 165);
            this.statusStrip3.Name = "statusStrip3";
            this.statusStrip3.Size = new System.Drawing.Size(738, 23);
            this.statusStrip3.TabIndex = 11;
            this.statusStrip3.Text = "statusStrip3";
            // 
            // loadSummaryStatus
            // 
            this.loadSummaryStatus.Name = "loadSummaryStatus";
            this.loadSummaryStatus.Size = new System.Drawing.Size(128, 18);
            this.loadSummaryStatus.Text = "Load Summary Status :";
            this.loadSummaryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // loadSummaryProgressBar
            // 
            this.loadSummaryProgressBar.Name = "loadSummaryProgressBar";
            this.loadSummaryProgressBar.Size = new System.Drawing.Size(50, 17);
            // 
            // loadSummaryCancelButton
            // 
            this.loadSummaryCancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadSummaryCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("loadSummaryCancelButton.Image")));
            this.loadSummaryCancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadSummaryCancelButton.Name = "loadSummaryCancelButton";
            this.loadSummaryCancelButton.Size = new System.Drawing.Size(130, 21);
            this.loadSummaryCancelButton.Text = "Cancel Load Summary";
            // 
            // createWebpagesStatus
            // 
            this.createWebpagesStatus.Name = "createWebpagesStatus";
            this.createWebpagesStatus.Size = new System.Drawing.Size(140, 18);
            this.createWebpagesStatus.Text = "Create Webpages Status :";
            // 
            // createWebpagesProgressBar
            // 
            this.createWebpagesProgressBar.Name = "createWebpagesProgressBar";
            this.createWebpagesProgressBar.Size = new System.Drawing.Size(50, 17);
            // 
            // createWebpagesCancelButton
            // 
            this.createWebpagesCancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createWebpagesCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("createWebpagesCancelButton.Image")));
            this.createWebpagesCancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createWebpagesCancelButton.Name = "createWebpagesCancelButton";
            this.createWebpagesCancelButton.Size = new System.Drawing.Size(84, 21);
            this.createWebpagesCancelButton.Text = "Cancel Create";
            // 
            // SubmitPairsResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 234);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Name = "SubmitPairsResults";
            this.Text = "SubmitPairsResults";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.statusStrip3.ResumeLayout(false);
            this.statusStrip3.PerformLayout();
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel publishStatus;
        private System.Windows.Forms.ToolStripProgressBar publishProgressBar;
        private System.Windows.Forms.ToolStripButton publishCancelButton;
        private System.Windows.Forms.StatusStrip statusStrip3;
        private System.Windows.Forms.ToolStripStatusLabel loadSummaryStatus;
        private System.Windows.Forms.ToolStripProgressBar loadSummaryProgressBar;
        private System.Windows.Forms.ToolStripButton loadSummaryCancelButton;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel createWebpagesStatus;
        private System.Windows.Forms.ToolStripProgressBar createWebpagesProgressBar;
        private System.Windows.Forms.ToolStripButton createWebpagesCancelButton;
    }
}