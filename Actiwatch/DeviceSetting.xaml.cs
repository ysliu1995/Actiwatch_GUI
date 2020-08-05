using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;

namespace Actiwatch
{
    /// <summary>
    /// DeviceSetting.xaml 的互動邏輯
    /// </summary>
    public partial class DeviceSetting : UserControl
    {
        
        private string state;
        private int total_page_number;
        private int index;
        private string[] SensorData = new string[256];
        private string[] total_timestamp;
        private List<string> total_time = new List<string>();
        private List<double> total_temp = new List<double>();
        private List<double> total_light = new List<double>();
        private List<double> total_x = new List<double>();
        private List<double> total_y = new List<double>();
        private List<double> total_z = new List<double>();
        private List<double> total_cpm1 = new List<double>();
        private List<double> total_cpm2 = new List<double>();
        

        public DispatcherTimer timer;
        public SerialPort port;
        public Thread readThread;

        public DeviceSetting()
        {
            InitializeComponent();

            GetComport();
            debugText.Text += "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GetComport();
        }
        //取得連接在PC的comport列表
        private void GetComport()
        {
            string[] ports = SerialPort.GetPortNames();

            ComportCombo.Items.Clear();
            foreach (string serialPort in ports)
            {
                if (!serialPort.Equals("COM1"))
                {
                    ComportCombo.Items.Add(serialPort);
                    if (ComportCombo.Items.Count > 0)
                    {
                        ComportCombo.SelectedIndex = 0;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenComport();

        }
        //開啟comport channel
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
                    Thread.Sleep(500);
                }
                if ((port != null) && (!port.IsOpen))
                {
                    port.Open();
                    if (port.IsOpen)
                    {
                        state = "idle";
                        readThread = new Thread(receive);
                        readThread.Start();
                        Console.WriteLine("Comport opened");

                        deviceStatus.Text = "Connected";
                        searchButton.IsEnabled = false;
                        connectButton.IsEnabled = false;
                        disconnectButton.IsEnabled = true;
                        downloadButton.IsEnabled = true;
                        clearButton.IsEnabled = true;
                        recordingButton.IsEnabled = false;

                        GetBattery();
                        timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromMilliseconds(10000);
                        timer.Tick += timer_Tick;
                        timer.Start();
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(String.Format("出問題啦:{0}", error.ToString()));
            }
        }

        //結束comport channel
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                readThread.Suspend();
                port.Close();
                Thread.Sleep(500);
                if (!port.IsOpen)
                {
                    downloadData.Visibility = Visibility.Hidden;
                    downloadData.DataContext = null;

                    timer.Stop();
                    deviceStatus.Text = "Disconnected";
                    debugText.Text = "";
                    batteryStatus.Text = "0 %";
                    batteryImage.Kind = PackIconKind.Battery0;
                    searchButton.IsEnabled = true;
                    connectButton.IsEnabled = true;
                    disconnectButton.IsEnabled = false;
                    downloadButton.IsEnabled = false;
                    clearButton.IsEnabled = false;
                    recordingButton.IsEnabled = false;
                }
            }
        }

        private void receive()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    int bytes = port.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    port.Read(buffer, 0, bytes);
                    string data = ByteArrayToHexString(buffer);
                    switch (state)
                    {
                        case "GetTotalPageNumber":
                            Console.WriteLine("start get total page number");
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                debugText.Text += "Start download data\n";
                            });
                            total_page_number = GetTotalPageNumber(data);
                            total_timestamp = new string[total_page_number * 40];

