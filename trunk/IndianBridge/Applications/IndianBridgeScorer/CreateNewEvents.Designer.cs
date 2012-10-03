namespace IndianBridgeScorer
{
    partial class CreateNewEvents
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.eventsListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numberOfNewEventsTextbox = new System.Windows.Forms.TextBox();
            this.qualifiersPerEventTextbox = new System.Windows.Forms.TextBox();
            this.totalQualifiersTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.createEventsButton = new System.Windows.Forms.Button();
            this.cancelCreateEventsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(958, 492);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.eventsListPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.numberOfNewEventsTextbox);
            this.splitContainer2.Panel2.Controls.Add(this.qualifiersPerEventTextbox);
            this.splitContainer2.Panel2.Controls.Add(this.totalQualifiersTextbox);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(958, 189);
            this.splitContainer2.SplitterDistance = 41;
            this.splitContainer2.TabIndex = 7;
            // 
            // eventsListPanel
            // 
            this.eventsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsListPanel.Location = new System.Drawing.Point(0, 0);
            this.eventsListPanel.Name = "eventsListPanel";
            this.eventsListPanel.Size = new System.Drawing.Size(958, 41);
            this.eventsListPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Split Into How Many Events :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(64, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Number Of Teams Per Event :";
            // 
            // numberOfNewEventsTextbox
            // 
            this.numberOfNewEventsTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfNewEventsTextbox.Location = new System.Drawing.Point(316, 13);
            this.numberOfNewEventsTextbox.Name = "numberOfNewEventsTextbox";
            this.numberOfNewEventsTextbox.Size = new System.Drawing.Size(53, 26);
            this.numberOfNewEventsTextbox.TabIndex = 1;
            this.numberOfNewEventsTextbox.Text = "1";
            this.numberOfNewEventsTextbox.TextChanged += new System.EventHandler(this.numberOfRoundRobinsTextbox_TextChanged);
            // 
            // qualifiersPerEventTextbox
            // 
            this.qualifiersPerEventTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qualifiersPerEventTextbox.Location = new System.Drawing.Point(316, 80);
            this.qualifiersPerEventTextbox.Name = "qualifiersPerEventTextbox";
            this.qualifiersPerEventTextbox.ReadOnly = true;
            this.qualifiersPerEventTextbox.Size = new System.Drawing.Size(53, 26);
            this.qualifiersPerEventTextbox.TabIndex = 5;
            this.qualifiersPerEventTextbox.Text = "1";
            // 
            // totalQualifiersTextbox
            // 
            this.totalQualifiersTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalQualifiersTextbox.Location = new System.Drawing.Point(316, 48);
            this.totalQualifiersTextbox.Name = "totalQualifiersTextbox";
            this.totalQualifiersTextbox.ReadOnly = true;
            this.totalQualifiersTextbox.Size = new System.Drawing.Size(53, 26);
            this.totalQualifiersTextbox.TabIndex = 3;
            this.totalQualifiersTextbox.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(170, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Total Qualifiers :";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.createEventsButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cancelCreateEventsButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 492);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(958, 50);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // createEventsButton
            // 
            this.createEventsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.createEventsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createEventsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createEventsButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.createEventsButton.Location = new System.Drawing.Point(6, 6);
            this.createEventsButton.Name = "createEventsButton";
            this.createEventsButton.Size = new System.Drawing.Size(468, 38);
            this.createEventsButton.TabIndex = 7;
            this.createEventsButton.Text = "Create Events";
            this.createEventsButton.UseVisualStyleBackColor = false;
            this.createEventsButton.Click += new System.EventHandler(this.createEventsButton_Click);
            // 
            // cancelCreateEventsButton
            // 
            this.cancelCreateEventsButton.BackColor = System.Drawing.Color.Red;
            this.cancelCreateEventsButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.cancelCreateEventsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelCreateEventsButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cancelCreateEventsButton.Location = new System.Drawing.Point(483, 6);
            this.cancelCreateEventsButton.Name = "cancelCreateEventsButton";
            this.cancelCreateEventsButton.Size = new System.Drawing.Size(469, 38);
            this.cancelCreateEventsButton.TabIndex = 8;
            this.cancelCreateEventsButton.Text = "CANCEL";
            this.cancelCreateEventsButton.UseVisualStyleBackColor = false;
            this.cancelCreateEventsButton.Click += new System.EventHandler(this.cancelCreateEventsButton_Click);
            // 
            // CreateNewEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 542);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "CreateNewEvents";
            this.Text = "Create Next Round Robins";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button createEventsButton;
        private System.Windows.Forms.Button cancelCreateEventsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numberOfNewEventsTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox qualifiersPerEventTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox totalQualifiersTextbox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.FlowLayoutPanel eventsListPanel;
    }
}