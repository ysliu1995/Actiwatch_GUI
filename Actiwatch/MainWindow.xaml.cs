using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserControl usc = new DeviceSetting();
            GridMain.Children.Add(usc);
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            Console.WriteLine(((ListViewItem)((ListView)sender).SelectedItem).Name);
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "Download":
                    usc = new DeviceSetting();
                    GridMain.Children.Add(usc);
                    break;
                case "DialyRecord":
                    usc = new DialyRecord();
                    GridMain.Children.Add(usc);
                    break;
                case "PhysicalActivity":
                    usc = new PhysicalActivity();
                    GridMain.Children.Add(usc);
                    break;
                case "Sleep":
                    usc = new Sleep();
                    GridMain.Children.Add(usc);
                    break;
                case "Report":
                    usc = new SleepReport();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DeviceSetting ds = new DeviceSetting();
            if(ds.timer != null)
            {
                ds.timer.Stop();
            }
        }

    }
}
