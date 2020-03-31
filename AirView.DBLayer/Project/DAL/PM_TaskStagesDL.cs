using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWI.Libraries.Common;
using System.Reflection;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class PM_TaskStagesDL
    {
        public bool Manage(string DestinationTableName, DataTable Table)
        {
            return DataContext.InsertBulkIntoSQL(DestinationTableName, Table);
        }


        public bool Manage(string Filter, long ProjectId, DataTable TaskStages)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "[dbo].[PM_ManageTaskStages]");
                bool IsExe = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@TaskStages", TaskStages));
                return IsExe;
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

        public bool Manage(string Filter, long ProjectId, long TaskId, DataTable TaskStages)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "[dbo].[PM_ManageTaskStages]");
                bool IsExe = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectId", ProjectId, "@TaskStages", TaskStages, "@TaskId", TaskId));
                return IsExe;
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

        public DataTable Get(string filter, long ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "[dbo].[PM_GetTaskStages]");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId));
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

        public DataTable Get(string filter, long ProjectId, long TaskId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "[dbo].[PM_GetTaskStages]");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ProjectId", ProjectId, "@TaskId", TaskId));
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

        public int Get(string filter, int StageId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "[dbo].[PM_GetTaskStages]");
                return DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", filter, "@StageId", StageId));
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

