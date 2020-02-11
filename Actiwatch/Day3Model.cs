using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day3Model
    {
        public Day3Model(double[] x)
        {
            this.Day3title = "X axis";
            this.Day3Points = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.Day3Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        public string Day3title { get; private set; }

        public IList<DataPoint> Day3Points { get; private set; }
    }
}
