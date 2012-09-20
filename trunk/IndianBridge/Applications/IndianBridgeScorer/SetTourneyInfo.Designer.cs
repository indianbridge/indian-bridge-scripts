namespace IndianBridgeScorer
{
    partial class SetTourneyInfo
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
            this.tourneyNameTextBox = new System.Windows.Forms.TextBox();
            this.resultsWebsiteRootTextBox = new System.Windows.Forms.TextBox();
            this.setupTourneyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tourney Name : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Results Website Root : ";
            // 
            // tourneyNameTextBox
            // 
            this.tourneyNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourneyNameTextBox.Location = new System.Drawing.Point(282, 9);
            this.tourneyNameTextBox.Name = "tourneyNameTextBox";
            this.tourneyNameTextBox.Size = new System.Drawing.Size(344, 32);
            this.tourneyNameTextBox.TabIndex = 2;
            // 
            // resultsWebsiteRootTextBox
            // 
            this.resultsWebsiteRootTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsWebsiteRootTextBox.Location = new System.Drawing.Point(282, 48);
            this.resultsWebsiteRootTextBox.Multiline = true;
            this.resultsWebsiteRootTextBox.Name = "resultsWebsiteRootTextBox";
            this.resultsWebsiteRootTextBox.Size = new System.Drawing.Size(616, 75);
            this.resultsWebsiteRootTextBox.TabIndex = 3;
            // 
            // setupTourneyButton
            // 
            this.setupTourneyButton.AutoSize = true;
            this.setupTourneyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.setupTourneyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setupTourneyButton.Location = new System.Drawing.Point(18, 155);
            this.setupTourneyButton.Name = "setupTourneyButton";
            this.setupTourneyButton.Size = new System.Drawing.Size(421, 36);
            this.setupTourneyButton.TabIndex = 4;
            this.setupTourneyButton.Text = "Setup Tourney";
            this.setupTourneyButton.UseVisualStyleBackColor = false;
            this.setupTourneyButton.Click += new System.EventHandler(this.setupTourneyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(477, 155);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(421, 36);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SetTourneyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 210);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.setupTourneyButton);
            this.Controls.Add(this.resultsWebsiteRootTextBox);
            this.Controls.Add(this.tourneyNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SetTourneyInfo";
            this.Text = "SetTourneyInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tourneyNameTextBox;
        private System.Windows.Forms.TextBox resultsWebsiteRootTextBox;
        private System.Windows.Forms.Button setupTourneyButton;
        private System.Windows.Forms.Button cancelButton;
    }
}