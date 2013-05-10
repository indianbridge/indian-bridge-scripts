namespace GenerateBFIPage
{
    partial class GenerateBFIPage
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
            this.SelectCSVFile = new System.Windows.Forms.OpenFileDialog();
            this.shortCodeTextBox = new System.Windows.Forms.TextBox();
            this.websitePrefixTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NumberOfColumnsComboBox = new System.Windows.Forms.ComboBox();
            this.Columns = new System.Windows.Forms.Label();
            this.GenerateShortcodeButton = new System.Windows.Forms.Button();
            this.GenerateStateButton = new System.Windows.Forms.Button();
            this.GenerateDirectorsListButton = new System.Windows.Forms.Button();
            this.GeneratePotentialDirectorsListButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectCSVFile
            // 
            this.SelectCSVFile.FileName = "People.csv";
            // 
            // shortCodeTextBox
            // 
            this.shortCodeTextBox.Location = new System.Drawing.Point(12, 122);
            this.shortCodeTextBox.Multiline = true;
            this.shortCodeTextBox.Name = "shortCodeTextBox";
            this.shortCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.shortCodeTextBox.Size = new System.Drawing.Size(950, 385);
            this.shortCodeTextBox.TabIndex = 0;
            // 
            // websitePrefixTextBox
            // 
            this.websitePrefixTextBox.Location = new System.Drawing.Point(72, 6);
            this.websitePrefixTextBox.Name = "websitePrefixTextBox";
            this.websitePrefixTextBox.Size = new System.Drawing.Size(389, 20);
            this.websitePrefixTextBox.TabIndex = 1;
            this.websitePrefixTextBox.Text = "http://localhost/bfi/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Site Prefix";
            // 
            // NumberOfColumnsComboBox
            // 
            this.NumberOfColumnsComboBox.FormattingEnabled = true;
            this.NumberOfColumnsComboBox.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.NumberOfColumnsComboBox.Location = new System.Drawing.Point(72, 33);
            this.NumberOfColumnsComboBox.Name = "NumberOfColumnsComboBox";
            this.NumberOfColumnsComboBox.Size = new System.Drawing.Size(121, 21);
            this.NumberOfColumnsComboBox.TabIndex = 3;
            this.NumberOfColumnsComboBox.Text = "2";
            // 
            // Columns
            // 
            this.Columns.AutoSize = true;
            this.Columns.Location = new System.Drawing.Point(12, 33);
            this.Columns.Name = "Columns";
            this.Columns.Size = new System.Drawing.Size(47, 13);
            this.Columns.TabIndex = 4;
            this.Columns.Text = "Columns";
            // 
            // GenerateShortcodeButton
            // 
            this.GenerateShortcodeButton.Location = new System.Drawing.Point(15, 60);
            this.GenerateShortcodeButton.Name = "GenerateShortcodeButton";
            this.GenerateShortcodeButton.Size = new System.Drawing.Size(201, 56);
            this.GenerateShortcodeButton.TabIndex = 5;
            this.GenerateShortcodeButton.Text = "Generate Office Bearers";
            this.GenerateShortcodeButton.UseVisualStyleBackColor = true;
            this.GenerateShortcodeButton.Click += new System.EventHandler(this.GenerateShortcodeButton_Click);
            // 
            // GenerateStateButton
            // 
            this.GenerateStateButton.Location = new System.Drawing.Point(222, 60);
            this.GenerateStateButton.Name = "GenerateStateButton";
            this.GenerateStateButton.Size = new System.Drawing.Size(201, 56);
            this.GenerateStateButton.TabIndex = 6;
            this.GenerateStateButton.Text = "Generate State";
            this.GenerateStateButton.UseVisualStyleBackColor = true;
            this.GenerateStateButton.Click += new System.EventHandler(this.GenerateStateButton_Click);
            // 
            // GenerateDirectorsListButton
            // 
            this.GenerateDirectorsListButton.Location = new System.Drawing.Point(429, 60);
            this.GenerateDirectorsListButton.Name = "GenerateDirectorsListButton";
            this.GenerateDirectorsListButton.Size = new System.Drawing.Size(201, 56);
            this.GenerateDirectorsListButton.TabIndex = 7;
            this.GenerateDirectorsListButton.Text = "Generate Director List";
            this.GenerateDirectorsListButton.UseVisualStyleBackColor = true;
            this.GenerateDirectorsListButton.Click += new System.EventHandler(this.GenerateDirectorsListButton_Click);
            // 
            // GeneratePotentialDirectorsListButton
            // 
            this.GeneratePotentialDirectorsListButton.Location = new System.Drawing.Point(636, 60);
            this.GeneratePotentialDirectorsListButton.Name = "GeneratePotentialDirectorsListButton";
            this.GeneratePotentialDirectorsListButton.Size = new System.Drawing.Size(201, 56);
            this.GeneratePotentialDirectorsListButton.TabIndex = 8;
            this.GeneratePotentialDirectorsListButton.Text = "Generate Potential Directors";
            this.GeneratePotentialDirectorsListButton.UseVisualStyleBackColor = true;
            this.GeneratePotentialDirectorsListButton.Click += new System.EventHandler(this.GeneratePotentialDirectorsListButton_Click);
            // 
            // GenerateBFIPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 514);
            this.Controls.Add(this.GeneratePotentialDirectorsListButton);
            this.Controls.Add(this.GenerateDirectorsListButton);
            this.Controls.Add(this.GenerateStateButton);
            this.Controls.Add(this.GenerateShortcodeButton);
            this.Controls.Add(this.Columns);
            this.Controls.Add(this.NumberOfColumnsComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.websitePrefixTextBox);
            this.Controls.Add(this.shortCodeTextBox);
            this.Name = "GenerateBFIPage";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog SelectCSVFile;
        private System.Windows.Forms.TextBox shortCodeTextBox;
        private System.Windows.Forms.TextBox websitePrefixTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox NumberOfColumnsComboBox;
        private System.Windows.Forms.Label Columns;
        private System.Windows.Forms.Button GenerateShortcodeButton;
        private System.Windows.Forms.Button GenerateStateButton;
        private System.Windows.Forms.Button GenerateDirectorsListButton;
        private System.Windows.Forms.Button GeneratePotentialDirectorsListButton;
    }
}

