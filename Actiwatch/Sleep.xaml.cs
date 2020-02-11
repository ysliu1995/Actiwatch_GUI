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

        public Sleep()
        {
            InitializeComponent();

            Day1.DataContext = new Day1Model(Global.Dialy_List[0].GetVM());
            Day1.Title = Global.Dialy_List[0].GetDatetime();
            Day1.TitleFontSize = 10;
            lin1axes.FontSize = 8;
            Day2.DataContext = new Day2Model(Global.Dialy_List[1].GetVM());
            Day2.Title = Global.Dialy_List[1].GetDatetime();
            Day2.TitleFontSize = 10;
            lin2axes.FontSize = 8;
            Day3.DataContext = new Day3Model(Global.Dialy_List[2].GetVM());
            Day3.Title = Global.Dialy_List[2].GetDatetime();
            Day3.TitleFontSize = 10;
            lin3axes.FontSize = 8;
            Day4.DataContext = new Day4Model(Global.Dialy_List[3].GetVM());
            Day4.Title = Global.Dialy_List[3].GetDatetime();
            Day4.TitleFontSize = 10;
            lin1axes.FontSize = 8;
            Day5.DataContext = new Day5Model(Global.Dialy_List[4].GetVM());
            Day5.Title = Global.Dialy_List[4].GetDatetime();
            Day5.TitleFontSize = 10;
            lin5axes.FontSize = 8;
            Day6.DataContext = new Day6Model(Global.Dialy_List[5].GetVM());
            Day6.Title = Global.Dialy_List[5].GetDatetime();
            Day6.TitleFontSize = 10;
            lin6axes.FontSize = 8;
            Day7.DataContext = new Day7Model(Global.Dialy_List[6].GetVM());
            Day7.Title = Global.Dialy_List[6].GetDatetime();
            Day7.TitleFontSize = 10;
            lin7axes.FontSize = 8;

            Day1.Annotations.Add(new RectangleAnnotation() { MinimumX = 10, MaximumX = 20, MinimumY = 0, MaximumY = 2000, Fill = Color.FromArgb(120, 0, 0, 255) });


            if (Global.Dialy_List.Count < 7)
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, Global.Dialy_List.Count, Global.Dialy_List.Count);
            }
            else
            {
                pageContent.Text = String.Format("{0} ~ {1} of {2}", 1, 7, Global.Dialy_List.Count);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PRINT(Global.Dialy_List[0].GetDatetime());
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
        private void PRINT(string text)
        {
            Console.WriteLine(text);
        }
    }
}
