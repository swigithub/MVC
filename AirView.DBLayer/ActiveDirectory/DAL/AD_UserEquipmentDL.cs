using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.AD.DAL
{
    /*----MoB!----*/
    public class AD_UserEquipmentDL
    {
        public DataTable Get(string filter, string value = null, string value2 = null, string value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetUserEquipments");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value, "@Value2", value2, "@Value3", value3));
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

        public DataSet GetDataSet(string filter, string value = null, string value2 = null, string value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetUserEquipments");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value, "@Value2", value2, "@Value3", value3));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }
     
        public bool Manage(string Filter, Int64 UEId, Int64 UETypeId, string Manufacturer, string Model, string SerialNo,string MAC,string UENumber,bool IsActive,string token, Int64 UEOwnerId, string UERefNo)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageUserEquipments");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UEId", UEId, "@UETypeId", UETypeId, "@Manufacturer", Manufacturer,
                                                         "@Model", Model, "@SerialNo", SerialNo, "@MAC", MAC, "@UENumber", UENumber, "@IsActive", IsActive, "@Token", token, "@UEOwnerId", UEOwnerId, "@UERefNo", UERefNo));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool ManageStatus(string Filer, string SerialNo = null, bool IsActive = false, string MAC = null, string UENumber = null, string UEStatusId = null, string Token =null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageUserEquipments");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer, "@SerialNo", SerialNo, "@IsActive", IsActive, "@MAC", MAC, "@UENumber", UENumber, "@UEStatusId", UEStatusId, "@Token", Token));
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
