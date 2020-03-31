using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_DeviceLockCommands
    {
        public Int64 CmdId { get; set; }
        public string MenuType { get; set; }
        public string DeviceModel { get; set; }
        public int RATCode { get; set; }
        public Int64 NetworkModeId { get; set; }
        public int BandCode { get; set; }
        public Int64 BandId { get; set; }
        public string RATText { get; set; }
        public string BandText { get; set; }
        public string CommandCode { get; set; }
    }
}
