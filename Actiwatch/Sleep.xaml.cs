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

            for (int i = 0; i < Global.Dialy_List.Count; i++)
            {
                showData(i);
                if (i == 6) break;
            }
            if (Global.Dialy_List.Count < 7)
            {
                for (int i = Global.Dialy_List.Count; i < 7; i++)
                {
                    HiddenData(i);
                }
            }

            for (int i = 0; i < Global.Dialy_List.Count; i++)
            {
                dateList.Add(Global.Dialy_List[i].GetDatetime());
            }
            if (Global.Dialy_List.Count < 7)
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
            }
            else
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, 7, Global.Dialy_List.Count);
            }

        }
        private void showData(int idx)
        {
            switch (idx)
            {
                case 0:
                    Day1.Visibility = Visibility.Visible;
                    Day1.DataContext = new Day1Model(Global.Dialy_List[(pageIndex - 1) * 7].GetVM());
                    day1Axes.FontSize = 8;
                    Day1Date.Text = Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime();
                    break;
                case 1:
                    Day2.Visibility = Visibility.Visible;
                    Day2.DataContext = new Day2Model(Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetVM());
                    day2Axes.FontSize = 8;
                    Day2Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetDatetime();
                    break;
                case 2:
                    Day3.Visibility = Visibility.Visible;
                    Day3.DataContext = new Day3Model(Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetVM());
                    day3Axes.FontSize = 8;
                    Day3Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetDatetime();
                    break;
                case 3:
                    Day4.Visibility = Visibility.Visible;
                    Day4.DataContext = new Day4Model(Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetVM());
                    day4Axes.FontSize = 8;
                    Day4Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetDatetime();
                    break;
                case 4:
                    Day5.Visibility = Visibility.Visible;
                    Day5.DataContext = new Day5Model(Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetVM());
                    day5Axes.FontSize = 8;
                    Day5Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetDatetime();
                    break;
                case 5:
                    Day6.Visibility = Visibility.Visible;
                    Day6.DataContext = new Day6Model(Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetVM());
                    day6Axes.FontSize = 8;
                    Day6Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetDatetime();
                    break;
                case 6:
                    Day7.Visibility = Visibility.Visible;
                    Day7.DataContext = new Day7Model(Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetVM());
                    day7Axes.FontSize = 8;
                    Day7Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetDatetime();
                    break;
                default:
                    break;
            }
        }

        private void HiddenData(int idx)
        {
            switch (idx)
            {
                case 0:
                    Day1.Visibility = Visibility.Hidden;
                    Day1Date.Text = "";
                    break;
                case 1:
                    Day2.Visibility = Visibility.Hidden;
                    Day2Date.Text = "";
                    break;
                case 2:
                    Day3.Visibility = Visibility.Hidden;
                    Day3Date.Text = "";
                    break;
                case 3:
                    Day4.Visibility = Visibility.Hidden;
                    Day4Date.Text = "";
                    break;
                case 4:
                    Day5.Visibility = Visibility.Hidden;
                    Day5Date.Text = "";
                    break;
                case 5:
                    Day6.Visibility = Visibility.Hidden;
                    Day6Date.Text = "";
                    break;
                case 6:
                    Day7.Visibility = Visibility.Hidden;
                    Day7Date.Text = "";
                    break;
                default:
                    break;
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
                    for (int i = (pageIndex - 1) * 7; i < Global.Dialy_List.Count; i++)
                    {
                        showData(i % 7);
                    }
                    for (int i = Global.Dialy_List.Count; i < (pageIndex * 7); i++)
                    {
                        HiddenData(i % 7);
                    }
                }
                else
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, pageIndex * 7, Global.Dialy_List.Count);
                    for (int i = (pageIndex - 1) * 7; i < pageIndex * 7; i++)
                    {
                        showData(i % 7);
                    }
                }
            }
        }
        private void rightButton(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex < ((float)Global.Dialy_List.Count / 7))
            {
                pageIndex++;
                if (Global.Dialy_List.Count < (pageIndex * 7))
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
                    for (int i = (pageIndex - 1) * 7; i < Global.Dialy_List.Count; i++)
                    {
                        showData(i % 7);
                    }
                    for (int i = Global.Dialy_List.Count; i < (pageIndex * 7); i++)
                    {
                        print(i + "");
                        HiddenData(i % 7);
                    }
                }
                else
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, pageIndex * 7, Global.Dialy_List.Count);
                    for (int i = (pageIndex - 1) * 7; i < pageIndex * 7; i++)
                    {
                        showData(i % 7);
                    }
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
                    chartList[i].Annotations.Add(new RectangleAnnotation() { MinimumX = Global.Dialy_List[(pageIndex - 1) * 7 + i + 1].GetStartRange(), MaximumX = Global.Dialy_List[(pageIndex - 1) * 7 + i + 1].GetEndRange(), MinimumY = 0, MaximumY = 2000, Fill = Color.FromArgb(120, 0, 0, 255) });
                    chartList[i].InvalidatePlot(true);
                }
            });
        }
    }
}
