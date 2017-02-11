using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_means
{
    class PointList
    {
        public List<PointData> pointcollection { get; set; }
        public PointList()
        {
            pointcollection = new List<PointData> { };
        }
    }
}
