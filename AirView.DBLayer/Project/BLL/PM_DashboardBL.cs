using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.Model;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class PM_DashboardBL
    {
        PM_DashboardDL pd = new PM_DashboardDL();
        public List<object> ToList(string Filter, Int64 projectId,int Page,int Offset)
        {
            DataTable dt = pd.GetDataTable(Filter, projectId,Page , Offset);
            //List<PM_Dashboard> list = new List<PM_Dashboard>();
            List<object> lst = new List<object>();
            List<string> stages = new List<string>();
           
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
            return lst;
        }

        public List<PM_Dashboard> GetWorkOrder(string Filter, Int64 projectId,int Page,int Offset)
        {
            DataTable dt = pd.GetDataTable(Filter, projectId,Page,Offset);
            //List<PM_Dashboard> list = new List<PM_Dashboard>();
            List<PM_Dashboard> lst = new List<PM_Dashboard>();
            List<string> stages = new List<string>();
            lst = dt.ToList<PM_Dashboard>();
            int colCount = dt.Columns.Count;
     
            return lst;
        }

        public List<PM_Issues> GetProjectIssue(string filter, Int64 projectId, int Page, int Offset,string searchoption, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string UserId=null)
        {
            DataSet ds = pd.GetDashboardWO(filter, projectId, 5, Offset, searchoption,TaskIds,LocationIds,FromDate,ToDate,UserId);
            DataTable dt = ds.Tables[0];
            List<PM_Issues> lst = new List<PM_Issues>();
            lst = dt.ToList<PM_Issues>();
            if (ds.Tables.Count == 2)
            {
                    DataTable Count = ds.Tables[1];

                if (lst.Count!=0)
                {
                    lst.FirstOrDefault().Count = (!string.IsNullOrEmpty(Count.Rows[0]["Count"].ToString())) ? Convert.ToInt32(Count.Rows[0]["Count"].ToString()) : 0;
                }
                    
           
                    
            }
            //List<PM_Dashboard> list = new List<PM_Dashboard>();
            
          

            return lst;
        }
        public List<PM_Task> GetStages(string filter, Int64 projectId, Int64 MilestoneId)
        {
            DataTable dt = pd.GetStages(filter, projectId,MilestoneId);
            
            List<PM_Task> lst = new List<PM_Task>();
            lst = dt.ToList<PM_Task>();

            return lst;
        }

        public List<AD_Defination> GetDefination(string filter)
        {
            DataTable dt = pd.ToList(filter);
            List<AD_Defination> list = new List<AD_Defination>();
            list = dt.ToList<AD_Defination>();
            return list;


        }
    }
}
