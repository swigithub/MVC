using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using System.Collections.Generic;
using System.Data;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_GetSettingTemplateBL
    {
        public List<AV_GetSettingTemplate> ToList(string Filter = null, string value1 = null, string value2 = null, string value3 = null, string value4 = null, string value5 = null)
        {

            AV_GetSettingTemplateDL st = new AV_GetSettingTemplateDL();
            DataTable dt = st.Get( Filter,  value1,  value2, value3, value4, value5);
            List<AV_GetSettingTemplate> lst = new List<AV_GetSettingTemplate>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AV_GetSettingTemplate Temp = new AV_GetSettingTemplate();

                    Temp.TestCateoryID = int.Parse(dt.Rows[i]["TestCategoryId"].ToString());
                    Temp.TestCategory = dt.Rows[i]["TestCategory"].ToString();

                    Temp.TestTypeID = int.Parse(dt.Rows[i]["TestTypeId"].ToString());
                    Temp.TestType = dt.Rows[i]["TestType"].ToString();

                    Temp.KpiID = (!string.IsNullOrEmpty(dt.Rows[i]["KpiId"].ToString())) ? int.Parse(dt.Rows[i]["KpiId"].ToString()): 0 ;
                    Temp.Kpi = dt.Rows[i]["Kpi"].ToString();
                    Temp.KpiValue =(dt.Columns.Contains("KpiValue"))? dt.Rows[i]["KpiValue"].ToString():"";
                    lst.Add(Temp);
                }
            }

            return lst;
        }
    }
}
