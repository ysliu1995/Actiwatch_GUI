using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class TempViewModel
    {
        public TempViewModel(float[] light)
        {
            this.TempTitle = "Temperature";
            this.TempPoints = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.TempPoints.Add(new DataPoint(i, light[i]));
            }
        }
        public string TempTitle { get; private set; }
        public IList<DataPoint> TempPoints { get; private set; }
    }
}
