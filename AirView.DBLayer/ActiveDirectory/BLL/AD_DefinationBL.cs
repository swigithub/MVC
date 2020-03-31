using AirView.DBLayer.Security.Entities;
using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AD.BLL
{
    /*----MoB!----*/

    public class AD_DefinationBL
    {
        AD_DefinationDL dd = new AD_DefinationDL();

        public List<AD_Defination> RegionsToList(string value = null)
        {

            DataTable dt = dd.GetRegion(value);
            List<AD_Defination> lst = new List<AD_Defination>();


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Defination def = new AD_Defination();
                    def.DefinationName = dt.Rows[i]["Region"].ToString();
                    def.DefinationId =int.Parse( dt.Rows[i]["DefinationId"].ToString());
                  
                    lst.Add(def);
                }
            }

            return lst;
        }

        public int List_Work_Group(string Filter, string WorkgroupName)
        {
            try
            {
                return dd.List_Work_Group(Filter, WorkgroupName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Sec_Workgroup> List_Work_Group(string Filter, int WorkgroupId)
        {
            try
            {
                DataTable dataTableModel = dd.List_Work_Group(Filter, WorkgroupId);

                List<Sec_Workgroup> ListModel = dataTableModel.ToList<Sec_Workgroup>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public int List_Work_Group1(string Filter, string WorkgroupName, int WorkgroupId = 0, Int32 userId=0)
        {
            try
            {
                return dd.List_Work_Group(Filter, WorkgroupName, WorkgroupId,userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Sec_Workgroup> List_Work_Group(string Filter)
        {
            try
            {
                DataTable dataTableModel = dd.List_Work_Group(Filter);

                List<Sec_Workgroup> ListModel = dataTableModel.ToList<Sec_Workgroup>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public int Delete_Group(string Filter, int WorkgroupId)
        {
            try
            {
                return dd.Delete_Group(Filter, WorkgroupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

      

        public List<AD_Defination> ToList(string filter,string value=null)
        {

            DataTable dt = dd.Get(filter, value);
          
            List<AD_Defination> lst = dt.ToList<AD_Defination>();
           
            //List<AD_Defination> lst = new List<AD_Defination>();
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        AD_Defination def = new AD_Defination();
            //        def.DefinationName = dt.Rows[i]["DefinationName"].ToString();
            //        def.DefinationId = int.Parse(dt.Rows[i]["DefinationId"].ToString());
            //        def.PDefinationId = (!string.IsNullOrEmpty(dt.Rows[i]["PDefinationId"].ToString())) ? int.Parse(dt.Rows[i]["PDefinationId"].ToString()) : 0;
            //        def.DefinationTypeId = (!string.IsNullOrEmpty(dt.Rows[i]["DefinationTypeId"].ToString())) ? int.Parse(dt.Rows[i]["DefinationTypeId"].ToString()) : 0;
            //        def.IsActive =(dt.Columns.Contains("IsActive"))? bool.Parse(dt.Rows[i]["IsActive"].ToString()):false ;
            //        def.ColorCode = (dt.Columns.Contains("ColorCode")) ? dt.Rows[i]["ColorCode"].ToString() : "";
            //        def.KeyCode = (dt.Columns.Contains("KeyCode")) ? dt.Rows[i]["KeyCode"].ToString() : "";
            //        def.DisplayText = (dt.Columns.Contains("DisplayText")) ? dt.Rows[i]["DisplayText"].ToString() : "";
            //        def.DefinationType = (dt.Columns.Contains("DefinationType")) ? dt.Rows[i]["DefinationType"].ToString() : "";
            //        def.PDefinationName = (dt.Columns.Contains("PDefinationName")) ? dt.Rows[i]["PDefinationName"].ToString() : "";
            //        lst.Add(def);
            //    }
            //}

            return lst;
        }


        public List<object> ToColumnList(string filter, string where = null)
        {
            List<object> lst = new List<object>();
            DataTable dt = dd.GetColumn(filter, where);
            DataTable table = dd.Get("GetDisplayTypes");
            List<AD_Defination> typ = table.ToList<AD_Defination>();

            foreach (DataColumn item in dt.Columns)
            {
                var type = typ.Where(x => x.DefinationName == item.ColumnName).Select(x=>x.DisplayType).FirstOrDefault();
                if (type != null)
                {
                    lst.Add(new { name = item.ColumnName, DataType = item.DataType.FullName.ToString(), DisplayType = type });
                }
                else
                {
                    lst.Add(new { name = item.ColumnName, DataType = item.DataType.FullName.ToString(), DisplayType = "" });
                }
            }
          
            

            return lst;
        }


        public List<object> ToDefinitionList(string filter,string value = null, string select = null,string group = null)
        {
            List<object> lst = new List<object>();
            DataTable dt = dd.GetColumn(filter, value,select, group);
            //DataTable table = dd.Get("GetDisplayTypes");
            //List<object> obj = dt.ToList<object>();
            if (value == "1=0")
            {
                DataTable table = dd.Get("GetDisplayTypes");
                List<AD_Defination> typ = table.ToList<AD_Defination>();
                foreach (DataColumn item in dt.Columns)
                {
                    var type = typ.Where(x => x.DefinationName == item.ColumnName).Select(x => x.DisplayType).FirstOrDefault();
                    if (type != null)
                    {
                        lst.Add(new { name = item.ColumnName, DataType = item.DataType.FullName.ToString(), DisplayType = type });
                    }
                    else
                    {
                        lst.Add(new { name = item.ColumnName, DataType = item.DataType.FullName.ToString(), DisplayType = "" });
                    }
                }
            }
            else
            {
                int colCount = dt.Columns.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic objExpando = new System.Dynamic.ExpandoObject();
                    var obj = objExpando as IDictionary<string, object>;

                    for (int i = 0; i < colCount; i++)
                    {
                        string key = dr.Table.Columns[i].ColumnName.ToString();
                        string val = dr[key].ToString();

                        obj[key] = val;
                    }
                    lst.Add(obj);
                }
            }
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return lst;
        }

 
        public List<SelectedList> SelectedList(string filter, string value = null,string Message=null)
        {
            SelectedList sl = new SelectedList();
            
            var rec = ToList(filter, value).Select(m => new SelectedList { Text = m.DefinationName, Value = m.DefinationId.ToString() }).ToList();
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }
            
            return rec;
        }
        public List<AD_Defination> MultiSelecet(string filter, string value = null, string Message = null)
        {
            List<AD_Defination> rec = ToList(filter, value).ToList();
            
           return rec;
        }
        public AD_Defination Single(string filter, string value)
        {

            DataTable dt = dd.Get(filter, value);
            return dt.ToList<AD_Defination>().FirstOrDefault();


            //AD_Defination def = new AD_Defination();


            //if (dt!=null && dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {    
            //        def.DefinationName = dt.Rows[i]["DefinationName"].ToString();
            //        def.DefinationId = int.Parse(dt.Rows[i]["DefinationId"].ToString());
            //        def.PDefinationId = (!string.IsNullOrEmpty(dt.Rows[i]["PDefinationId"].ToString())) ? int.Parse(dt.Rows[i]["PDefinationId"].ToString()) : 0;
            //        def.DefinationTypeId = (!string.IsNullOrEmpty(dt.Rows[i]["DefinationTypeId"].ToString())) ? int.Parse(dt.Rows[i]["DefinationTypeId"].ToString()) : 0;
            //        def.IsActive = (dt.Columns.Contains("IsActive")) ? bool.Parse(dt.Rows[i]["IsActive"].ToString()) : false;
            //        def.ColorCode = (dt.Columns.Contains("ColorCode")) ? dt.Rows[i]["ColorCode"].ToString() : "";
            //        def.KeyCode = (dt.Columns.Contains("KeyCode")) ? dt.Rows[i]["KeyCode"].ToString() : "";
            //        def.DisplayText = (dt.Columns.Contains("DisplayText")) ? dt.Rows[i]["DisplayText"].ToString() : "";
            //        break;
            //    }
            //}

            //return def;
        }
        public List<AD_Defination> GetCitiesByRegionId(int Id, bool status = true)
        {

            DataTable dt = dd.GetCitiesByRegionId(Id, status);
            List<AD_Defination> lst = new List<AD_Defination>();


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AD_Defination def = new AD_Defination();
                    def.DefinationName = dt.Rows[i]["DefinationName"].ToString();
                    def.DefinationId = int.Parse(dt.Rows[i]["DefinationId"].ToString());

                    lst.Add(def);
                }
            }

            return lst;
        }


    }
}
