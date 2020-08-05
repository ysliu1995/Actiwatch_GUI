using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    struct Atg
    {
        public double[] actAtg;
        public double[] ppAtg;
    }
    struct SleepIndex
    {
        public double SE;
        public double SOT;
        public double WASO;
        public double TST;
    }
    class SleepAlgorithm
    { 
        
        private int ChooseMode(double[] ppAtg)
        {
            List<double> density = new List<double>();
            int windowsize = 25;
            int cnt = 0;
            for(int i=0;i< ppAtg.Length - windowsize + 1;i++)
            {
                cnt = 0;
                for(int j = 0; j < 25; j++)
                    if (ppAtg[i + j] == 1) cnt++;
                density.Add((double)cnt/25);
            }
            if (density.Sum()/density.Count > 0.0575)
                return 0;
            else
                return 1;
        }
        private int[] CheckSOT(int[] sta)
        {
            int cntThreshold = 6;
            int[] stage = new int[sta.Length];
            int cnt = 0;
            int position = 0;

            stage = (int[])sta.Clone();
            for(int i = 0; i < stage.Length; i++)
            {
                if (stage[i] == 0)
                {
                    cnt++;
                    if (cnt >= cntThreshold)
                    {
                        position = i;
                        break;
                    }
                }
                else
                    cnt = 0;
            }
            if(position-cntThreshold >= 0)
            {
                for(int i = 0; i < position - cntThreshold;i++)
                {
                    stage[i] = 1;
                }
            }
            return stage;
        }
        private int[] GroupReduce(double[] actAtg, double[] ppAtg, int[] sta)
        {
            int[] stage = new int[sta.Length];
            int cnt = 0;
            int start = 0;
            int end = 0;
            List<List<int>> store = new List<List<int>>();
            int groupThreshold = 3;

            stage = (int[])sta.Clone();
            for(int i = 0; i < stage.Length; i++)
            {
                if((stage[i] == 1 || stage[i] ==2) && cnt == 0)
                {
                    cnt++;
                    start = i;
                }
                else if (stage[i] == 1 || stage[i] == 2)
                {
                    cnt++;
                }
                else if (stage[i] ==0 && cnt >= groupThreshold)
                {
                    end = i - 1;
                    cnt = 0;
                    List<int> tmp = new List<int>();
                    tmp.Add(start);
                    tmp.Add(end);
                    store.Add(tmp);
                    start = -1;
                    end = 0;
                }
                else
                {
                    start = -1;
                    cnt = 0;
                }
            }
            if(start!=-1 && cnt >= groupThreshold)
            {
                List<int> tmp = new List<int>();
                tmp.Add(start);
                tmp.Add(end);
                store.Add(tmp);
            }
            for(int i = 0; i < store.Count; i++)
            {
                int s = store[i][0];
                int e = store[i][1];
                int startFlag = 1;
                int finishFlag = 1;
                while (startFlag == 1)
                {
                    if (s == 0)
                        startFlag = 0;
                    else
                    {
                        if (actAtg[s] == 1)
                        {
                            stage[s] = 0;
                            s++;
                            if (s == actAtg.Length)
                                startFlag = 0;
                        }
                        else
                            startFlag = 0;
                    }
                }
                while (finishFlag == 1)
                {
                    if (actAtg[e] == 1)
                    {
                        stage[e] = 0;
                        e--;
                        if (e == -1)
                            finishFlag = 0;
                    }
                    else
                        finishFlag = 0;
                }
            }

            return stage;
        }
        private Atg Preprocessing(double[] z)
        {
            int totalEpoch = (z.Length / 30);
            double[] actAtg = new double[totalEpoch];
            double[] ppAtg = new double[totalEpoch];
            double[] diff = new double[29];
            List<int> interval = new List<int>();

            int cnt = 0;
            double threshold = 70;

            for(int i = 0; i < totalEpoch; i++)
            {
                cnt = 0;
                for (int j = 1; j < 30; j++) diff[j - 1] = z[i*30+j] - z[i* 30 + j - 1];
                for(int j = 1; j < 28; j++)
                {
                    if(diff[j] > diff[j-1] && diff[j] > diff[j+1] && diff[j] >= threshold)
                    {
                        cnt++;
                        interval.Add(j);
                    }
                }
                if (cnt >= 1)
                    actAtg[i] = 1; //movement
                else
                    actAtg[i] = 0; //No movement

                if (cnt >= 2)
                {
                    int minInterval = 30;
                    for(int j = 1; j < interval.Count; j++)
                        if (interval[j] - interval[j - 1] < minInterval) minInterval = interval[j] - interval[j - 1];
                    if (minInterval <= 11)
                        ppAtg[i] = 1;
                    else
                        ppAtg[i] = 0;
                } 
                else
                    ppAtg[i] = 0; //No movement
            }
            Atg atg;
            atg.actAtg = actAtg;
            atg.ppAtg = ppAtg;
            return atg;
        }

        private int[] algorithm(double[] actAtg, double[] ppAtg, int mode)
        {
            int windowSize = 0;
            double densityThreshold = 0.1;
            if (mode == 0)
                windowSize = 79;
            else
                windowSize = 13;
            List<Double> density = new List<double>();
            int wh = (windowSize - 1) / 2;
            int t = actAtg.Length;
            for(int i = 0; i < t; i++)
            {
                if (i < wh)
                {
                    int cnt = 0;
                    for(int j = 0; j < i + wh; j++)
                    {
                        if (ppAtg[j] == 1) cnt++;
                    }
                    density.Add((double)cnt / (i + wh + 1));
                }
                else if (i >= t - wh)
                {
                    int cnt = 0;
                    for (int j = i-wh; j < t; j++)
                    {
                        if (ppAtg[j] == 1) cnt++;
                    }
                    density.Add((double)cnt / (t-i+1+wh));
                }
                else
                {
                    int cnt = 0;
                    for (int j = i - wh; j < i+wh+1; j++)
                    {
                        if (ppAtg[j] == 1) cnt++;
                    }
                    density.Add((double)cnt / windowSize);
                }
            }
            int[] sta = new int[t];
            for(int i = 0;i < t; i++){
                if (actAtg[i] == 1)
                    sta[i] = 1;
                else if (density[i] >= densityThreshold)
                    sta[i] = 1;
                else
                    sta[i] = 0;
            }
            return sta;
        }

        public int[] Run(double[] z)
        {
            Atg atg = Preprocessing(z);
            int mode = ChooseMode(atg.ppAtg);
            int[] sta = algorithm(atg.actAtg, atg.ppAtg, mode);
            sta = CheckSOT(sta);
            sta = GroupReduce(atg.actAtg, atg.ppAtg, sta);
            return sta;
        }

        public SleepIndex SleepQuality(int[] sta)
        {
            SleepIndex SI;
            int wake = 0;
            int sleep = 0;
            int onset = 0;
            for (int i = 0; i < sta.Length; i++)
            {
                if (sta[i] == 1)
                    wake++;
                else
                    sleep++;
            }
            while (sta[onset] == 1)
                onset++;
            Console.WriteLine(sleep);
            Console.WriteLine(wake);
            Console.WriteLine(onset);
            SI.SE = ((double)(sleep) / (sleep + wake)) / 2 * 100;
            SI.SOT = (double)onset / 2;
            SI.WASO = (double)(wake - onset) / 2;
            SI.TST = (double)sleep / 2;
            return SI;
        }
    }
}
