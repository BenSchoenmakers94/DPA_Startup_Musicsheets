using System;
using System.Globalization;
using System.Windows.Data;
using DPA_Musicsheets.Models.Commands;

namespace DPA_Musicsheets.ViewModels.Converters
{
    public class ActionOptionIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var i = (int) value;
            return (ActionOption) i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
