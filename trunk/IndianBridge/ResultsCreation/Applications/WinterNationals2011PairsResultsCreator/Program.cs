﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinterNationals2011PairsResultsCreator
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
            Application.Run(new WinterNationals2011PairsResultsCreator());
        }
    }
}