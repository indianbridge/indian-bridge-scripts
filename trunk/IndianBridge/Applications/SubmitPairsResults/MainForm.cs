using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;
using System.IO;
using IndianBridge.ResultsManager;

namespace SubmitPairsResults
{
    public partial class MainForm : Form
    {
        LoadingEvents loadingEventsForm;
        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        private PairsDatabaseParameters m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
        private DataTable cityEvents = null;
        //private string googleSiteName = "indiancitybridgeresults";
        private string googleSiteRootPageName = "";

        public MainForm()
        {
            InitializeComponent();
            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
            this.calendarGetEvents_endDate.Value = DateTime.Today;
            this.calendarGetEvents_startDate.Value = DateTime.Today.AddDays(-7);
        }

        private void calendarGetEvents_Button_Click(object sender, EventArgs e)
        {
            status.Text += "Getting Events based on search criteria specified on the left. Please be patient as this may take a few minutes. When events have been retrieved they will be displayed in the table below."+Environment.NewLine;
            status.Refresh();
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            loadingEventsForm = new LoadingEvents();
            loadingEventsForm.StartPosition = FormStartPosition.CenterParent;
            this.dataGridView1.DataSource = null;
            this.getCalendarEvents.RunWorkerAsync(this.dataGridView1);
            this.Enabled = false;
            loadingEventsForm.ShowDialog(this);
            Trace.Listeners.Remove(_textBoxListener);
        }

        private void getCalendarEvents_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CalendarAPI cApi = new CalendarAPI("indianbridge.dummy@gmail.com", "kibitzer");
                SortableBindingList<IndianCalendarEvent> test = cApi.getEvents(calendarGetEvents_startDate.Value, calendarGetEvents_endDate.Value, calendarGetEvents_SearchTextbox.Text);
                SpreadSheetAPI ssa = new SpreadSheetAPI("Pair Results City And Event Names", "indianbridge.dummy@gmail.com", "kibitzer");
                DataTable table = ssa.getValuesFromSheet("Sheet1");
                e.Result = Tuple.Create<SortableBindingList<IndianCalendarEvent>, DataTable>(test, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                e.Result = null;
            }
        }

        private bool loadSummary()
        {
            DialogResult result = SelectSummaryFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    String text = File.ReadAllText(SelectSummaryFileDialog.FileName);
                    String summaryText = Utilities.compressText_(text);
                    m_eventInformation = PairsGeneral.getEventInformation_(summaryText);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Exception when trying to read Summary file : " + exception.Message);
                    return false;
                }
                return true;
            }
            return false;
        }

        private void getCalendarEvents_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Tuple<SortableBindingList<IndianCalendarEvent>, DataTable> info = e.Result as Tuple<SortableBindingList<IndianCalendarEvent>, DataTable>;
                this.cityEvents = info.Item2;
                this.dataGridView1.DataSource = info.Item1;
                this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Descending);
                this.dataGridView1.Columns[this.dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[1].DefaultCellStyle.Format = "MMMM dd, yyyy";
            }
            this.Enabled = true;
            if (loadingEventsForm != null)
            {
                loadingEventsForm.Close();
                loadingEventsForm.Dispose();
            }
        }

        private string getCityName(DataTable table, string location)
        {
            if (table == null) return "Other Cities";
            // For each row, print the values of each column. 
            foreach (DataRow row in table.Rows)
            {
                string cityName = row["City_Name"].ToString();
                if (location.IndexOf(cityName, StringComparison.OrdinalIgnoreCase) >= 0) return cityName;
            }
            return "Other Cities";
        }

        private string getEventName(DataTable table, string cityName, string title)
        {
            if (table == null) return "Other Events";
            DataRow row = table.Rows.Find(cityName);
            if (row == null) return "Other Events";
            string eventName = row["Event_Names"].ToString();
            string[] tokens = eventName.Split(',');
            foreach (string token in tokens)
            {
                if (title.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) return token;
            }
            return "Other Events";
        }

        private bool loadSummaryIntoDatabase()
        {
            status.Text += "Loading Results into Database"+Environment.NewLine;
            status.Refresh();
            PairsSummaryToDatabase std = new PairsSummaryToDatabase(m_eventInformation);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                std.loadSummaryIntoDatabase();
                m_databaseParameters = std.getDatabaseParameters();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to load results to database/n" + e.Message);
                return false;
            }
            return true;
        }

        private bool createWebpages()
        {
            status.Text += "Creating Local webpages from results database"+Environment.NewLine;
            status.Refresh();
            PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                dtw.createWebpages_();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to load create local webpages from results database/n" + e.Message);
                return false;
            }
            return true;
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string eventTitle = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            DateTime eventDate = (DateTime)dataGridView1[1, dataGridView1.CurrentRow.Index].Value;
            string eventLocation = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
            if (loadSummary())
            {
                checkEventInformation cei = new checkEventInformation(eventTitle, eventDate, m_eventInformation.eventName, m_eventInformation.eventDate);
                cei.ShowDialog();
                if (cei.eventInformationOK)
                {
                    var cityName = getCityName(cityEvents, eventLocation);
                    var eventName = getEventName(cityEvents, cityName, eventTitle);
                    if (eventName == "Other Events")
                    {
                        string pageName = Utilities.makeIdentifier_(eventTitle + "  " + eventDate.ToString("yyyy_MM_dd"));
                        m_eventInformation.databaseFileName = Path.Combine(Globals.m_rootDirectory, "Databases", cityName, pageName)+".mdb";
                        m_eventInformation.webpagesDirectory = Path.Combine(Globals.m_rootDirectory, "Webpages", cityName, pageName);
                        googleSiteRootPageName = "results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventTitle + "  " + eventDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        m_eventInformation.databaseFileName =
                            Path.Combine(Path.Combine(Globals.m_rootDirectory, "Databases", cityName, eventName), eventDate.ToString("yyyy_MM_dd"))+".mdb";
                        m_eventInformation.webpagesDirectory =
                            Path.Combine(Path.Combine(Globals.m_rootDirectory, "Webpages", cityName, eventName), eventDate.ToString("yyyy_MM_dd"));
                        googleSiteRootPageName = "results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventName) + "/" + eventDate.ToString("yyyy-MM-dd");
                    }
                    if (loadSummaryIntoDatabase())
                    {
                        createWebpages();
                    }
                }
            }
        }

    }
}
