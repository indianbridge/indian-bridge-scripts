namespace IndianBridge
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sendFileViaFTPButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.operationStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.operationCancelButton = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openExcelFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.connectToSQLButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sendFileViaFTPButton
            // 
            this.sendFileViaFTPButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendFileViaFTPButton.Location = new System.Drawing.Point(12, 12);
            this.sendFileViaFTPButton.Name = "sendFileViaFTPButton";
            this.sendFileViaFTPButton.Size = new System.Drawing.Size(325, 85);
            this.sendFileViaFTPButton.TabIndex = 0;
            this.sendFileViaFTPButton.Text = "Send File Via FTP";
            this.sendFileViaFTPButton.UseVisualStyleBackColor = true;
            this.sendFileViaFTPButton.Click += new System.EventHandler(this.sendFileViaFTPButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationStatus,
            this.operationProgressBar,
            this.operationCancelButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 345);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(927, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // operationStatus
            // 
            this.operationStatus.Name = "operationStatus";
            this.operationStatus.Size = new System.Drawing.Size(101, 18);
            this.operationStatus.Text = "Operation : Status";
            this.operationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // operationProgressBar
            // 
            this.operationProgressBar.Name = "operationProgressBar";
            this.operationProgressBar.Size = new System.Drawing.Size(50, 17);
            // 
            // operationCancelButton
            // 
            this.operationCancelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.operationCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("operationCancelButton.Image")));
            this.operationCancelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.operationCancelButton.Name = "operationCancelButton";
            this.operationCancelButton.Size = new System.Drawing.Size(130, 21);
            this.operationCancelButton.Text = "Cancel Load Summary";
            // 
            // openExcelFileDialog
            // 
            this.openExcelFileDialog.FileName = "openExcelFiledialog";
            // 
            // connectToSQLButton
            // 
            this.connectToSQLButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectToSQLButton.Location = new System.Drawing.Point(343, 12);
            this.connectToSQLButton.Name = "connectToSQLButton";
            this.connectToSQLButton.Size = new System.Drawing.Size(325, 85);
            this.connectToSQLButton.TabIndex = 2;
            this.connectToSQLButton.Text = "Connect To SQL";
            this.connectToSQLButton.UseVisualStyleBackColor = true;
            this.connectToSQLButton.Click += new System.EventHandler(this.connectToSQLButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 104);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(881, 226);
            this.textBox1.TabIndex = 3;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 368);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.connectToSQLButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.sendFileViaFTPButton);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button sendFileViaFTPButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel operationStatus;
        private System.Windows.Forms.ToolStripProgressBar operationProgressBar;
        private System.Windows.Forms.ToolStripButton operationCancelButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openExcelFileDialog;
        private System.Windows.Forms.Button connectToSQLButton;
        private System.Windows.Forms.TextBox textBox1;



    }
}

