using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.Data;
using System.Windows.Forms;

namespace IndianBridge.Common
{
    public struct NiniField
    {
        public string fieldName;
        public string fieldType;
        public string fieldValue;
        public string fieldSource;

        public NiniField(string name, string type, string value, string source = "")
        {
            fieldName = name;
            fieldType = type;
            fieldValue = value;
            fieldSource = source;
        }
    }
    public static class NiniUtilities
    {
        public static Dictionary<string, IniConfigSource> m_niniFiles = new Dictionary<string, IniConfigSource>();

        public static void createNiniFile(string fileName, List<NiniField> fields)
        {
            IniConfigSource source = new IniConfigSource();
            foreach (NiniField field in fields)
            {
                IConfig config = source.AddConfig(field.fieldName);
                config.Set("Type", field.fieldType);
                config.Set("Value", field.fieldValue);
                config.Set("Source", field.fieldSource);
            }

            source.Save(fileName);
        }

        public static void loadNiniConfig(string fileName)
        {
            IniConfigSource source = new IniConfigSource(fileName);
            m_niniFiles[fileName] = source;
        }

        public static DataGridView loadNiniConfigToDataGridView(string fileName)
        {
            IniConfigSource source = new IniConfigSource(fileName);
            m_niniFiles[fileName] = source;
            DataGridView dgv = new DataGridView();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.ColumnCount = 3;
            dgv.Columns[0].Name = "Parameter_Name";
            dgv.Columns[1].Name = "Parameter_Value";
            dgv.Columns[2].Name = "Parameter_Type";
            dgv.Columns[2].Visible = false;
            dgv.RowCount = source.Configs.Count+1;
            int i = 0;
            foreach (IConfig config in source.Configs)
            {
                DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
                dgv[0, i] = textBoxCell;
                dgv[0, i].Value = config.Name;
                dgv[2, i].Value = config.GetString("Type");
                switch (config.GetString("Type").ToLower())
                {
                    case "boolean":
                        dgv[1,i] = new DataGridViewCheckBoxCell();
                        dgv[1, i].Value = config.GetBoolean("Value");
                        break;
                    case "list":
                        DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                        comboBoxCell.Items.AddRange(config.GetString("Source").Split(','));
                        dgv[1, i] = comboBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        break;
                    case "integer":
                    case "string":
                    case "number":
                    default:
                        textBoxCell = new DataGridViewTextBoxCell();
                        dgv[1, i] = textBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        break;
                }
                i++;
            }
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            return dgv;
        }

        public static void loadNiniConfigToDataGridView(string fileName, DataGridView dgv)
        {
            IniConfigSource source = new IniConfigSource(fileName);
            m_niniFiles[fileName] = source;
            dgv.DataSource = null;
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            DataTable table = new DataTable();
            table.Columns.Add("Parameter_Name");
            table.Columns.Add("Parameter_Value");
            table.Columns.Add("Parameter_Type");
            table.PrimaryKey = new DataColumn[] { table.Columns["Parameter_Name"] };
            for (int j = 0; j < source.Configs.Count; j++)
            {
                DataRow dRow = table.NewRow();
                dRow["Parameter_Name"] = "Parameter_" + j;
                table.Rows.Add(dRow);
            }
            dgv.DataSource = table;
            int i = 0;
            foreach (IConfig config in source.Configs)
            {
                DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
                dgv[0, i] = textBoxCell;
                dgv[0, i].Value = config.Name;
                switch (config.GetString("Type").ToLower())
                {
                    case "boolean":
                        dgv.Rows[i].Cells[1] = new DataGridViewCheckBoxCell();
                        dgv[1, i].Value = config.GetBoolean("Value");
                        dgv[2, i].Value = "Boolean";
                        break;
                    case "integer":
                        textBoxCell = new DataGridViewTextBoxCell();
                        dgv[1, i] = textBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        dgv[2, i].Value = "Integer";
                        break;
                    case "string":
                        textBoxCell = new DataGridViewTextBoxCell();
                        dgv[1, i] = textBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        dgv[2, i].Value = "String";
                        break;
                    case "number" :
                        textBoxCell = new DataGridViewTextBoxCell();
                        dgv[1, i] = textBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        dgv[2, i].Value = "Number";
                        break;
                    case "list":
                        DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                        comboBoxCell.Items.AddRange(config.GetString("Source").Split(','));
                        dgv[1, i] = comboBoxCell;
                        dgv[1, i].Value = config.GetString("Value");
                        dgv[2, i].Value = "List";
                        break;
                    default:
                        return;
                }
                i++;
            }
            dgv.Columns["Parameter_Type"].Visible = false;
        }

