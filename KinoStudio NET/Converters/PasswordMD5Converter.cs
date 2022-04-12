using KinoStudio_NET.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KinoStudio_NET.Converters
{
    public class PasswordMD5Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            new MD5Helper(value as string ?? "").GetEncryptedLine();
    }
}