                            Console.WriteLine("Total page number : " + total_page_number);
                            if (total_page_number < 1)
                            {
                                Console.WriteLine("No data");
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    debugText.Text += "No data\n";
                                    downloadProgressPanel.Visibility = Visibility.Hidden;
                                });
                                state = "idle";
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
                                    Console.WriteLine("error");
                                }
                                else
                                {
                                    Console.WriteLine("足夠封包");
                                    GetSensorData(data);
                                    GetPageData(++index);
                                }
                            }
                            else
                            {
                                state = "Fill";
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    debugText.Text += "Download finish\n";
                                });
                                //Console.WriteLine("Get data finish");
                            }
                            break;
                        case "Fill":
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                downloadProgressPanel.Visibility = Visibility.Hidden;
                                downloadData.Visibility = Visibility.Visible;
                                downloadData.DataContext = new DownloadDataModel(total_x, total_y, total_z);

                                SaveFileDialog saveFileDialog = new SaveFileDialog();
                                saveFileDialog.Filter = "所有檔案 (*.*)|*.*";
                                saveFileDialog.Title = "Save raw data";
                                saveFileDialog.DefaultExt = "txt";//設定預設格式（可以不設）
                                saveFileDialog.AddExtension = true;//設定自動在檔名中新增副檔名
                                saveFileDialog.ShowDialog();

                                if(saveFileDialog.ShowDialog() == true)
                                {
                                    Console.WriteLine(saveFileDialog.FileName);
                                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                                    sw.WriteLine("date,temp,light,x,y,z,cpm1,cpm2");
                                    for (int i=0;i< total_time.Count; i++)
                                    {
                                        sw.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", total_time[i], total_temp[i], total_light[i], total_x[i], total_y[i], total_z[i], total_cpm1[i], total_cpm2[i]));            // 寫入文字
                                    }
                                    sw.Close();						// 關閉串流
                                }
                                else
                                {
                                    Console.WriteLine("Cancel");
                                }
                                state = "idle";
                            });
                            break;
                        case "Recording":
                            if (data.Length == 10)
                            {
                                if (data == "55010101AA")
                                {
                                    Console.WriteLine("start recording");
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        debugText.Text += "Start recording\n";
                                        downloadButton.IsEnabled = false;
                                        clearButton.IsEnabled = false;
                                        recordingButton.IsEnabled = false;
                                        timer.Stop();
                                    });
                                    state = "idle";
                                }
                            }
                            break;
                        case "Clear":
                            if(data.Length == 10)
                            {
                                if (data == "55030101AA")
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        debugText.Text += "Flash cleaning\n";
                                    });
                                }else if(data == "55030102AA")
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        debugText.Text += "Cleaning finish\n";
                                        recordingButton.IsEnabled = true;
                                    });
                                    state = "idle";
                                }
                            }
                            break;
                        case "Stop":
                            if (data.Length == 8)
                            {
                                if (data == "550600AA")
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        debugText.Text += "Stop recording\n";
                                    });
                                    state = "idle";
                                }
                            }
                            break;
                        case "Battery":
                            if (data.Length == 10)
                            {
                                double voltage = (double)Convert.ToInt32(data[4] + "" + data[5] + "" + data[6] + "" + data[7] + "", 16) / 100;
                                Console.WriteLine(voltage + "");
                                string batteryPercentage = "";
                                if (voltage >= 4.2)
                                {
                                    batteryPercentage = String.Format("{0:0.00}%", 100);
                                }else if(voltage <= 3.55)
                                {
                                    batteryPercentage = String.Format("{0:0.00}%", 0);
                                }
                                else
                                {
                                    batteryPercentage = String.Format("{0:0.00}%", (voltage - 3.55) / 0.65 * 100);
                                }
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    batteryStatus.Text = batteryPercentage;
                                    if((voltage - 3.55) / 0.65 * 100 >= 95)
                                    {
                                        batteryImage.Kind = PackIconKind.Battery100;
                                    }
                                    else if((voltage - 3.55) / 0.65 * 100 >= 80)
                                    {
                                        batteryImage.Kind = PackIconKind.Battery80;
                                    }else if((voltage - 3.55) / 0.65 * 100 >= 60)
                                    {
                                        batteryImage.Kind = PackIconKind.Battery60;
                                    }
                                    else
                                    {
                                        batteryImage.Kind = PackIconKind.Battery40;
                                    }
                                });
                                state = "idle";
                            }
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
        //解析page資料(256 bytes)
        private void GetSensorData(string data)
        {
            for (int i = 0; i < 256; i++)
            {
                SensorData[i] = (data[6 + 2 * i] + "") + (data[6 + 2 * i + 1] + "");
            }
            string timestamp = Convert.ToString(Convert.ToInt32(SensorData[3], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[2], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[1], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[0], 16), 2).PadLeft(8, '0');
            string tmp = "";
            for (int i = 2; i < 6; i++)
            {
                tmp += timestamp[i];
            }
            int year = Convert.ToInt32(tmp, 2) + 2019;
            tmp = "";
            for (int i = 6; i < 10; i++)
            {
                tmp += timestamp[i];
            }
            int month = Convert.ToInt32(tmp, 2);
            tmp = "";
            for (int i = 10; i < 15; i++)
            {
                tmp += timestamp[i];
            }
            int day = Convert.ToInt32(tmp, 2);
            tmp = "";
            for (int i = 15; i < 20; i++)
            {
                tmp += timestamp[i];
            }
            int hour = Convert.ToInt32(tmp, 2);
            tmp = "";
            for (int i = 20; i < 26; i++)
            {
                tmp += timestamp[i];
            }
            int minute = Convert.ToInt32(tmp, 2);
            tmp = "";
            for (int i = 26; i < 32; i++)
            {
                tmp += timestamp[i];
            }
            int second = Convert.ToInt32(tmp, 2);
            timestamp = year + "-" + (month + "").PadLeft(2, '0') + "-" + (day + "").PadLeft(2, '0') + " " +
                (hour + "").PadLeft(2, '0') + ":" +
                (minute + "").PadLeft(2, '0') + ":" +
                (second + "").PadLeft(2, '0');
            Console.WriteLine(timestamp);
            DateTime taskDate = DateTime.ParseExact(timestamp, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            long unixTime = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();

            float temp = Convert.ToInt32(SensorData[5] + SensorData[4], 16);
            int light = Convert.ToInt32(SensorData[7] + SensorData[6], 16);
            int cpm1 = Convert.ToInt32(SensorData[251] + SensorData[250] + SensorData[249] + SensorData[248], 16);
            int cpm2 = Convert.ToInt32(SensorData[255] + SensorData[254] + SensorData[253] + SensorData[252], 16);
            

            for (int j = 0; j < 40; j++)
            {
                string OneSecondDataX = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 1)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 0)], 16), 2).PadLeft(8, '0');
                string OneSecondDataY = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 3)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 2)], 16), 2).PadLeft(8, '0');
                string OneSecondDataZ = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 5)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 4)], 16), 2).PadLeft(8, '0');
                int x = Convert.ToInt32(OneSecondDataX, 2) > 2047 ? (65536 - Convert.ToInt32(OneSecondDataX, 2)) * -1 : Convert.ToInt32(OneSecondDataX, 2);
                int y = Convert.ToInt32(OneSecondDataY, 2) > 2047 ? (65536 - Convert.ToInt32(OneSecondDataY, 2)) * -1 : Convert.ToInt32(OneSecondDataY, 2);
                int z = Convert.ToInt32(OneSecondDataZ, 2) > 2047 ? (65536 - Convert.ToInt32(OneSecondDataZ, 2)) * -1 : Convert.ToInt32(OneSecondDataZ, 2);
                x *= 4;
                y *= 4;
                z *= 4;
                DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(unixTime);
                string datetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                
                total_time.Add(datetime);
                total_temp.Add(temp);
                total_light.Add(light);
                total_x.Add(x);
                total_y.Add(y);
                total_z.Add(z);
                total_cpm1.Add(cpm1);
                total_cpm2.Add(cpm2);
                unixTime += 1;
            }
        }
        //要求第幾個page的資料
        private void GetPageData(int index)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                downloadProgress.Value = (double)index / total_page_number * 100;
                progressPercentage.Text = String.Format("Downloading...({0:0.00} %)", (double)index / total_page_number * 100);
            }));
            
            Console.WriteLine((double)index / total_page_number * 100 + "");
            state = "GetPageData";
            string getdata = "550504" + Convert.ToString(index, 16).ToUpper().PadLeft(8, '0') + "AA";
            port.Write(HexToByte(getdata), 0, 8);
        }
        //轉換page
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
        //要求開始記錄
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Recording");
            state = "Recording";
            byte[] bytestosend = { 0x55, 0x01, 0x06, (byte)(DateTime.Now.Year - 2000), (byte)(DateTime.Now.Month), (byte)(DateTime.Now.Day), (byte)(DateTime.Now.Hour), (byte)(DateTime.Now.Minute), (byte)(DateTime.Now.Second), 0xAA };
            port.Write(bytestosend, 0, 10);
        }

        

        //啟動要求電量的timer, 時間間隔為10秒
        private void timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(state);
            if (state == "idle")
            {
                GetBattery();
            }
        }
        //要求電量
        private void GetBattery()
        {
            Console.WriteLine("Get battery");
            state = "Battery";
            byte[] bytestosend = { 0x55, 0x07, 0x00, 0xAA };
            port.Write(bytestosend, 0, 4);
        }

        //要求總共多少page
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            downloadProgressPanel.Visibility = Visibility.Visible;

            state = "GetTotalPageNumber";

            byte[] bytestosend = { 0x55, 0x04, 0x00, 0xaa };
            port.Write(bytestosend, 0, 4);
            Console.WriteLine("comport write");
        }
        
        //清空flash資料
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("start clear");
            state = "Clear";
            byte[] bytestosend = { 0x55, 0x03, 0x02, 0x00, 0xFF, 0xAA };
            port.Write(bytestosend, 0, 6);
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
        //private void StopButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("stop recording");
        //    state = "Stop";
        //    byte[] bytestosend = { 0x55, 0x02, 0x01, 0x02, 0xAA };
        //    port.Write(bytestosend, 0, 5);
        //}
    }
}
