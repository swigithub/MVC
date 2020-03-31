using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
   public class AV_NetLayerStatusBL
    {
        AV_NetLayerStatusDL nlsd = new AV_NetLayerStatusDL();
        public AV_NetLayerStatus ToSingle(string filter, int NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 SiteId,string Value1)
        {
            DataTable dtbl = new DataTable();
            dtbl = nlsd.Get( filter,  NetworkModeId,  BandId,  CarrierId,  SiteId, Value1);
            List<AV_NetLayerStatus> NetLayerStatus = dtbl.ToList<AV_NetLayerStatus>();
            if (NetLayerStatus.Count>0)
            {
                return NetLayerStatus.FirstOrDefault();
            }
            return null;
        }


        public DataTable GetNetworkLayers(string filter, int NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 SiteId, string Value1)
        {
            DataTable dtbl = new DataTable();
           return nlsd.Get(filter, NetworkModeId, BandId, CarrierId, SiteId, Value1);
           
        }
    }
}
