using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using ClassLibrary;

namespace Lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // # Properties
        public ViewData Data { get; set; }
        public VMGrid inGrid { get; set; } = new(10, 1.1, 10.3, VMf.vmdTan);

        // # MainWindow
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Data = new();
        }

        // # Commands for each menuitem
        //New
        private void New_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DisplaySaveFileMessage())
                {
                    Data.Benchmark.Collection_time.Clear();
                    Data.Benchmark.Collection_accuracy.Clear();
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Open
        private void Open_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DisplaySaveFileMessage())
                {
                    // Configure open file dialog box
                    Microsoft.Win32.OpenFileDialog dlgo = new Microsoft.Win32.OpenFileDialog
                    {
                        FileName = "", // Default file name
                        DefaultExt = ".txt", // Default file extension
                        Filter = "Text documents (.txt)|*.txt" // Filter files by extension
                    };

                    bool? result;
                    // Show open file dialog box
                    result = dlgo.ShowDialog();

                    // Process open file dialog box results
                    if (result == true)
                    {
                        // Open document
                        string filename = dlgo.FileName;
                        bool errors = Data.Load(filename);
                        Data.VMBenchmarkChanged = false;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Save
        private void SaveAs_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlgs = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Document", // Default file name
                    DefaultExt = ".txt", // Default file extension
                    Filter = "Text documents (.txt)|*.txt" // Filter files by extension
                };

                bool? result;
                // Show save file dialog box
                result = dlgs.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlgs.FileName;
                    bool errors = Data.Save(filename);
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Add VMTime
        private void Add_VMTime_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.Benchmark.AddVMTime(inGrid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Add VMAccuracy
        private void Add_VMAccuracy_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.Benchmark.AddVMAccuracy(inGrid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handles menu click
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            try
            {
                // What element was clicked?
                switch (menuItem.Header.ToString())
                {
                    case "Add VMTime":
                        Data.Benchmark.AddVMTime(inGrid);
                        break;
                    case "Add VMAccuracy":
                        Data.Benchmark.AddVMAccuracy(inGrid);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // # Will display MessageBox and return "true" or "false" depending on conditions
        public bool DisplaySaveFileMessage()
        {
            try
            {
                // Was Collection changed?
                if (Data.VMBenchmarkChanged)
                {
                    // Display SaveFile offer
                    MessageBoxResult UserChoice = MessageBox.Show($"Save changes to file?", "Lab 4", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (UserChoice)
                    {
                        // Abort oberation by user
                        case MessageBoxResult.Cancel:
                            return false;
                        // Save file
                        case MessageBoxResult.Yes:
                            // Configure save file dialog box
                            Microsoft.Win32.SaveFileDialog dlgs = new Microsoft.Win32.SaveFileDialog();
                            dlgs.FileName = "Document"; // Default file name
                            dlgs.DefaultExt = ".txt"; // Default file extension
                            dlgs.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                            // Show save file dialog box
                            bool? result = dlgs.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                // Save document
                                string filename = dlgs.FileName;
                                bool errors = Data.Save(filename);
                                // Return can we go on or not
                                return errors;
                            }
                            else
                            {
                                // If user closed DaveFileDialog
                                return false;
                            }
                        // Do not save
                        case MessageBoxResult.No:
                            return true;
                    }
                    // If we got here, something for sure went wrong
                    return false;
                }
                else
                {
                    // No changes present
                    return true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // # Check if VMBenchmark was changed
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DisplaySaveFileMessage())
            {
                base.OnClosing(e);
            }
        }

        private void Func_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBoxL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
