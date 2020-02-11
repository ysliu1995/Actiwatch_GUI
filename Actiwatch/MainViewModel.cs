using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class MainViewModel
    {
        public MainViewModel(double[] x)
        {
            this.Title = "X axis";
            this.Points = new List<DataPoint>();
            for(int i = 0; i < 86400; i++)
            {
                this.Points.Add(new DataPoint(i, x[i]));
            }
        }
        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
}
