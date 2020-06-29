using System;
using System.IO;
using System.Net.Cache;
using System.Windows;
using Microsoft.Win32;

namespace ImageViewer
{
    public class MainWindow : Window
    {
        public MainWindow(FileInfo imagePath)
        {
            if(imagePath == null)
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = false;
                fileDialog.ShowDialog();
                if(string.IsNullOrEmpty(fileDialog.FileName))
                {
                    return;
                }
                imagePath = new FileInfo(fileDialog.FileName);
            }
            SizeToContent = SizeToContent.WidthAndHeight;
            Content = new MainView(imagePath);
        }
    }
}