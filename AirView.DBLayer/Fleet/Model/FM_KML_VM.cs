using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_KML_VM
    {
        public List<FM_Vehicle> Vehicle { get; set; }
        public FM_RouteKML KML { get; set; }
        public Boolean IsKML { get; set; }
    }
}
