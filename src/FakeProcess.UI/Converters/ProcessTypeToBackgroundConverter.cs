using FakeProcess.Domain.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FakeProcess.UI.Converters
{
    public class ProcessTypeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           switch ((ProcessType)value)
           {
                case ProcessType.TypeA:
                    return new SolidColorBrush(Color.FromRgb(60,103,33));
                case ProcessType.TypeB:
                    return new SolidColorBrush(Color.FromRgb(109, 55, 117));
                default:
                    return new SolidColorBrush(Color.FromRgb(60, 103, 33));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
