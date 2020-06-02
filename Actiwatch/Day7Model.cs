using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day7Model
    {
        public Day7Model(double[] x)
        {
            
            this.Day7Points = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.Day7Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        

        public IList<DataPoint> Day7Points { get; private set; }
    }
}
