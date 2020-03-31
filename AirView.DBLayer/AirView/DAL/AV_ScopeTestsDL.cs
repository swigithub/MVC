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
    public class AV_ScopeTestsDL
    {
        public DataTable Get(string filter, Int64 ClientId , Int64 CityId ,Int64 NetworkModeId,Int64 ScopeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetScopeTests");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ClientId", ClientId, "@CityId", CityId, "@NetworkModeId", NetworkModeId,"@ScopeId", ScopeId));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        //public bool Manage(string Filter, Int64 UEId, string UEName, string IMEI, bool IsIdle, string @DeviceToken)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageUEPbx");
        //        loCommand = DataContext.StartTransaction(loCommand);
        //        bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UEId", UEId, "@UEName", UEName, "@IMEI", IMEI, "@IsIdle", IsIdle, "@DeviceToken", DeviceToken));
        //        DataContext.EndTransaction(loCommand);
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



        public bool Insert(DataTable dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_InsertScopeTests");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", dt));
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
    }
}
