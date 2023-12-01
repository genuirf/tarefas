using System;
using System.Globalization;
using System.Windows.Data;

namespace tarefas.Converters
{
      public class StringTruncateConverter : IValueConverter
      {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                  if (value is string stringValue && parameter is string parameterValue)
                  {
                        if (int.TryParse(parameterValue, out int maxLength))
                        {
                              if (stringValue.Length > maxLength)
                              {
                                    return stringValue.Substring(0, maxLength) + "...";
                              }
                        }
                  }

                  return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                  throw new NotImplementedException();
            }
      }
}
