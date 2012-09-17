namespace IndianBridgeScorer
{
    partial class TeamScorer
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
            this.changeEventSetup = new System.Windows.Forms.Button();
            this.enterNames = new System.Windows.Forms.Button();
            this.enterScoresIMPsButton = new System.Windows.Forms.Button();
            this.enterDrawButton = new System.Windows.Forms.Button();
            this.drawUsingRoundCombobox = new System.Windows.Forms.ComboBox();
            this.drawGroup = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.generateDrawForCombobox = new System.Windows.Forms.ToolStripComboBox();
            this.randomDrawButton = new System.Windows.Forms.Button();
            this.roundDrawButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enterDrawScoresVPsButton = new System.Windows.Forms.Button();
            this.enterDrawScoresIMPsButton = new System.Windows.Forms.Button();
            this.enterScoresVPsButton = new System.Windows.Forms.Button();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.scoredForRoundCombobox = new System.Windows.Forms.ToolStripComboBox();
            this.createLocalWebpagesButton = new System.Windows.Forms.Button();
            this.drawGroup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // changeEventSetup
            // 
            this.changeEventSetup.AutoSize = true;
            this.changeEventSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.changeEventSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeEventSetup.Location = new System.Drawing.Point(13, 13);
            this.changeEventSetup.Name = "changeEventSetup";
            this.changeEventSetup.Size = new System.Drawing.Size(241, 36);
            this.changeEventSetup.TabIndex = 0;
            this.changeEventSetup.Text = "Change Event Setup";
            this.changeEventSetup.UseVisualStyleBackColor = false;
            this.changeEventSetup.Click += new System.EventHandler(this.changeEventSetup_Click);
            // 
            // enterNames
            // 
            this.enterNames.AutoSize = true;
            this.enterNames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterNames.Location = new System.Drawing.Point(12, 55);
            this.enterNames.Name = "enterNames";
            this.enterNames.Size = new System.Drawing.Size(241, 36);
            this.enterNames.TabIndex = 1;
            this.enterNames.Text = "Enter Team Names";
            this.enterNames.UseVisualStyleBackColor = false;
            this.enterNames.Click += new System.EventHandler(this.enterNames_Click);
            // 
            // enterScoresIMPsButton
            // 
            this.enterScoresIMPsButton.AutoSize = true;
            this.enterScoresIMPsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterScoresIMPsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterScoresIMPsButton.Location = new System.Drawing.Point(6, 52);
            this.enterScoresIMPsButton.Name = "enterScoresIMPsButton";
            this.enterScoresIMPsButton.Size = new System.Drawing.Size(271, 36);
            this.enterScoresIMPsButton.TabIndex = 2;
            this.enterScoresIMPsButton.Text = "Enter Scores in IMPs";
            this.enterScoresIMPsButton.UseVisualStyleBackColor = false;
            this.enterScoresIMPsButton.Click += new System.EventHandler(this.enterScoresIMPsButton_Click);
            // 
            // enterDrawButton
            // 
            this.enterDrawButton.AutoSize = true;
            this.enterDrawButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterDrawButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterDrawButton.Location = new System.Drawing.Point(6, 188);
            this.enterDrawButton.Name = "enterDrawButton";
            this.enterDrawButton.Size = new System.Drawing.Size(301, 36);
            this.enterDrawButton.TabIndex = 4;
            this.enterDrawButton.Text = "Enter/Edit Draw";
            this.enterDrawButton.UseVisualStyleBackColor = false;
            this.enterDrawButton.Click += new System.EventHandler(this.enterDrawButton_Click);
            // 
            // drawUsingRoundCombobox
            // 
            this.drawUsingRoundCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drawUsingRoundCombobox.FormattingEnabled = true;
            this.drawUsingRoundCombobox.Location = new System.Drawing.Point(313, 58);
            this.drawUsingRoundCombobox.Name = "drawUsingRoundCombobox";
            this.drawUsingRoundCombobox.Size = new System.Drawing.Size(65, 33);
            this.drawUsingRoundCombobox.TabIndex = 5;
            // 
            // drawGroup
            // 
            this.drawGroup.Controls.Add(this.toolStrip1);
            this.drawGroup.Controls.Add(this.randomDrawButton);
            this.drawGroup.Controls.Add(this.roundDrawButton);
            this.drawGroup.Controls.Add(this.enterDrawButton);
            this.drawGroup.Controls.Add(this.drawUsingRoundCombobox);
            this.drawGroup.Location = new System.Drawing.Point(13, 109);
            this.drawGroup.Name = "drawGroup";
            this.drawGroup.Size = new System.Drawing.Size(412, 230);
            this.drawGroup.TabIndex = 6;
            this.drawGroup.TabStop = false;
            this.drawGroup.Text = "Manage Draws";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.generateDrawForCombobox});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(406, 38);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(306, 35);
            this.toolStripLabel1.Text = "Generate Draws for Round : ";
            // 
            // generateDrawForCombobox
            // 
            this.generateDrawForCombobox.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.generateDrawForCombobox.Name = "generateDrawForCombobox";
            this.generateDrawForCombobox.Size = new System.Drawing.Size(75, 38);
            // 
            // randomDrawButton
            // 
            this.randomDrawButton.AutoSize = true;
            this.randomDrawButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.randomDrawButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.randomDrawButton.Location = new System.Drawing.Point(6, 146);
            this.randomDrawButton.Name = "randomDrawButton";
            this.randomDrawButton.Size = new System.Drawing.Size(301, 36);
            this.randomDrawButton.TabIndex = 8;
            this.randomDrawButton.Text = "Random Draw";
            this.randomDrawButton.UseVisualStyleBackColor = false;
            this.randomDrawButton.Click += new System.EventHandler(this.randomDrawButton_Click);
            // 
            // roundDrawButton
            // 
            this.roundDrawButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.roundDrawButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundDrawButton.Location = new System.Drawing.Point(6, 57);
            this.roundDrawButton.Name = "roundDrawButton";
            this.roundDrawButton.Size = new System.Drawing.Size(301, 73);
            this.roundDrawButton.TabIndex = 7;
            this.roundDrawButton.Text = "Generate Draw based on Scores after Round : ";
            this.roundDrawButton.UseVisualStyleBackColor = false;
            this.roundDrawButton.Click += new System.EventHandler(this.roundDrawButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enterDrawScoresVPsButton);
            this.groupBox1.Controls.Add(this.enterDrawScoresIMPsButton);
            this.groupBox1.Controls.Add(this.enterScoresVPsButton);
            this.groupBox1.Controls.Add(this.toolStrip2);
            this.groupBox1.Controls.Add(this.enterScoresIMPsButton);
            this.groupBox1.Location = new System.Drawing.Point(431, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 230);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manage Scores";
            // 
            // enterDrawScoresVPsButton
            // 
            this.enterDrawScoresVPsButton.AutoSize = true;
            this.enterDrawScoresVPsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterDrawScoresVPsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterDrawScoresVPsButton.Location = new System.Drawing.Point(6, 177);
            this.enterDrawScoresVPsButton.Name = "enterDrawScoresVPsButton";
            this.enterDrawScoresVPsButton.Size = new System.Drawing.Size(271, 36);
            this.enterDrawScoresVPsButton.TabIndex = 11;
            this.enterDrawScoresVPsButton.Text = "Enter Draw and Scores in VPs";
            this.enterDrawScoresVPsButton.UseVisualStyleBackColor = false;
            this.enterDrawScoresVPsButton.Click += new System.EventHandler(this.enterDrawScoresVPsButton_Click);
            // 
            // enterDrawScoresIMPsButton
            // 
            this.enterDrawScoresIMPsButton.AutoSize = true;
            this.enterDrawScoresIMPsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterDrawScoresIMPsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterDrawScoresIMPsButton.Location = new System.Drawing.Point(6, 136);
            this.enterDrawScoresIMPsButton.Name = "enterDrawScoresIMPsButton";
            this.enterDrawScoresIMPsButton.Size = new System.Drawing.Size(271, 36);
            this.enterDrawScoresIMPsButton.TabIndex = 10;
            this.enterDrawScoresIMPsButton.Text = "Enter Draw and Scores in IMPs";
            this.enterDrawScoresIMPsButton.UseVisualStyleBackColor = false;
            this.enterDrawScoresIMPsButton.Click += new System.EventHandler(this.enterDrawScoresIMPsButton_Click);
            // 
            // enterScoresVPsButton
            // 
            this.enterScoresVPsButton.AutoSize = true;
            this.enterScoresVPsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.enterScoresVPsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterScoresVPsButton.Location = new System.Drawing.Point(6, 94);
            this.enterScoresVPsButton.Name = "enterScoresVPsButton";
            this.enterScoresVPsButton.Size = new System.Drawing.Size(271, 36);
            this.enterScoresVPsButton.TabIndex = 9;
            this.enterScoresVPsButton.Text = "Enter Scores in VPs";
            this.enterScoresVPsButton.UseVisualStyleBackColor = false;
            this.enterScoresVPsButton.Click += new System.EventHandler(this.enterScoresVPsButton_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.scoredForRoundCombobox});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(317, 38);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(207, 35);
            this.toolStripLabel2.Text = "Scores for Round : ";
            // 
            // scoredForRoundCombobox
            // 
            this.scoredForRoundCombobox.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.scoredForRoundCombobox.Name = "scoredForRoundCombobox";
            this.scoredForRoundCombobox.Size = new System.Drawing.Size(75, 38);
            // 
            // createLocalWebpagesButton
            // 
            this.createLocalWebpagesButton.AutoSize = true;
            this.createLocalWebpagesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.createLocalWebpagesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createLocalWebpagesButton.Location = new System.Drawing.Point(19, 360);
            this.createLocalWebpagesButton.Name = "createLocalWebpagesButton";
            this.createLocalWebpagesButton.Size = new System.Drawing.Size(276, 36);
            this.createLocalWebpagesButton.TabIndex = 8;
            this.createLocalWebpagesButton.Text = "Create Local Webpages";
            this.createLocalWebpagesButton.UseVisualStyleBackColor = false;
            this.createLocalWebpagesButton.Click += new System.EventHandler(this.createLocalWebpagesButton_Click);
            // 
            // TeamScorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 449);
            this.Controls.Add(this.createLocalWebpagesButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.drawGroup);
            this.Controls.Add(this.enterNames);
            this.Controls.Add(this.changeEventSetup);
            this.Name = "TeamScorer";
            this.Text = "TeamsScorer";
            this.drawGroup.ResumeLayout(false);
            this.drawGroup.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button changeEventSetup;
        private System.Windows.Forms.Button enterNames;
        private System.Windows.Forms.Button enterScoresIMPsButton;
        private System.Windows.Forms.Button enterDrawButton;
        private System.Windows.Forms.ComboBox drawUsingRoundCombobox;
        private System.Windows.Forms.GroupBox drawGroup;
        private System.Windows.Forms.Button randomDrawButton;
        private System.Windows.Forms.Button roundDrawButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox generateDrawForCombobox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox scoredForRoundCombobox;
        private System.Windows.Forms.Button enterDrawScoresIMPsButton;
        private System.Windows.Forms.Button enterScoresVPsButton;
        private System.Windows.Forms.Button enterDrawScoresVPsButton;
        private System.Windows.Forms.Button createLocalWebpagesButton;
    }
}