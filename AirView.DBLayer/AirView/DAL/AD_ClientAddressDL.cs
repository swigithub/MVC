using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.AirView.DAL
{

    /*----Mubashar----*/
    public class AD_ClientAddressDL
    {
        public bool Insert(DataTable ClientAddress)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientAddress");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", ClientAddress));
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
        public bool Manage(string Filter, DataTable ClientAddress)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientAddress");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", ClientAddress));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch(Exception ex)
            {
                // DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public int Manage(string Filter, int AddressId, string Address, string Street, int AddressCityId, int StateId, int CountryId, int ZipCode, bool IsHeadOffice, int ClientId,bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientAddress");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@AddressId", AddressId, "@Address", Address, "@Street", Street, "@CityId", AddressCityId, "@StateId", StateId
                    , "@CountryId", CountryId, "@ZipCode", ZipCode, "@IsHeadOffice", IsHeadOffice, "@ClientId", ClientId, "@IsActive", IsActive));
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
        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetClientAddress");
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



        //public int Manage(string Filter, int RoleId, string Name, string Description, bool ActiveStatus, string DefaultUrl)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManageRole");

        //        SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
        //        returnParameter.Direction = ParameterDirection.ReturnValue;
        //        loCommand = DataContext.StartTransaction(loCommand);

        //        int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@RoleId", RoleId, "@Name", Name, "@Description", Description, "@ModifyDate", DateTime.Now
        //            , "@IsActive", ActiveStatus, "@DefaultUrl", DefaultUrl));
        //        DataContext.EndTransaction(loCommand);
        //        int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
        //        return result;
        //    }
        //    catch
        //    {
        //        DataContext.CancelTransaction(loCommand);
        //        throw;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}
        //public DataTable GetRoles(string filter, string Value = null)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Get_Role");
        //        return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", Value));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}
    }
}
