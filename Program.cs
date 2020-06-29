using System;
using System.IO;
using System.Windows;

namespace ImageViewer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            application.Run(new MainWindow(args.Length >= 1 ? new FileInfo(args[0]) : null));
        }
    }
}