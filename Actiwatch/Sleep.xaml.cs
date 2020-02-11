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
        private Day1Model oxyPlotModel;
        public Sleep()
        {
            InitializeComponent();
            Day1.DataContext = new Day1Model(Global.Dialy_List[0].GetVM());
            Day1.TitleFontSize = 10;
            lin2axes.FontSize = 8;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PRINT(Global.Dialy_List[0].GetDatetime());
        }
        private void PRINT(string text)
        {
            Console.WriteLine(text);
        }
    }
}
