﻿using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class Day6Model
    {
        public Day6Model(string datetime, double[] x, string mode)
        {
            this.Day6Points = new List<DataPoint>();
            DateTime taskDate = DateTime.ParseExact(datetime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            long time = ((DateTimeOffset)taskDate).ToUnixTimeSeconds();
            if (mode.Equals("sleep")) time += 43200;
            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            for (int i = 0; i < 86400; i++)
            {
                this.Day6Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(dt), x[i]));
                time++;
                dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(time);
            }
        }


        public IList<DataPoint> Day6Points { get; private set; }
    }
}
