using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.DAL
{
   public class TSS_DashboardDL
    {

        public bool Manage(string Filter, DataTable List, int siteId, int siteSurveyId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteAttendees");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", List, "@SiteId", siteId, "@SiteSurveyId", siteSurveyId));
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

        public DataTable GetAttendees(string filter, int siteid, int sitesurveyid)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetSurveySiteAttendees");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@SiteID", siteid, "@SiteSurveyID", sitesurveyid));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

    }
}
