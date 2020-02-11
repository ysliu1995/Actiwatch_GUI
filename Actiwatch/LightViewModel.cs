using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class LightViewModel
    {
        public LightViewModel(int[] light)
        {
            this.LightTitle = "Light";
            this.LightPoints = new List<DataPoint>();
            for (int i = 0; i < 86400; i++)
            {
                this.LightPoints.Add(new DataPoint(i, light[i]));
            }
        }
        public string LightTitle { get; private set; }
        public IList<DataPoint> LightPoints { get; private set; }
    }
    
}
