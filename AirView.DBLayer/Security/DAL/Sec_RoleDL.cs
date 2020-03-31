using System;
using System.Data;
using System.Data.SqlClient;

using SWI.Libraries.Common;
namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class Sec_RoleDL
    {

        public int Manage(string Filter, int RoleId, string Name, string Description,bool ActiveStatus,string DefaultUrl)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageRole");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter",Filter, "@RoleId", RoleId, "@Name", Name, "@Description", Description, "@ModifyDate", DateTime.Now
                    , "@IsActive", ActiveStatus, "@DefaultUrl", DefaultUrl));
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

        public DataTable GetRoles(string filter,string Value=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Get_Role");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", Value));
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
