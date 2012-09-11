using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubmitPairsResults
{
    public partial class checkEventInformation : Form
    {
        public Boolean eventInformationOK = false;
        public checkEventInformation(String eventNameC, DateTime eventDateC,
            String eventNameR, DateTime eventDateR)
        {
            InitializeComponent();
            this.eventDateFromResultsFile.Value = eventDateR;
            this.eventDateFromCalendar.Value = eventDateC;
            this.eventNameFromCalendar.Text = eventNameC;
            this.eventNameFromResultsFile.Text = eventNameR;
            this.eventDateGroupBox.BackColor = sameDay(eventDateC,eventDateR) ? Color.Green : Color.Red;
            this.eventNameGroupBox.BackColor = (eventNameC != eventNameR) ? Color.Red : Color.Green;
        }

        private bool sameDay(DateTime eventDateC, DateTime eventDateR)
        {
            if (eventDateC.Year == eventDateR.Year && eventDateC.Month == eventDateR.Month && eventDateC.Day == eventDateR.Day) return true;
            return false;
        }

        private void eventInformationConsistent_Click(object sender, EventArgs e)
        {
            eventInformationOK = true;
            this.Close();
        }

        private void eventInfoInconsistent_Click(object sender, EventArgs e)
        {
            eventInformationOK = false;
            this.Close();
        }
    }
}
