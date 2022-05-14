
using System.Collections.ObjectModel;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassLibrary
{
    public enum VMf
    {
        vmdTan,
        vmdErfInv
    }
    [Serializable]
    public class VMGrid
    {
        public int Length { get; set; }
        public double LeftEnd { get; set; }
        public double RightEnd { get; set; }
        public double Step
        {
            get
            {
                return Math.Abs(RightEnd - LeftEnd) / Length;
            }
        }
        public VMf CurFunction { get; set; }

   
        public VMGrid(int Length_, double LeftEnd_, double RightEnd_, VMf CurFunction_)
        {
            Length = Length_;
            LeftEnd = LeftEnd_;
            RightEnd = RightEnd_;
            CurFunction = CurFunction_;
        }

        public VMGrid(VMGrid grid)
        {
            Length = grid.Length;
            LeftEnd = grid.LeftEnd;
            RightEnd = grid.RightEnd;
            CurFunction = grid.CurFunction;
        }

        public VMGrid()
        {
            Length = 0;
            LeftEnd = 0;
            RightEnd = 1;
            CurFunction = VMf.vmdTan;
        }

        public double Left_set
        {
            get
            {
                return LeftEnd;
            }
            set
            {
                LeftEnd = value;
            }
        }

        public double Right_set
        {
            get
            {
                return RightEnd;
            }
            set
            {
                RightEnd = value;
            }
        }
    }
    [Serializable]
    public struct VMTime
    {
        public VMGrid Grid { get; set; }
        public double VML_HA_Time { get; set; }
        public double VML_EP_Time { get; set; }
        public double WO_MKL_Time { get; set; }
        public double VML_HA_Coef { get; set; }
        public double VML_EP_Coef { get; set; }

        override public string ToString()
        {
            return string.Format($"Grid: Length: {Grid.Length}, LeftEnd: {Grid.LeftEnd:0.00000000}, " +
                $"RightEnd: {Grid.RightEnd:0.00000000}, Step: {Grid.Step:0.00000000}, " +
                $"CurFunction: {Grid.CurFunction}\nVML_HA_Time: {VML_HA_Time:0.00000000}; " +
                $"VML_EP_Time: {VML_EP_Time:0.00000000}; VML_HA_Coef: {VML_HA_Coef:0.00000000}; " +
                $"VML_EP_Coef: {VML_EP_Coef:0.00000000}.");
            //add wo_mkl time
        }
    }

    
    public struct VMAccuracy
    {
        public static float ToSingle(string s)
        {
            return Convert.ToSingle(s);
        }
        public VMGrid Grid { get; set; }
        public double Max_abs_diff { get; set; }
        public double Arg_for_Max_Diff { get; set; }
        public double VML_HA_value { get; set; }
        public double VML_EP_value { get; set; }

        override public string ToString()
        {
            return string.Format($"Grid: Length: {Grid.Length}, LeftEnd: {Grid.LeftEnd:0.00000000}, " +
                $"RightEnd: {Grid.RightEnd:0.00000000}, Step: {Grid.Step:0.00000000}, " +
                $"CurFunction: {Grid.CurFunction}\nAbs_of_Diff: {Max_abs_diff:0.00000000}; " +
                $"Arg_for_Max_Diff: {Arg_for_Max_Diff:0.00000000}; " +
                $"VML_HA_value: {VML_HA_value:0.00000000}; " +
                $"VML_EP_value: {VML_EP_value:0.00000000}.");
        }
    }

    [Serializable]
    public class VMBenchmark
    {
        [DllImport("..\\..\\..\\..\\x64\\Debug\\DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GlobMKLFunc(int length, double[] vector, int CurFunction, double[] 
                                                    res1, double[] res2, double[] res3, double[] res4);

        public ObservableCollection<VMTime> Collection_time { get; set; }
        public ObservableCollection<VMAccuracy> Collection_accuracy { get; set; }

        public double Min_VML_HA_Coef
        {
            get
            {
                if (Collection_time.Count < 1)
                {
                    return -1;
                }
                double minCoef = Collection_time[0].VML_HA_Coef;
                foreach (VMTime item in Collection_time)
                {
                    if (item.VML_HA_Coef < minCoef)
                    {
                        minCoef = item.VML_HA_Coef;
                    }
                }
                return minCoef;
            }
        }
        public double Max_VML_HA_Coef
        {
            get
            {
                if (Collection_time.Count < 1)
                {
                    return -1;
                }
                double maxCoef = Collection_time[0].VML_HA_Coef;
                foreach (VMTime item in Collection_time)
                {
                    if (item.VML_HA_Coef > maxCoef)
                    {
                        maxCoef = item.VML_HA_Coef;
                    }
                }
                return maxCoef;
            }
        }

        public void AddVMTime(VMGrid Grid)
        {
            VMTime item = new();
            item.Grid = new(Grid);
            double[] vector = new double[Grid.Length];
            for (int i = 0; i < Grid.Length; i++)
            {
                vector[i] = Grid.LeftEnd + (i * Grid.Step);
            }
            double[] res_HA = new double[Grid.Length];
            double[] res_EP = new double[Grid.Length];
            double[] res_wo_MKL = new double[Grid.Length];
            double[] Times = new double[3];
            double status = GlobMKLFunc(Grid.Length, vector, (int)Grid.CurFunction, res_HA, res_EP, res_wo_MKL, Times);
            if (status != 0)
            {
                throw new InvalidCastException($"GlobMKLFunc faild with: {status}");
            }
            item.VML_HA_Time = Times[0];
            item.VML_EP_Time = Times[1];
            item.WO_MKL_Time = Times[2];
            item.VML_HA_Coef = Times[0] / Times[2];
            item.VML_EP_Coef = Times[1] / Times[2];
            Collection_time.Add(item);
        }

        public void AddVMAccuracy(VMGrid Grid)
        {
            VMAccuracy item = new();
            item.Grid = new(Grid);
            double[] vector = new double[Grid.Length];
            for (int i = 0; i < Grid.Length; i++)
            {
                vector[i] = Grid.LeftEnd + (i * Grid.Step);
            }
            double[] res_HA = new double[Grid.Length];
            double[] res_EP = new double[Grid.Length];
            double[] res_wo_MKL = new double[Grid.Length];
            double[] Times = new double[3];
            double status = GlobMKLFunc(Grid.Length, vector, (int)Grid.CurFunction, res_HA, res_EP, res_wo_MKL, Times);
            item.Max_abs_diff = 0;
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Math.Abs(res_HA[i] - res_EP[i]) > item.Max_abs_diff)
                {
                    item.Max_abs_diff = Math.Abs(res_HA[i] - res_EP[i]);
                    item.Arg_for_Max_Diff = vector[i];
                    item.VML_HA_value = res_HA[i];
                    item.VML_EP_value = res_EP[i];
                }
            }
            Collection_accuracy.Add(item);
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        public VMBenchmark()
        {
            Collection_time = new();
            Collection_accuracy = new();
            Collection_time.CollectionChanged += Collection_CollectionChanged;
        }


        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Min_VML_HA_Coef)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Max_VML_HA_Coef)));
        }
    }

}
