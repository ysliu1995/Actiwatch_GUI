using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    public class TempViewModel
    {
        public TempViewModel(string datetime, float[] temp)
        {
            this.TempTitle = "Temperature";
            this.TempPoints = new List<DataPoint>();
            DateTime taskDate = DateTime.ParseExact(datetime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            for (int i = 0; i < 86400; i++)
            {
                this.TempPoints.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(dt), temp[i]/10));
                time++;
                dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            }
            //for (int i = 0; i < 86400; i++)
            //{
            //    this.TempPoints.Add(new DataPoint((double)i / 3600, temp[i]/10));
            //}
        }
        public string TempTitle { get; private set; }
        public IList<DataPoint> TempPoints { get; private set; }
    }
}
