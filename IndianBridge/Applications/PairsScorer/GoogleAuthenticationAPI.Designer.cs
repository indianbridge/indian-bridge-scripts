namespace IndianBridge.Applications
{
    partial class GoogleAuthenticationAPI
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
            this.authorizationCode_Textbox = new System.Windows.Forms.TextBox();
            this.run_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // authorizationCode_Textbox
            // 
            this.authorizationCode_Textbox.Location = new System.Drawing.Point(12, 30);
            this.authorizationCode_Textbox.Multiline = true;
            this.authorizationCode_Textbox.Name = "authorizationCode_Textbox";
            this.authorizationCode_Textbox.Size = new System.Drawing.Size(641, 319);
            this.authorizationCode_Textbox.TabIndex = 0;
            // 
            // run_Button
            // 
            this.run_Button.Location = new System.Drawing.Point(199, 422);
            this.run_Button.Name = "run_Button";
            this.run_Button.Size = new System.Drawing.Size(454, 28);
            this.run_Button.TabIndex = 2;
            this.run_Button.Text = "Run";
            this.run_Button.UseVisualStyleBackColor = true;
            this.run_Button.Click += new System.EventHandler(this.run_Button_Click);
            // 
            // GoogleAuthenticationAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 506);
            this.Controls.Add(this.run_Button);
            this.Controls.Add(this.authorizationCode_Textbox);
            this.Name = "GoogleAuthenticationAPI";
            this.Text = "GoogleAuthenticationAPI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox authorizationCode_Textbox;
        private System.Windows.Forms.Button run_Button;
    }
}