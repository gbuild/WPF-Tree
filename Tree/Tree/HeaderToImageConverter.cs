using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Tree
{
    /// <summary>
    /// Конвертирует полное имя в изображения
    /// </summary>
    [ValueConversion(typeof(string),typeof(BitmapImage))]

    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;

            if (path == null)
                return null;

            var name = MainWindow.GetFileFolderName(path);

            var image = "images/Files.png";

            if (string.IsNullOrEmpty(name))
                image = "images/drive.png";
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "images/ClosedFolder.png";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
