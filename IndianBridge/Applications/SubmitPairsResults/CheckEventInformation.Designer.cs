namespace SubmitPairsResults
{
    partial class checkEventInformation
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
            this.calendarDate_Label = new System.Windows.Forms.Label();
            this.eventDateFromCalendar = new System.Windows.Forms.DateTimePicker();
            this.eventDateFromResultsFile_Label = new System.Windows.Forms.Label();
            this.eventDateFromResultsFile = new System.Windows.Forms.DateTimePicker();
            this.eventNameFromCalendar = new System.Windows.Forms.TextBox();
            this.eventNameFromCalendar_Label = new System.Windows.Forms.Label();
            this.eventNameFromResultsFile = new System.Windows.Forms.TextBox();
            this.eventNameFromResultsFile_Label = new System.Windows.Forms.Label();
            this.eventInformationConsistent = new System.Windows.Forms.Button();
            this.eventInfoInconsistent = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.eventDateGroupBox = new System.Windows.Forms.GroupBox();
            this.eventNameGroupBox = new System.Windows.Forms.GroupBox();
            this.eventDateGroupBox.SuspendLayout();
            this.eventNameGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // calendarDate_Label
            // 
            this.calendarDate_Label.AutoSize = true;
            this.calendarDate_Label.Location = new System.Drawing.Point(6, 16);
            this.calendarDate_Label.Name = "calendarDate_Label";
            this.calendarDate_Label.Size = new System.Drawing.Size(129, 13);
            this.calendarDate_Label.TabIndex = 28;
            this.calendarDate_Label.Text = "Event Date from Calendar";
            // 
            // eventDateFromCalendar
            // 
            this.eventDateFromCalendar.CalendarForeColor = System.Drawing.SystemColors.WindowText;
            this.eventDateFromCalendar.CustomFormat = "MMMM dd, yyyy";
            this.eventDateFromCalendar.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.eventDateFromCalendar.Location = new System.Drawing.Point(6, 32);
            this.eventDateFromCalendar.Name = "eventDateFromCalendar";
            this.eventDateFromCalendar.Size = new System.Drawing.Size(200, 20);
            this.eventDateFromCalendar.TabIndex = 27;
            this.eventDateFromCalendar.Value = new System.DateTime(2012, 8, 22, 0, 0, 0, 0);
            // 
            // eventDateFromResultsFile_Label
            // 
            this.eventDateFromResultsFile_Label.AutoSize = true;
            this.eventDateFromResultsFile_Label.Location = new System.Drawing.Point(209, 16);
            this.eventDateFromResultsFile_Label.Name = "eventDateFromResultsFile_Label";
            this.eventDateFromResultsFile_Label.Size = new System.Drawing.Size(141, 13);
            this.eventDateFromResultsFile_Label.TabIndex = 30;
            this.eventDateFromResultsFile_Label.Text = "Event Date from Results File";
            // 
            // eventDateFromResultsFile
            // 
            this.eventDateFromResultsFile.CalendarForeColor = System.Drawing.SystemColors.WindowText;
            this.eventDateFromResultsFile.CustomFormat = "MMMM dd, yyyy";
            this.eventDateFromResultsFile.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.eventDateFromResultsFile.Location = new System.Drawing.Point(212, 32);
            this.eventDateFromResultsFile.Name = "eventDateFromResultsFile";
            this.eventDateFromResultsFile.Size = new System.Drawing.Size(200, 20);
            this.eventDateFromResultsFile.TabIndex = 29;
            this.eventDateFromResultsFile.Value = new System.DateTime(2012, 8, 22, 0, 0, 0, 0);
            // 
            // eventNameFromCalendar
            // 
            this.eventNameFromCalendar.Location = new System.Drawing.Point(6, 31);
            this.eventNameFromCalendar.Multiline = true;
            this.eventNameFromCalendar.Name = "eventNameFromCalendar";
            this.eventNameFromCalendar.Size = new System.Drawing.Size(200, 56);
            this.eventNameFromCalendar.TabIndex = 32;
            // 
            // eventNameFromCalendar_Label
            // 
            this.eventNameFromCalendar_Label.AutoSize = true;
            this.eventNameFromCalendar_Label.Location = new System.Drawing.Point(3, 15);
            this.eventNameFromCalendar_Label.Name = "eventNameFromCalendar_Label";
            this.eventNameFromCalendar_Label.Size = new System.Drawing.Size(137, 13);
            this.eventNameFromCalendar_Label.TabIndex = 31;
            this.eventNameFromCalendar_Label.Text = "Event Name From Calendar";
            // 
            // eventNameFromResultsFile
            // 
            this.eventNameFromResultsFile.Location = new System.Drawing.Point(212, 31);
            this.eventNameFromResultsFile.Multiline = true;
            this.eventNameFromResultsFile.Name = "eventNameFromResultsFile";
            this.eventNameFromResultsFile.Size = new System.Drawing.Size(200, 56);
            this.eventNameFromResultsFile.TabIndex = 34;
            // 
            // eventNameFromResultsFile_Label
            // 
            this.eventNameFromResultsFile_Label.AutoSize = true;
            this.eventNameFromResultsFile_Label.Location = new System.Drawing.Point(209, 15);
            this.eventNameFromResultsFile_Label.Name = "eventNameFromResultsFile_Label";
            this.eventNameFromResultsFile_Label.Size = new System.Drawing.Size(149, 13);
            this.eventNameFromResultsFile_Label.TabIndex = 33;
            this.eventNameFromResultsFile_Label.Text = "Event Name From Results File";
            // 
            // eventInformationConsistent
            // 
            this.eventInformationConsistent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eventInformationConsistent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventInformationConsistent.ForeColor = System.Drawing.Color.White;
            this.eventInformationConsistent.Location = new System.Drawing.Point(7, 224);
            this.eventInformationConsistent.Name = "eventInformationConsistent";
            this.eventInformationConsistent.Size = new System.Drawing.Size(200, 42);
            this.eventInformationConsistent.TabIndex = 35;
            this.eventInformationConsistent.Text = "Event Info is Correct. Proceed";
            this.eventInformationConsistent.UseVisualStyleBackColor = false;
            this.eventInformationConsistent.Click += new System.EventHandler(this.eventInformationConsistent_Click);
            // 
            // eventInfoInconsistent
            // 
            this.eventInfoInconsistent.BackColor = System.Drawing.Color.Red;
            this.eventInfoInconsistent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventInfoInconsistent.ForeColor = System.Drawing.Color.White;
            this.eventInfoInconsistent.Location = new System.Drawing.Point(232, 224);
            this.eventInfoInconsistent.Name = "eventInfoInconsistent";
            this.eventInfoInconsistent.Size = new System.Drawing.Size(200, 42);
            this.eventInfoInconsistent.TabIndex = 36;
            this.eventInfoInconsistent.Text = "Event Info is not correct. Cancel";
            this.eventInfoInconsistent.UseVisualStyleBackColor = false;
            this.eventInfoInconsistent.Click += new System.EventHandler(this.eventInfoInconsistent_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(420, 38);
            this.textBox1.TabIndex = 37;
            this.textBox1.Text = "Please Check that you have selected the correct calendar event and the correct re" +
    "sults file.";
            // 
            // eventDateGroupBox
            // 
            this.eventDateGroupBox.BackColor = System.Drawing.Color.Red;
            this.eventDateGroupBox.Controls.Add(this.eventDateFromResultsFile_Label);
            this.eventDateGroupBox.Controls.Add(this.eventDateFromResultsFile);
            this.eventDateGroupBox.Controls.Add(this.calendarDate_Label);
            this.eventDateGroupBox.Controls.Add(this.eventDateFromCalendar);
            this.eventDateGroupBox.Location = new System.Drawing.Point(12, 156);
            this.eventDateGroupBox.Name = "eventDateGroupBox";
            this.eventDateGroupBox.Size = new System.Drawing.Size(420, 62);
            this.eventDateGroupBox.TabIndex = 38;
            this.eventDateGroupBox.TabStop = false;
            this.eventDateGroupBox.Text = "Event Date Check";
            // 
            // eventNameGroupBox
            // 
            this.eventNameGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eventNameGroupBox.Controls.Add(this.eventNameFromResultsFile);
            this.eventNameGroupBox.Controls.Add(this.eventNameFromResultsFile_Label);
            this.eventNameGroupBox.Controls.Add(this.eventNameFromCalendar);
            this.eventNameGroupBox.Controls.Add(this.eventNameFromCalendar_Label);
            this.eventNameGroupBox.Location = new System.Drawing.Point(12, 57);
            this.eventNameGroupBox.Name = "eventNameGroupBox";
            this.eventNameGroupBox.Size = new System.Drawing.Size(420, 93);
            this.eventNameGroupBox.TabIndex = 39;
            this.eventNameGroupBox.TabStop = false;
            this.eventNameGroupBox.Text = "Event Name Check";
            // 
            // checkEventInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 278);
            this.Controls.Add(this.eventNameGroupBox);
            this.Controls.Add(this.eventDateGroupBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.eventInfoInconsistent);
            this.Controls.Add(this.eventInformationConsistent);
            this.Name = "checkEventInformation";
            this.Text = "CheckEventInformation";
            this.eventDateGroupBox.ResumeLayout(false);
            this.eventDateGroupBox.PerformLayout();
            this.eventNameGroupBox.ResumeLayout(false);
            this.eventNameGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label calendarDate_Label;
        private System.Windows.Forms.DateTimePicker eventDateFromCalendar;
        private System.Windows.Forms.Label eventDateFromResultsFile_Label;
        private System.Windows.Forms.DateTimePicker eventDateFromResultsFile;
        private System.Windows.Forms.TextBox eventNameFromCalendar;
        private System.Windows.Forms.Label eventNameFromCalendar_Label;
        private System.Windows.Forms.TextBox eventNameFromResultsFile;
        private System.Windows.Forms.Label eventNameFromResultsFile_Label;
        private System.Windows.Forms.Button eventInformationConsistent;
        private System.Windows.Forms.Button eventInfoInconsistent;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox eventDateGroupBox;
        private System.Windows.Forms.GroupBox eventNameGroupBox;
    }
}