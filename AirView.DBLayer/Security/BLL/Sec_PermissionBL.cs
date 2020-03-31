using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;


namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
    public class Sec_PermissionBL
    {
        private  Sec_PermissionDL pd = new Sec_PermissionDL();
        public List<Sec_Permission> ToList(string filter, string value=null,string value2=null)
        {

            DataTable dt = pd.Get(filter, value, value2);
            return DataTableToList(dt);


        }

        private List<Sec_Permission> DataTableToList(DataTable dt) {
            List<Sec_Permission> lstPermission = new List<Sec_Permission>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstPermission.Add(DataTableToObject(dt, i));
                }
            }

            return lstPermission;
        }

        private Sec_Permission DataTableToObject(DataTable dt,int Row)
        {
            Sec_Permission Per = new Sec_Permission();

            Per.Id =DataType.ToInt64(dt.Rows[Row]["Id"].ToString());
            Per.ParentId = DataType.ToInt64(dt.Rows[Row]["ParentId"].ToString());
            Per.ModuleId =(dt.Columns.Contains("ModuleId"))? DataType.ToInt64(dt.Rows[Row]["ModuleId"].ToString()):0;
          
            Per.Title = dt.Rows[Row]["Title"].ToString();
            Per.URL = dt.Rows[Row]["URL"].ToString();
            Per.Code = dt.Rows[Row]["Code"].ToString();
            Per.Icon = dt.Rows[Row]["Icon"].ToString();
            Per.IsMenuItem =DataType.ToBoolean(dt.Rows[Row]["IsMenuItem"].ToString());
            Per.IsUsed = DataType.ToBoolean(dt.Rows[Row]["IsUsed"].ToString());
            Per.IsModule = DataType.ToBoolean(dt.Rows[Row]["IsModule"].ToString());
            Per.SortOrder = (dt.Columns.Contains("SortOrder")) ? DataType.ToInt32(dt.Rows[Row]["SortOrder"].ToString()):0;
            //
            return Per;
        }

        public Sec_Permission Single(string filter, string value, string value2 = null)
        {

            DataTable dt = pd.Get(filter, value, value2);
            Sec_Permission Per = new Sec_Permission();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Per = DataTableToObject(dt, 0);
                }

            }

            return Per;
        }


        public int GetLastId()
        {
            try
            {
                DataTable dt = pd.Get("LastId", "");
                List<Sec_Permission> lstPermission = new List<Sec_Permission>();
                int LastId = 0;
                if (dt.Rows.Count > 0)
                {
                    LastId = int.Parse(dt.Rows[0]["Id"].ToString());
                }
                return LastId;
            }
            catch (Exception)
            {

                return 0;
            }
           
        }

        public bool Manage(string Filter, Sec_Permission p) {
          return  pd.Manage(Filter, p.Id, p.ParentId, p.Title, p.URL, p.Code, p.Icon, p.IsMenuItem, p.IsUsed,p.ModuleId,p.IsModule);
        }


        public List<Sec_Permission> Paging(int Skip, int Take, string Search, ref int TotalCount)
        {
            try
            {

                DataSet ds = pd.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search);
                if (ds.Tables.Count > 0)
                {
                    DataTable Count = ds.Tables[1];
                    if (Count != null && Count.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(Count.Rows[0]["TotalRecord"].ToString());
                    }

                    return DataTableToList(ds.Tables[0]);
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
