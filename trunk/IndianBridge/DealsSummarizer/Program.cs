using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;

namespace DealsSummarizer
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds = new DataSet();
            string VPScaleTableName = "VPScales";
            //string VPScaleDatabaseFileName = Path.Combine(Directory.GetCurrentDirectory(), "Databases", "VPScales.mdb");
            string strAccessConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\snarasim\Documents\Bridge\indian-bridge-scripts\IndianBridge\IndianBridgeScorer\Databases\VPScales.mdb;";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            int numberOfBoards = 8;
            string sql = "SELECT * From VPScales WHERE VP_Scale=30 AND Number_Of_Boards_Lower<=" + numberOfBoards + " AND Number_Of_Boards_Upper>=" + numberOfBoards;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, myAccessConn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            if (ds.Tables[VPScaleTableName] != null)
            {
                ds.Tables[VPScaleTableName].Clear();
            }
            da.Fill(ds, VPScaleTableName);
            DataTable table = ds.Tables[VPScaleTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["VP_Scale"], table.Columns["Number_Of_Boards_Lower"], table.Columns["Number_Of_Boards_Upper"], table.Columns["Number_Of_IMPs_Lower"], table.Columns["Number_Of_IMPs_Upper"] };
            myAccessConn.Close();
            myAccessConn = null;

            /*string strAccessConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\snarasim\Documents\Bridge\indian-bridge-scripts\IndianBridge\IndianBridgeScorer\Databases\VPScales.mdb;";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string tableName = "VPScales";
            string sql = "SELECT * From "+tableName;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, myAccessConn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.Fill(ds, tableName);
            DataTable table = ds.Tables[tableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["VP_Scale"], table.Columns["Number_Of_Boards_Lower"], table.Columns["Number_Of_Boards_Upper"], table.Columns["Number_Of_IMPs_Lower"], table.Columns["Number_Of_IMPs_Upper"] };
            myAccessConn.Close();
            myAccessConn = null;
            StreamReader readFile = new StreamReader(@"C:\Users\snarasim\Documents\Bridge\indian-bridge-scripts\IndianBridge\IndianBridgeScorer\Databases\VPScale_30.csv");
            string line;
            string[] boards;
            line = readFile.ReadLine();
            boards = line.Split(',');
            string[] tokens;
            while ((line = readFile.ReadLine()) != null)
            {
                int lastBoard = -1;
                tokens = line.Split(',');
                string vps = tokens[0];
                string[] vpTokens = vps.Split('-');
                int team1vps = int.Parse(vpTokens[0].Trim());
                int team2vps = int.Parse(vpTokens[1].Trim());
                for (int i = 1; i < tokens.Length; ++i)
                {
                    string imps = tokens[i];
                    int imps_lower,imps_upper;
                    if (imps.Contains('+')) {
                        string[] impTokens = imps.Split('+');
                        imps_lower = int.Parse(impTokens[0].Trim());
                        imps_upper = 10000;
                    }
                    else {
                        string[] impTokens = imps.Split('-');
                        imps_lower = int.Parse(impTokens[0].Trim());
                        imps_upper = int.Parse(impTokens[1].Trim());
                    }
                    int boards_lower, boards_upper;
                    string board = boards[i];
                    if (board.Contains('-'))
                    {
                        string[] boardTokens = board.Split('-');
                        boards_lower = int.Parse(boardTokens[0].Trim());
                        boards_upper = int.Parse(boardTokens[1].Trim());
                    }
                    else
                    {
                        boards_lower = lastBoard + 1;
                        boards_upper = int.Parse(board.Trim());
                    }
                    lastBoard = boards_upper;
                    DataRow dRow = table.NewRow();
                    dRow["VP_Scale"] = 30;
                    dRow["Number_Of_Boards_Lower"] = boards_lower;
                    dRow["Number_Of_Boards_Upper"] = boards_upper;
                    dRow["Number_Of_IMPs_Lower"] = imps_lower;
                    dRow["Number_Of_IMPs_Upper"] = imps_upper;
                    dRow["Team_1_VPs"] = team1vps;
                    dRow["Team_2_VPs"] = team2vps;
                    table.Rows.Add(dRow);
                }
            }
            da.Update(ds, tableName);*/

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return;
            //try
            //{
            //    int totalHands = 0;
            //    int NTbetter = 0;
            //    //int handsMaking6N = 0;
            //    //int handsMaking6HW = 0;
            //    //int handsMaking6HE = 0;
            //    //int handsMaking2H = 0;
            //    //int handsWith4CardMajor = 0;
            //    //int gamesMaking = 0;
            //    //int gamesMakingWith4CardMajor = 0;
            //    // Create an instance of StreamReader to read from a file.
            //    // The using statement also closes the StreamReader.
            //    const string appPath = @"C:\Users\snarasim\Downloads\deal319win\deal319\output.txt";
            //    const string outputPath = @"C:\Users\snarasim\Downloads\deal319win\deal319\output_slam.html";
            //    System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath);
            //    file.WriteLine("<html><head>");
            //    file.WriteLine("<style type=\"text/css\">");
            //    file.WriteLine(".NTbetter {font-weight:bold;}");
            //    //file.WriteLine(".Making6HW {color:blue;}");
            //    //file.WriteLine(".Making6HE {background-color:#b0c4de;}");
            //    file.WriteLine("</style>");
            //    file.WriteLine("</head><body>");
            //    file.WriteLine("<div><span class='Making6N'>Text</span> - Making 6NT</div>");
            //    //file.WriteLine("<div><span class='Making2H'>Text</span> - Making 2H</div>");
            //    file.WriteLine("<div><span class='Making6HW'>Text</span> - Making 6H West</div>");
            //    file.WriteLine("<div><span class='Making6HE'>Text</span> - Making 6H East</div>");
            //    using (StreamReader sr = new StreamReader(appPath))
            //    {
            //        String line;
            //        // Read and display lines from the file until the end of
            //        // the file is reached.
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            //bool has4CardMajors = false;
            //            totalHands++;
            //            string[] tokens = line.Split('|');
            //            /*string[] hands = tokens[0].Split('.');
            //            if (hands[0].Length == 4 || hands[1].Length == 4)
            //            {
            //                has4CardMajors = true;
            //                handsWith4CardMajor++;
            //            }*/
            //            //string[] ntricks = tokens[4].Split(' ');
            //            string[] etricks = tokens[5].Split(' ');
            //            string[] wtricks = tokens[7].Split(' ');
            //            bool NTisbetter = false;
            //           // bool making6HW = false;
            //            //bool making6HE = false;
            //            //bool making2H = false;
            //            if (Convert.ToInt32(wtricks[4]) >= Convert.ToInt32(wtricks[1]) && Convert.ToInt32(wtricks[4]) >= Convert.ToInt32(etricks[1])) {
            //                NTbetter++;
            //                NTisbetter = true;
            //            }
            //            /*if (Convert.ToInt32(stricks[1]) > 7)
            //            {
            //                handsMaking2H++;
            //                making2H = true;
            //            }*/
            //            /*if (Convert.ToInt32(wtricks[1]) > 11)
            //            {
            //                handsMaking6HW++;
            //                making6HW = true;
            //            }
            //            if (Convert.ToInt32(etricks[1]) > 11)
            //            {
            //                handsMaking6HE++;
            //                making6HE = true;
            //            }*/
            //            /*if (Convert.ToInt32(stricks[0]) > 9 || Convert.ToInt32(stricks[1]) > 9 || Convert.ToInt32(stricks[4]) > 8)
            //            {
            //                gamesMaking++;
            //            }*/
            //            /*|| Convert.ToInt32(ntricks[1]) > 9 || Convert.ToInt32(stricks[0]) > 9 || Convert.ToInt32(stricks[1]) > 9)
            //            {
            //                gamesMaking++;
            //                makingGame = true;
            //                if (hands[0].Length == 4 || hands[1].Length == 4) gamesMakingWith4CardMajor++;
            //            }*/
            //            file.WriteLine("<div class='" + (NTisbetter ? "NTbetter" : "") + "'>" + line + "</div>");
            //        }
            //        string footer = "Total = " + totalHands + " NT is better = " + NTbetter + " H is better = " + (totalHands-NTbetter);
            //        file.WriteLine("<h2>" + footer + "</h2>");
            //        Console.WriteLine(footer);
            //    }
            //    file.WriteLine("</body></html>");
            //    file.Close();
            //}
            //catch (Exception e)
            //{
            //    // Let the user know what went wrong.
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}

        }
    }
}
