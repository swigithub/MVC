using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.BLL
{
    /*----MoB!----*/
    /*----06-09-2017----*/
    public class TSS_QuestionLogicBL
    {
        private TSS_QuestionLogicDL qld = new TSS_QuestionLogicDL();
        public bool Manage(string Filter,string value, DataTable dt)
        {
            try
            {
                return qld.Manage(Filter, value, dt);
            }
            catch (Exception)
            {
                throw;
            }         
        }
        public List<TSS_QuestionLogic> ToList(string Filter, string value = null)
        {
            try
            {
                DataTable dt = qld.GetDataTable(Filter, value);
                return dt.ToList<TSS_QuestionLogic>();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public List<TSS_QuestionLogic> ToQuestionIds(string Filter, string value = null)
        {
            DataTable dt = qld.GetDataTable(Filter, value);
            var result = dt.ToList<TSS_QuestionLogic>();
            string ids = result.Select(x => x.ToQuestionId).FirstOrDefault();
           List<Int64> id = ids.Split(',').Select(Int64.Parse).ToList();

            //foreach (DataRow item in dt.Rows)
            //    {
            //        Int64 ToQuestionId = Convert.ToInt64(item["ToQuestionId"].ToString());
            //        //Ids = id.Split(',').Select(Int64.Parse).ToList();
            //        Ids.Add(ToQuestionId);
            //    }
            result.FirstOrDefault().ToQuestionIdNew = id;
            return result;
        }

        //public List<Int64> ToQuestionIds(string Filter, string value = null)
        //{
        //    DataTable dt = qld.GetDataTable(Filter, value);
        //    List<Int64> Ids = new List<Int64>();
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        Int64 ToQuestionId = Convert.ToInt64(item["ToQuestionId"].ToString());
        //        //Ids = id.Split(',').Select(Int64.Parse).ToList();
        //        Ids.Add(ToQuestionId);
        //    }
        //    //List<int> Ids = dt.Split(',').Select(int.Parse).ToList();
        //    return Ids;
        //}
    }
}
