using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
    public class AV_DriveRoutesDL
    {
        public int Manage(string Filter, Int64 RouteId,string RoutePath ,Int64  CreatedBy,Int64 SiteId, Int64 ScopeId,string TestType,bool IsSelected)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDriveRoutes");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);
                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@RouteId",RouteId, 
                                                   "@RoutePath",RoutePath, "@CreatedBy",CreatedBy, "@SiteId", SiteId, "@ScopeId", ScopeId, "@TestType",TestType,
                                                   "@IsSelected", IsSelected));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get(string filter,string value = null, string value1 = null, string value2 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetDriveRoutes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@Value", value, "@Value1", value1, "@Value2", value2));
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
