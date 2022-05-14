using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Lab_1
{
    public class VMGridConv : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return $"{values[0]} {values[1]:0.0000} {values[2]:0.0000}";
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "0 0 0";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string val = value as string;
                string[] values = val.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != 3)
                {
                    throw new InvalidOperationException("Less or more than 3 values");
                }
                int val1 = int.Parse(values[0]);
                double val2 = double.Parse(values[1]);
                double val3 = double.Parse(values[2]);
                object[] res = new object[3];
                res[0] = val1;
                res[1] = val2;
                res[2] = val3;
                return res;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                int val1 = 5;
                double val2 = 1.1;
                double val3 = 1.5;
                object[] res = new object[3];
                res[0] = val1;
                res[1] = val2;
                res[2] = val3;
                return res;
            }
        }
    }
}
