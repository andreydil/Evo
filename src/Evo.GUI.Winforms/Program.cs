using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Evo.GUI.Winforms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var frm = new FrmMain();

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "-benchmark")
                {
                    int ticks = 10000;
                    if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                    {
                        if (!int.TryParse(args[1], out ticks))
                        {
                            ticks = 10000;
                        }
                    }
                    frm.EnableBenchmarking(ticks);
                }
                else
                {
                    var filename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
                    MessageBox.Show($"Usage:\r\n{filename} -benchmark [number of ticks]", "Command line arguments");
                    return;
                }
            }
            Application.Run(frm);
        }
    }
}
