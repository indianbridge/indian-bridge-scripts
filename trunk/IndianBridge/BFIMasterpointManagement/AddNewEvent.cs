using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.WordpressAPIs;
using System.Web.Script.Serialization;
using IndianBridge.Common;

namespace BFIMasterpointManagement
{
    public partial class AddNewEvent : Form
    {
        public ManageMasterpoints m_mm;
        public AddNewEvent(ManageMasterpoints mm, DataGridView em)
        {
            m_mm = mm;
            InitializeComponent();
            em.Sort(em.Columns["event_code"], ListSortDirection.Descending);
            int tCodeIndex = em.Columns["event_code"].Index;
            string lastCode = (string)em.Rows[2].Cells[tCodeIndex].Value;
            string nextCode = Utilities.getNextCode(lastCode);
            foreach (DataGridViewRow row in em.Rows)
            {
                if ((string)row.Cells[tCodeIndex].Value == nextCode) return;
                eventCodeTextbox.Text = nextCode;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(eventCodeTextbox.Text))
            {
                MessageBox.Show("Level Code cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextbox.Text))
            {
                MessageBox.Show("Description cannot be empty string!");
                return;
            }
            this.loginPanel.Enabled = false;
            this.loadingPicture.Enabled = true;
            this.loadingPicture.Visible = true;
            this.loadingPicture.BringToFront();
            this.Refresh();
            EventInfo eventInfo = new EventInfo();
            eventInfo.event_code = eventCodeTextbox.Text;
            eventInfo.description = descriptionTextbox.Text;
            string json_result = m_mm.addEvent(eventInfo);
            Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Error when trying to add Event because : " + result["content"], "Error adding Table Data !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["content"], "Add Event Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            this.loadingPicture.Visible = false;
            this.loginPanel.Enabled = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
