using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.BLL
{
    public class AD_ChartSettings_BL
    {
        public List<ChartSettings> Get(string filter, string value = null)
        {
            AD_ChartSettings_DL dl = new AD_ChartSettings_DL();
            DataTable dt = dl.Get(filter, value);
           return dt.ToList<ChartSettings>();
        }
        public bool Insert(string filter, List<ChartSettings> wo,string UserId)
        {
            try
            {
                AD_ChartSettings_DL dl = new AD_ChartSettings_DL();
                dbDataTable ddt = new dbDataTable();
                DataTable dt = ddt.List();
                if (wo != null)
                {
                    foreach(var item in wo)
                    {
                        myDataTable.AddRow(dt, "Value1", item.SrId, "Value2", item.ColorCode, "Value3", item.SeriesType, "Value4", item.TaskId, "Value5", item.Color
                     );
                    }
                }

                dl.Manage(filter, dt, UserId);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
