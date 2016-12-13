using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;
using Xamarin.Forms;

namespace Converter
{
    public class JoinListConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var modelList = value as IEnumerable<NameType>;
            if (modelList != null)
            {
                return string.Join(", ", modelList.Select(m => m.Name));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}