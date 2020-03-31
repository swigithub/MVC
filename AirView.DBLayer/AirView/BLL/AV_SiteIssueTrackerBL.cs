using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
using System.Data;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
  public  class AV_SiteIssueTrackerBL
    {
      private  AV_SiteIssueTrackerDL sitD = new AV_SiteIssueTrackerDL();
        public bool Manage(string Filter, AV_SiteIssueTracker sit,Int64 UserId)
        {
            try
            {
               
                return sitD.Manage(Filter, sit.TrackingId, sit.SiteId, sit.TesterId, sit.NetworkModeId, sit.BandId, sit.CarrierId, sit.ScopeId, sit.Description, sit.Status,sit.Picture,sit.IssueType, UserId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        //public AV_SiteIssueTracker Single(string filter, string value1 = null, string value2 = null)
        //{

        //    DataTable dt = sitD.Get(filter, value1, value2);
        //    AV_SiteIssueTracker sit = new AV_SiteIssueTracker();

        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            sit.TrackingId = int.Parse(dt.Rows[i]["TrackingId"].ToString());
        //            sit.SiteId = int.Parse(dt.Rows[i]["SiteId"].ToString());
        //            sit.TesterId = int.Parse(dt.Rows[i]["TesterId"].ToString());
        //            sit.NetworkModeId = int.Parse(dt.Rows[i]["NetworkModeId"].ToString());
        //            sit.BandId = int.Parse(dt.Rows[i]["BandId"].ToString());
        //            sit.CarrierId = int.Parse(dt.Rows[i]["CarrierId"].ToString());
        //            sit.ScopeId = int.Parse(dt.Rows[i]["ScopeId"].ToString());
        //            sit.Description = dt.Rows[i]["Description"].ToString();
        //            sit.Status = dt.Rows[i]["Status"].ToString();
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["ReportDate"].ToString()))
        //            {
        //                sit.ReportDate = DateTime.Parse(dt.Rows[i]["ReportDate"].ToString());
        //            }
        //            sit.PTrakingId = (!string.IsNullOrEmpty(dt.Rows[i]["PTrakingId"].ToString())) ? int.Parse(dt.Rows[i]["PTrakingId"].ToString()) : 0;

        //            sit.Tester =(!dt.Columns.Contains("Tester"))? "": dt.Rows[i]["Tester"].ToString();
        //            sit.Picture = (!dt.Columns.Contains("Picture"))? "": dt.Rows[i]["Picture"].ToString();


        //            break;
        //        }
        //    }

        //    return sit;
        //}


        public List<AV_SiteIssueTracker> ToList(string filter, AV_SiteIssueTracker iss)
        {

            DataTable dt = sitD.Get(filter, iss);
            AV_SiteIssueTracker sit;

            List<AV_SiteIssueTracker> lst = new List<AV_SiteIssueTracker>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sit = new AV_SiteIssueTracker();
                    sit.TrackingId = int.Parse(dt.Rows[i]["TrackingId"].ToString());
                    sit.SiteId = int.Parse(dt.Rows[i]["SiteId"].ToString());
                    sit.TesterId = int.Parse(dt.Rows[i]["TesterId"].ToString());
                    sit.NetworkModeId = int.Parse(dt.Rows[i]["NetworkModeId"].ToString());
                    sit.BandId = int.Parse(dt.Rows[i]["BandId"].ToString());
                    sit.CarrierId = int.Parse(dt.Rows[i]["CarrierId"].ToString());
                    sit.ScopeId = int.Parse(dt.Rows[i]["ScopeId"].ToString());
                    sit.Description = dt.Rows[i]["Description"].ToString();
                    sit.Status = dt.Rows[i]["Status"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ReportDate"].ToString()))
                    {
                        sit.ReportDate = DateTime.Parse(dt.Rows[i]["ReportDate"].ToString());
                    }
                    sit.PTrakingId = (!string.IsNullOrEmpty(dt.Rows[i]["PTrakingId"].ToString())) ? int.Parse(dt.Rows[i]["PTrakingId"].ToString()) : 0;

                    sit.Tester = (!dt.Columns.Contains("Tester")) ? "" : dt.Rows[i]["Tester"].ToString();
                    sit.Picture = (!dt.Columns.Contains("Picture")) ? "" : dt.Rows[i]["Picture"].ToString();
                    sit.ImagePath = (!dt.Columns.Contains("ImagePath")) ? "" : dt.Rows[i]["ImagePath"].ToString();
                    sit.IssueTypeName = (!dt.Columns.Contains("IssueTypeName")) ? "" : dt.Rows[i]["IssueTypeName"].ToString();
                    lst.Add(sit);
                   
                }
            }

            return lst;
        }
    }
}
