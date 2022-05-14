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
    class VMAccuracyConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    VMAccuracy val = (VMAccuracy)value;
                    return $"VML_HA value: {val.VML_HA_value:0.00000000}, VML_EP value: {val.VML_EP_value:0.00000000}";
                }
                return "";
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "VML_HA value: ERROR, VML_EP value: ERROR";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new VMTime();
        }
    }
}
