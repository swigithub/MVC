using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_VehicleGroup
    {
        public int VehicleGroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsAssign { get; set; }
        public Boolean IsDelete { get; set; }
    }
}
