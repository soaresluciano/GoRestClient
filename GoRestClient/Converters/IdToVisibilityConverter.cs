using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoRestClient.Converters
{
    public class IdToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Check if a given Id is valid to make a component visible.
        /// </summary>
        /// <param name="value">Id to be evaluated.</param>
        /// <param name="parameter">'True' to inverse the logic.</param>
        /// <returns>Visibility related to the Id status.</returns>
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