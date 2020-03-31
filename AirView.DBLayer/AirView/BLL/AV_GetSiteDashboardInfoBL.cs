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
  public  class AV_GetSiteDashboardInfoBL
    {
        public AV_GetSiteDashboardInfo GetDataSet(Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId, string FilterOption)
        {
            try
            {
                AV_GetSiteDashboardInfoDL sdd = new AV_GetSiteDashboardInfoDL();
                DataSet ds = sdd.GetDataSet( SiteId,  NetworkModeId,  BandId,  CarrierId,  ScopeId,  FilterOption,null);


                AV_GetSiteDashboardInfo sd = new AV_GetSiteDashboardInfo();
                 sd.TeamMember= ds.Tables[0].ToList<SiteDashboardTeamMember>(); 

                 sd.ClientOrVendor= ds.Tables[1].ToList<SiteDashboardClientOrVendor>(); 

                 sd.PingThroughtput= ds.Tables[2].ToList<SiteDashboardThroughtputChart>().OrderBy(m=>m.SiteCode).ToList(); 
                 sd.DLThroughtput= ds.Tables[3].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList(); 
                 sd.ULThroughtput = ds.Tables[4].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList(); 
                 sd.MOMTStatus = ds.Tables[5].ToList<MOMTStatus>(); 
                 sd.HandoverStatus = ds.Tables[6].ToList<HandoverStatus>();

                sd.OoklaTestResult = ds.Tables[7].ToList<OoklaTestResult>();
             




                return sd;
            }
            catch
            {

                throw;
            }

        }



        public AV_GetSiteDashboardInfo GetSectorDataSet(Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId,string Sector ,string FilterOption)
        {
            try
            {
                AV_GetSiteDashboardInfoDL sdd = new AV_GetSiteDashboardInfoDL();
                DataSet ds = sdd.GetDataSet(SiteId, NetworkModeId, BandId, CarrierId, ScopeId, FilterOption, Sector);


                AV_GetSiteDashboardInfo sd = new AV_GetSiteDashboardInfo();

                sd.PingThroughtput = ds.Tables[0].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList();
                sd.DLThroughtput = ds.Tables[1].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList();
                sd.ULThroughtput = ds.Tables[2].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList();
                sd.SiteSectorInfo = ds.Tables[3].ToList<SiteSectorInfo>(); //.OrderBy(m => m.SiteCode).ToList();

             
                sd.PciSignalStrength = ds.Tables[4].ToList<PciSignalStrength>();
            List< SiteDashboardThroughtputChartmomt > objMTSMTMOSMT = ds.Tables[5].ToList<SiteDashboardThroughtputChartmomt>();
                if(objMTSMTMOSMT.Count()>0)
                {
                    sd.GraphDataMTMOSMOSMT = objMTSMTMOSMT.GroupBy(x => x.TestType).Select(p => new groupByTestType
                    {

                        TestTypeGroup = p.Key,
                        SiteDashboardThroughtputChartmomtList = p.OrderBy(x=>x.SiteCode).ToList()

                    }).ToList();

                }

                return sd;
            }
            catch
            {

                throw;
            }

        }
    }
}
