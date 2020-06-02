using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day5Model
    {
        public Day5Model(double[] x)
        {
            
            this.Day5Points = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.Day5Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        

        public IList<DataPoint> Day5Points { get; private set; }
    }
}
