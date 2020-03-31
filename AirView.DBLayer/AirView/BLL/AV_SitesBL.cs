using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
  public  class AV_SitesBL
    {
        /*----MoB!----*/

        AV_SitesDL sd = new AV_SitesDL();
        public List<AV_Site> ToList(string filter, string value = null, string NetworkModeId = null, string BandId = null, string CarrierId = null, string ScopeId = null, string value2 = null, string value3 = null)
        {
            try
            {
                DataTable dt = sd.Get(filter, value , NetworkModeId ,  BandId  , CarrierId, ScopeId,value2,value3  );
                List<AV_Site> Sit = dt.ToList<AV_Site>();
                return Sit;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public DataTable ToDataTable(string filter, string value = null, string NetworkModeId = null, string BandId = null, string CarrierId = null, string ScopeId = null, string value2 = null, string value3 = null)
        {
            try
            {
                return sd.Get(filter, value, NetworkModeId, BandId, CarrierId, ScopeId, value2, value3);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public AV_Site Single(string filter, string value = null, string NetworkModeId = null, string BandId = null, string CarrierId = null, string ScopeId = null)
        {
            try
            {
                DataTable dt = sd.Get(filter, value, NetworkModeId, BandId, CarrierId, ScopeId);
                if (dt!=null)
                {
                    List<AV_Site> Sit = dt.ToList<AV_Site>();
                    if (Sit.Count > 0)
                    {
                        return Sit.FirstOrDefault();
                    }
                }
                
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void SiteWithSectors(int SiteId,ref AV_Site Site,ref List<AV_Sector> Sectors)
        {
            try
            {
                DataSet ds = sd.GetDataSet("SiteWithSectors", SiteId.ToString());
                if (ds.Tables.Count>0)
                {
                    DataTable Sit = ds.Tables[0];

                    var rec = Sit.ToList<AV_Site>();
                    if (rec.Count > 0)
                    {
                        Site = rec.FirstOrDefault();
                    }
                    DataTable sec = ds.Tables[1];
                    Site.Sectors= sec.ToList<AV_Sector>(); 

                    Sectors = sec.ToList<AV_Sector>();
                }
               

            }
            catch 
            {
                throw;
            }

            
        }

        //public bool Manage(string Filter, AV_Site s)
        //{
        //    return sd.Manage(Filter, s.SiteId.ToString(), s.IsActive.ToString(), null,null,null);
        //}
    }
}
