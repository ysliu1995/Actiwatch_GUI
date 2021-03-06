﻿using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public SeriesCollection rawSeriesCollection { get; set; }
        public string[] rawLabels { get; set; }
        public Func<double, string> rawFormatter { get; set; }

        public SeriesCollection stageSeriesCollection { get; set; }
        public string[] stageLabels { get; set; }
        public Func<double, string> stageFormatter { get; set; }

        private ChartValues<double> SE = new ChartValues<double>();
        private ChartValues<double> SOT = new ChartValues<double>();
        private ChartValues<double> WASO = new ChartValues<double>();
        private ChartValues<double> TST = new ChartValues<double>();
        private List<string> date = new List<string>();
        private List<string> sleepDate = new List<string>();
        private List<string> sleepDateList = new List<string>();
        private List<List<double>> sleepRaw = new List<List<double>>();
        private List<List<int>> sleepStage = new List<List<int>>();

        public SleepReport()
        {
            InitializeComponent();

            initChart();
            DataContext = this;
            Analysis();
            
        }
        //初始化圖表
        private void initChart()
        {
            SESeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Sleep efficiency",
                    Values = new ChartValues<double> { },
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
        }
        //跑演算法
        private void Analysis()
        {
            SleepAlgorithm algo = new SleepAlgorithm();
            SleepIndex SI;


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
                    if (SI.SOT < 5)
                        SOT.Add(5);
                    else
                        SOT.Add(SI.SOT);
                    WASO.Add(SI.WASO);
                    TST.Add(SI.TST);

                    sleepDateList.Add(Global.Dialy_List[i].GetDatetime());
                    sleepDate.Add(Global.Dialy_List[i].startTime);
                    sleepRaw.Add(sleepZ);
                    sleepStage.Add(stage.ToList());
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
            
            if(sleepDate.Count > 0)
            {
                //重新加入日期列表
                StageCombo.Items.Clear();
                foreach (string date in sleepDateList)
                {
                    StageCombo.Items.Add(date);
                }
                if (StageCombo.Items.Count > 0)
                {
                    StageCombo.SelectedIndex = 0;
                }
                stage.DataContext = new StageModel(sleepDate[0], sleepStage[0].ToArray());
                //初始化Y軸
                stageLabelAxes.Labels.Clear();
                stageLabelAxes.Labels.Add("Sleep");
                stageLabelAxes.Labels.Add("Wake");
            }
            
        }
        //將分析的睡眠結果寫入csv檔
        private void WriteToCSV()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "所有檔案 (*.*)|*.*";
            saveFileDialog.Title = "Save raw data";
            saveFileDialog.DefaultExt = "csv";//設定預設格式（可以不設）
            saveFileDialog.AddExtension = true;//設定自動在檔名中新增副檔名
            saveFileDialog.ShowDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                Console.WriteLine(saveFileDialog.FileName);
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.WriteLine("Date, SE (%), SOT (min), WASO (min), TST (min)");
                for (int i = 0; i < SE.Count; i++)
                {
                    sw.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", date[i], SE[i].ToString(), SOT[i].ToString(), WASO[i].ToString(), TST[i].ToString()));            // 寫入文字
                }
                sw.Close();                     // 關閉串流
            }
            else
            {
                Console.WriteLine("Cancel");
            }
        }
        //改變日期觸發函式
        private void StageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StageCombo.SelectedIndex;
            if (index >= 0)
            {
                stage.DataContext = new StageModel(sleepDate[index], sleepStage[index].ToArray());
            }
        }
        //下載按鈕觸發
        private void reportDownload(object sender, System.Windows.RoutedEventArgs e)
        {
            WriteToCSV();
        }
    }
}
