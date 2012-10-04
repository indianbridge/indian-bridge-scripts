namespace IndianBridgeScorer
{
    partial class TourneyManagement
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
            this.createTourneyButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.loadExistingTourneyButton = new System.Windows.Forms.Button();
            this.tourneyListCombobox = new System.Windows.Forms.ComboBox();
            this.deleteTourneyButton = new System.Windows.Forms.Button();
            this.importTourneyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createTourneyButton
            // 
            this.createTourneyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createTourneyButton.ForeColor = System.Drawing.Color.Blue;
            this.createTourneyButton.Location = new System.Drawing.Point(10, 12);
            this.createTourneyButton.Name = "createTourneyButton";
            this.createTourneyButton.Size = new System.Drawing.Size(437, 56);
            this.createTourneyButton.TabIndex = 7;
            this.createTourneyButton.Text = "Create New Tourney";
            this.createTourneyButton.UseVisualStyleBackColor = true;
            this.createTourneyButton.Click += new System.EventHandler(this.createTourneyButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(195, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 37);
            this.label4.TabIndex = 1;
            this.label4.Text = "OR";
            // 
            // loadExistingTourneyButton
            // 
            this.loadExistingTourneyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadExistingTourneyButton.ForeColor = System.Drawing.Color.Blue;
            this.loadExistingTourneyButton.Location = new System.Drawing.Point(10, 211);
            this.loadExistingTourneyButton.Name = "loadExistingTourneyButton";
            this.loadExistingTourneyButton.Size = new System.Drawing.Size(437, 56);
            this.loadExistingTourneyButton.TabIndex = 8;
            this.loadExistingTourneyButton.Text = "Load Selected Tourney";
            this.loadExistingTourneyButton.UseVisualStyleBackColor = true;
            this.loadExistingTourneyButton.Click += new System.EventHandler(this.loadExistingTourneyButton_Click);
            // 
            // tourneyListCombobox
            // 
            this.tourneyListCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tourneyListCombobox.FormattingEnabled = true;
            this.tourneyListCombobox.Location = new System.Drawing.Point(12, 176);
            this.tourneyListCombobox.Name = "tourneyListCombobox";
            this.tourneyListCombobox.Size = new System.Drawing.Size(434, 21);
            this.tourneyListCombobox.TabIndex = 9;
            // 
            // deleteTourneyButton
            // 
            this.deleteTourneyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteTourneyButton.ForeColor = System.Drawing.Color.Blue;
            this.deleteTourneyButton.Location = new System.Drawing.Point(10, 273);
            this.deleteTourneyButton.Name = "deleteTourneyButton";
            this.deleteTourneyButton.Size = new System.Drawing.Size(437, 56);
            this.deleteTourneyButton.TabIndex = 10;
            this.deleteTourneyButton.Text = "Delete Selected Tourney";
            this.deleteTourneyButton.UseVisualStyleBackColor = true;
            this.deleteTourneyButton.Click += new System.EventHandler(this.deleteTourneyButton_Click);
            // 
            // importTourneyButton
            // 
            this.importTourneyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importTourneyButton.ForeColor = System.Drawing.Color.Blue;
            this.importTourneyButton.Location = new System.Drawing.Point(10, 74);
            this.importTourneyButton.Name = "importTourneyButton";
            this.importTourneyButton.Size = new System.Drawing.Size(437, 56);
            this.importTourneyButton.TabIndex = 11;
            this.importTourneyButton.Text = "Import Tourney From Google Spreadsheet";
            this.importTourneyButton.UseVisualStyleBackColor = true;
            this.importTourneyButton.Click += new System.EventHandler(this.importTourneyButton_Click);
            // 
            // TourneyManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(457, 339);
            this.Controls.Add(this.importTourneyButton);
            this.Controls.Add(this.deleteTourneyButton);
            this.Controls.Add(this.tourneyListCombobox);
            this.Controls.Add(this.loadExistingTourneyButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.createTourneyButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TourneyManagement";
            this.Text = "Tourney Management Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createTourneyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button loadExistingTourneyButton;
        private System.Windows.Forms.ComboBox tourneyListCombobox;
        private System.Windows.Forms.Button deleteTourneyButton;
        private System.Windows.Forms.Button importTourneyButton;
    }
}