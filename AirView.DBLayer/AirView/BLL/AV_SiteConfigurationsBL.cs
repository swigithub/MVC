using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
  public  class AV_SiteConfigurationsBL
    {
        public List<AV_SiteConfigurations> ToList(string Filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null)
        {

            AV_SiteConfigurationsDL st = new AV_SiteConfigurationsDL();
            DataTable dt = st.Get(Filter, Value1, Value2, Value3, Value4);
            return dt.ToList<AV_SiteConfigurations>();
            //List<AV_SiteConfigurations> lst = new List<AV_SiteConfigurations>();
            //if (dt!=null && dt.Rows.Count > 0)
            //{

            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        AV_SiteConfigurations conf = new AV_SiteConfigurations();

            //        conf.ClientId = int.Parse(dt.Rows[i]["ClientId"].ToString());
            //        conf.CityId = int.Parse(dt.Rows[i]["CityId"].ToString());

            //        conf.SiteId =(dt.Columns.Contains("SiteId"))? int.Parse(dt.Rows[i]["SiteId"].ToString()):0;
            //        conf.RevisionId =(!string.IsNullOrEmpty(dt.Rows[i]["RevisionId"].ToString())) ? int.Parse(dt.Rows[i]["RevisionId"].ToString()):0;

            //        conf.TestTypeId = int.Parse(dt.Rows[i]["TestTypeId"].ToString());
            //        conf.KpiId = int.Parse(dt.Rows[i]["KpiId"].ToString());
            //        conf.KpiValue = dt.Rows[i]["KpiValue"].ToString();
            //        conf.TestCatogoryId = int.Parse(dt.Rows[i]["TestCategoryId"].ToString());

            //        conf.TestCategory = dt.Rows[i]["TestCategory"].ToString();
            //        conf.TestType = dt.Rows[i]["TestType"].ToString();
            //        conf.KPI = dt.Rows[i]["KPI"].ToString();
            //        conf.kpiKey = dt.Rows[i]["kpiKey"].ToString();
            //        lst.Add(conf);
            //    }
            //}

           // return lst;
        }
    }
}
