using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
  public  class FleetRptCriteria
    {
        public string ReportFilter { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string Device { get; set; }
        public int Speed { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Radius { get; set; }
        public decimal FuelConsumption { get; set; }
        public decimal FuelPrice { get; set; }
        public int startTemprature { get; set; }
        public decimal endTemprature { get; set; }
        public int parkingTime { get; set; }
        public int drivingTime { get; set; }
        public string startWork { get; set; }
        public string endWork { get; set; }
        public int Idlespeedmorethan { get; set; }
        public int Driveoverhour { get; set; }
        public int Noparkrest { get; set; }

    }
}
