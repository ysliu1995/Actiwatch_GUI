﻿using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private string[] hour = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"};
        private string[] minute = { "00", "10", "20", "30", "40", "50"};

        public Sleep()
        {
            InitializeComponent();


            chartList = new List<OxyPlot.Wpf.Plot> { Day1, Day2, Day3, Day4, Day5, Day6, Day7 };
            dateList = new List<string>();

            for (int i = 0; i < 24; i++) {
                startHour.Items.Add(hour[i]);
                endHour.Items.Add(hour[i]);
            }
            for (int i = 0; i < 6; i++)
            {
                startMinute.Items.Add(minute[i]);
                endMinute.Items.Add(minute[i]);
            }
            for (int i = 0; i < Global.Dialy_List.Count; i++)
            {
                chooseDate.Items.Add(Global.Dialy_List[i].GetDatetime());
                timeInBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                timeOutOfBed.Items.Add(Global.Dialy_List[i].GetDatetime());
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

            timeInBed.SelectedIndex = 0;
            timeOutOfBed.SelectedIndex = 0;
            startHour.SelectedIndex = 0;
            endHour.SelectedIndex = 0;
            startMinute.SelectedIndex = 0;
            endMinute.SelectedIndex = 0;
            chooseDate.SelectedIndex = 0;

            //DateTime taskDate = DateTime.ParseExact(Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            //DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            //time += 1000;
            //dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    chartList[0].Annotations.Clear();
            //    chartList[0].Annotations.Add(new RectangleAnnotation() { MinimumX = OxyPlot.Axes.DateTimeAxis.ToDouble(startDate), MaximumX = OxyPlot.Axes.DateTimeAxis.ToDouble(dt), MinimumY = 0, MaximumY = 2000, Fill = Color.FromArgb(120, 0, 0, 255) });
            //    chartList[0].InvalidatePlot(true);
            //});
        }
        private void showData(int idx)
        {
            switch (idx)
            {
                case 0:
                    Day1.Visibility = Visibility.Visible;
                    Day1.DataContext = new Day1Model(Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7].GetSleepTime(), "sleep");
                    day1Axes.FontSize = 8;
                    Day1Date.Text = Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime();
                    break;
                case 1:
                    Day2.Visibility = Visibility.Visible;
                    Day2.DataContext = new Day2Model(Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetSleepTime(), "sleep");
                    day2Axes.FontSize = 8;
                    Day2Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetDatetime();
                    break;
                case 2:
                    Day3.Visibility = Visibility.Visible;
                    Day3.DataContext = new Day3Model(Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetSleepTime(), "sleep");
                    day3Axes.FontSize = 8;
                    Day3Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetDatetime();
                    break;
                case 3:
                    Day4.Visibility = Visibility.Visible;
                    Day4.DataContext = new Day4Model(Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetSleepTime(), "sleep");
                    day4Axes.FontSize = 8;
                    Day4Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetDatetime();
                    break;
                case 4:
                    Day5.Visibility = Visibility.Visible;
                    Day5.DataContext = new Day5Model(Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetSleepTime(), "sleep");
                    day5Axes.FontSize = 8;
                    Day5Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetDatetime();
                    break;
                case 5:
                    Day6.Visibility = Visibility.Visible;
                    Day6.DataContext = new Day6Model(Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetSleepTime(), "sleep");
                    day6Axes.FontSize = 8;
                    Day6Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetDatetime();
                    break;
                case 6:
                    Day7.Visibility = Visibility.Visible;
                    Day7.DataContext = new Day7Model(Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetSleepTime(), "sleep");
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
                chooseDate.Items.Clear();
                timeInBed.Items.Clear();
                timeOutOfBed.Items.Clear();
                pageIndex--;
                if (Global.Dialy_List.Count < (pageIndex * 7))
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
                    for (int i = (pageIndex - 1) * 7; i < Global.Dialy_List.Count; i++)
                    {
                        chooseDate.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeInBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeOutOfBed.Items.Add(Global.Dialy_List[i].GetDatetime());
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
                        chooseDate.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeInBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeOutOfBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        showData(i % 7);
                    }
                }
                timeInBed.SelectedIndex = 0;
                timeOutOfBed.SelectedIndex = 0;
                startHour.SelectedIndex = 0;
                endHour.SelectedIndex = 0;
                startMinute.SelectedIndex = 0;
                endMinute.SelectedIndex = 0;
                chooseDate.SelectedIndex = 0;
            }
        }
        private void rightButton(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex < ((float)Global.Dialy_List.Count / 7))
            {
                chooseDate.Items.Clear();
                timeInBed.Items.Clear();
                timeOutOfBed.Items.Clear();
                pageIndex++;
                if (Global.Dialy_List.Count < (pageIndex * 7))
                {
                    pageContent.Text = String.Format("{0} ~ {1} of {2}", (pageIndex - 1) * 7 + 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
                    for (int i = (pageIndex - 1) * 7; i < Global.Dialy_List.Count; i++)
                    {
                        chooseDate.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeInBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeOutOfBed.Items.Add(Global.Dialy_List[i].GetDatetime());
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
                        chooseDate.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeInBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        timeOutOfBed.Items.Add(Global.Dialy_List[i].GetDatetime());
                        showData(i % 7);
                    }
                }
                timeInBed.SelectedIndex = 0;
                timeOutOfBed.SelectedIndex = 0;
                startHour.SelectedIndex = 0;
                endHour.SelectedIndex = 0;
                startMinute.SelectedIndex = 0;
                endMinute.SelectedIndex = 0;
                chooseDate.SelectedIndex = 0;
            }
        }
        private void print(string text)
        {
            Console.WriteLine(text);
        }

        private void RefreshDateButton(object sender, RoutedEventArgs e)
        {
            string startSleepTime = String.Format("{0} {1}:{2}:00", timeInBed.Text, startHour.Text, startMinute.Text);
            string endSleepTime = String.Format("{0} {1}:{2}:00", timeOutOfBed.Text, endHour.Text, endMinute.Text);
            int chooseIndex = chooseDate.SelectedIndex;

            Console.WriteLine(chooseIndex);
            Console.WriteLine(startSleepTime);
            Console.WriteLine(endSleepTime);

            DateTime start = DateTime.ParseExact(startSleepTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)start).ToUnixTimeSeconds();
            DateTime startDt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            DateTime end = DateTime.ParseExact(endSleepTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            time = ((DateTimeOffset)end).ToUnixTimeSeconds();
            DateTime endDt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);

            Application.Current.Dispatcher.Invoke(() =>
            {
                chartList[chooseIndex].Annotations.Clear();
                chartList[chooseIndex].Annotations.Add(new RectangleAnnotation() { MinimumX = OxyPlot.Axes.DateTimeAxis.ToDouble(startDt), MaximumX = OxyPlot.Axes.DateTimeAxis.ToDouble(endDt), MinimumY = 0, MaximumY = 2000, Fill = Color.FromArgb(120, 255, 151, 151) });
                chartList[chooseIndex].InvalidatePlot(true);
            });

        }
    }
}
