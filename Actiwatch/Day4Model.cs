using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day4Model
    {
        public Day4Model(double[] x)
        {
            this.Day4title = "X axis";
            this.Day4Points = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.Day4Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        public string Day4title { get; private set; }

        public IList<DataPoint> Day4Points { get; private set; }
    }
}
