using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class DialyData
    {
        public string datetime;
        public float[] temp = new float[86400];
        public int[] light = new int[86400];
        public double[] vm = new double[86400];
        public double[] vmDiff = new double[86399];
        public double[] sleepVm = new double[86400];
        public double[] sleepZ = new double[86400];
        public int[] x = new int[86400];
        public int[] y = new int[86400];
        public int[] z = new int[86400];
        public int startRange;
        public int endRange;
        public string startTime;
        public string endTime;
        public Boolean haveSleep;
        public double SE;
        public double SOT;
        public double WASO;
        public double TST;

        public DialyData(string datetime, float[] temp, int[] light, double[] vm, double[] vmDiff, int[] x, int[] y, int[] z)
        {
            this.datetime = datetime;
            this.temp = (float[])temp.Clone();
            this.light = (int[])light.Clone();
            this.vm = (double[])vm.Clone();
            this.vmDiff = (double[])vmDiff.Clone();
            this.x = (int[])x.Clone();
            this.y = (int[])y.Clone();
            this.z = (int[])z.Clone();
            this.startRange = 0;
            this.endRange = 0;
            this.haveSleep = false;
            this.startTime = "";
            this.endTime = "";
        }
        public void SetStartRange(int startRange)
        {
            this.startRange = startRange;
        }
        public void SetEndRange(int endRange)
        {
            this.endRange = endRange;
        }
        public double GetStartRange()
        {
            return this.startRange;
        }
        public double GetEndRange()
        {
            return this.endRange;
        }
        public string GetDatetime()
        {
            return this.datetime;
        }
        public float[] GetTemp()
        {
            return this.temp;
        }
        public int[] GetLight()
        {
            return this.light;
        }
        public double[] GetVM()
        {
            return this.vm;
        }
        public double[] GetVMDiff()
        {
            return this.vmDiff;
        }
        public int[] GetX()
        {
            return this.x;
        }
        public int[] GetY()
        {
            return this.y;
        }
        public int[] GetZ()
        {
            return this.z;
        }
        public void SetSleepTime(double[] sleepVm, double[] sleepZ)
        {
            this.sleepVm = (double[])sleepVm.Clone();
            this.sleepZ = (double[])sleepZ.Clone();
        }
        public double[] GetSleepTime()
        {
            return this.sleepVm;
        }
        public double[] GetSleepZ()
        {
            return this.sleepZ;
        }
        public double[] GetPhysicalActivity()
        {
            double[] PA = new double[1440];
            double cpm = 0;
            for (int i = 0; i < 1440; i++)
            {
                cpm = 0;
                for (int j = 1; j < 60; j++)
                {
                    cpm += Math.Abs(this.vm[i * 60 + j] - this.vm[i * 60 + j - 1]);
                }
                PA[i] = cpm;
            }
            return PA;
        }
    }
}
