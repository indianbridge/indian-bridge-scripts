namespace IndianBridge.Applications
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
            this.Status = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.LoadDatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.LoadDatabaseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.Location = new System.Drawing.Point(20, 155);
            this.Status.Multiline = true;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Status.Size = new System.Drawing.Size(871, 157);
            this.Status.TabIndex = 0;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(41, 32);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(296, 41);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // LoadDatabaseButton
            // 
            this.LoadDatabaseButton.Location = new System.Drawing.Point(41, 80);
            this.LoadDatabaseButton.Name = "LoadDatabaseButton";
            this.LoadDatabaseButton.Size = new System.Drawing.Size(296, 36);
            this.LoadDatabaseButton.TabIndex = 2;
            this.LoadDatabaseButton.Text = "Load Database";
            this.LoadDatabaseButton.UseVisualStyleBackColor = true;
            this.LoadDatabaseButton.Click += new System.EventHandler(this.LoadDatabaseButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 325);
            this.Controls.Add(this.LoadDatabaseButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.Status);
            this.Name = "MainWindow";
            this.Text = "GoogleSpreadsheetParser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Status;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.OpenFileDialog LoadDatabaseFileDialog;
        private System.Windows.Forms.Button LoadDatabaseButton;
    }
}

