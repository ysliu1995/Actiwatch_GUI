using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class GsensorViewModel
    {
        public GsensorViewModel(string datetime, double[] x)
        {
            this.Title = "G-sensor";
            this.Points = new List<DataPoint>();
            DateTime taskDate = DateTime.ParseExact(datetime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            for (int i = 0; i < 86400; i++)
            {
                this.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(dt), x[i]));
                time++;
                dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            }
            //for (int i = 0; i < 86400; i++)
            //{
            //    this.Points.Add(new DataPoint((double)i / 3600, x[i]));
            //}
        }
        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; }
    }
}
