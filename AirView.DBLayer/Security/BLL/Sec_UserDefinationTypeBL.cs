using AirView.DBLayer.Security.DAL;
using AirView.DBLayer.Security.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace SWI.Libraries.Security.BLL
{
    public class Sec_UserDefinationTypeBL
    {
        private Sec_UserDefinationTypeDL pd = new Sec_UserDefinationTypeDL();
        public List<Sec_UserDefinationType> ToList(string filter, string value = null, string value2 = null)
        {

            DataTable dt = pd.Get(filter, value);
            return DataTableToList(dt);


        }
        private List<Sec_UserDefinationType> DataTableToList(DataTable dt)
        {
            List<Sec_UserDefinationType> lstPermission = new List<Sec_UserDefinationType>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstPermission.Add(DataTableToObject(dt, i));
                }
            }

            return lstPermission;
        }
        private Sec_UserDefinationType DataTableToObject(DataTable dt, int Row)
        {
            Sec_UserDefinationType Per = new Sec_UserDefinationType();

            Per.Id = DataType.ToInt64(dt.Rows[Row]["Id"].ToString());
            Per.UserId = DataType.ToInt64(dt.Rows[Row]["UserId"].ToString());
            Per.DefinationTypeId = DataType.ToInt64(dt.Rows[Row]["DefinationTypeId"].ToString());
            //
            return Per;
        }

        public List<Sec_UserDefinationType> ToListDefinations(string filter,string value)
        {
            DataTable dt = pd.Get(filter,value);
            List<Sec_UserDefinationType> lst = new List<Sec_UserDefinationType>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Sec_UserDefinationType def = new Sec_UserDefinationType();

                    def.DefinationType = dt.Rows[i]["DefinationType"].ToString();
                    def.DefinationTypeId = int.Parse(dt.Rows[i]["DefinationTypeId"].ToString());
                    def.PDefinationTypeId = (!string.IsNullOrEmpty(dt.Rows[i]["PDefinationTypeId"].ToString())) ? int.Parse(dt.Rows[i]["PDefinationTypeId"].ToString()) : 0;
                    lst.Add(def);
                }
            }
            return lst;
        }
    }
}
