using System;
using System.IO;
using System.Threading;
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
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream();
            
                while(true)
                {
                    try
                    {
                        using(var imageStream = imagePath.OpenRead())
                        {
                            imageStream.CopyTo(bitmapImage.StreamSource);
                            imageStream.Close();
                            bitmapImage.StreamSource.Position = 0;
                        }

                        break;
                    }
                    catch(IOException exc)
                    {
                        if(exc.Message.StartsWith("The process cannot access the file"))
                        {
                            Thread.Sleep(10);
                        }
                        else throw;
                    }
                }
                bitmapImage.EndInit();
                image.Stretch = Stretch.None;
                image.Source = bitmapImage;
            }
            catch(Exception exc)
            {
                MessageBox.Show("Unable to open image: " + exc.Message);
                Application.Current.MainWindow.Close();
            }
        }
    }
}