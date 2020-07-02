using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Actiwatch
{
    /// <summary>
    /// SleepReport.xaml 的互動邏輯
    /// </summary>
    public partial class SleepReport : UserControl
    {
        public SeriesCollection SESeriesCollection { get; set; }
        public string[] SELabels { get; set; }
        public Func<double, string> SEFormatter { get; set; }

        public SeriesCollection SOTSeriesCollection { get; set; }
        public string[] SOTLabels { get; set; }
        public Func<double, string> SOTFormatter { get; set; }

        public SeriesCollection WASOSeriesCollection { get; set; }
        public string[] WASOLabels { get; set; }
        public Func<double, string> WASOFormatter { get; set; }

        public SeriesCollection TSTSeriesCollection { get; set; }
        public string[] TSTLabels { get; set; }
        public Func<double, string> TSTFormatter { get; set; }

        public SleepReport()
        {
            InitializeComponent();

            SESeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Sleep efficiency",
                    Values = new ChartValues<double> {  },
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x96, 0x88))
                }
            };
            

            SOTSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Sleep onset time",
                    Values = new ChartValues<double> {  },
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x96, 0x88))
                }
            };
            

            WASOSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Wake after sleep onset time",
                    Values = new ChartValues<double> {  },
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x96, 0x88))
                }
            };
            

            TSTSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total sleep time",
                    Values = new ChartValues<double> {  },
                    Fill = new SolidColorBrush(Color.FromRgb(0x00, 0x96, 0x88))
                }
            };
            

            DataContext = this;

            SleepAlgorithm algo = new SleepAlgorithm();
            SleepIndex SI;
            List<string> date = new List<string>();
            ChartValues<double> SE = new ChartValues<double>();
            ChartValues<double> SOT = new ChartValues<double>();
            ChartValues<double> WASO = new ChartValues<double>();
            ChartValues<double> TST = new ChartValues<double>();
            for (int i = 0; i < Global.Dialy_List.Count; i++)
            {
                if (Global.Dialy_List[i].haveSleep)
                {
                    Console.WriteLine("Sleep report");
                    Console.WriteLine(Global.Dialy_List[i].startRange);
                    Console.WriteLine(Global.Dialy_List[i].endRange);
                    double[] z = Global.Dialy_List[i].GetSleepZ();
                    List<double> sleepZ = new List<double>();
                    for (int j = Global.Dialy_List[i].startRange; j < Global.Dialy_List[i].endRange; j++)
                    {
                        sleepZ.Add(z[j]);
                    }
                    int[] stage = algo.Run(sleepZ.ToArray());
                    SI = algo.SleepQuality(stage);
                    Global.Dialy_List[i].SE = SI.SE;
                    Global.Dialy_List[i].SOT = SI.SOT;
                    Global.Dialy_List[i].WASO = SI.WASO;
                    Global.Dialy_List[i].TST = SI.TST;
                    date.Add(Global.Dialy_List[i].GetDatetime());

                    SE.Add(SI.SE);
                    SOT.Add(SI.SOT);
                    WASO.Add(SI.WASO);
                    TST.Add(SI.TST);
                }
            }
            SELabels = date.ToArray();
            SOTLabels = date.ToArray();
            WASOLabels = date.ToArray();
            TSTLabels = date.ToArray();
            SEFormatter = value => value.ToString("N");
            SOTFormatter = value => value.ToString("N");
            WASOFormatter = value => value.ToString("N");
            TSTFormatter = value => value.ToString("N");

            SESeriesCollection[0].Values = SE;
            SOTSeriesCollection[0].Values = SOT;
            WASOSeriesCollection[0].Values = WASO;
            TSTSeriesCollection[0].Values = TST;
        }
    }
}
