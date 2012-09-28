using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IndianBridgeScorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new PDScorer("Test",@"C:\Temp\myDatabase.mdb","",""));
            Application.Run(new TourneyManagement());
        }
    }
}