        public static int getIntValue(string fileName, string parameterName)
        {
            return getIntValue(fileName, parameterName, 0);
        }

        public static double getDoubleValue(string fileName, string parameterName)
        {
            return getDoubleValue(fileName, parameterName, 0.0);
        }

        public static bool getBooleanValue(string fileName, string parameterName)
        {
            return getBooleanValue(fileName, parameterName, false);
        }

        public static string getStringValue(string fileName, string parameterName)
        {
            return getStringValue(fileName, parameterName,"");
        }

        private static void checkParameter<T>(string fileName, string parameterName, string parameterType, T defaultValue, string source = "")
        {
            IniConfigSource configSource = m_niniFiles[fileName];
            if (configSource.Configs[parameterName] == null)
            {
                IConfig config = configSource.AddConfig(parameterName);
                config.Set("Type", parameterType);
                config.Set("Value", defaultValue);
                config.Set("Source", source);
                configSource.Save();
            }
        }

        public static int getIntValue(string fileName, string parameterName, int defaultValue)
        {
            checkParameter(fileName, parameterName, "Integer", defaultValue);
            return m_niniFiles[fileName].Configs[parameterName].GetInt("Value", defaultValue);
        }

        public static double getDoubleValue(string fileName, string parameterName, double defaultValue)
        {
            checkParameter(fileName, parameterName, "Number",defaultValue);
            return m_niniFiles[fileName].Configs[parameterName].GetDouble("Value", defaultValue);
        }

        public static bool getBooleanValue(string fileName, string parameterName, bool defaultValue)
        {
            checkParameter(fileName, parameterName,"Boolean", defaultValue);
            return m_niniFiles[fileName].Configs[parameterName].GetBoolean("Value", defaultValue);
        }

        public static string getStringValue(string fileName, string parameterName, string defaultValue, string source = "")
        {
            checkParameter(fileName, parameterName, string.IsNullOrWhiteSpace(source)?"String":"List",defaultValue,source);
            return m_niniFiles[fileName].Configs[parameterName].GetString("Value", defaultValue);
        }

        public static string getSource(string fileName, string parameterName)
        {
            return m_niniFiles[fileName].Configs[parameterName].GetString("Source");
        }
        public static void setValue<T>(string fileName, string parameterName, T parameterValue, bool save = false)
        {
            m_niniFiles[fileName].Configs[parameterName].Set("Value", parameterValue);
            if (save) m_niniFiles[fileName].Save();
        }

        public static void saveNiniConfig(string fileName)
        {
            IniConfigSource source = m_niniFiles[fileName];
            source.Save();
        }

        public static void saveDataGridViewToNiniConfig(string fileName, DataGridView dgv)
        {
            IniConfigSource source = m_niniFiles[fileName];
            foreach (DataGridViewRow dRow in dgv.Rows)
            {
                string parameterName = (string)dRow.Cells["Parameter_Name"].Value;
                string parameterType = (string)dRow.Cells["Parameter_Type"].Value;
                switch (parameterType.ToLower())
                {
                    case "boolean":
                        bool boolValue = (bool)dRow.Cells["Parameter_Value"].Value;
                        source.Configs[parameterName].Set("Value", boolValue);
                        break;
                    case "integer":
                    case "string":
                    case "number":
                    case "list":
                    default:
                        string stringValue = (string)dRow.Cells["Parameter_Value"].Value;
                        source.Configs[parameterName].Set("Value", stringValue);
                        break;
                }
            }
            source.Save();
        }

    }
}
