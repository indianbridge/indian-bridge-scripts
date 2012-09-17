using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class SelectCalendarEvent : Form
    {
        public Tuple<SortableBindingList<IndianCalendarEvent>, DataTable> calendarInfo = null;
        public Tuple<String, DateTime, String> calendarEventInfo = null;
        LoadingEvents loadingEventsForm;
        public static CalendarAPI calendarAPI;
        public static int selectedEntryNumber = -1;

        public SelectCalendarEvent()
        {
            InitializeComponent();
            calendarAPI = new CalendarAPI("indianbridge.dummy@gmail.com", "kibitzer");
            this.calendarGetEvents_endDate.Value = DateTime.Today;
            this.calendarGetEvents_startDate.Value = DateTime.Today.AddDays(-7);
            loadEvents(false);
        }

        public SelectCalendarEvent(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            calendarAPI = new CalendarAPI("indianbridge.dummy@gmail.com", "kibitzer");
            this.calendarGetEvents_endDate.Value = startDate;
            this.calendarGetEvents_startDate.Value = endDate;
            loadEvents(false);
        }


        private void loadEvents(Boolean showLoading)
        {
            this.dataGridView1.DataSource = null;
            
            loadEventsInBackGround.RunWorkerAsync(dataGridView1);
            if (showLoading)
            {
                this.Enabled = false;
                loadingEventsForm = new LoadingEvents();
                loadingEventsForm.StartPosition = FormStartPosition.CenterParent;
                loadingEventsForm.ShowDialog(this);
            }
        }

        private void calendarGetEvents_Button_Click(object sender, EventArgs e)
        {
            loadEvents(true);
        }

        private void loadEventsInBackGround_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SortableBindingList<IndianCalendarEvent> test = calendarAPI.getEvents(calendarGetEvents_startDate.Value, calendarGetEvents_endDate.Value, calendarGetEvents_SearchTextbox.Text);
                SpreadSheetAPI ssa = new SpreadSheetAPI("Pair Results City And Event Names", "indianbridge.dummy@gmail.com", "kibitzer");
                DataTable table = ssa.getValuesFromSheet("Sheet1");
                e.Result = Tuple.Create<SortableBindingList<IndianCalendarEvent>, DataTable>(test, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Here" + ex.Message);
                e.Result = null;
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            calendarEventInfo = null;
            this.Close();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string eventTitle = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            DateTime eventDate = (DateTime)dataGridView1[1, dataGridView1.CurrentRow.Index].Value;
            string eventLocation = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            selectedEntryNumber = (int)dataGridView1[3, dataGridView1.CurrentRow.Index].Value;
            calendarEventInfo = Tuple.Create<String, DateTime, String>(eventTitle, eventDate, eventLocation);
            this.Close();
        }

        private void loadEventsInBackGround_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            if (e.Result != null)
            {
                calendarInfo = e.Result as Tuple<SortableBindingList<IndianCalendarEvent>, DataTable>;
                this.dataGridView1.DataSource = calendarInfo.Item1;
                this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Descending);
                this.dataGridView1.Columns[this.dataGridView1.Columns.Count - 1].Visible = false;
                this.dataGridView1.Columns[this.dataGridView1.Columns.Count - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[1].DefaultCellStyle.Format = "MMMM dd, yyyy";
            }
            if (loadingEventsForm != null)
            {
                loadingEventsForm.Close();
                loadingEventsForm.Dispose();
                loadingEventsForm = null;
            }
        }

        private void SelectCalendarEvent_Shown(object sender, EventArgs e)
        {
            if (loadEventsInBackGround.IsBusy)
            {
                this.Enabled = false;
                loadingEventsForm = new LoadingEvents();
                loadingEventsForm.StartPosition = FormStartPosition.CenterParent;
                loadingEventsForm.ShowDialog(this);
            }
        }

    }
}
