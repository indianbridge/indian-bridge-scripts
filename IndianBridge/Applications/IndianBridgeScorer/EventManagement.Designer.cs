namespace IndianBridgeScorer
{
    partial class EventManagement
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
            this.tourneyNameLabel = new System.Windows.Forms.Label();
            this.addNewEventButton = new System.Windows.Forms.Button();
            this.eventsList = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tourneyNameLabel
            // 
            this.tourneyNameLabel.AutoSize = true;
            this.tourneyNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tourneyNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourneyNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tourneyNameLabel.Location = new System.Drawing.Point(12, 9);
            this.tourneyNameLabel.Name = "tourneyNameLabel";
            this.tourneyNameLabel.Size = new System.Drawing.Size(188, 26);
            this.tourneyNameLabel.TabIndex = 2;
            this.tourneyNameLabel.Text = "Tourney Name : ";
            // 
            // addNewEventButton
            // 
            this.addNewEventButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addNewEventButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNewEventButton.Location = new System.Drawing.Point(12, 260);
            this.addNewEventButton.Name = "addNewEventButton";
            this.addNewEventButton.Size = new System.Drawing.Size(297, 43);
            this.addNewEventButton.TabIndex = 4;
            this.addNewEventButton.Text = "Add New Event";
            this.addNewEventButton.UseVisualStyleBackColor = false;
            this.addNewEventButton.Click += new System.EventHandler(this.addNewEventButton_Click);
            // 
            // eventsList
            // 
            this.eventsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsList.AutoScroll = true;
            this.eventsList.AutoSize = true;
            this.eventsList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.eventsList.ColumnCount = 4;
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.eventsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventsList.Location = new System.Drawing.Point(6, 30);
            this.eventsList.Name = "eventsList";
            this.eventsList.RowCount = 1;
            this.eventsList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.eventsList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.eventsList.Size = new System.Drawing.Size(700, 171);
            this.eventsList.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.eventsList);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 215);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Events in Tourney";
            // 
            // EventManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(731, 310);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.addNewEventButton);
            this.Controls.Add(this.tourneyNameLabel);
            this.Name = "EventManagement";
            this.Text = "Tourney Name : ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tourneyNameLabel;
        private System.Windows.Forms.Button addNewEventButton;
        private System.Windows.Forms.TableLayoutPanel eventsList;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}