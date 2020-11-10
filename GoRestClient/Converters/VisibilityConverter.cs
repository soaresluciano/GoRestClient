using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoRestClient.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint.TryParse(value?.ToString(), out var number);
            bool.TryParse(parameter?.ToString(), out var switcher);
            var result = switcher ? number != 0 : number == 0;
            return result ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}