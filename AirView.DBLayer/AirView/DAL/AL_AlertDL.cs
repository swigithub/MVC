using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.DAL
{
    public class AL_AlertDL
    {
        public DataTable Get(string Filter, int UserId, int ParentEntityId, int ChildEntityId, string Alert)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", UserId, "@ParentEntityId", ParentEntityId, "@ChildEntityId", ChildEntityId, "@Alert", Alert));
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


        public DataTable Get(string Filter, AL_GetNotification Info)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@AlertRecieverId", Info.AlertRecieverId));
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

        public DataTable Set(string Filter, AL_SetNotification Info)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@AlertConfigId", Info.AlertConfigId, "@EntityId", Info.EntityId, "@Notification", Info.Notification, "@AlertRecieverId", Info.AlertRecieverId, "@UserId", Info.UserId, "@IsEmailAlertSent", Info.IsEmailAlertSent, "@IsPushAlertRead", Info.IsPushAlertRead, "@IsPushAlertSent", Info.IsPushAlertSent));

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


        public DataTable Get(string Filter, AL_GetAlertConfiguration Configuration)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", Configuration.UserId, "@KeyCode", Configuration.KeyCode, "@RoleId", Configuration.RoleId));
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

        public DataTable Configuration(string Filter, string KeyCode)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@KeyCode", KeyCode));
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
        
        public int Insert(string Filter, int EntityId, int AlertSender, int ParentEntityId, int ChildEntityId, string Alert, int StatusId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                //loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                //return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@EntityId", EntityId, "@AlertSender", AlertSender, "@ParentEntityId", ParentEntityId, "@ChildEntityId", ChildEntityId, "@Alert", Alert));

                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@EntityId", EntityId, "@AlertSender", AlertSender, "@ParentEntityId", ParentEntityId, "@ChildEntityId", ChildEntityId, "@Alert", Alert, "@StatusId", StatusId));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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

        public int Subscribe(string Filter, string KeyCode, int EntityId, string ConfigId, int UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                //loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                //return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@EntityId", EntityId, "@AlertSender", AlertSender, "@ParentEntityId", ParentEntityId, "@ChildEntityId", ChildEntityId, "@Alert", Alert));

                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@KeyCode", KeyCode, "@EntityId", EntityId, "@ConfigId", ConfigId, "@UserId", UserId));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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

        public int Subscription(string Filter, string KeyCode, AL_AlertSubscriptionUser model)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@KeyCode", KeyCode, "@AlertConfigId", model.AlertConfigId , "@UserId", model.UserId, "@IsSubscribed", model.IsSubscribed, "@IsPushAlertRequired", model.IsPushAlertRequired, "@IsEmailAlertRequired", model.IsEmailAlertRequired));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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

        
        public int UpdateConfiguration(string Filter, AL_GetAlertConfiguration ConfigurationList)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", ConfigurationList.UserId, "@RoleId", ConfigurationList.RoleId, "@IsSubscribed", ConfigurationList.IsSubscribed, "@IsPushAlertRequired", ConfigurationList.IsPushAlertRequired, "@IsEmailAlertRequired", ConfigurationList.IsEmailAlertRequired, "@AlertConfigId", ConfigurationList.AlertConfigId));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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

        public int UpdateNotification(string Filter, AL_GetNotification ConfigurationList)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AL_ManageAlert");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@AlertRecieverId", ConfigurationList.AlertRecieverId, "@IsPushAlertRead", ConfigurationList.IsPushAlertRead, "@AlertId", ConfigurationList.AlertId));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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
