namespace CSVParser
{
	partial class Form1
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
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnSelectFile = new System.Windows.Forms.Button();
			this.lblContents = new System.Windows.Forms.Label();
			this.txtFileContents = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.txtResults = new System.Windows.Forms.TextBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.btnSaveHtml = new System.Windows.Forms.Button();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Location = new System.Drawing.Point(39, 50);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(96, 32);
			this.btnSelectFile.TabIndex = 0;
			this.btnSelectFile.Text = "Select CSV File";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
			// 
			// lblContents
			// 
			this.lblContents.AutoSize = true;
			this.lblContents.Location = new System.Drawing.Point(370, 34);
			this.lblContents.Name = "lblContents";
			this.lblContents.Size = new System.Drawing.Size(45, 13);
			this.lblContents.TabIndex = 3;
			this.lblContents.Text = "Preview";
			// 
			// txtFileContents
			// 
			this.txtFileContents.Location = new System.Drawing.Point(373, 50);
			this.txtFileContents.Multiline = true;
			this.txtFileContents.Name = "txtFileContents";
			this.txtFileContents.Size = new System.Drawing.Size(507, 391);
			this.txtFileContents.TabIndex = 2;
			// 
			// txtResults
			// 
			this.txtResults.Location = new System.Drawing.Point(39, 221);
			this.txtResults.Multiline = true;
			this.txtResults.Name = "txtResults";
			this.txtResults.Size = new System.Drawing.Size(294, 216);
			this.txtResults.TabIndex = 4;
			this.txtResults.Visible = false;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(157, 64);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(109, 17);
			this.checkBox1.TabIndex = 5;
			this.checkBox1.Text = "Show Debug Info";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// btnSaveHtml
			// 
			this.btnSaveHtml.Location = new System.Drawing.Point(39, 165);
			this.btnSaveHtml.Name = "btnSaveHtml";
			this.btnSaveHtml.Size = new System.Drawing.Size(96, 32);
			this.btnSaveHtml.TabIndex = 6;
			this.btnSaveHtml.Text = "Save HTML";
			this.btnSaveHtml.UseVisualStyleBackColor = true;
			this.btnSaveHtml.Visible = false;
			this.btnSaveHtml.Click += new System.EventHandler(this.btnSaveHtml_Click);
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(39, 127);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(192, 20);
			this.txtTitle.TabIndex = 7;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(36, 111);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(112, 13);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "HTML Document Title";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 453);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.btnSaveHtml);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.txtResults);
			this.Controls.Add(this.lblContents);
			this.Controls.Add(this.txtFileContents);
			this.Controls.Add(this.btnSelectFile);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnSelectFile;
		private System.Windows.Forms.Label lblContents;
		private System.Windows.Forms.TextBox txtFileContents;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.TextBox txtResults;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button btnSaveHtml;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Label lblTitle;
	}
}

