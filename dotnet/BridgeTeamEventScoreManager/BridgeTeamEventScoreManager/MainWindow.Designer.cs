namespace BridgeTeamEventScoreManager
{
    partial class MainWindow
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
            this.googleClientLoginGroupBox = new System.Windows.Forms.GroupBox();
            this.errorMessageTextBox = new System.Windows.Forms.TextBox();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.authenticationStatusTextBox = new System.Windows.Forms.TextBox();
            this.loginStatusTextBox = new System.Windows.Forms.TextBox();
            this.autheticationStatusLabel = new System.Windows.Forms.Label();
            this.loginStatus = new System.Windows.Forms.Label();
            this.loginLogoutButton = new System.Windows.Forms.Button();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.databaseValidityTextbox = new System.Windows.Forms.TextBox();
            this.databaseValidityLabel = new System.Windows.Forms.Label();
            this.createNewDatabaseButton = new System.Windows.Forms.Button();
            this.changeDatabaseButton = new System.Windows.Forms.Button();
            this.selectedDatabaseTextBox = new System.Windows.Forms.TextBox();
            this.selectedDatabaseLabel = new System.Windows.Forms.Label();
            this.selectDatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.googleClientLoginGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // googleClientLoginGroupBox
            // 
            this.googleClientLoginGroupBox.Controls.Add(this.errorMessageTextBox);
            this.googleClientLoginGroupBox.Controls.Add(this.errorMessageLabel);
            this.googleClientLoginGroupBox.Controls.Add(this.authenticationStatusTextBox);
            this.googleClientLoginGroupBox.Controls.Add(this.loginStatusTextBox);
            this.googleClientLoginGroupBox.Controls.Add(this.autheticationStatusLabel);
            this.googleClientLoginGroupBox.Controls.Add(this.loginStatus);
            this.googleClientLoginGroupBox.Controls.Add(this.loginLogoutButton);
            this.googleClientLoginGroupBox.Location = new System.Drawing.Point(12, 12);
            this.googleClientLoginGroupBox.Name = "googleClientLoginGroupBox";
            this.googleClientLoginGroupBox.Size = new System.Drawing.Size(571, 216);
            this.googleClientLoginGroupBox.TabIndex = 2;
            this.googleClientLoginGroupBox.TabStop = false;
            this.googleClientLoginGroupBox.Text = "Google Client Login Status";
            // 
            // errorMessageTextBox
            // 
            this.errorMessageTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.errorMessageTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.errorMessageTextBox.Location = new System.Drawing.Point(123, 84);
            this.errorMessageTextBox.Multiline = true;
            this.errorMessageTextBox.Name = "errorMessageTextBox";
            this.errorMessageTextBox.ReadOnly = true;
            this.errorMessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorMessageTextBox.Size = new System.Drawing.Size(442, 83);
            this.errorMessageTextBox.TabIndex = 6;
            this.errorMessageTextBox.Text = "None";
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.Location = new System.Drawing.Point(39, 84);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(78, 13);
            this.errorMessageLabel.TabIndex = 5;
            this.errorMessageLabel.Text = "Error Message:";
            // 
            // authenticationStatusTextBox
            // 
            this.authenticationStatusTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.authenticationStatusTextBox.ForeColor = System.Drawing.Color.Red;
            this.authenticationStatusTextBox.Location = new System.Drawing.Point(123, 58);
            this.authenticationStatusTextBox.Name = "authenticationStatusTextBox";
            this.authenticationStatusTextBox.ReadOnly = true;
            this.authenticationStatusTextBox.Size = new System.Drawing.Size(100, 20);
            this.authenticationStatusTextBox.TabIndex = 4;
            this.authenticationStatusTextBox.Text = "Not Authenticated";
            // 
            // loginStatusTextBox
            // 
            this.loginStatusTextBox.ForeColor = System.Drawing.Color.Red;
            this.loginStatusTextBox.Location = new System.Drawing.Point(123, 32);
            this.loginStatusTextBox.Name = "loginStatusTextBox";
            this.loginStatusTextBox.ReadOnly = true;
            this.loginStatusTextBox.Size = new System.Drawing.Size(100, 20);
            this.loginStatusTextBox.TabIndex = 3;
            this.loginStatusTextBox.Text = "Not Logged In";
            // 
            // autheticationStatusLabel
            // 
            this.autheticationStatusLabel.AutoSize = true;
            this.autheticationStatusLabel.Location = new System.Drawing.Point(6, 58);
            this.autheticationStatusLabel.Name = "autheticationStatusLabel";
            this.autheticationStatusLabel.Size = new System.Drawing.Size(111, 13);
            this.autheticationStatusLabel.TabIndex = 2;
            this.autheticationStatusLabel.Text = "Authentication Status:";
            // 
            // loginStatus
            // 
            this.loginStatus.AutoSize = true;
            this.loginStatus.Location = new System.Drawing.Point(48, 35);
            this.loginStatus.Name = "loginStatus";
            this.loginStatus.Size = new System.Drawing.Size(69, 13);
            this.loginStatus.TabIndex = 1;
            this.loginStatus.Text = "Login Status:";
            // 
            // loginLogoutButton
            // 
            this.loginLogoutButton.Location = new System.Drawing.Point(6, 173);
            this.loginLogoutButton.Name = "loginLogoutButton";
            this.loginLogoutButton.Size = new System.Drawing.Size(559, 36);
            this.loginLogoutButton.TabIndex = 0;
            this.loginLogoutButton.Text = "Login to Google Account";
            this.loginLogoutButton.UseVisualStyleBackColor = true;
            this.loginLogoutButton.Click += new System.EventHandler(this.loginLogoutButton_Click);
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.databaseValidityTextbox);
            this.databaseGroupBox.Controls.Add(this.databaseValidityLabel);
            this.databaseGroupBox.Controls.Add(this.createNewDatabaseButton);
            this.databaseGroupBox.Controls.Add(this.changeDatabaseButton);
            this.databaseGroupBox.Controls.Add(this.selectedDatabaseTextBox);
            this.databaseGroupBox.Controls.Add(this.selectedDatabaseLabel);
            this.databaseGroupBox.Location = new System.Drawing.Point(589, 12);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(582, 212);
            this.databaseGroupBox.TabIndex = 3;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "Working Database";
            // 
            // databaseValidityTextbox
            // 
            this.databaseValidityTextbox.ForeColor = System.Drawing.Color.Red;
            this.databaseValidityTextbox.Location = new System.Drawing.Point(110, 84);
            this.databaseValidityTextbox.Name = "databaseValidityTextbox";
            this.databaseValidityTextbox.ReadOnly = true;
            this.databaseValidityTextbox.Size = new System.Drawing.Size(100, 20);
            this.databaseValidityTextbox.TabIndex = 8;
            this.databaseValidityTextbox.Text = "Invalid";
            // 
            // databaseValidityLabel
            // 
            this.databaseValidityLabel.AutoSize = true;
            this.databaseValidityLabel.Location = new System.Drawing.Point(15, 84);
            this.databaseValidityLabel.Name = "databaseValidityLabel";
            this.databaseValidityLabel.Size = new System.Drawing.Size(92, 13);
            this.databaseValidityLabel.TabIndex = 7;
            this.databaseValidityLabel.Text = "Database Validity:";
            // 
            // createNewDatabaseButton
            // 
            this.createNewDatabaseButton.Location = new System.Drawing.Point(459, 55);
            this.createNewDatabaseButton.Name = "createNewDatabaseButton";
            this.createNewDatabaseButton.Size = new System.Drawing.Size(72, 23);
            this.createNewDatabaseButton.TabIndex = 6;
            this.createNewDatabaseButton.Text = "Create New";
            this.createNewDatabaseButton.UseVisualStyleBackColor = true;
            this.createNewDatabaseButton.Click += new System.EventHandler(this.createNewDatabaseButton_Click);
            // 
            // changeDatabaseButton
            // 
            this.changeDatabaseButton.Location = new System.Drawing.Point(459, 27);
            this.changeDatabaseButton.Name = "changeDatabaseButton";
            this.changeDatabaseButton.Size = new System.Drawing.Size(72, 23);
            this.changeDatabaseButton.TabIndex = 5;
            this.changeDatabaseButton.Text = "Change";
            this.changeDatabaseButton.UseVisualStyleBackColor = true;
            this.changeDatabaseButton.Click += new System.EventHandler(this.changeDatabaseButton_Click);
            // 
            // selectedDatabaseTextBox
            // 
            this.selectedDatabaseTextBox.ForeColor = System.Drawing.Color.Red;
            this.selectedDatabaseTextBox.Location = new System.Drawing.Point(110, 28);
            this.selectedDatabaseTextBox.Multiline = true;
            this.selectedDatabaseTextBox.Name = "selectedDatabaseTextBox";
            this.selectedDatabaseTextBox.ReadOnly = true;
            this.selectedDatabaseTextBox.Size = new System.Drawing.Size(343, 43);
            this.selectedDatabaseTextBox.TabIndex = 4;
            this.selectedDatabaseTextBox.Text = "None";
            // 
            // selectedDatabaseLabel
            // 
            this.selectedDatabaseLabel.AutoSize = true;
            this.selectedDatabaseLabel.Location = new System.Drawing.Point(6, 32);
            this.selectedDatabaseLabel.Name = "selectedDatabaseLabel";
            this.selectedDatabaseLabel.Size = new System.Drawing.Size(101, 13);
            this.selectedDatabaseLabel.TabIndex = 0;
            this.selectedDatabaseLabel.Text = "Selected Database:";
            // 
            // selectDatabaseFileDialog
            // 
            this.selectDatabaseFileDialog.FileName = "selectDatabaseFileDialog";
            this.selectDatabaseFileDialog.Filter = "Access Database files (*.mdb)|*.mdb|All files (*.*)|*.*";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(42, 281);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(964, 321);
            this.textBox1.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 741);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.databaseGroupBox);
            this.Controls.Add(this.googleClientLoginGroupBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MainWindow";
            this.Text = "Bridge Team Event Score Manager";
            this.googleClientLoginGroupBox.ResumeLayout(false);
            this.googleClientLoginGroupBox.PerformLayout();
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox googleClientLoginGroupBox;
        private System.Windows.Forms.TextBox authenticationStatusTextBox;
        private System.Windows.Forms.TextBox loginStatusTextBox;
        private System.Windows.Forms.Label autheticationStatusLabel;
        private System.Windows.Forms.Label loginStatus;
        private System.Windows.Forms.Button loginLogoutButton;
        private System.Windows.Forms.TextBox errorMessageTextBox;
        private System.Windows.Forms.Label errorMessageLabel;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.Label databaseValidityLabel;
        private System.Windows.Forms.Button createNewDatabaseButton;
        private System.Windows.Forms.Button changeDatabaseButton;
        private System.Windows.Forms.TextBox selectedDatabaseTextBox;
        private System.Windows.Forms.Label selectedDatabaseLabel;
        private System.Windows.Forms.TextBox databaseValidityTextbox;
        private System.Windows.Forms.OpenFileDialog selectDatabaseFileDialog;
        private System.Windows.Forms.TextBox textBox1;
    }
}

