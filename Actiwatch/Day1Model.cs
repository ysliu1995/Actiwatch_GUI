using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day1Model
    {
        public Day1Model(double[] x)
        {
            this.Day1title = "X axis";
            this.Day1Points = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.Day1Points.Add(new DataPoint((double)i/3600, x[i]));
            }
        }
        public string Day1title { get; private set; }

        public IList<DataPoint> Day1Points { get; private set; }
    }
}
