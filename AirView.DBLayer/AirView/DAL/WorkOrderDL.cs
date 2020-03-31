using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/

    public class WorkOrderDL
    {

        public bool Insert(string Filter, DataTable Workorder,Int64 UserId,string IMEI)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_InsertWorkorder");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter",Filter, "@Workorder", Workorder, "@SubmittedById",UserId, "@IMEI",IMEI));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch(Exception ex)
            {
              //  DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool Insert(string Filter,Int64 Count,string CellPath="")
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_InsertWorkorder");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteCount",Count,"@CellPath",CellPath, "@SubmittedById",0, "@Workorder",null));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch (Exception ex)
            {
                //  DataContext.CancelTransaction(loCommand);
                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public bool Edit(AV_Site sit, DataTable Sectors)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_EditWorkOrder");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@SiteId",sit.SiteId,"@SiteCode",sit.SiteCode,"@Latitude",sit.Latitude,"@Longitude",sit.Longitude, "@Description",sit.Description, "@CityId",sit.CityId, "@SiteAddress",sit.SiteAddress, "@Sectors",Sectors));
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


        public DataTable Get(string Filter, string value1=null, string value2 = null, string value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWorkOrder");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter",Filter, "@value1",value1, "@value2", value2, "@value3", value3));
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

        public int SiteCodeExist(string filter, string value1)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWorkOrder");
                loCommand = DataContext.StartTransaction(loCommand);
                int result = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", filter, "@value1", value1));
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
        public DataTable GetSiteBands(string Filter, Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetBands");
                return DataContext.Select(DataContext.AddParameters(loCommand,  "@Filter", Filter, "@SITEID", SiteId));
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        //public DataTable Get(string Filter, Int64 value1)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWorkOrder");
        //        return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value1", value1, "@value2", null, "@value3", null));
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}

        // for assign
        public DataTable GetAssign(int TesterId, string Status)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetWorkOrderAssign");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@TesterId", TesterId, "@Status", Status));
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
