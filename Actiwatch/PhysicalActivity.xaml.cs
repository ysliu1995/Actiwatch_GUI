using OxyPlot.Wpf;
using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Application = System.Windows.Application;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace Actiwatch
{

    public partial class PhysicalActivity : UserControl
    {
        private int pageIndex = 1;
        private List<OxyPlot.Wpf.Plot> chartList;
        private List<string> dateList;
        private int lpa = 500;
        private int mvpa = 4000;


        public PhysicalActivity()
        {
            InitializeComponent();

            chartList = new List<OxyPlot.Wpf.Plot> { Day1, Day2, Day3, Day4, Day5, Day6, Day7 };
            dateList = new List<string>();
            
            for(int i=0;i < Global.Dialy_List.Count; i++)
            {
                showData(i);
                if (i == 6) break;
            }
            if(Global.Dialy_List.Count < 7)
            {
                for(int i= Global.Dialy_List.Count; i < 7; i++)
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
                    Day1PA.Visibility = Visibility.Visible;
                    Day1.DataContext = new Day1Model(Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7].GetVM(), "activity");
                    day1Axes.FontSize = 8;
                    Day1Date.Text = Global.Dialy_List[(pageIndex - 1) * 7].GetDatetime();

                    double[] pa1 = Global.Dialy_List[(pageIndex - 1) * 7].GetPhysicalActivity();
                    Day1PA.Annotations.Clear();
                    Day1PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa1.Length; i++)
                    {
                        if (pa1[i] >= mvpa)
                        {
                            Day1PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i + 1, MaximumX = i + 2, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117), Stroke = Color.FromArgb(120, 0, 0, 0), StrokeThickness = 1 });
                        }
                        else if (pa1[i] >= lpa)
                        {
                            Day1PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i + 1, MaximumX = i + 2, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day1PA.InvalidatePlot(true);
                    break;
                case 1:
                    Day2.Visibility = Visibility.Visible;
                    Day2PA.Visibility = Visibility.Visible;
                    Day2.DataContext = new Day2Model(Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetVM(), "activity");
                    day2Axes.FontSize = 8;
                    Day2Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetDatetime();
                    double[] pa2 = Global.Dialy_List[(pageIndex - 1) * 7 + 1].GetPhysicalActivity();
                    Day2PA.Annotations.Clear();
                    Day2PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa2.Length; i++)
                    {
                        if (pa2[i] >= mvpa)
                        {
                            Day2PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa2[i] >= lpa)
                        {
                            Day2PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day2PA.InvalidatePlot(true);
                    break;
                case 2:
                    Day3.Visibility = Visibility.Visible;
                    Day3PA.Visibility = Visibility.Visible;
                    Day3.DataContext = new Day3Model(Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetVM(), "activity");
                    day3Axes.FontSize = 8;
                    Day3Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetDatetime();
                    double[] pa3 = Global.Dialy_List[(pageIndex - 1) * 7 + 2].GetPhysicalActivity();
                    Day3PA.Annotations.Clear();
                    Day3PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa3.Length; i++)
                    {
                        if (pa3[i] >= mvpa)
                        {
                            Day3PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa3[i] >= lpa)
                        {
                            Day3PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day3PA.InvalidatePlot(true);
                    break;
                case 3:
                    Day4.Visibility = Visibility.Visible;
                    Day4PA.Visibility = Visibility.Visible;
                    Day4.DataContext = new Day4Model(Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetVM(), "activity");
                    day4Axes.FontSize = 8;
                    Day4Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetDatetime();
                    double[] pa4 = Global.Dialy_List[(pageIndex - 1) * 7 + 3].GetPhysicalActivity();
                    Day4PA.Annotations.Clear();
                    Day4PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa4.Length; i++)
                    {
                        if (pa4[i] >= mvpa)
                        {
                            Day4PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa4[i] >= lpa)
                        {
                            Day4PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day4PA.InvalidatePlot(true);
                    break;
                case 4:
                    Day5.Visibility = Visibility.Visible;
                    Day5PA.Visibility = Visibility.Visible;
                    Day5.DataContext = new Day5Model(Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetVM(), "activity");
                    day5Axes.FontSize = 8;
                    Day5Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetDatetime();
                    double[] pa5 = Global.Dialy_List[(pageIndex - 1) * 7 + 4].GetPhysicalActivity();
                    Day5PA.Annotations.Clear();
                    Day5PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa5.Length; i++)
                    {
                        if (pa5[i] >= mvpa)
                        {
                            Day5PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa5[i] >= lpa)
                        {
                            Day5PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day5PA.InvalidatePlot(true);
                    break;
                case 5:
                    Day6.Visibility = Visibility.Visible;
                    Day6PA.Visibility = Visibility.Visible;
                    Day6.DataContext = new Day6Model(Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetVM(), "activity");
                    day6Axes.FontSize = 8;
                    Day6Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetDatetime();
                    double[] pa6 = Global.Dialy_List[(pageIndex - 1) * 7 + 5].GetPhysicalActivity();
                    Day6PA.Annotations.Clear();
                    Day6PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa6.Length; i++)
                    {
                        if (pa6[i] >= mvpa)
                        {
                            Day6PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa6[i] >= lpa)
                        {
                            Day6PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day6PA.InvalidatePlot(true);
                    break;
                case 6:
                    Day7.Visibility = Visibility.Visible;
                    Day7PA.Visibility = Visibility.Visible;
                    Day7.DataContext = new Day7Model(Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetDatetime(), Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetVM(), "activity");
                    day7Axes.FontSize = 8;
                    Day7Date.Text = Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetDatetime();
                    double[] pa7 = Global.Dialy_List[(pageIndex - 1) * 7 + 6].GetPhysicalActivity();
                    Day7PA.Annotations.Clear();
                    Day7PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = 0, MaximumX = 1440, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 1, 180, 104) });
                    for (int i = 0; i < pa7.Length; i++)
                    {
                        if (pa7[i] >= mvpa)
                        {
                            Day7PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 255, 117, 117) });
                        }
                        else if (pa7[i] >= lpa)
                        {
                            Day7PA.Annotations.Add(new OxyPlot.Wpf.RectangleAnnotation() { MinimumX = i, MaximumX = i + 1, MinimumY = 0, MaximumY = 1, Fill = Color.FromArgb(120, 0, 128, 255) });
                        }
                    }
                    Day7PA.InvalidatePlot(true);
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
                    Day1PA.Visibility = Visibility.Hidden;
                    Day1Date.Text = "";
                    break;
                case 1:
                    Day2.Visibility = Visibility.Hidden;
                    Day2PA.Visibility = Visibility.Hidden;
                    Day2Date.Text = "";
                    break;
                case 2:
                    Day3.Visibility = Visibility.Hidden;
                    Day3PA.Visibility = Visibility.Hidden;
                    Day3Date.Text = "";
                    break;
                case 3:
                    Day4.Visibility = Visibility.Hidden;
                    Day4PA.Visibility = Visibility.Hidden;
                    Day4Date.Text = "";
                    break;
                case 4:
                    Day5.Visibility = Visibility.Hidden;
                    Day5PA.Visibility = Visibility.Hidden;
                    Day5Date.Text = "";
                    break;
                case 5:
                    Day6.Visibility = Visibility.Hidden;
                    Day6PA.Visibility = Visibility.Hidden;
                    Day6Date.Text = "";
                    break;
                case 6:
                    Day7.Visibility = Visibility.Hidden;
                    Day7PA.Visibility = Visibility.Hidden;
                    Day7Date.Text = "";
                    break;
                default:
                    break;
            }
        }

        private void print(string text)
        {
            Console.WriteLine(text);
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
                    for(int i = (pageIndex - 1) * 7;i< Global.Dialy_List.Count; i++)
                    {
                        showData(i % 7);
                    }
                    for (int i = Global.Dialy_List.Count; i < (pageIndex * 7); i++)
                    {
                        print(i+"");
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
    }
}
