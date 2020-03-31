using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_UEPbx
    {
        public Int64 UEId { get; set; }
        public string UEName { get; set; }
        public string IMEI { get; set; }
        public bool IsIdle { get; set; }
        public string DeviceToken { get; set; }
    }
}
