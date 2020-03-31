using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_RFPlotLegends
    {
        public Int64 srId { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 PlotTypeId { get; set; }
        public double rangeFrom { get; set; }
        public double rangeTo { get; set; }
        public string rangeColor { get; set; }
        public string KeyCode { get; set; }

        //added by junaid
        public string PlotType { get; set; }
        public int count { get; set; }

    }
}
