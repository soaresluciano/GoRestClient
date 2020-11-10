using GoRestClient.Services.Models.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoRestClient.Converters
{
    public class StatusToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convert a <see cref="Status"/> enumeration item into a boolean value.
        /// </summary>
        /// <param name="value"><see cref="Status"/> enumeration</param>
        /// <returns><see cref="Status.Active"/> = True and <see cref="Status.Active"/> = False</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() switch
            {
                nameof(Status.Active) => true,
                nameof(Status.Inactive) => false,
                _ => DependencyProperty.UnsetValue
            };
        }

        /// <summary>
        /// Convert a boolean value into a <see cref="Status"/> enumeration item.
        /// </summary>
        /// <param name="value">Boolean value.</param>
        /// <returns>True = <see cref="Status.Active"/> and False = <see cref="Status.Active"/> </returns>
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
