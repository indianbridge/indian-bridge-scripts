namespace IndianBridge.Applications
{
    partial class GetTextInput
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
            this.GetTextInputTextBox = new System.Windows.Forms.TextBox();
            this.GetTextInputLabel = new System.Windows.Forms.Label();
            this.GetTextInputButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetTextInputTextBox
            // 
            this.GetTextInputTextBox.Location = new System.Drawing.Point(11, 28);
            this.GetTextInputTextBox.Name = "GetTextInputTextBox";
            this.GetTextInputTextBox.Size = new System.Drawing.Size(556, 20);
            this.GetTextInputTextBox.TabIndex = 0;
            // 
            // GetTextInputLabel
            // 
            this.GetTextInputLabel.AutoSize = true;
            this.GetTextInputLabel.Location = new System.Drawing.Point(11, 8);
            this.GetTextInputLabel.Name = "GetTextInputLabel";
            this.GetTextInputLabel.Size = new System.Drawing.Size(128, 13);
            this.GetTextInputLabel.TabIndex = 1;
            this.GetTextInputLabel.Text = "Provide the required input";
            // 
            // GetTextInputButton
            // 
            this.GetTextInputButton.Location = new System.Drawing.Point(11, 55);
            this.GetTextInputButton.Name = "GetTextInputButton";
            this.GetTextInputButton.Size = new System.Drawing.Size(556, 22);
            this.GetTextInputButton.TabIndex = 2;
            this.GetTextInputButton.Text = "Submit";
            this.GetTextInputButton.UseVisualStyleBackColor = true;
            this.GetTextInputButton.Click += new System.EventHandler(this.GetTextInputButton_Click);
            // 
            // GetTextInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 95);
            this.Controls.Add(this.GetTextInputButton);
            this.Controls.Add(this.GetTextInputLabel);
            this.Controls.Add(this.GetTextInputTextBox);
            this.Name = "GetTextInput";
            this.Text = "GetTextInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox GetTextInputTextBox;
        private System.Windows.Forms.Label GetTextInputLabel;
        private System.Windows.Forms.Button GetTextInputButton;
    }
}