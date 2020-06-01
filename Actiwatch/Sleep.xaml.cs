using OxyPlot.Wpf;
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

namespace Actiwatch
{
    /// <summary>
    /// Sleep.xaml 的互動邏輯
    /// </summary>
    public partial class Sleep : UserControl
    {
        private int pageIndex = 1;
        private List<OxyPlot.Wpf.Plot> chartList;
        private List<string> dateList;

        public Sleep()
        {
            InitializeComponent();


            chartList = new List<OxyPlot.Wpf.Plot> { Day1, Day2, Day3, Day4, Day5, Day6, Day7 };
            dateList = new List<string>();
            Day1Model a = new Day1Model(Global.Dialy_List[0].GetVMDiff());
            Day1Model b = new Day1Model(Global.Dialy_List[1].GetVMDiff());
            Day1.DataContext = a;
            Day1.Title = Global.Dialy_List[0].GetDatetime();
            Day1.TitleFontSize = 10;
            lin1axes.FontSize = 8;
            Day2.DataContext = b;
            Day2.Title = Global.Dialy_List[1].GetDatetime();
            Day2.TitleFontSize = 10;
            lin2axes.FontSize = 8;
            Day3.DataContext = new Day3Model(Global.Dialy_List[2].GetVMDiff());
            Day3.Title = Global.Dialy_List[2].GetDatetime();
            Day3.TitleFontSize = 10;
            lin3axes.FontSize = 8;
            Day4.DataContext = new Day4Model(Global.Dialy_List[3].GetVMDiff());
            Day4.Title = Global.Dialy_List[3].GetDatetime();
            Day4.TitleFontSize = 10;
            lin1axes.FontSize = 8;
            Day5.DataContext = new Day5Model(Global.Dialy_List[4].GetVMDiff());
            Day5.Title = Global.Dialy_List[4].GetDatetime();
            Day5.TitleFontSize = 10;
            lin5axes.FontSize = 8;
            Day6.DataContext = new Day6Model(Global.Dialy_List[5].GetVMDiff());
            Day6.Title = Global.Dialy_List[5].GetDatetime();
            Day6.TitleFontSize = 10;
            lin6axes.FontSize = 8;
            Day7.DataContext = new Day7Model(Global.Dialy_List[6].GetVMDiff());
            Day7.Title = Global.Dialy_List[6].GetDatetime();
            Day7.TitleFontSize = 10;
            lin7axes.FontSize = 8;
            

            dateList.Add(Global.Dialy_List[0].GetDatetime());
            dateList.Add(Global.Dialy_List[1].GetDatetime());
            dateList.Add(Global.Dialy_List[2].GetDatetime());
            dateList.Add(Global.Dialy_List[3].GetDatetime());
            dateList.Add(Global.Dialy_List[4].GetDatetime());
            dateList.Add(Global.Dialy_List[5].GetDatetime());
            dateList.Add(Global.Dialy_List[6].GetDatetime());
            

            if (Global.Dialy_List.Count < 7)
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
            }
            else
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, 7, Global.Dialy_List.Count);
            }
            
        }
        private void leftButton(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex > 1)
            {
                pageIndex--;
                if (Global.Dialy_List.Count < (pageIndex * 7))
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
                }
                else
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, pageIndex * 7, Global.Dialy_List.Count);
                }
            }
        }
        private void rightButton(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex < ((float)Global.Dialy_List.Count / 7))
            {
                pageIndex++;
                if(Global.Dialy_List.Count < (pageIndex * 7))
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
                }
                else
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, pageIndex * 7, Global.Dialy_List.Count);
                }
            }
        }
        private void print(string text)
        {
            Console.WriteLine(text);
        }

        private void RefreshDateButton(object sender, RoutedEventArgs e)
        {
            string[] splittime;
            double start = 0;
            double end = 0;
            print(startDate.Text.Replace('/', '-'));
            print(endDate.Text);
            print(startTime.Text.Replace('/', '-'));
            print(endTime.Text);

            DateTime oDate = DateTime.Parse(startDate.Text);


            splittime = startTime.Text.Split(':');
            start = Convert.ToDouble(splittime[0]) + Convert.ToDouble(splittime[1]) / 60;
            splittime = endTime.Text.Split(':');
            end = Convert.ToDouble(splittime[0]) + Convert.ToDouble(splittime[1]) / 60;

            start = start > 12 ? start - 12 : start + 12;
            end = end > 12 ? end - 12 : end + 12;

            print(start + "");
            print(end + "");

            int dayIndex = dateList.FindIndex(x => x.Equals(oDate.ToString("yyyy-MM-dd")));

            Global.Dialy_List[dayIndex].SetStartRange(start);
            Global.Dialy_List[dayIndex].SetEndRange(end);

            Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < chartList.Count; i++)
                {
                    chartList[i].Annotations.Clear();
                    chartList[i].Annotations.Add(new RectangleAnnotation() { MinimumX = Global.Dialy_List[(pageIndex-1)*7+i+1].GetStartRange(), MaximumX = Global.Dialy_List[(pageIndex - 1) * 7 + i + 1].GetEndRange(), MinimumY = 0, MaximumY = 2000, Fill = Color.FromArgb(120, 0, 0, 255) });
                    chartList[i].InvalidatePlot(true);
                }
            });
        }
    }
}
