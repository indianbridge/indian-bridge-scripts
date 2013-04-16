using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.IO;

namespace IndianBridgeScorer
{
    public partial class CreateNewEvents : Form
    {
        Dictionary<string, DataView> m_dataViews = new Dictionary<string, DataView>();
        Dictionary<int, Dictionary<string, bool[]>> m_selectedTeams = new Dictionary<int, Dictionary<string, bool[]>>();
        Dictionary<string, int> m_numberOfQualifiers = new Dictionary<string, int>();
        private int m_numberOfNewEvents = 0;
        private int m_totalQualifiers = 0;
        private int m_qualifiersPerEvent = 0;
        private int m_numberOfSelectedEvents = 0;
        private List<string> m_selectedEventNames = new List<string>();
        private bool m_isKnockout = false;
        private int m_currentTabNumber = 0;
        private string m_databaseFileName = "";

        public CreateNewEvents(bool isKnockout)
        {
            m_isKnockout = isKnockout;
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            createEventsButton.Text = (m_isKnockout ? "Create Knockout Events" : "Create Swiss League Events");
            loadSwissLeagues();
            update();
        }

        private void loadSwissLeagues()
        {
            Label label = new Label();
            label.Text = "Events to Select Teams from : ";
            eventsListPanel.Controls.Clear();
            eventsListPanel.Controls.Add(label);
            DataTable table = AccessDatabaseUtilities.getDataTable(Constants.getCurrentTourneyEventsFileName(), Constants.TableName.TourneyEvents);
            foreach (DataRow dRow in table.Rows)
            {
                string eventType = AccessDatabaseUtilities.getStringValue(dRow, "Event_Type");
                string eventName = AccessDatabaseUtilities.getStringValue(dRow, "Event_Name");
                if (eventType == Constants.EventType.TeamsSwissLeague)
                {
                    CheckBox cb = new CheckBox();
                    cb.Text = eventName;
                    cb.AutoSize = true;
                    cb.Click +=new EventHandler(eventCheckBoxes_Click);
                    eventsListPanel.Controls.Add(cb);
                }
            }
        }

        void eventCheckBoxes_Click(object sender, EventArgs e)
        {
            update();
        }

        private void update()
        {
            bool result = int.TryParse(numberOfNewEventsTextbox.Text, out m_numberOfNewEvents);
            if (!result || m_numberOfNewEvents < 1)
            {
                Utilities.showErrorMessage("Number of New Events specified is not a valid integer");
                return;
            }
            m_numberOfSelectedEvents = 0;
            m_totalQualifiers = 0;
            m_selectedEventNames.Clear();
            for (int i = 1; i <= m_numberOfNewEvents; ++i) m_selectedTeams[i] = new Dictionary<string, bool[]>();
            foreach (Control control in eventsListPanel.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox cb = control as CheckBox;
                    if (cb.Checked) {
                        string eventName = cb.Text;
                        m_selectedEventNames.Add(eventName);
                        loadTeams(eventName);
                        NiniUtilities.loadNiniConfig(Constants.getEventInformationFileName(eventName));
                        m_numberOfSelectedEvents++;
                        m_numberOfQualifiers[eventName] = NiniUtilities.getIntValue(Constants.getEventInformationFileName(eventName), Constants.NumberOfQualifiersFieldName);
                        m_totalQualifiers += m_numberOfQualifiers[eventName];
                        selectTeams(eventName);
                    }
                }
            }
            totalQualifiersTextbox.Text = "" + m_totalQualifiers;
            m_qualifiersPerEvent = m_totalQualifiers / m_numberOfNewEvents;
            qualifiersPerEventTextbox.Text = "" + m_qualifiersPerEvent;
            loadNewEventTabs();
        }

