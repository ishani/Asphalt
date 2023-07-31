using Asphalt.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asphalt.Demo
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
            Application.SetCompatibleTextRenderingDefault( false );

            Application.ApplicationExit += delegate
            {
                FontLibrary.Instance.Dispose();
            };

            Application.Run( new DemoForm() );
        }
    }
}
