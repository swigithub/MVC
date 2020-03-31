using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_ReportBL
    {
        AV_ReportDL rd = new AV_ReportDL();
        public List<AV_SiteTestLog> ToList(string filter)
        {
            try
            {
                DataTable dt = rd.Get(filter);
              //  dt.Columns.Remove("Site");
                List<AV_SiteTestLog> log = dt.ToList<AV_SiteTestLog>();
                return log;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<AV_SiteTestLog> ToList(string filter,string Columns)
        {
            try
            {
                DataTable dt = rd.Get(filter);
                string[] Column = Columns.Split(',');
                foreach (var col in Column)
                {
                    if (dt.Columns.Contains(col))
                    {
                        dt.Columns.Remove(col);
                    }
                }

                return dt.ToList<AV_SiteTestLog>();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
