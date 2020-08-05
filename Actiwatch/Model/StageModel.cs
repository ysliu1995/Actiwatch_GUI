using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class StageModel
    {
        public StageModel(string datetime, int[] x)
        {
            this.StagePoints = new List<DataPoint>();
            this.StageTitle = "Sleep stage";
            DateTime taskDate = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            for (int i = 0; i < x.Length; i++)
            {
                this.StagePoints.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(dt), x[i]));
                time += 30;
                dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            }
        }

        public string StageTitle { get; private set; }
        public IList<DataPoint> StagePoints { get; private set; }
    }
}
