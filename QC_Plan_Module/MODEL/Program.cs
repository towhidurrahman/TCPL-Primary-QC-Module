using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QC_Plan_Module.MODEL;

namespace QC_Plan_Module
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
            //Application.Run(new frmPrimaryQCModuleUI());
            Application.Run(new frmLogin());
        }
    }
}
