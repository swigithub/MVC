using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
    public class AV_SectorColorBL
    {

        AV_SectorColorDL sd = new AV_SectorColorDL();
        public List<AV_SectorColor> ToList(string filter, Int64 UserId)
        {
            try
            {
                DataTable dt = sd.Get(filter, UserId);
                if (dt != null)
                {
                    List<AV_SectorColor> sec = dt.ToList<AV_SectorColor>();
                    return sec;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }  
    }
}
