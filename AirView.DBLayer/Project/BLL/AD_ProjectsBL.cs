using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class AD_ProjectsBL
    {
        AD_ProjectsDL pDL = new AD_ProjectsDL();
        public int Manage(string filter, AD_Projects p)
        {
            return pDL.Manage(filter, p.ProjectID, p.ProjectName, p.ProjectScopeID, p.CompanyID, p.VendorID, p.StartDate, p.EndDate, p.StatusID, p.Color, p.Description, p.IsActive);
        }

        public AD_Projects Single(string filter, string value = null)
        {
            try
            {
                DataTable dt = pDL.GetProjects(filter, value);
                AD_Projects projects = new AD_Projects();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 0;
                    projects.ProjectID = int.Parse(dt.Rows[i]["ProjectID"].ToString());
                    projects.ProjectName = dt.Rows[i]["ProjectName"].ToString();
                    projects.ProjectScopeID = dt.Rows[i]["ProjectScopeID"].ToString();
                    projects.CompanyID = int.Parse(dt.Rows[i]["CompanyID"].ToString());
                    projects.Company = dt.Rows[i]["Company"].ToString();
                    projects.VendorID = int.Parse(dt.Rows[i]["VendorID"].ToString());
                    projects.Vendor = dt.Rows[i]["Vendor"].ToString();
                    projects.StartDate = Convert.ToDateTime(dt.Rows[i]["StartDate"].ToString());

                    if (!String.IsNullOrEmpty((dt.Rows[i]["EndDate"].ToString())))
                    {
                        projects.EndDate = Convert.ToDateTime(dt.Rows[i]["EndDate"].ToString());
                    }

                    //projects.EndDate = Convert.ToDateTime(String.IsNullOrEmpty(dt.Rows[i]["EndDate"].ToString()) ? "NULL" : dt.Rows[i]["EndDate"].ToString());

                    projects.StatusID = int.Parse(dt.Rows[i]["StatusID"].ToString());
                    projects.Status = dt.Rows[i]["Status"].ToString();
                    projects.Color = dt.Rows[i]["Color"].ToString();
                    projects.Description = dt.Rows[i]["Description"].ToString();
                    projects.IsActive = bool.Parse(dt.Rows[i]["IsActive"].ToString());

                    return projects;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<AD_Projects> ToList(string filter, string value = null, string value1 = null, string value2 = null,
            string value3 = null, string value4 = null, string value5 = null, int pageIndex = 0, int pageSize = 0)
        {
            DataTable dt = pDL.GetProjects(filter, value, value1, value2, value3, value4, value5, pageIndex, pageSize);
            List<AD_Projects> Project = dt.ToList<AD_Projects>();
            return Project;
        }

        public List<AD_Projects> ListProject(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.GetProjects(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects p = new AD_Projects();
                    p.ProjectID = int.Parse(dt.Rows[i]["ProjectID"].ToString());
                    p.ProjectName = dt.Rows[i]["ProjectName"].ToString();
                    lst.Add(p);
                }
            }
            return lst;
        }

        public List<AD_Projects> ListCompany(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.Get(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects c = new AD_Projects();
                    c.CompanyID = int.Parse(dt.Rows[i]["ClientId"].ToString());
                    c.Company = dt.Rows[i]["ClientName"].ToString();
                    lst.Add(c);
                }
            }
            return lst;
        }

        public List<AD_Projects> ListVendors(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.Get(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects c = new AD_Projects();
                    c.VendorID = int.Parse(dt.Rows[i]["ClientId"].ToString());
                    c.Vendor = dt.Rows[i]["ClientName"].ToString();
                    lst.Add(c);
                }
            }
            return lst;
        }

        public List<AD_Projects> ListScope(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.GetScopeOrStatus(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects p = new AD_Projects();
                    p.ProjectScopeID = dt.Rows[i]["DefinationId"].ToString();
                    p.Scope = dt.Rows[i]["DefinationName"].ToString();
                    p.TypeId = int.Parse(dt.Rows[i]["DefinationTypeId"].ToString());
                    lst.Add(p);
                }
            }
            return lst;
        }

        public List<AD_Projects> ListStatus(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.GetScopeOrStatus(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects p = new AD_Projects();
                    p.StatusID = int.Parse(dt.Rows[i]["DefinationId"].ToString());
                    p.Status = dt.Rows[i]["DefinationName"].ToString();
                    lst.Add(p);
                }
            }
            return lst;
        }

        //public List<SelectedList> SelectedList(string filter, string value = null, string Message = null)
        //{
        //    SelectedList sl = new SelectedList();

        //    var rec = ToListMarket(filter, value).Select(m => new SelectedList { Text = m.DefinationName, Value = m.DefinationId.ToString() }).ToList();

        //    if (!string.IsNullOrEmpty(Message))
        //    {
        //        sl.Text = Message;
        //        sl.Value = "0";
        //        rec.Add(sl);
        //        rec = rec.OrderBy(m => m.Value).ToList();
        //    }

        //    return rec;
        //}

        //public List<AD_Projects> ToListMarket(string filter, string value = null)
        //{
        //    AD_ProjectsDL dl = new AD_ProjectsDL();
        //    DataTable dt = dl.GetCitiesMarkeet(filter, value);
        //    List<AD_Projects> lst = dt.ToList<AD_Projects>();
        //    return lst;
        //}

        public List<AD_Projects> ListCitiesMarket(string filter, string value = null)
        {
            AD_ProjectsDL dl = new AD_ProjectsDL();

            DataTable dt = dl.GetCitiesMarket(filter, value);

            List<AD_Projects> lst = new List<AD_Projects>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Projects p = new AD_Projects();
                    p.TypeValue = Convert.ToInt32(dt.Rows[i]["DefinationId"].ToString());
                    p.DefinationName = dt.Rows[i]["DefinationName"].ToString();
                    p.TypeId = int.Parse(dt.Rows[i]["DefinationTypeId"].ToString());
                    lst.Add(p);
                }
            }
            return lst;
        }

        //public AV_GetSiteDashboardInfo GetDataSet(Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopeId, string FilterOption)
        //{
        //    try
        //    {
        //        AV_GetSiteDashboardInfoDL sdd = new AV_GetSiteDashboardInfoDL();
        //        DataSet ds = sdd.GetDataSet( SiteId,  NetworkModeId,  BandId,  CarrierId,  ScopeId,  FilterOption,null);


        //        AV_GetSiteDashboardInfo sd = new AV_GetSiteDashboardInfo();
        //         sd.TeamMember= ds.Tables[0].ToList<SiteDashboardTeamMember>(); 

        //         sd.ClientOrVendor= ds.Tables[1].ToList<SiteDashboardClientOrVendor>(); 

        //         sd.PingThroughtput= ds.Tables[2].ToList<SiteDashboardThroughtputChart>().OrderBy(m=>m.SiteCode).ToList(); 
        //         sd.DLThroughtput= ds.Tables[3].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList(); 
        //         sd.ULThroughtput = ds.Tables[4].ToList<SiteDashboardThroughtputChart>().OrderBy(m => m.SiteCode).ToList(); 
        //         sd.MOMTStatus = ds.Tables[5].ToList<MOMTStatus>(); 
        //         sd.HandoverStatus = ds.Tables[6].ToList<HandoverStatus>();

        //        sd.OoklaTestResult = ds.Tables[7].ToList<OoklaTestResult>();

              


        //        return sd;
        //    }
        //    catch
        //    {

        //        throw;
        //    }

        //}
    }
}
