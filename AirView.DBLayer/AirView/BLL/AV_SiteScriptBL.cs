using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_SiteScriptBL
    {
        AV_SiteScriptDL ssd = new AV_SiteScriptDL();
        public List<AV_SiteScript> ToList(string filter, string value, string value2 = null, string value3 = null, string value4 = null, string value5 = null)
        {
            try
            {
                DataTable dt = ssd.Get(filter, value, value2, value3, value4, value5);
                List<AV_SiteScript> rec = dt.ToList<AV_SiteScript>();
                return rec;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Save(string Filter, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageSiteScript");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", List));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

    }
}
