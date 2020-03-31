using AirView.DBLayer.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_WidgetsBL
    {
        private AV_WidgetsDL wd = new AV_WidgetsDL();

        public List<AV_Widgets> ToList(string filter, string value=null)
        {
            DataTable dt = wd.Get(filter, value);
            return dt.ToList<AV_Widgets>();
        }

        public AV_Widgets ToSingle(string filter, string value)
        {
            DataTable dt = wd.Get(filter, value);
            return dt.ToList<AV_Widgets>().FirstOrDefault();
        }


        public List<AV_WidgetCategory> ToListCategory(string filter, string value = null)
        {
            DataTable dt = wd.Get(filter, value);
            var rec= dt.ToList<AV_Widgets>();

            List<AV_WidgetCategory> lst = new List<AV_WidgetCategory>();

            AV_WidgetCategory obj;
            var grp =  rec.GroupBy(m => m.Category).Select(m => m.First()).ToList();
            foreach (var item in grp)
            {
                obj = new AV_WidgetCategory();
                obj.Category = item.Category;
                foreach (var item2 in rec.Where(m=>m.Category==item.Category))
                {
                    item2.SqlQuery = null;
                    obj.Widgets.Add(item2);
                }
                lst.Add(obj);
            }

            return lst;
        }
    }
}
