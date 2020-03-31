using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.AirView.DAL
{
    /*----Mubashar----*/

    public class AD_ClientContactsDL
    {
        public bool Insert(DataTable ClientContact)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "insert_clientcontact");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", ClientContact));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch
            {
                // DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Manage(string Filter,DataTable Table)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientContacts");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", Table));
                DataContext.EndTransaction(loCommand);
                return result;

             //   loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientContacts");

             // loCommand = DataContext.StartTransaction(loCommand);
             //  int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter,"@List", Table));
             //DataContext.EndTransaction(loCommand);
             //   return 0;
            }
            catch(Exception ex)
            {
                //  DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int Manage(string Filter, int ContactId, string ContactPerson, string Designation, string Gender, string Title, decimal ContactNo, string ContactType,bool IsPrimary, int ClientId, int UserId, int RegionId, int ContactCityId, bool IsActive, int? ReportToId,DataTable Table)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientContacts");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ContactId", ContactId, "@ContactPerson", ContactPerson, "@Designation", Designation, "@Gender", Gender, "@Title", Title
                    , "@ContactNo", ContactNo, "@ContactType", ContactType, "@IsPrimary", IsPrimary, "@ClientId", ClientId, "@UserId", UserId, "@RegionId", RegionId, "@CityId", ContactCityId, "@IsActive", IsActive, "@ReportToId", ReportToId, "@List", Table));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch
            {
              //  DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetClientContacts");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@value", Value));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    
    }
}
