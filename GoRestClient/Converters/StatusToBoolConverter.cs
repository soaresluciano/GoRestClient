using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GoRestClient.Models.Enums;

namespace GoRestClient.Converters
{
    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() switch
            {
                nameof(Status.Active) => true,
                nameof(Status.Inactive) => false,
                _ => DependencyProperty.UnsetValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? Status.Active : Status.Inactive;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
