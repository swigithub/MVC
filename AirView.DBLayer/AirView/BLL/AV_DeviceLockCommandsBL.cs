using AirView.DBLayer.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_DeviceLockCommandsBL
    {
        AV_DeviceLockCommandsDL dcd = new AV_DeviceLockCommandsDL();
        public List<AV_DeviceLockCommands> ToList(AV_DeviceLockCommands dc)
        {
            DataTable dt = dcd.Get(dc.MenuType,dc.NetworkModeId,dc.BandId,dc.DeviceModel);
            return dt.ToList<AV_DeviceLockCommands>();
        }

        public AV_DeviceLockCommands ToSingle(AV_DeviceLockCommands dc)
        {
            DataTable dt = dcd.Get(dc.MenuType, dc.NetworkModeId, dc.BandId, dc.DeviceModel);
            return dt.ToList<AV_DeviceLockCommands>().FirstOrDefault();
        }
    }
}
