using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.ComponentModel;
using ClassLibrary;

namespace Lab_1
{
    public class ViewData : INotifyPropertyChanged
    {
        private bool _VMBenchmarkChanged;
        // Properties
        public VMBenchmark Benchmark { get; set; }
        public bool VMBenchmarkChanged
        {
            get { return _VMBenchmarkChanged; }
            set
            {
                if (value != _VMBenchmarkChanged)
                {
                    _VMBenchmarkChanged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VMBenchmarkChanged)));
                }
            }
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public ViewData()
        {
            Benchmark = new();
            VMBenchmarkChanged = false;
            Benchmark.Collection_time.CollectionChanged += Collection_CollectionChanged;
            Benchmark.Collection_accuracy.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            VMBenchmarkChanged = true;
        }

        // Methods
        public void AddVMTime(VMGrid Grid)
        {
            try
            {
                Benchmark.AddVMTime(Grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddVMAccuracy(VMGrid Grid)
        {
            try
            {
                Benchmark.AddVMAccuracy(Grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool Save(string filename)
        {
            try
            {
                StreamWriter writer = new(filename, false);
                try
                {
                    writer.WriteLine(Benchmark.Collection_time.Count());
                    foreach (VMTime item in Benchmark.Collection_time)
                    {
                        writer.WriteLine(item.Grid.Length);
                        writer.WriteLine($"{item.Grid.LeftEnd:0.00000000}");
                        writer.WriteLine($"{item.Grid.RightEnd:0.00000000}");
                        writer.WriteLine($"{item.Grid.Step:0.00000000}");
                        writer.WriteLine((int)item.Grid.CurFunction);
                        writer.WriteLine($"{item.VML_HA_Time:0.00000000}");
                        writer.WriteLine($"{item.VML_EP_Time:0.00000000}");
                        writer.WriteLine($"{item.VML_HA_Coef:0.00000000}");
                        writer.WriteLine($"{item.VML_EP_Coef:0.00000000}");
                    }
                    writer.WriteLine(Benchmark.Collection_accuracy.Count());
                    foreach (VMAccuracy item in Benchmark.Collection_accuracy)
                    {
                        writer.WriteLine(item.Grid.Length);
                        writer.WriteLine($"{item.Grid.LeftEnd:0.00000000}");
                        writer.WriteLine($"{item.Grid.RightEnd:0.00000000}");
                        writer.WriteLine($"{item.Grid.Step:0.00000000}");
                        writer.WriteLine((int)item.Grid.CurFunction);
                        writer.WriteLine($"{item.Max_abs_diff:0.00000000}");
                        writer.WriteLine($"{item.Arg_for_Max_Diff:0.00000000}");
                        writer.WriteLine($"{item.VML_HA_value:0.00000000}");
                        writer.WriteLine($"{item.VML_EP_value:0.00000000}");
                    }
                }
                catch (Exception e)
                {
                    Benchmark.Collection_time.Clear();
                    Benchmark.Collection_accuracy.Clear();
                    MessageBox.Show($"Unable to save file: {e.Message}.", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                    writer.Close();
                    return false;
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Benchmark.Collection_time.Clear();
                Benchmark.Collection_accuracy.Clear();
                MessageBox.Show($"Unable to save file: {e.Message}.", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool Load(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);
                try
                {
                    Benchmark.Collection_time.Clear();
                    Benchmark.Collection_accuracy.Clear();
                    int count1 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count1; i++)
                    {
                        VMTime item = new();
                        int Grid_Length = Int32.Parse(reader.ReadLine());
                        double Grid_LeftEnd = double.Parse(reader.ReadLine());
                        double Grid_RightEnd = double.Parse(reader.ReadLine());
                        double Grid_Step = double.Parse(reader.ReadLine());
                        VMf Grid_CurFunction = (VMf)int.Parse(reader.ReadLine());
                        VMGrid Grid = new(Grid_Length, Grid_LeftEnd, Grid_RightEnd, Grid_CurFunction);
                        item.Grid = Grid;
                        item.VML_HA_Time = double.Parse(reader.ReadLine());
                        item.VML_EP_Time = double.Parse(reader.ReadLine());
                        item.VML_HA_Coef = double.Parse(reader.ReadLine());
                        item.VML_EP_Coef = double.Parse(reader.ReadLine());
                        Benchmark.Collection_time.Add(item);
                    }
                    int count2 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count2; i++)
                    {
                        VMAccuracy item = new();
                        int Grid_Length = Int32.Parse(reader.ReadLine());
                        double Grid_LeftEnd = double.Parse(reader.ReadLine());
                        double Grid_RightEnd = double.Parse(reader.ReadLine());
                        double Grid_Step = double.Parse(reader.ReadLine());
                        VMf Grid_CurFunction = (VMf)int.Parse(reader.ReadLine());
                        VMGrid Grid = new(Grid_Length, Grid_LeftEnd, Grid_RightEnd, Grid_CurFunction);
                        item.Grid = Grid;
                        item.Max_abs_diff = double.Parse(reader.ReadLine());
                        item.Arg_for_Max_Diff = double.Parse(reader.ReadLine());
                        item.VML_HA_value = double.Parse(reader.ReadLine());
                        item.VML_EP_value = double.Parse(reader.ReadLine());
                        Benchmark.Collection_accuracy.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Benchmark.Collection_time.Clear();
                    Benchmark.Collection_accuracy.Clear();
                    MessageBox.Show($"Unable to load file: {e.Message}.", "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
                    reader.Close();
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Benchmark.Collection_time.Clear();
                Benchmark.Collection_accuracy.Clear();
                MessageBox.Show($"Unable to load file: {e.Message}.", "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