        private void selectTeams(string eventName)
        {
            int numberOfTeams = m_dataViews[eventName].Count;
            for (int i = 1; i <= m_numberOfNewEvents; ++i)
            {
                m_selectedTeams[i][eventName] = new bool[numberOfTeams];
                for (int j = 0; j < numberOfTeams; ++j) m_selectedTeams[i][eventName][j] = false;
            }
            int count = 1;
            int numberOfQualifiers = m_numberOfQualifiers[eventName];
            while (count < numberOfQualifiers)
            {
                for (int j = 1; j <= m_numberOfNewEvents; ++j)
                {
                    m_selectedTeams[j][eventName][count - 1] = true;
                    m_selectedTeams[j][eventName][numberOfQualifiers - count] = true;
                    count++;
                }
                if (count < numberOfQualifiers)
                {
                    for (int j = m_numberOfNewEvents; j >= 1; --j)
                    {
                        m_selectedTeams[j][eventName][count - 1] = true;
                        m_selectedTeams[j][eventName][numberOfQualifiers - count] = true;
                        count++;
                    }
                }
            }
        }

        private void loadNewEventTabs()
        {
            splitContainer1.Panel2.Controls.Clear();
            TabControl mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(mainTabControl);
            for (int i = 1; i <= m_numberOfNewEvents; ++i)
            {
                string tabName = "Select Qualifiers for Event " + i;           
                loadTab(mainTabControl,tabName,i);
            }
        }

        private void loadTab(TabControl mainTabControl, string tabName, int tabNumber)
        {
            mainTabControl.TabPages.Add(tabName, tabName);
            TabPage tab = mainTabControl.TabPages[tabName];
            tab.Controls.Clear();
            SplitContainer mainPanel = new SplitContainer();
            mainPanel.Orientation = Orientation.Horizontal;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.SplitterDistance = 5;
            tab.Controls.Add(mainPanel);
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            mainPanel.Panel1.Controls.Add(flowPanel);
            Label label = new Label();
            label.Text = "Name of this Event :";
            flowPanel.Controls.Add(label);
            TextBox tb = new TextBox();
            tb.TextChanged += new EventHandler(tb_TextChanged);
            tb.Text = "New"+(m_isKnockout?"Knockout":"Swiss")+"_" + tabNumber;
            tb.Name = "eventNameTextbox_" + tabNumber;
            flowPanel.Controls.Add(tb);
            TabControl subTabControl = new TabControl();
            subTabControl.Dock = DockStyle.Fill;
            subTabControl.BringToFront();
            mainPanel.Panel2.Controls.Add(subTabControl);
            int tabCount = 1;
            foreach(string eventName in m_selectedEventNames) {
                loadSubTab(subTabControl, eventName, tabNumber,tabCount++);
            }
        }

        private void loadSubTab(TabControl subTabControl, string eventName,int parentTabNumber, int tabNumber)
        {
            subTabControl.TabPages.Add(eventName, eventName);
            TabPage tab = subTabControl.TabPages[eventName];
            if (m_dataViews.ContainsKey(eventName) && m_dataViews[eventName] != null)
            {
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.AutoScroll = true;
                tab.Controls.Add(panel);
                int count = 1;
                foreach (DataRowView rowView in m_dataViews[eventName])
                {
                    DataRow dRow = rowView.Row;
                    string text = dRow["Team_Name"] + " (" + dRow["Rank"] + ")";
                    CheckBox cb = new CheckBox();
                    cb.AutoSize = true;
                    cb.Text = text;
                    cb.Tag = new Tuple<int, string,int>(parentTabNumber,eventName, count - 1);
                    cb.Click += new EventHandler(selectTeam_Click);
                    cb.Checked = m_selectedTeams[parentTabNumber][eventName][count - 1];
                    panel.Controls.Add(cb);
                    count++;
                }
            }
        }

        void selectTeam_Click(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Tuple<int, string, int> value = (Tuple<int, string, int>)cb.Tag;
            m_selectedTeams[value.Item1][value.Item2][value.Item3] = cb.Checked;
        }

