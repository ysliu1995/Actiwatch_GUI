using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;
namespace Actiwatch
{
    /// <summary>
    /// DialyRecord.xaml 的互動邏輯
    /// </summary>
    public partial class DialyRecord : UserControl
    {
        public DialyRecord()
        {
            InitializeComponent();

            Reload();
        }

        //選擇儲存下來的檔案(.txt)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //清除全域變數
            Global.Dialy_List = new List<DialyData>();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                { // Create an instance of StreamReader to read from a file.
                  // The using statement also closes the StreamReader.
                    string[] data;

                    using (StreamReader sr = new StreamReader(dialog.FileName))     //小寫TXT
                    {
                        String line;
                        float[] tmp_temp = new float[86400];
                        int[] tmp_light = new int[86400];
                        double[] tmp_vm = new double[86400];
                        double[] tmp_vm_diff = new double[86399];
                        int[] tmp_x = new int[86400];
                        int[] tmp_y = new int[86400];
                        int[] tmp_z = new int[86400];
                        sr.ReadLine();
                        line = sr.ReadLine();
                        Console.WriteLine(line);
                        data = line.Split(',');
                        DateTime taskDate = DateTime.ParseExact(data[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        long FirstUnixTime = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
                        DateTime InitialDate = DateTime.ParseExact(data[0].Split(' ')[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        long InitialUnixTime = ((DateTimeOffset)InitialDate).ToUnixTimeSeconds();
                        long PreTime = FirstUnixTime - InitialUnixTime + 1;
                        int index = Convert.ToInt32(PreTime);
                        for (int i = 0; i < PreTime; i++)
                        {
                            tmp_temp[i] = 0;
                            tmp_light[i] = 0;
                            tmp_vm[i] = 0;
                            tmp_x[i] = 0;
                            tmp_y[i] = 0;
                            tmp_z[i] = 0;
                        }
                        tmp_temp[index] = float.Parse(data[1]);
                        tmp_light[index] = Convert.ToInt32(data[2]);
                        tmp_vm[index] = Math.Sqrt(Math.Pow(Convert.ToDouble(data[3]), 2) + Math.Pow(Convert.ToDouble(data[4]), 2) + Math.Pow(Convert.ToDouble(data[5]), 2));
                        tmp_x[index] = Convert.ToInt32(data[3]);
                        tmp_y[index] = Convert.ToInt32(data[4]);
                        tmp_z[index] = Convert.ToInt32(data[5]);
                        index++;
                        while ((line = sr.ReadLine()) != null)
                        {
                            data = line.Split(',');

                            tmp_temp[index] = float.Parse(data[1]);
                            tmp_light[index] = Convert.ToInt32(data[2]);
                            tmp_vm[index] = Math.Sqrt(Math.Pow(Convert.ToDouble(data[3]), 2) + Math.Pow(Convert.ToDouble(data[4]), 2) + Math.Pow(Convert.ToDouble(data[5]), 2));
                            tmp_x[index] = Convert.ToInt32(data[3]);
                            tmp_y[index] = Convert.ToInt32(data[4]);
                            tmp_z[index] = Convert.ToInt32(data[5]);
                            index++;
                            if (index == 86400)
                            {
                                for (int i = 0; i < 86399; i++)
                                {
                                    tmp_vm_diff[i] = Math.Pow(tmp_vm[i + 1] - tmp_vm[i], 2);
                                }
                                DialyData tmp = new DialyData(data[0].Split(' ')[0], tmp_temp, tmp_light, tmp_vm, tmp_vm_diff, tmp_x, tmp_y, tmp_z);
                                Global.Dialy_List.Add(tmp);
                                index = 0;
                                for (int i = 0; i < 86400; i++)
                                {
                                    tmp_temp[i] = 0;
                                    tmp_light[i] = 0;
                                    tmp_vm[i] = 0;
                                    tmp_x[i] = 0;
                                    tmp_y[i] = 0;
                                    tmp_z[i] = 0;
                                }
                            }
                        }
                        if (index < 86400)
                        {

                            for (int i = index; i < 86400; i++)
                            {
                                tmp_temp[i] = 0;
                                tmp_light[i] = 0;
                                tmp_vm[i] = 0;
                                tmp_x[i] = 0;
                                tmp_y[i] = 0;
                                tmp_z[i] = 0;
                            }
                            for (int i = 0; i < 86399; i++)
                            {
                                tmp_vm_diff[i] = Math.Pow(tmp_vm[i + 1] - tmp_vm[i], 2);
                            }
                            DialyData tmp = new DialyData(data[0].Split(' ')[0], tmp_temp, tmp_light, tmp_vm, tmp_vm_diff, tmp_x, tmp_y, tmp_z);
                            Global.Dialy_List.Add(tmp);
                        }
                        //重新加入日期列表
                        DialyCombo.Items.Clear();
                        foreach (DialyData dialy in Global.Dialy_List)
                        {
                            DialyCombo.Items.Add(dialy.GetDatetime());
                        }
                        if (DialyCombo.Items.Count > 0)
                        {
                            DialyCombo.SelectedIndex = 0;
                        }
                        //時間中午12點到中午12點
                        double[] newArray = new double[86400];
                        double[] newZArray = new double[86400];
                        for (int i=0;i< Global.Dialy_List.Count - 1; i++)
                        {
                            for (int j = 0; j < 43200; j++) {
                                newArray[j] = Global.Dialy_List[i].GetVM()[j + 43200];
                                newZArray[j] = Global.Dialy_List[i].GetZ()[j + 43200];
                            }
                            for (int j = 0; j < 43200; j++) {
                                newArray[j + 43200] = Global.Dialy_List[i + 1].GetVM()[j];
                                newZArray[j + 43200] = Global.Dialy_List[i + 1].GetZ()[j];
                            } 
                            Global.Dialy_List[i].SetSleepTime(newArray, newZArray);
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Gsensor.DataContext = new GsensorViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetVM());
                            Light.DataContext = new LightViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetLight());
                            Temp.DataContext = new TempViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetTemp());
                            
                        });
                    }
                }
                catch (Exception err)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(err.Message);
                }
            }
        }
        //選擇哪一天的資料
        private void DialyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DialyCombo.SelectedIndex;
            if(index > 0)
            {
                Console.WriteLine(index + "");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // UI modify
                    Gsensor.DataContext = new GsensorViewModel(Global.Dialy_List[index].GetDatetime(), Global.Dialy_List[index].GetVM());
                    Light.DataContext = new LightViewModel(Global.Dialy_List[index].GetDatetime(), Global.Dialy_List[index].GetLight());
                    Temp.DataContext = new TempViewModel(Global.Dialy_List[index].GetDatetime(), Global.Dialy_List[index].GetTemp());
                });
            }
        }
        
        //儲存每一天的檔案
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "所有檔案 (*.*)|*.*";
            saveFileDialog.Title = "Save raw data";
            saveFileDialog.DefaultExt = "txt";//設定預設格式（可以不設）
            saveFileDialog.AddExtension = true;//設定自動在檔名中新增副檔名
            saveFileDialog.ShowDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(saveFileDialog.FileName);
                for (int i = 0; i < Global.Dialy_List.Count; i++)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog.FileName + "_" + Global.Dialy_List[i].datetime);
                    sw.WriteLine("vm,temp,light");
                    for (int j = 0; j < 86400; j++)
                    {
                        sw.WriteLine(String.Format("{0}, {1}, {2}", Global.Dialy_List[i].GetVM()[j], Global.Dialy_List[i].GetTemp()[j], Global.Dialy_List[i].GetLight()[j]));
                    }
                    sw.Close();
                    sw = new StreamWriter(saveFileDialog.FileName + "_" + Global.Dialy_List[i].datetime + "_PA");
                    sw.WriteLine("PA_cpm");
                    for (int j = 0; j < 1440; j++)
                    {
                        sw.WriteLine(String.Format("{0}", Global.Dialy_List[i].GetPhysicalActivity()[j]));
                    }
                    sw.Close();
                }
            }
            else
            {
                Console.WriteLine("Cancel");
            }
        }
        //重載資料
        private void Reload()
        {
            if(Global.Dialy_List.Count != 0)
            {
                //重新加入日期列表
                DialyCombo.Items.Clear();
                foreach (DialyData dialy in Global.Dialy_List)
                {
                    DialyCombo.Items.Add(dialy.GetDatetime());
                }
                if (DialyCombo.Items.Count > 0)
                {
                    DialyCombo.SelectedIndex = 0;
                }
                //載入圖表
                Gsensor.DataContext = new GsensorViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetVM());
                Light.DataContext = new LightViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetLight());
                Temp.DataContext = new TempViewModel(Global.Dialy_List[0].GetDatetime(), Global.Dialy_List[0].GetTemp());
            }
        }
    }
}
