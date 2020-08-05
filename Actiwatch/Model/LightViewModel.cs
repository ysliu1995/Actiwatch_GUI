using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class LightViewModel
    {
        public LightViewModel(string datetime, int[] light)
        {
            this.LightTitle = "Light";
            this.LightPoints = new List<DataPoint>();
            DateTime taskDate = DateTime.ParseExact(datetime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            for (int i = 0; i < 86400; i++)
            {
                this.LightPoints.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(dt), light[i]/1000));
                time++;
                dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            }
            //for (int i = 0; i < 86400; i++)
            //{
            //    this.LightPoints.Add(new DataPoint((double)i / 3600, (double)light[i]/1000));
            //}
        }
        public string LightTitle { get; private set; }
        public IList<DataPoint> LightPoints { get; private set; }
    }
    
}
