using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;


namespace AirView.DBLayer.AirView.BLL
{
    //----King--Coder--Safi-UK-----//
    public class AD_DefinationTypesBL
    {

        AD_DefinationTypesDL dtd = new AD_DefinationTypesDL();
        public List<AD_DefinationTypes> ToList(string filter, string value = null)
        {
            DataTable dt = dtd.Get(filter,value);
            List<AD_DefinationTypes> lst = new List<AD_DefinationTypes>();
            if (dt!=null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_DefinationTypes def = new AD_DefinationTypes();
                    def.DefinationType = dt.Rows[i]["DefinationType"].ToString();
                    def.DefinationTypeId = int.Parse(dt.Rows[i]["DefinationTypeId"].ToString());
                    def.PDefinationTypeId =(!string.IsNullOrEmpty(dt.Rows[i]["PDefinationTypeId"].ToString())) ?int.Parse(dt.Rows[i]["PDefinationTypeId"].ToString()):0;
                    lst.Add(def);
                }
            }
            return lst;
        }
        public bool DeleteSingleDefination(string filter, string value)
        {
            bool dt = dtd.Delete(filter, value);
            return dt;
        }

        public void Insert(string Filter,AD_DefinationTypes dt)
        {
            AD_DefinationTypesDL dtdl = new AD_DefinationTypesDL();
             bool resul=dtdl.add(Filter, dt);
        }

        public AD_DefinationTypes SingleDefinationType(string filter ,string value)
        {
            DataTable dt = dtd.GetSingle(filter,value);
            AD_DefinationTypes def = new AD_DefinationTypes();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    def.DefinationType = dt.Rows[i]["DefinationType"].ToString();
                    def.DefinationTypeId = int.Parse(dt.Rows[i]["DefinationTypeId"].ToString());
                    def.PDefinationTypeId = (!string.IsNullOrEmpty(dt.Rows[i]["PDefinationTypeId"].ToString())) ? int.Parse(dt.Rows[i]["PDefinationTypeId"].ToString()) : 0;
                    //def.IsActive  = bool.Parse(dt.Rows[i]["IsActive"].ToString())? bool.Parse(dt.Rows[i]["IsActive"].ToString()) : true;
                    def.IsActive = (Boolean)dt.Rows[i]["IsActive"];
                }
            }
            return def;
        }

        public List<AD_DefinationTypes> Paging(int Skip, int Take, string Search, ref Int64 TotalCount)
        {
            try
            {

                DataSet ds = dtd.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search);
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
        private List<AD_DefinationTypes> DataTableToList(DataTable dt)
        {
            List<AD_DefinationTypes> lstPermission = new List<AD_DefinationTypes>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstPermission.Add(DataTableToObject(dt, i));
                }
            }

            return lstPermission;
        }
        private AD_DefinationTypes DataTableToObject(DataTable dt, int Row)
        {
            AD_DefinationTypes Per = new AD_DefinationTypes();

            Per.DefinationTypeId = DataType.ToInt64(dt.Rows[Row]["DefinationTypeId"].ToString());
            Per.PDefinationTypeName = dt.Rows[Row]["PDefinationTypeId"].ToString();
             Per.DefinationType = dt.Rows[Row]["DefinationType"].ToString();
            Per.IsActive = DataType.ToBoolean(dt.Rows[Row]["IsActive"].ToString());
            Per.SortOrder = (dt.Columns.Contains("SortOrder")) ? DataType.ToInt32(dt.Rows[Row]["SortOrder"].ToString()) : 0;
            //
            return Per;
        }
    }
}
