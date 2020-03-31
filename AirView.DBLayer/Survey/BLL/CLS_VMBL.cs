using AirView.DBLayer.Survey.DAL;
using AirView.DBLayer.Survey.Model;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.BLL
{
  public  class CLS_VMBL
    {
            private CLS_VMDL vmd = new CLS_VMDL();
            public List<CLS_VM> ToList(string Filter, string value = null)
            {
                DataTable dt = vmd.GetDataTable(Filter, value);
            return dt.ToList<CLS_VM>();
            }
        public List<AV_SiteScript> ToListSectors(string Filter, Int64 SiteId,int LayerId)
        {
            DataTable dt = vmd.GetSectorDataTable(Filter, SiteId, LayerId);
            return dt.ToList<AV_SiteScript>();
        }

    }
}
