using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day2Model
    {
        public Day2Model(double[] x)
        {
            this.Day2title = "X axis";
            this.Day2Points = new List<DataPoint>();
            for (int i = 0; i < 86399; i++)
            {
                this.Day2Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        public string Day2title { get; private set; }

        public IList<DataPoint> Day2Points { get; private set; }
    }
}
