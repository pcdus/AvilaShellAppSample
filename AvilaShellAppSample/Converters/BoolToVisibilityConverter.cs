using System;
using System.Globalization;
using Xamarin.Forms;

namespace AvilaShellAppSample.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            var boolValue = (value is bool && (bool)value) ? true : false;

            // parameter is used to invert the result of the value
            if (parameter != null)
            {
                bool par = true;
                if ((bool.TryParse(parameter.ToString(), out par)) && (par)) boolValue = !boolValue;
            }

            return boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
