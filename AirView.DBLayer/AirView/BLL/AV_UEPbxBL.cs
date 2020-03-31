using SWI.Libraries.AirView.DAL;
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
   public class AV_UEPbxBL
    {
        AV_UEPbxDL ued = new AV_UEPbxDL();

        public AV_UEPbx ToSingle(string filter, string value = null)
        {
            try
            {
                DataTable dt = ued.Get( filter,  value );
                var ue = dt.ToList<AV_UEPbx>();
                if (ue.Count > 0)
                {
                    return ue.FirstOrDefault();
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<AV_UEPbx> ToList(string filter, string value = null)
        {
            try
            {
                DataTable dt = ued.Get(filter, value);
                List<AV_UEPbx> log = dt.ToList<AV_UEPbx>();
                return log;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Manage(string filter, AV_UEPbx ue) {
         return   ued.Manage(filter,ue.UEId,ue.UEName,ue.IMEI,ue.IsIdle,ue.DeviceToken);


        }
    }
}
