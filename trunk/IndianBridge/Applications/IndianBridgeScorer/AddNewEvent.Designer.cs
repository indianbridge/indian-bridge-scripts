namespace IndianBridgeScorer
{
    partial class AddNewEvent
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.eventNameTextbox = new System.Windows.Forms.TextBox();
            this.eventTypeComboBox = new System.Windows.Forms.ComboBox();
            this.addNewEventButton = new System.Windows.Forms.Button();
            this.cancelAddNewEventButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event Name : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Event Type : ";
            // 
            // eventNameTextbox
            // 
            this.eventNameTextbox.Location = new System.Drawing.Point(183, 13);
            this.eventNameTextbox.Name = "eventNameTextbox";
            this.eventNameTextbox.Size = new System.Drawing.Size(244, 20);
            this.eventNameTextbox.TabIndex = 2;
            // 
            // eventTypeComboBox
            // 
            this.eventTypeComboBox.FormattingEnabled = true;
            this.eventTypeComboBox.Items.AddRange(new object[] {
            "Team",
            "Pairs",
            "PD"});
            this.eventTypeComboBox.Location = new System.Drawing.Point(184, 52);
            this.eventTypeComboBox.Name = "eventTypeComboBox";
            this.eventTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.eventTypeComboBox.TabIndex = 3;
            // 
            // addNewEventButton
            // 
            this.addNewEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewEventButton.Location = new System.Drawing.Point(18, 97);
            this.addNewEventButton.Name = "addNewEventButton";
            this.addNewEventButton.Size = new System.Drawing.Size(196, 50);
            this.addNewEventButton.TabIndex = 4;
            this.addNewEventButton.Text = "Add New Event";
            this.addNewEventButton.UseVisualStyleBackColor = false;
            this.addNewEventButton.Click += new System.EventHandler(this.addNewEventButton_Click);
            // 
            // cancelAddNewEventButton
            // 
            this.cancelAddNewEventButton.BackColor = System.Drawing.Color.Red;
            this.cancelAddNewEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelAddNewEventButton.Location = new System.Drawing.Point(220, 97);
            this.cancelAddNewEventButton.Name = "cancelAddNewEventButton";
            this.cancelAddNewEventButton.Size = new System.Drawing.Size(201, 50);
            this.cancelAddNewEventButton.TabIndex = 5;
            this.cancelAddNewEventButton.Text = "CANCEL";
            this.cancelAddNewEventButton.UseVisualStyleBackColor = false;
            this.cancelAddNewEventButton.Click += new System.EventHandler(this.cancelAddNewEventButton_Click);
            // 
            // AddNewEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 159);
            this.Controls.Add(this.cancelAddNewEventButton);
            this.Controls.Add(this.addNewEventButton);
            this.Controls.Add(this.eventTypeComboBox);
            this.Controls.Add(this.eventNameTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNewEvent";
            this.Text = "Add New Event To Tourney : ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox eventNameTextbox;
        private System.Windows.Forms.ComboBox eventTypeComboBox;
        private System.Windows.Forms.Button addNewEventButton;
        private System.Windows.Forms.Button cancelAddNewEventButton;
    }
}