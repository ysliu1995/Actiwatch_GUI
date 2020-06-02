﻿using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private SerialPort port;
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
        private Thread readThread;


        public DeviceSetting()
        {
            InitializeComponent();

            GetComport();
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
                    if (port.IsOpen)
                    {
                        readThread = new Thread(receive);
                        readThread.Start();
                        Console.WriteLine("Comport opened");

                        searchButton.IsEnabled = false;
                        connectButton.IsEnabled = false;
                        disconnectButton.IsEnabled = true;
                        stopButton.IsEnabled = true;
                        downloadButton.IsEnabled = true;
                        clearButton.IsEnabled = true;
                        recordingButton.IsEnabled = true;
                    }
                }
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

        private void receive()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
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
                            total_timestamp = new string[total_page_number * 40];

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
                                    PRINT("足夠封包");
                                    GetSensorData(data);
                                    GetPageData(++index);
                                }
                            }
                            else
                            {
                                state = "Stop";
                                //PRINT("Get data finish");
                            }
                            break;
                        case "Stop":
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                downloadProgressPanel.Visibility = Visibility.Hidden;
                                downloadData.Visibility = Visibility.Visible;
                                downloadData.DataContext = new DownloadDataModel(total_x, total_y, total_z);

                                System.Windows.Forms.FolderBrowserDialog path = new System.Windows.Forms.FolderBrowserDialog();
                                path.ShowDialog();
                            });
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
            timestamp = year + "/" + (month + "").PadLeft(2, '0') + "/" + (day + "").PadLeft(2, '0') + " " +
                (hour + "").PadLeft(2, '0') + ":" +
                (minute + "").PadLeft(2, '0') + ":" +
                (second + "").PadLeft(2, '0');
            PRINT(timestamp);
            DateTime taskDate = DateTime.ParseExact(timestamp, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            long unixTime = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();

            float temp = Convert.ToInt32(SensorData[5] + SensorData[4], 16);
            int light = Convert.ToInt32(SensorData[7] + SensorData[6], 16);


            for (int j = 0; j < 40; j++)
            {
                string OneSecondDataX = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 1)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 0)], 16), 2).PadLeft(8, '0');
                string OneSecondDataY = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 3)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 2)], 16), 2).PadLeft(8, '0');
                string OneSecondDataZ = Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 5)], 16), 2).PadLeft(8, '0')
                + Convert.ToString(Convert.ToInt32(SensorData[8 + (j * 6 + 4)], 16), 2).PadLeft(8, '0');
                int x = Convert.ToInt32(OneSecondDataX, 2) > 2047 ? 65536 - Convert.ToInt32(OneSecondDataX, 2) : Convert.ToInt32(OneSecondDataX, 2);
                int y = Convert.ToInt32(OneSecondDataY, 2) > 2047 ? 65536 - Convert.ToInt32(OneSecondDataY, 2) : Convert.ToInt32(OneSecondDataY, 2);
                int z = Convert.ToInt32(OneSecondDataZ, 2) > 2047 ? 65536 - Convert.ToInt32(OneSecondDataZ, 2) : Convert.ToInt32(OneSecondDataZ, 2);
                x *= 4;
                y *= 4;
                z *= 4;
                DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(unixTime);
                string datetime = dt.ToString("yyyy/MM/dd HH:mm:ss");
                
                total_time.Add(datetime);
                total_temp.Add(temp);
                total_light.Add(light);
                total_x.Add(x);
                total_y.Add(y);
                total_z.Add(z);
                unixTime += 1;
            }
        }

        private void GetPageData(int index)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                downloadProgress.Value = (double)index / total_page_number * 100;
                progressPercentage.Text = String.Format("Downloading...({0:0.00} %)", (double)index / total_page_number * 100);
            }));
            
            PRINT((double)index / total_page_number * 100 + "");
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
        

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            downloadProgressPanel.Visibility = Visibility.Visible;

            state = "GetTotalPageNumber";

            byte[] bytestosend = { 0x55, 0x04, 0x00, 0xaa };
            port.Write(bytestosend, 0, 4);
            Console.WriteLine("comport write");
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                readThread.Suspend();
                port.Dispose();
                if (!port.IsOpen)
                {
                    searchButton.IsEnabled = true;
                    connectButton.IsEnabled = true;
                    disconnectButton.IsEnabled = false;
                    stopButton.IsEnabled = false;
                    downloadButton.IsEnabled = false;
                    clearButton.IsEnabled = false;
                    recordingButton.IsEnabled = false;
                }
            }
        }
    }
}
