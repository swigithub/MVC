using System;
using System.Data;
using System.Data.SqlClient;

using SWI.Libraries.Common;
namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class Sec_UserDL
    {
        public  int SaveNew_Update(Int64 UserId,Int64 RoleId, string FirstName, string LastName, string UserName,string password, string Email,string Address,string contact,double? homeLatitude,double? homeLongitude,string Title="",string Gender="",decimal? CompanyId=0,string Designation="",DateTime? HiringDate=null,decimal? ReportToId=0,string Color="",bool IsManager=false)
        {
           
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_User_Insert_Update");
                

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@UserId", UserId, "@RoleId",RoleId, "@FirstName", FirstName, "@LastName", LastName, "@UserName", UserName, "@Password", password,
                                                    "@Address",Address, "@Contact",contact, "@Email", Email, "@Update_at",DateTime.Now, "@homeLatitude", homeLatitude, "@homeLongitude", homeLongitude, "@Title", Title, "@Gender", Gender, "@CompanyId", CompanyId, "@Designation", Designation, "@HiringDate", HiringDate, "@ReportToId", ReportToId, "@Color", Color, "@IsManager", IsManager));

                DataContext.EndTransaction(loCommand);

                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch(Exception ex)
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Manage(string Filer, string value1 = null, string value2 = null, string value3 = null, string value4 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUsers");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer, "@value1", value1, "@value2", value2, "@value3", value3, "@value4", value4));
                DataContext.EndTransaction(loCommand);
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

        public  DataTable Get(string filter, string Value1=null, string Value2 = null, string Value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUsers");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3));
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

        public  DataSet GetDataSet(string filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetUsers");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value1", Value1, "@Value2", Value2, "@Value3", Value3, "@Value4", Value4));
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

        public bool ManageUserDefinationTypes(int UserId, DataTable data)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageUserDefinationTypes");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@UserId", UserId, "@List", data));
                DataContext.EndTransaction(loCommand);
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
    }
}
