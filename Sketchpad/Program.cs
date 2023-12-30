using System;
using System.Windows.Forms;

namespace Sketchpad
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Welcome we = new Welcome();
            we.ShowDialog();
            Application.Run(new MainForm());
        }
    }
}