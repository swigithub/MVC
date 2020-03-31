using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_ImportProjectPlanDL
    {
        public bool Manage(string Filter, string Value, DataTable List)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_DataImport");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", Value, "@Data", List));
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


        //public bool ManageImports(string Filter, string Value, DataTable List)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();
        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ImportPlan");
        //        loCommand = DataContext.StartTransaction(loCommand);
        //        var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", Value, "@Data", List));
        //        DataContext.EndTransaction(loCommand);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}

        public DataTable GetDataTable(string Filter, string value, DataTable Data,long? UserId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ImportPlan");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", value,"@Data",Data));
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetDataTable(string Filter, string value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_ImportPlan");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", value));
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetDataTable(string Filter, long ProjectId=0,long UserId=0,long RevisionId=0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "PM_Get_ImportPlanHistory");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@UserId", UserId, "@RevisionId", RevisionId));
            }
            catch (Exception ex)
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
