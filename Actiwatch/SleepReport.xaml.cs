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
    /// SleepReport.xaml 的互動邏輯
    /// </summary>
    public partial class SleepReport : UserControl
    {
        public SleepReport()
        {
            InitializeComponent();

            SE.DataContext = new SleepReportModel.SE();
            SOT.DataContext = new SleepReportModel.SOT();
            WASO.DataContext = new SleepReportModel.WASO();
            TST.DataContext = new SleepReportModel.TST();
        }
    }
}
