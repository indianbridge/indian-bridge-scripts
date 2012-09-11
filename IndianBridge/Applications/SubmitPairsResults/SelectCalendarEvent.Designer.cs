namespace SubmitPairsResults
{
    partial class SelectCalendarEvent
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fromDate_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_SearchTextbox = new System.Windows.Forms.TextBox();
            this.calendarGetEvents_startDate = new System.Windows.Forms.DateTimePicker();
            this.queryString_Label = new System.Windows.Forms.Label();
            this.calendarGetEvents_endDate = new System.Windows.Forms.DateTimePicker();
            this.calendarGetEvents_Button = new System.Windows.Forms.Button();
            this.toDate_Label = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.loadEventsInBackGround = new System.ComponentModel.BackgroundWorker();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            this.groupBox1.TabIndex = 32;
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
            // calendarGetEvents_SearchTextbox
            // 
            this.calendarGetEvents_SearchTextbox.Location = new System.Drawing.Point(82, 82);
            this.calendarGetEvents_SearchTextbox.Name = "calendarGetEvents_SearchTextbox";
            this.calendarGetEvents_SearchTextbox.Size = new System.Drawing.Size(200, 20);
            this.calendarGetEvents_SearchTextbox.TabIndex = 30;
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
            // queryString_Label
            // 
            this.queryString_Label.AutoSize = true;
            this.queryString_Label.Location = new System.Drawing.Point(6, 82);
            this.queryString_Label.Name = "queryString_Label";
            this.queryString_Label.Size = new System.Drawing.Size(65, 13);
            this.queryString_Label.TabIndex = 29;
            this.queryString_Label.Text = "Search Text";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Location = new System.Drawing.Point(9, 162);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(902, 282);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Calendar Events Table";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(883, 258);
            this.dataGridView1.TabIndex = 33;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(312, 94);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(586, 65);
            this.textBox1.TabIndex = 39;
            this.textBox1.Text = "Use the controls on the left to set search criteria.\r\nClick Get Events button to " +
    "list events in the table below (this might take a few minutes).\r\nDouble click on" +
    " any event to select it.";
            // 
            // loadEventsInBackGround
            // 
            this.loadEventsInBackGround.DoWork += new System.ComponentModel.DoWorkEventHandler(this.loadEventsInBackGround_DoWork);
            this.loadEventsInBackGround.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.loadEventsInBackGround_RunWorkerCompleted);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Location = new System.Drawing.Point(312, 12);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(586, 76);
            this.cancel_Button.TabIndex = 40;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // SelectCalendarEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 459);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectCalendarEvent";
            this.Text = "SelectCalendarEvent";
            this.Shown += new System.EventHandler(this.SelectCalendarEvent_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label fromDate_Label;
        private System.Windows.Forms.TextBox calendarGetEvents_SearchTextbox;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_startDate;
        private System.Windows.Forms.Label queryString_Label;
        private System.Windows.Forms.DateTimePicker calendarGetEvents_endDate;
        private System.Windows.Forms.Button calendarGetEvents_Button;
        private System.Windows.Forms.Label toDate_Label;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.ComponentModel.BackgroundWorker loadEventsInBackGround;
        private System.Windows.Forms.Button cancel_Button;
    }
}