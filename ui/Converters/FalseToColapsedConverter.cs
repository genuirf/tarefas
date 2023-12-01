using System.Windows.Data;

namespace tarefas.Converters
{
      public class FalseToColapsedConverter : IValueConverter
      {

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                  bool v = util.funcoes.CBool(value);

                  return (!v) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                  System.Windows.Visibility v = (System.Windows.Visibility)value;
                  return (v == System.Windows.Visibility.Collapsed) ? false : true;
            }
      }
}
