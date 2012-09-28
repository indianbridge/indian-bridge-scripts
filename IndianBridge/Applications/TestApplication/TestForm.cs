using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using IndianBridge.Common;
using Nini.Config;

namespace IndianBridge
{
    public partial class TestForm : Form
    {
        AppSettings appset;
        public class AppSettings
        {
            private bool saveOnClose = true;
            private string greetingText = "Welcome to your application!";
            private int itemsInMRU = 4;
            private int maxRepeatRate = 10;
            private bool settingsChanged = false;
            private int appVersion = 1;

            public bool SaveOnClose
            {
                get { return saveOnClose; }
                set { saveOnClose = value; }
            }
            public string GreetingText
            {
                get { return greetingText; }
                set { greetingText = value; }
            }
            public int MaxRepeatRate
            {
                get { return maxRepeatRate; }
                set { maxRepeatRate = value; }
            }
            public int ItemsInMRUList
            {
                get { return itemsInMRU; }
                set { itemsInMRU = value; }
            }
            public bool SettingsChanged
            {
                get { return settingsChanged; }
                set { settingsChanged = value; }
            }
            public int AppVersion
            {
                get { return appVersion; }
                set {
                    if (value < 5) appVersion = value;
                    else MessageBox.Show("Error");
                }
            }
        }
        public TestForm()
        {
            InitializeComponent();
            appset = new AppSettings();
            this.propertyGrid1.SelectedObject = appset;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "ChangedItem", e.ChangedItem);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "OldValue", e.OldValue);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "PropertyValueChanged Event");
            MessageBox.Show(""+appset.AppVersion);
        }

        private void propertyGrid1_Validating(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Test");
        }

    }
}
