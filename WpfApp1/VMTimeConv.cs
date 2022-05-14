using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using ClassLibrary;

namespace Lab_1
{
    public class VMTimeConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if(value != null)
                {
                    VMTime val = (VMTime)value;
                    return $"VML_HA Coef: {val.VML_HA_Coef:0.00000000}, VML_EP Coef: {val.VML_EP_Coef:0.00000000}";
                }
                return "";
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "VML_HA Coef: ERROR, VML_EP Coef: ERROR";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new VMTime();
        }
    }
}
