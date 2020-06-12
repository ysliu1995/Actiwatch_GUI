using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day6Model
    {
        public Day6Model(double[] x)
        {
            this.Day6title = "X axis";
            this.Day6Points = new List<DataPoint>();
            for (int i = 0; i < 86399; i++)
            {
                this.Day6Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        public string Day6title { get; private set; }

        public IList<DataPoint> Day6Points { get; private set; }
    }
}
