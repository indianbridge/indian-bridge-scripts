namespace BFIMasterpointManagement
{
    partial class AddNewTournament
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
            this.tournamentCodeTextbox = new System.Windows.Forms.TextBox();
            this.loadingPicture = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.descriptionTextbox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tournamentLevelCombobox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).BeginInit();
            this.loginPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tournamentCodeTextbox
            // 
            this.tournamentCodeTextbox.Location = new System.Drawing.Point(115, 6);
            this.tournamentCodeTextbox.Name = "tournamentCodeTextbox";
            this.tournamentCodeTextbox.Size = new System.Drawing.Size(251, 20);
            this.tournamentCodeTextbox.TabIndex = 8;
            // 
            // loadingPicture
            // 
            this.loadingPicture.Image = global::BFIMasterpointManagement.Properties.Resources.loading;
            this.loadingPicture.Location = new System.Drawing.Point(160, 14);
            this.loadingPicture.Name = "loadingPicture";
            this.loadingPicture.Size = new System.Drawing.Size(67, 51);
            this.loadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingPicture.TabIndex = 7;
            this.loadingPicture.TabStop = false;
            this.loadingPicture.Visible = false;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(201, 84);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tournament Code : ";
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.tournamentLevelCombobox);
            this.loginPanel.Controls.Add(this.label3);
            this.loginPanel.Controls.Add(this.descriptionTextbox);
            this.loginPanel.Controls.Add(this.tournamentCodeTextbox);
            this.loginPanel.Controls.Add(this.loadingPicture);
            this.loginPanel.Controls.Add(this.okButton);
            this.loginPanel.Controls.Add(this.label1);
            this.loginPanel.Controls.Add(this.cancelButton);
            this.loginPanel.Controls.Add(this.label2);
            this.loginPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginPanel.Location = new System.Drawing.Point(0, 0);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(378, 118);
            this.loginPanel.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Tournament Level : ";
            // 
            // descriptionTextbox
            // 
            this.descriptionTextbox.Location = new System.Drawing.Point(115, 32);
            this.descriptionTextbox.Name = "descriptionTextbox";
            this.descriptionTextbox.Size = new System.Drawing.Size(251, 20);
            this.descriptionTextbox.TabIndex = 9;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(282, 84);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description : ";
            // 
            // tournamentLevelCombobox
            // 
            this.tournamentLevelCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tournamentLevelCombobox.FormattingEnabled = true;
            this.tournamentLevelCombobox.Location = new System.Drawing.Point(115, 61);
            this.tournamentLevelCombobox.Name = "tournamentLevelCombobox";
            this.tournamentLevelCombobox.Size = new System.Drawing.Size(251, 21);
            this.tournamentLevelCombobox.TabIndex = 12;
            // 
            // AddNewTournament
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 118);
            this.Controls.Add(this.loginPanel);
            this.Name = "AddNewTournament";
            this.Text = "AddNewTournament";
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).EndInit();
            this.loginPanel.ResumeLayout(false);
            this.loginPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tournamentCodeTextbox;
        private System.Windows.Forms.PictureBox loadingPicture;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox descriptionTextbox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox tournamentLevelCombobox;
    }
}