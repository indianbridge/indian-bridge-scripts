using System;
namespace SubmitPairsResults
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.calendarGetEvents_SearchTextbox = new System.Windows.Forms.TextBox();
            this.queryString_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_Button = new System.Windows.Forms.Button();
            this.toDate_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_endDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fromDate_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_startDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.getCalendarEvents = new System.ComponentModel.BackgroundWorker();
            this.instructions = new System.Windows.Forms.RichTextBox();
            this.SelectSummaryFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.status = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // calendarGetEvents_SearchTextbox
            // 
            this.calendarGetEvents_SearchTextbox.Location = new System.Drawing.Point(82, 82);
            this.calendarGetEvents_SearchTextbox.Name = "calendarGetEvents_SearchTextbox";
            this.calendarGetEvents_SearchTextbox.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_SearchTextbox.TabIndex = 30;
            // 
            // queryString_Label
            // 
            this.queryString_Label.AutoSize = true;
            this.queryString_Label.Location = new System.Drawing.Point(6, 82);
            this.queryString_Label.Name = "queryString_Label";
            this.queryString_Label.Size = new System.Drawing.Size(65, 13);
            this.queryString_Label.TabIndex = 29;
            this.queryString_Label.Text = "Search Text";
            // 
            // calendarGetEvents_Button
            // 
            this.calendarGetEvents_Button.Location = new System.Drawing.Point(9, 109);
            this.calendarGetEvents_Button.Name = "calendarGetEvents_Button";
            this.calendarGetEvents_Button.Size = new System.Drawing.Size(273, 23);
            this.calendarGetEvents_Button.TabIndex = 28;
            this.calendarGetEvents_Button.Text = "Get Events";
            this.calendarGetEvents_Button.UseVisualStyleBackColor = true;
            this.calendarGetEvents_Button.Click += new System.EventHandler(this.calendarGetEvents_Button_Click);
            // 
            // toDate_Label
            // 
            this.toDate_Label.AutoSize = true;
            this.toDate_Label.Location = new System.Drawing.Point(6, 52);
            this.toDate_Label.Name = "toDate_Label";
            this.toDate_Label.Size = new System.Drawing.Size(46, 13);
            this.toDate_Label.TabIndex = 27;
            this.toDate_Label.Text = "To Date";
            // 
            // calendarGetEvents_endDate
            // 
            this.calendarGetEvents_endDate.CustomFormat = "MMMM dd, yyyy";
            this.calendarGetEvents_endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.calendarGetEvents_endDate.Location = new System.Drawing.Point(82, 52);
            this.calendarGetEvents_endDate.Name = "calendarGetEvents_endDate";
            this.calendarGetEvents_endDate.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_endDate.TabIndex = 25;
            this.calendarGetEvents_endDate.Value = new System.DateTime(2012, 8, 29, 0, 0, 0, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fromDate_Label);
            this.groupBox1.Controls.Add(this.calendarGetEvents_SearchTextbox);
            this.groupBox1.Controls.Add(this.calendarGetEvents_startDate);
            this.groupBox1.Controls.Add(this.queryString_Label);
            this.groupBox1.Controls.Add(this.calendarGetEvents_endDate);
            this.groupBox1.Controls.Add(this.calendarGetEvents_Button);
            this.groupBox1.Controls.Add(this.toDate_Label);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 145);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calendar Event Search";
            // 
            // fromDate_Label
            // 
            this.fromDate_Label.AutoSize = true;
            this.fromDate_Label.Location = new System.Drawing.Point(6, 26);
            this.fromDate_Label.Name = "fromDate_Label";
            this.fromDate_Label.Size = new System.Drawing.Size(53, 13);
            this.fromDate_Label.TabIndex = 26;
            this.fromDate_Label.Text = "FromDate";
            // 
            // calendarGetEvents_startDate
            // 
            this.calendarGetEvents_startDate.CustomFormat = "MMMM dd, yyyy";
            this.calendarGetEvents_startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.calendarGetEvents_startDate.Location = new System.Drawing.Point(82, 26);
            this.calendarGetEvents_startDate.Name = "calendarGetEvents_startDate";
            this.calendarGetEvents_startDate.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_startDate.TabIndex = 24;
            this.calendarGetEvents_startDate.Value = new System.DateTime(2012, 8, 22, 0, 0, 0, 0);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(883, 258);
            this.dataGridView1.TabIndex = 33;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // getCalendarEvents
            // 
            this.getCalendarEvents.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getCalendarEvents_DoWork);
            this.getCalendarEvents.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.getCalendarEvents_RunWorkerCompleted_1);
            // 
            // instructions
            // 
            this.instructions.Enabled = false;
            this.instructions.Location = new System.Drawing.Point(6, 19);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(582, 119);
            this.instructions.TabIndex = 34;
            this.instructions.Text = resources.GetString("instructions.Text");
            // 
            // SelectSummaryFileDialog
            // 
            this.SelectSummaryFileDialog.Title = "Select Summary File to Load";
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.SystemColors.Control;
            this.status.Enabled = false;
            this.status.Location = new System.Drawing.Point(6, 19);
            this.status.Multiline = true;
            this.status.Name = "status";
            this.status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.status.Size = new System.Drawing.Size(883, 265);
            this.status.TabIndex = 35;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.instructions);
            this.groupBox2.Location = new System.Drawing.Point(313, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(597, 144);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Instructions";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Location = new System.Drawing.Point(8, 160);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(902, 282);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Calendar Events Table";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.status);
            this.groupBox4.Location = new System.Drawing.Point(8, 448);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(902, 291);
            this.groupBox4.TabIndex = 38;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Status Messages";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 749);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Submit Pairs Results";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox calendarGetEvents_SearchTextbox;
        private System.Windows.Forms.Label queryString_Label;
        private System.Windows.Forms.Button calendarGetEvents_Button;
        private System.Windows.Forms.Label toDate_Label;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_endDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.ComponentModel.BackgroundWorker getCalendarEvents;
        private System.Windows.Forms.RichTextBox instructions;
        private System.Windows.Forms.OpenFileDialog SelectSummaryFileDialog;
        private System.Windows.Forms.Label fromDate_Label;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_startDate;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}

