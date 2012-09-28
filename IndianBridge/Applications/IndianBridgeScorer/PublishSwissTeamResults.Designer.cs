namespace IndianBridgeScorer
{
    partial class PublishSwissTeamResults
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
            this.status = new System.Windows.Forms.TextBox();
            this.currentOperationTitle = new System.Windows.Forms.GroupBox();
            this.currentOperationTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(6, 17);
            this.status.Multiline = true;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.status.Size = new System.Drawing.Size(823, 340);
            this.status.TabIndex = 0;
            // 
            // currentOperationTitle
            // 
            this.currentOperationTitle.Controls.Add(this.status);
            this.currentOperationTitle.Location = new System.Drawing.Point(12, 12);
            this.currentOperationTitle.Name = "currentOperationTitle";
            this.currentOperationTitle.Size = new System.Drawing.Size(838, 363);
            this.currentOperationTitle.TabIndex = 1;
            this.currentOperationTitle.TabStop = false;
            this.currentOperationTitle.Text = "Current Operation : ";
            // 
            // CreateAndPublishResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 393);
            this.Controls.Add(this.currentOperationTitle);
            this.Name = "CreateAndPublishResults";
            this.Text = "Create and Publish Results";
            this.Shown += new System.EventHandler(this.PublishResults_Shown);
            this.currentOperationTitle.ResumeLayout(false);
            this.currentOperationTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.GroupBox currentOperationTitle;
    }
}