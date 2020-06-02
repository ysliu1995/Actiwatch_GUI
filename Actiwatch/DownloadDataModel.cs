using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actiwatch
{
    class DownloadDataModel
    {
        public DownloadDataModel(List<double> x, List<double> y, List<double> z)
        {
            this.xPoints = new List<DataPoint>();
            this.yPoints = new List<DataPoint>();
            this.zPoints = new List<DataPoint>();
            for (int i = 0; i < x.Count; i++)
            {
                this.xPoints.Add(new DataPoint((double)i, x[i]));
                this.yPoints.Add(new DataPoint((double)i, y[i]));
                this.zPoints.Add(new DataPoint((double)i, z[i]));
            }
        }
        public IList<DataPoint> xPoints { get; private set; }
        public IList<DataPoint> yPoints { get; private set; }
        public IList<DataPoint> zPoints { get; private set; }
    }
}
