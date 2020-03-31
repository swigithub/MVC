using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using System.Collections.Generic;
using System.Data;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_GetScannerSettingTemplateBL
    {
        public List<AV_GetScannerSettingTemplate> ToList(string Filter = null, string value1 = null, string value2 = null, string value3 = null, string value4 = null, string value5 = null,string value6=null,string value7=null)
        {

            AV_GetScannerSettingTemplateDL st = new AV_GetScannerSettingTemplateDL();
            DataTable dt = st.Get( Filter,  value1,  value2, value3, value4, value5,value6,value7);
            List<AV_GetScannerSettingTemplate> lst = new List<AV_GetScannerSettingTemplate>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AV_GetScannerSettingTemplate Temp = new AV_GetScannerSettingTemplate();

                    Temp.TestCateoryID = int.Parse(dt.Rows[i]["TestCategoryId"].ToString());
                    Temp.TestCategory = dt.Rows[i]["TestCategory"].ToString();

                    Temp.TestTypeID = int.Parse(dt.Rows[i]["TestTypeId"].ToString());
                    Temp.TestType = dt.Rows[i]["TestType"].ToString();

                    Temp.KpiID = (!string.IsNullOrEmpty(dt.Rows[i]["KpiId"].ToString())) ? int.Parse(dt.Rows[i]["KpiId"].ToString()): 0 ;
                    Temp.Kpi = dt.Rows[i]["Kpi"].ToString();
                    Temp.DisplayType = dt.Rows[i]["DisplayType"].ToString();
                    Temp.InputType = dt.Rows[i]["InputType"].ToString();
                    Temp.SelectList = dt.Rows[i]["SelectList"].ToString();
                    Temp.KpiValue =(dt.Columns.Contains("KpiValue"))? dt.Rows[i]["KpiValue"].ToString():"";
                    lst.Add(Temp);
                }
            }

            return lst;
        }
    }
}
