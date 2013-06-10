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
            try
            {
                int totalHands = 0;
                int handsMaking4HNS = 0;
                int handsMakingGameEW = 0;
                int handsFailing3H = 0;
                //int NTbetter = 0;
                //int handsMaking6N = 0;
                //int handsMaking6HW = 0;
                //int handsMaking6HE = 0;
                //int handsMaking2H = 0;
                //int handsWith4CardMajor = 0;
                //int gamesMaking = 0;
                //int gamesMakingWith4CardMajor = 0;
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                const string appPath = @"C:\Users\snarasim\Documents\Bridge\deal319win\deal319\roger_miller_output.txt";
                const string outputPath = @"C:\Users\snarasim\Documents\Bridge\deal319win\deal319\roger_miller_output.html";
                System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath);
                file.WriteLine("<html><head>");
                file.WriteLine("<style type=\"text/css\">");
                file.WriteLine(".NTbetter {font-weight:bold;}");
                file.WriteLine(".Making4HNS {color:blue;}");
                file.WriteLine(".MakingGameEW {background-color:#b0c4de;}");
                file.WriteLine("</style>");
                file.WriteLine("</head><body>");
                file.WriteLine("<div><span class='Making4HNS'>Text</span> - Making 4H NS</div>");
                //file.WriteLine("<div><span class='Making2H'>Text</span> - Making 2H</div>");
                //file.WriteLine("<div><span class='Making6HW'>Text</span> - Making 6H West</div>");
                file.WriteLine("<div><span class='MakingGameEW'>Text</span> - Making Game EW</div>");
                using (StreamReader sr = new StreamReader(appPath))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        //bool has4CardMajors = false;
                        totalHands++;
                        string[] tokens = line.Split('|');
                        /*string[] hands = tokens[0].Split('.');
                        if (hands[0].Length == 4 || hands[1].Length == 4)
                        {
                            has4CardMajors = true;
                            handsWith4CardMajor++;
                        }*/
                        string[] ntricks = tokens[4].Split(' ');
                        string[] etricks = tokens[5].Split(' ');
                        string[] stricks = tokens[6].Split(' ');
                        string[] wtricks = tokens[7].Split(' ');
                        //bool NTisbetter = false;
                        // bool making6HW = false;
                        //bool making6HE = false;
                        //bool making2H = false;
                        bool making4HNS = false;
                        bool makingGameEW = false;
                        //bool failing3HNS = false;
                        if (Convert.ToInt32(ntricks[1]) >= 10)
                        {
                            handsMaking4HNS++;
                            making4HNS = true;
                        }
                        else if (Convert.ToInt32(ntricks[1]) < 9)
                        {
                            handsFailing3H++;
                            //failing3HNS = true;
                        }
                        if (making4HNS && 
                            ((Convert.ToInt32(etricks[0]) >= 7 || Convert.ToInt32(etricks[1]) >= 7 || Convert.ToInt32(etricks[2]) >= 8 || Convert.ToInt32(etricks[3]) >= 8 || Convert.ToInt32(etricks[4]) >= 6) ||
                            (Convert.ToInt32(wtricks[0]) >= 7 || Convert.ToInt32(wtricks[1]) >= 7 || Convert.ToInt32(wtricks[2]) >= 8 || Convert.ToInt32(wtricks[3]) >= 8 || Convert.ToInt32(wtricks[4]) >= 6)))
                        {
                            handsMakingGameEW++;
                            makingGameEW = true;
                        }

                        /*if (Convert.ToInt32(stricks[1]) > 7)
                        {
                            handsMaking2H++;
                            making2H = true;
                        }*/
                        /*if (Convert.ToInt32(wtricks[1]) > 11)
                        {
                            handsMaking6HW++;
                            making6HW = true;
                        }
                        if (Convert.ToInt32(etricks[1]) > 11)
                        {
                            handsMaking6HE++;
                            making6HE = true;
                        }*/
                        /*if (Convert.ToInt32(stricks[0]) > 9 || Convert.ToInt32(stricks[1]) > 9 || Convert.ToInt32(stricks[4]) > 8)
                        {
                            gamesMaking++;
                        }*/
                        /*|| Convert.ToInt32(ntricks[1]) > 9 || Convert.ToInt32(stricks[0]) > 9 || Convert.ToInt32(stricks[1]) > 9)
                        {
                            gamesMaking++;
                            makingGame = true;
                            if (hands[0].Length == 4 || hands[1].Length == 4) gamesMakingWith4CardMajor++;
                        }*/
                        file.WriteLine("<div class='" + (making4HNS ? "making4HNS" : "") + (makingGameEW ? " makingGameEW" : "") + "'>" + line + "</div>");
                    }
                    string footer = "Total = " + totalHands + " 4H NS Makes = " + handsMaking4HNS + " 3H NS Fails = " + handsFailing3H + " Game Or Profitable Save EW = " + handsMakingGameEW;
                    file.WriteLine("<h2>" + footer + "</h2>");
                    Console.WriteLine(footer);
                }
                file.WriteLine("</body></html>");
                file.Close();
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
    }
}
