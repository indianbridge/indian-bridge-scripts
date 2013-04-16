namespace IndianBridgeScorer
{
    partial class WithdrawTeams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WithdrawTeams));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.withdrawTeamsDataGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.saveWithdrawTeamsToDatabase = new System.Windows.Forms.Button();
            this.reloadWithdrawTeamsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.withdrawTeamsDataGridView)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(838, 518);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Yellow;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(838, 65);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = resources.GetString("textBox1.Text");
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
            this.splitContainer2.Panel1.Controls.Add(this.withdrawTeamsDataGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer2.Size = new System.Drawing.Size(838, 449);
            this.splitContainer2.SplitterDistance = 395;
            this.splitContainer2.TabIndex = 15;
            // 
            // withdrawTeamsDataGridView
            // 
            this.withdrawTeamsDataGridView.AllowUserToAddRows = false;
            this.withdrawTeamsDataGridView.AllowUserToDeleteRows = false;
            this.withdrawTeamsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.withdrawTeamsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.withdrawTeamsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.withdrawTeamsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.withdrawTeamsDataGridView.Name = "withdrawTeamsDataGridView";
            this.withdrawTeamsDataGridView.Size = new System.Drawing.Size(838, 395);
            this.withdrawTeamsDataGridView.TabIndex = 0;
            this.withdrawTeamsDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.withdrawTeamsDataGridView_DataBindingComplete);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.05988F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.94012F));
            this.tableLayoutPanel3.Controls.Add(this.saveWithdrawTeamsToDatabase, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.reloadWithdrawTeamsButton, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(838, 50);
            this.tableLayoutPanel3.TabIndex = 15;
            // 
            // saveWithdrawTeamsToDatabase
            // 
            this.saveWithdrawTeamsToDatabase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.saveWithdrawTeamsToDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveWithdrawTeamsToDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveWithdrawTeamsToDatabase.ForeColor = System.Drawing.SystemColors.WindowText;
            this.saveWithdrawTeamsToDatabase.Location = new System.Drawing.Point(6, 6);
            this.saveWithdrawTeamsToDatabase.Name = "saveWithdrawTeamsToDatabase";
            this.saveWithdrawTeamsToDatabase.Size = new System.Drawing.Size(408, 38);
            this.saveWithdrawTeamsToDatabase.TabIndex = 7;
            this.saveWithdrawTeamsToDatabase.Text = "Save Withdrawn Teams to Database";
            this.saveWithdrawTeamsToDatabase.UseVisualStyleBackColor = false;
            // 
            // reloadWithdrawTeamsButton
            // 
            this.reloadWithdrawTeamsButton.BackColor = System.Drawing.Color.Red;
            this.reloadWithdrawTeamsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.reloadWithdrawTeamsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reloadWithdrawTeamsButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.reloadWithdrawTeamsButton.Location = new System.Drawing.Point(423, 6);
            this.reloadWithdrawTeamsButton.Name = "reloadWithdrawTeamsButton";
            this.reloadWithdrawTeamsButton.Size = new System.Drawing.Size(409, 38);
            this.reloadWithdrawTeamsButton.TabIndex = 8;
            this.reloadWithdrawTeamsButton.Text = "Reload Withdrawn Teams from Database";
            this.reloadWithdrawTeamsButton.UseVisualStyleBackColor = false;
            this.reloadWithdrawTeamsButton.Click += new System.EventHandler(this.reloadWithdrawTeamsButton_Click);
            // 
            // WithdrawTeams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 518);
            this.Controls.Add(this.splitContainer1);
            this.Name = "WithdrawTeams";
            this.Text = "WithdrawTeams";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.withdrawTeamsDataGridView)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView withdrawTeamsDataGridView;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button saveWithdrawTeamsToDatabase;
        private System.Windows.Forms.Button reloadWithdrawTeamsButton;
    }
}