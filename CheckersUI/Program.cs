using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Configuration;
using System.IO;

namespace CheckersUI
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

            // Read prolog file path from configuration
            string path = string.Empty;

            string confPath = string.Empty;
            object conf = ConfigurationManager.GetSection("PrologFile");
            if (conf is Hashtable)
            {
                Hashtable hashConf = (Hashtable)conf;
                if (hashConf.ContainsKey("path"))
                {
                    confPath = hashConf["path"].ToString();
                }
            }
            
            if (File.Exists(confPath)) path = confPath;
            else if (File.Exists("mmn17.pl")) path = "mmn17.pl";
            else
            {
                MessageBox.Show("Can't find prolog file. Please make sure you have the right path in the configuration file.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Board(path));
        }
    }
}
