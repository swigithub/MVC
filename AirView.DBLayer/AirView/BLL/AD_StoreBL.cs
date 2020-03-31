using AirView.DBLayer.AirView.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
 public   class AD_StoreBL
    {
        public DataTable Get(int[] app,string filter,DateTime? date)
        {
            try
            {
                AD_Store wod = new AD_Store();
                DataTable dtbl = new DataTable();
               if(filter == "get")
                {
                    dtbl = wod.Get(app, filter);
                }
               else if(filter == "search")
                {
                    dtbl = wod.Search(date);
                }
                return dtbl;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
