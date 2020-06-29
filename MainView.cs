using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    public class MainView : StackPanel
    {
        public MainView(FileInfo imagePath)
        {
            var image = new Image();
            LoadImage(imagePath, image);
            Children.Add(image);
            StartFileWatcher(imagePath, image);
        }

        private void StartFileWatcher(FileInfo imagePath, Image image)
        {
            var fileSystemWatcher = new FileSystemWatcher(imagePath.Directory.FullName, imagePath.Name);
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            fileSystemWatcher.Changed += (s, e) => Dispatcher.Invoke(() => LoadImage(imagePath, image));
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void LoadImage(FileInfo imagePath, Image image)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream();
            using(var imageStream = imagePath.OpenRead())
            {
                imageStream.CopyTo(bitmapImage.StreamSource);
                bitmapImage.StreamSource.Position = 0;
                imageStream.Close();
            }
            try
            {
                bitmapImage.EndInit();
            }
            catch(NotSupportedException exc)
            {
                MessageBox.Show("Unable to open image: " + exc.Message);
                Application.Current.MainWindow.Close();
            }
            image.Stretch = Stretch.None;
            image.Source = bitmapImage;
        }

    }
}