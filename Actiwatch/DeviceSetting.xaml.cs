using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// DeviceSetting.xaml 的互動邏輯
    /// </summary>
    public partial class DeviceSetting : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private List<String> comport_list;
        private SerialPort port;
        private string state;
        private int total_page_number;
        private int index;
        private string[] SensorData = new string[256];




        public DeviceSetting()
        {
            InitializeComponent();

            GetComport();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenComport();

        }

        private void OpenComport()
        {
            port = new SerialPort((String)ComportCombo.SelectedItem, 115200, Parity.None, 8, StopBits.One);
            port.ReadTimeout = 500;
            port.WriteTimeout = 500;
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
                if ((port != null) && (!port.IsOpen))
                {
                    port.Open();
                    //CancellationTokenSource cts = new CancellationTokenSource();
                    //Task task = Task.Run(() => receive(), cts.Token);
                    Thread readThread = new Thread(receive);
                    readThread.Start();
                    Console.WriteLine("Comport opened");
                }
                //port.DataReceived += ceive;
                
            }
            catch (Exception error)
            {
                MessageBox.Show(String.Format("出問題啦:{0}", error.ToString()));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GetComport();
        }

        private void GetComport()
        {
            string[] ports = SerialPort.GetPortNames();

            ComportCombo.Items.Clear();
            foreach (string serialPort in ports)
            {
                ComportCombo.Items.Add(serialPort);
                if (ComportCombo.Items.Count > 0)
                {
                    ComportCombo.SelectedIndex = 0;
                }
            }
        }
        private void receive()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(75);
                    int bytes = port.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    port.Read(buffer, 0, bytes);
                    string data = ByteArrayToHexString(buffer);
                    //PRINT(data);
                    switch (state)
                    {
                        case "GetTotalPageNumber":
                            PRINT("start get total page number");
                            total_page_number = GetTotalPageNumber(data);
                            total_page_number = 1;
                            PRINT("Total page number : " + total_page_number);
                            if (total_page_number < 1)
                            {
                                PRINT("No data");
                            }
                            else
                            {
                                index = 0;
                                GetPageData(index);
                            }
                            break;
                        case "GetPageData":
                            if (index < total_page_number)
                            {
                                if (data.Length != 524)
                                {
                                    GetPageData(index);
                                    PRINT("error");
                                }
                                else
                                {
                                    for (int i = 0; i < 256; i++)
                                    {
                                        SensorData[i] = (data[6 + 2 * i] + "") + (data[6 + 2 * i + 1] + "");
                                        PRINT(SensorData[i]);
                                    }
                                    PRINT(SensorData[3] + "");
                                    PRINT(SensorData[2] + "");
                                    PRINT(SensorData[1] + "");
                                    PRINT(SensorData[0] + "");
                                    //char timestamp;
                                    //for(int i = 3; i >= 0; i--)
                                    //{
                                    //    timestamp += SensorData[i];
                                    //}
                                    //string bin_timestamp = "";
                                    //for(int i = 0; i < 8; i++)
                                    //{
                                    //    PRINT(timestamp[i])
                                    //    //bin_timestamp += Convert.ToString(Convert.ToInt32(timestamp[i], 16), 2);
                                    //}
                                    //string timestamp = Convert.ToString(Convert.ToInt32(SensorData[3], 16), 2)
                                    //    + Convert.ToString(Convert.ToInt32(SensorData[2], 16), 2)
                                    //    + Convert.ToString(Convert.ToInt32(SensorData[1], 16), 2)
                                    //    + Convert.ToString(Convert.ToInt32(SensorData[0], 16), 2);
                                    //timestamp = timestamp.PadLeft(32, '0');
                                    ////PRINT(Convert.ToInt32(SensorData[1], 16));
                                    //string tmp = "";
                                    //for(int i = 2; i < 6; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int year = Convert.ToInt32(tmp, 16) + 2019;
                                    //tmp = "";
                                    //for (int i = 6; i < 10; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int month = Convert.ToInt32(tmp, 16);
                                    //tmp = "";
                                    //for (int i = 10; i < 15; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int day = Convert.ToInt32(tmp, 16);
                                    //tmp = "";
                                    //for (int i = 15; i < 20; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int hour = Convert.ToInt32(tmp, 16);
                                    //tmp = "";
                                    //for (int i = 20; i < 26; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int minute = Convert.ToInt32(tmp, 16);
                                    //tmp = "";
                                    //for (int i = 26; i < 32; i++)
                                    //{
                                    //    tmp += timestamp[i];
                                    //}
                                    //PRINT(tmp);
                                    //int second = Convert.ToInt32(tmp, 16);
                                    //PRINT(year + " year");
                                    //PRINT(month + " month");
                                    //PRINT(day + " day");
                                    //PRINT(hour + " hour");
                                    //PRINT(minute + " minute");
                                    //PRINT(second + " second");
                                    //PRINT(timestamp); 
                                    GetPageData(++index);
                                }
                            }
                            else
                            {
                                //PRINT("Get data finish");
                            }
                            break;
                        case "Recording":
                            break;
                        case "Clear":
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(String.Format("出問題啦:{0}", error.ToString()));
                }
            }
        }

        private void GetPageData(int index)
        {
            PRINT(index + "");
            state = "GetPageData";
            string getdata = "550504" + Convert.ToString(index, 16).ToUpper().PadLeft(8, '0') + "AA";
            port.Write(HexToByte(getdata), 0, 8);
        }

        private int GetTotalPageNumber(string data)
        {
            string convert = "";
            for (int i = 6; i > 2; i--)
            {
                convert += data[i * 2];
                convert += data[i * 2 + 1];
            }
            return int.Parse(convert, System.Globalization.NumberStyles.HexNumber);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            state = "GetTotalPageNumber";

            byte[] bytestosend = { 0x55, 0x04, 0x00, 0xAA };
            port.Write(bytestosend, 0, 4);
            Console.WriteLine("Comport Write");
        }

        private string ByteArrayToHexString(byte[] data)
	    { 
	        StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {   
	            sb.Append(Convert.ToString(b, 16).PadLeft(2, '0')); 
            }
	        return sb.ToString().ToUpper(); 
	    }
        private byte[] HexToByte(string hexString)
        {
            //運算後的位元組長度:16進位數字字串長/2
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                //每2位16進位數字轉換為一個10進位整數
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
        }

        private void PRINT(string text)
        {
            Console.WriteLine(text);
        }
    }
}
