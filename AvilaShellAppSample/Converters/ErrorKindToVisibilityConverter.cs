using System;
using System.Collections.Specialized;
using System.Globalization;
using AvilaShellAppSample.Services;
using Xamarin.Forms;

namespace AvilaShellAppSample.Converters
{
    public class ErrorKindToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            else
            {
                try
                {
                    var ek = (ServiceErrorKind)value;

                    string[] str = new string[((StringCollection)parameter).Count];
                    ((StringCollection)parameter).CopyTo(str, 0);

                    foreach(var s in str)
                    {
                        if (s.Contains(ek.ToString()))
                            return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
