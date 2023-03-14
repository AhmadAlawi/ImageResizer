using Aspose.Imaging;
using Aspose.Imaging.FileFormats.Dicom;
using Aspose.Imaging.FileFormats.Jpeg;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string? Path { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private static void CompressImage(string path, string savePath)
        {
            using var image = Aspose.Imaging.Image.Load(path);
            var rasterImage = (RasterImage)image;

            // Create BmpOptions
            var saveOptions = new Aspose.Imaging.ImageOptions.JpegOptions()
            {
                CompressionType = JpegCompressionMode.Progressive,
                Quality = 5
            };

            image.Save(savePath, saveOptions);
        }

        private string DetermineSize(string path)
        {
            var info = new FileInfo(path);
            var size = info.Length * 1.0;
            var units = new[] { "B", "KB", "MB", "GB" };
            var unitIndex = 0;
            while (size > 1024)
            {
                size /= 1024.0;
                unitIndex++;
            }
            return $"{size:0.00} {units[unitIndex]}";
        }


        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;..."
            };

            if (ofd.ShowDialog() == true)
            {
                StartButton.IsEnabled = true;
                Path = ofd.FileName;
                var bitmap = new BitmapImage();
                using (FileStream stream = File.OpenRead(Path))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                imgPreview.Source = bitmap;
                OriginalSize.Text = $"Original Size: {DetermineSize(ofd.FileName)}";
            }
        }

        private void CompressImage_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog { Filter = "Jpeg file|*.jpeg" };
            if (sfd.ShowDialog() == true)
            {
                string savePath = sfd.FileName;
                CompressImage(Path!, savePath);
                CompressedSize.Text = $"Compressed Size: {DetermineSize(savePath)}";
            }
        }
    }
}
