using FakeProcess.Domain.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FakeProcess.UI.Converters
{
    internal class ProcessTypeToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ProcessType)value)
            {
                case ProcessType.TypeA:
                    return new SolidColorBrush(Color.FromRgb(73, 135, 35));
                case ProcessType.TypeB:
                    return new SolidColorBrush(Color.FromRgb(143, 66, 155));
                default:
                    return new SolidColorBrush(Color.FromRgb(73, 135, 35));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
