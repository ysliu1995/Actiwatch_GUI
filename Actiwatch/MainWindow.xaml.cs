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
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
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
                case "PhysicalActivity":
                    usc = new PhysicalActivity();
                    GridMain.Children.Add(usc);
                    break;
                case "Sleep":
                    usc = new Sleep();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
    }
}
