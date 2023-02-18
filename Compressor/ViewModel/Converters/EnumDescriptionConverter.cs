using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Compressor.ViewModel.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        //From Binding Source 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum))
                return null; //throw new ArgumentException("Value is not an Enum");

            var a = (value as Enum).Description();
            return a;
        }

        //From Binding Target 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null; //throw new ArgumentException("Value is not a string");
            foreach (var item in Enum.GetValues(targetType))
            {
                var asString = (item as Enum).Description();
                if (asString == (string)value)
                {
                    return item;
                }
            }
            throw new ArgumentException("Unable to match string to Enum description");
        }
    }
    public static class Converter
    {
        public static string Description(this Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            var attribArray = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attribArray.Length == 0)
                return enumObj.ToString();

            var attrib = attribArray[0] as DescriptionAttribute;
            return attrib != null ? attrib.Description : null;
        }
    }

}
