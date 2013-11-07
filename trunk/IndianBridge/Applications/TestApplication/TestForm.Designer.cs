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
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.operationStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.operationProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.operationCancelButton = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.openExcelFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.usernameTB = new System.Windows.Forms.TextBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.sessionIDTB = new System.Windows.Forms.TextBox();
            this.validateButton = new System.Windows.Forms.Button();
            this.invalidateButton = new System.Windows.Forms.Button();
            this.getTableDataButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(325, 85);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(375, 16);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(497, 157);
            this.textBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 104);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(324, 69);
            this.button2.TabIndex = 3;
            this.button2.Text = "Read Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openExcelFileDialog
            // 
            this.openExcelFileDialog.FileName = "openExcelFiledialog";
            // 
            // usernameTB
            // 
            this.usernameTB.Location = new System.Drawing.Point(47, 221);
            this.usernameTB.Name = "usernameTB";
            this.usernameTB.Size = new System.Drawing.Size(263, 20);
            this.usernameTB.TabIndex = 4;
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(47, 247);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(263, 20);
            this.passwordTB.TabIndex = 5;
            // 
            // sessionIDTB
            // 
            this.sessionIDTB.Location = new System.Drawing.Point(47, 273);
            this.sessionIDTB.Name = "sessionIDTB";
            this.sessionIDTB.Size = new System.Drawing.Size(263, 20);
            this.sessionIDTB.TabIndex = 6;
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(413, 192);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(185, 49);
            this.validateButton.TabIndex = 7;
            this.validateButton.Text = "Validate";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // invalidateButton
            // 
            this.invalidateButton.Location = new System.Drawing.Point(413, 247);
            this.invalidateButton.Name = "invalidateButton";
            this.invalidateButton.Size = new System.Drawing.Size(185, 49);
            this.invalidateButton.TabIndex = 8;
            this.invalidateButton.Text = "Invalidate";
            this.invalidateButton.UseVisualStyleBackColor = true;
            this.invalidateButton.Click += new System.EventHandler(this.invalidateButton_Click);
            // 
            // getTableDataButton
            // 
            this.getTableDataButton.Location = new System.Drawing.Point(637, 192);
            this.getTableDataButton.Name = "getTableDataButton";
            this.getTableDataButton.Size = new System.Drawing.Size(185, 49);
            this.getTableDataButton.TabIndex = 9;
            this.getTableDataButton.Text = "Get Table Data";
            this.getTableDataButton.UseVisualStyleBackColor = true;
            this.getTableDataButton.Click += new System.EventHandler(this.getTableDataButton_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 368);
            this.Controls.Add(this.getTableDataButton);
            this.Controls.Add(this.invalidateButton);
            this.Controls.Add(this.validateButton);
            this.Controls.Add(this.sessionIDTB);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.usernameTB);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel operationStatus;
        private System.Windows.Forms.ToolStripProgressBar operationProgressBar;
        private System.Windows.Forms.ToolStripButton operationCancelButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openExcelFileDialog;
        private System.Windows.Forms.TextBox usernameTB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.TextBox sessionIDTB;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.Button invalidateButton;
        private System.Windows.Forms.Button getTableDataButton;



    }
}

