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
    class AD_HelpDL
    {
        public int Create(string filter, int ComponentId, int ModuleId, int FeatureId, string Title, string Description, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@ComponentID", ComponentId, "@ModuleID", ModuleId, "@FeatureID", FeatureId, "@Title", Title, "@Description", Description, "@IsActive", IsActive));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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


        public DataTable Read(string filter, bool listFilter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@IsActive", listFilter));
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

        public int UpdateRow(string filter, bool IsActive, int FeatureID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {  
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@FeatureID", FeatureID, "@IsActive", IsActive));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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

        public DataTable Read(string filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter));
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
        public DataTable Read(string filter, int CID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ModuleID", CID));
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
        public DataTable Read(string filter, int CID, int MID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ComponentID", CID, "@ModuleID", MID));
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

        public DataTable ReadPost(string filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@FeatureID", id));
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

        public int EditPost(string filter, int HelpId, string title, string description)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@HelpID", HelpId, "@Title", title, "@Description", description));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);
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
