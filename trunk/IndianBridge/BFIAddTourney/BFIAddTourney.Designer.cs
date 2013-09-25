namespace BFIAddTourney
{
    partial class BFIAddTourney
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
            this.tourneyNamesCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tourneyYearCombobox = new System.Windows.Forms.ComboBox();
            this.pageNamesDataGridView = new System.Windows.Forms.DataGridView();
            this.pageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.createPagesButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pageNamesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tourneyNamesCombobox
            // 
            this.tourneyNamesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tourneyNamesCombobox.FormattingEnabled = true;
            this.tourneyNamesCombobox.Location = new System.Drawing.Point(97, 3);
            this.tourneyNamesCombobox.Name = "tourneyNamesCombobox";
            this.tourneyNamesCombobox.Size = new System.Drawing.Size(412, 21);
            this.tourneyNamesCombobox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tourney Name : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tourney Year : ";
            // 
            // tourneyYearCombobox
            // 
            this.tourneyYearCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tourneyYearCombobox.FormattingEnabled = true;
            this.tourneyYearCombobox.Location = new System.Drawing.Point(97, 30);
            this.tourneyYearCombobox.Name = "tourneyYearCombobox";
            this.tourneyYearCombobox.Size = new System.Drawing.Size(92, 21);
            this.tourneyYearCombobox.TabIndex = 3;
            // 
            // pageNamesDataGridView
            // 
            this.pageNamesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pageNamesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pageName});
            this.pageNamesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageNamesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.pageNamesDataGridView.Name = "pageNamesDataGridView";
            this.pageNamesDataGridView.Size = new System.Drawing.Size(520, 261);
            this.pageNamesDataGridView.TabIndex = 4;
            // 
            // pageName
            // 
            this.pageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pageName.HeaderText = "Page Name";
            this.pageName.Name = "pageName";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tourney Pages : ";
            // 
            // createPagesButton
            // 
            this.createPagesButton.BackColor = System.Drawing.Color.Green;
            this.createPagesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createPagesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createPagesButton.ForeColor = System.Drawing.Color.White;
            this.createPagesButton.Location = new System.Drawing.Point(0, 0);
            this.createPagesButton.Name = "createPagesButton";
            this.createPagesButton.Size = new System.Drawing.Size(520, 67);
            this.createPagesButton.TabIndex = 6;
            this.createPagesButton.Text = "Create Pages";
            this.createPagesButton.UseVisualStyleBackColor = false;
            this.createPagesButton.Click += new System.EventHandler(this.createPagesButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.tourneyYearCombobox);
            this.splitContainer1.Panel1.Controls.Add(this.tourneyNamesCombobox);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(520, 396);
            this.splitContainer1.SplitterDistance = 60;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pageNamesDataGridView);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.createPagesButton);
            this.splitContainer2.Size = new System.Drawing.Size(520, 332);
            this.splitContainer2.SplitterDistance = 261;
            this.splitContainer2.TabIndex = 0;
            // 
            // BFIAddTourney
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 396);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BFIAddTourney";
            this.Text = "BFI Add Tourney Application";
            this.Load += new System.EventHandler(this.BFIAddTourney_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pageNamesDataGridView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox tourneyNamesCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox tourneyYearCombobox;
        private System.Windows.Forms.DataGridView pageNamesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn pageName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createPagesButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}

