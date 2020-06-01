using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class GsensorViewModel
    {
        public GsensorViewModel(double[] x)
        {
            this.Title = "Vector magnitude";
            this.Points = new List<DataPoint>();
            for(int i = 0; i < 86400; i++)
            {
                this.Points.Add(new DataPoint((double)i / 3600, x[i]));
            }
        }
        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
}