        void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            Size size = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            textBox1.Width = size.Width;
            textBox1.Height = size.Height;
        }



        private void loadTeams(string eventName)
        {
            DataTable table = AccessDatabaseUtilities.loadDatabaseToTable(Constants.getEventScoresFileName(eventName), Constants.TableName.EventNames);
            m_dataViews[eventName] = new DataView(table);
            m_dataViews[eventName].Sort = "Rank ASC, Tiebreaker_Score ASC";
        }

        private void numberOfRoundRobinsTextbox_TextChanged(object sender, EventArgs e)
        {
            update();
        }

        private void cancelCreateEventsButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(createEventsButton.Text))
            {
                this.Close();
            }
        }

        private void createEventsButton_Click(object sender, EventArgs e)
        {
            if (checkAssignments())
            {
                for (int i = 1; i <= m_numberOfNewEvents; ++i)
                {
                    createEvent(i);
                }
                Utilities.showBalloonNotification("Created " + m_numberOfNewEvents + " new " + (m_isKnockout ? "Knockout" : "Swiss League") + " events", "Creation Done");
                this.Close();
            }
        }

        private void createEvent(int tabNumber)
        {
            if (m_isKnockout) createKnockoutEvent(tabNumber);
            else createSwissLeagueEvent(tabNumber);

        }

        private void createKnockoutEvent(int tabNumber)
        {
            m_currentTabNumber = tabNumber;
            TextBox tb = this.Controls.Find("eventNameTextbox_" + tabNumber, true)[0] as TextBox;
            string eventName = tb.Text;
            string rootFolder = Path.Combine(Constants.getCurrentTourneyDatabasesFolder(), Utilities.makeIdentifier_(eventName));
            if (Directory.Exists(rootFolder))
            {
                DialogResult result = MessageBox.Show("An event already exists at " + rootFolder + ". Do you want to overwrite all contents?", "Event Exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                Directory.Delete(rootFolder, true);
                Directory.Delete(Path.Combine(Constants.getCurrentTourneyWebpagesFolder(), Utilities.makeIdentifier_(eventName)), true);
            }
            if (!addEvent(eventName, Constants.EventType.TeamsKnockout)) return;
            m_databaseFileName = Constants.getKnockoutEventScoresFileName(eventName);
            KnockoutSessions knockoutSessions = new KnockoutSessions(eventName, Constants.getKnockoutEventInfoFileName(eventName),
                Constants.getKnockoutEventScoresFileName(eventName), true);
            knockoutSessions.NumberOfTeams = m_qualifiersPerEvent;
            populateKnockoutTables();
            knockoutSessions.initializeMatches();
        }


        private void populateKnockoutTables()
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            int count = 0;
            foreach (string eventName in m_selectedEventNames)
            {
                for (int i = 0; i < m_dataViews[eventName].Count; ++i)
                {
                    if (m_selectedTeams[m_currentTabNumber][eventName][i])
                    {
                        DataRow dRow = m_dataViews[eventName][i].Row;
                        int originalTeamNumber = AccessDatabaseUtilities.getIntValue(dRow, "Team_Number");
                        string teamName = AccessDatabaseUtilities.getStringValue(dRow, "Team_Name");
                        string memberNames = AccessDatabaseUtilities.getStringValue(dRow, "Member_Names");
                        int teamNumber = count+1;
                        DataRow newRow = table.Rows[count];
                        newRow["Team_Number"] = teamNumber;
                        newRow["Team_Name"] = teamName;
                        newRow["Member_Names"] = memberNames;
                        newRow["Original_Team_Number"] = originalTeamNumber;
                        newRow["Original_Event_Name"] = eventName;
                        count++;
                    }
                }
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutTeams);
        }

        private void createSwissLeagueEvent(int tabNumber)
        {
            m_currentTabNumber = tabNumber;
            TextBox tb = this.Controls.Find("eventNameTextbox_" + tabNumber, true)[0] as TextBox;
            string eventName = tb.Text;
            string rootFolder = Path.Combine(Constants.getCurrentTourneyDatabasesFolder(), Utilities.makeIdentifier_(eventName));
            if (Directory.Exists(rootFolder))
            {
                DialogResult result = MessageBox.Show("An event already exists at " + rootFolder + ". Do you want to overwrite all contents?", "Event Exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                Directory.Delete(rootFolder, true);
                Directory.Delete(Path.Combine(Constants.getCurrentTourneyWebpagesFolder(), Utilities.makeIdentifier_(eventName)), true);
            }
            if (!addEvent(eventName, Constants.EventType.TeamsSwissLeague)) return;
            SwissTeamEventInfo swissTeamEventInfo = new SwissTeamEventInfo(eventName, Constants.getEventInformationFileName(eventName), true);
            swissTeamEventInfo.NumberOfBoardsPerRound = 8;
            swissTeamEventInfo.NumberOfTeams = m_qualifiersPerEvent;
            swissTeamEventInfo.NumberOfRounds = m_qualifiersPerEvent - 1;
            swissTeamEventInfo.NumberOfQualifiers = m_qualifiersPerEvent / 2;
            m_databaseFileName = Constants.getEventScoresFileName(eventName);
            createSwissLeagueDatabases(eventName, swissTeamEventInfo.NumberOfTeams, swissTeamEventInfo.NumberOfRounds);
        }

        private void createSwissLeagueDatabases(string eventName, int numberOfTeams, int numberOfRounds)
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);
            createSwissLeagueTeamsTable();
            createSwissLeagueScoresTable();
            createSwissLeagueComputedScoresTable(numberOfRounds);
            populateSwissLeagueTables(numberOfRounds, numberOfTeams);
        }

        private void populateSwissLeagueTables(int numberOfRounds, int numberOfTeams)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventNames);
            int count = 1;
            DataTable computedScoresTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventComputedScores);
            foreach (string eventName in m_selectedEventNames)
            {
                for (int i = 0; i < m_dataViews[eventName].Count; ++i)
                {
                    if (m_selectedTeams[m_currentTabNumber][eventName][i])
                    {
                        DataRow dRow = m_dataViews[eventName][i].Row;
                        int originalTeamNumber = AccessDatabaseUtilities.getIntValue(dRow, "Team_Number");
                        string teamName = AccessDatabaseUtilities.getStringValue(dRow, "Team_Name");
                        string memberNames = AccessDatabaseUtilities.getStringValue(dRow, "Member_Names");
                        double totalScore = AccessDatabaseUtilities.getDoubleValue(dRow, "Total_Score");
                        int teamNumber = count;
                        DataRow newRow = table.NewRow();
                        newRow["Team_Number"] = keepOriginalTeamNumbersCheckbox.Checked?originalTeamNumber:teamNumber;
                        newRow["Team_Name"] = teamName;
                        newRow["Member_Names"] = memberNames;
                        newRow["Carryover"] = carryoverTotalCheckbox.Checked ? totalScore : 0;
                        newRow["Original_Team_Number"] = originalTeamNumber;
                        newRow["Original_Event_Name"] = eventName;
                        newRow["Total_Score"] = 0;
                        newRow["Tiebreaker_Score"] = 0;
                        newRow["Rank"] = 1;
                        table.Rows.Add(newRow);
                        newRow = computedScoresTable.NewRow();
                        newRow["Team_Number"] = keepOriginalTeamNumbersCheckbox.Checked ? originalTeamNumber : teamNumber;
                        computedScoresTable.Rows.Add(newRow);
                        count++;
                    }
                }
                DataTable scoresTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventScores);
                int numberOfMatches = (numberOfTeams / 2) + numberOfTeams % 2;
                for (int i = 1; i <= numberOfRounds; ++i)
                {
                    for (int j = 1; j <= numberOfMatches; ++j)
                    {
                        DataRow dRow = scoresTable.NewRow();
                        dRow["Table_Number"] = j;
                        dRow["Round_Number"] = i;
                        scoresTable.Rows.Add(dRow);
                    }
                }
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventNames);
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventScores);
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventComputedScores);
        }

        private void createSwissLeagueTeamsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Member_Names", "TEXT", 255));
            fields.Add(new DatabaseField("Carryover", "NUMBER"));
            fields.Add(new DatabaseField("Original_Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Original_Event_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Total_Score", "NUMBER"));
            fields.Add(new DatabaseField("Tiebreaker_Score", "NUMBER"));
            fields.Add(new DatabaseField("Rank", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventNames, fields, primaryKeys);
        }

        private void createSwissLeagueScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Round_Number", "INTEGER"));
            fields.Add(new DatabaseField("Table_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_2_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_1_VPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_VPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_1_VP_Adjustment", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_VP_Adjustment", "NUMBER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Round_Number");
            primaryKeys.Add("Table_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventScores, fields, primaryKeys);
        }

        private void createSwissLeagueComputedScoresTable(int numberOfRounds)
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            List<string> primaryKeys = new List<string>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            primaryKeys.Add("Team_Number");
            for (int i = 1; i <= numberOfRounds; ++i)
            {
                fields.Add(new DatabaseField("Score_After_Round_" + i, "NUMBER"));
                fields.Add(new DatabaseField("Rank_After_Round_" + i, "NUMBER"));
                fields.Add(new DatabaseField("Tiebreaker_After_Round_" + i, "NUMBER"));
            }
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventComputedScores, fields, primaryKeys);
        }

        private bool addEvent(string eventName, string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event Name cannot be empty!", "Empty Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            DataTable table = AccessDatabaseUtilities.getDataTable(Constants.getCurrentTourneyEventsFileName(), Constants.TableName.TourneyEvents);
            DataRow dRow = table.Rows.Find(eventName);
            if (dRow != null)
            {
                MessageBox.Show("Another event with same name (" + eventName + ") already exists!" + Environment.NewLine + "Either delete the other event first or provide a different event name!", "Duplicate Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            dRow = table.NewRow();
            dRow["Event_Name"] = eventName;
            dRow["Event_Type"] = eventType;
            table.Rows.Add(dRow);
            AccessDatabaseUtilities.saveTableToDatabase(Constants.getCurrentTourneyEventsFileName(), Constants.TableName.TourneyEvents);
            return true;
        }

        private bool checkAssignments()
        {
            bool result = true;
            string message = "";

            // Count in each round robin
            for (int i = 1; i <= m_numberOfNewEvents; ++i)
            {
                int numberOfSelectedTeams = 0;
                foreach (string eventName in m_selectedEventNames) numberOfSelectedTeams += m_selectedTeams[i][eventName].Count(c => c);
                if (numberOfSelectedTeams != m_qualifiersPerEvent)
                {
                    Utilities.appendToMessage(ref message, "Number of Selected teams (adding teams selected from all the existing events) in for Event " + i + " (" + numberOfSelectedTeams + ") is not the same as number of qualifiers per Event (" + m_qualifiersPerEvent + ")");
                    result = false;
                }
            }
            // Check if same team in multiple round robins
            foreach (string eventName in m_selectedEventNames)
            {
                for (int i = 0; i < m_dataViews[eventName].Count; ++i)
                {
                    string extra = "";
                    if (appearsInMoreThanOneEvent(eventName, i, out extra))
                    {
                        result = false;
                        Utilities.appendToMessage(ref message, extra);
                    }
                }
            }
            if (!result) Utilities.showErrorMessage("Errors in assignments as noted" + Environment.NewLine + message);
            return result;

        }

        private bool appearsInMoreThanOneEvent(string eventName, int teamNumber, out string message)
        {
            message = "";
            bool result = false;
            for (int i = 1; i <= m_numberOfNewEvents; ++i)
            {
                if (m_selectedTeams[i][eventName][teamNumber])
                {
                    if (!string.IsNullOrWhiteSpace(message)) result = true;
                    Utilities.appendToMessage(ref message, "Team " + (teamNumber + 1) + " from "+eventName+" appears in Event " + i);
                }
            }
            return result;
        }

    }
}
