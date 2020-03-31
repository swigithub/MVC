using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirView.DBLayer.Project.DAL
{
    public class AD_ProjectsDL
    {
        //public bool Insert(DataTable project)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageClientAddressXXXXXXXX");
        //        loCommand = DataContext.StartTransaction(loCommand);
        //        bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", project));
        //        DataContext.EndTransaction(loCommand);
        //        return result;
        //    }
        //    catch(Exception)
        //    {
        //        // DataContext.CancelTransaction(loCommand);
        //        throw;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}

        public void Add_project(AD_Projects p)

        {
            //SqlCommand com = new SqlCommand("Sp_AddProject", con);
            SqlCommand com = DataContext.OpenConnection();
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProjectID", p.ProjectID);
            com.Parameters.AddWithValue("@ProjectName", p.ProjectName);
            com.Parameters.AddWithValue("@ProjectScopeID", p.ProjectScopeID);
            com.Parameters.AddWithValue("@CompanyID", p.CompanyID);
            com.Parameters.AddWithValue("@VendorID", p.VendorID);
            com.Parameters.AddWithValue("@StartDate", p.StartDate);
            com.Parameters.AddWithValue("@EndDate", p.EndDate);
            com.Parameters.AddWithValue("@StatusID", p.StatusID);
            com.Parameters.AddWithValue("@Color", p.Color);
            com.Parameters.AddWithValue("@Description", p.Description);
            com.Parameters.AddWithValue("@IsActive", p.IsActive);
            com.Parameters.AddWithValue("@TypeId", p.TypeId);

            //con.Open();

            com.ExecuteNonQuery();

            // con.Close();
        }

        public int Manage(string Filter, int ProjectID, string ProjectName, string ProjectScopeID, int CompanyID, int VendorID, DateTime? StartDate,
            DateTime? EndDate, int StatusID, string Color, string Description, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ManageNodes");

                SqlParameter returnParameter = loCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                loCommand = DataContext.StartTransaction(loCommand);

                int id = DataContext.ExecuteScalar(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ProjectID", ProjectID,
                    "@ProjectName", ProjectName, "@ProjectScopeID", ProjectScopeID, "@CompanyID", CompanyID, "@VendorID", VendorID,
                    "@StartDate", StartDate, "@EndDate", EndDate, "@StatusID", StatusID, "@Color", Color, "@Description", Description, "@IsActive", IsActive));
                DataContext.EndTransaction(loCommand);
                int result = Convert.ToInt32(loCommand.Parameters["@RETURN_VALUE"].Value);
                return result;
            }
            catch (Exception)
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetProjects(string filter, string Value = null, string Value1 = null, string Value2 = null,
            string Value3 = null, string Value4 = null, string Value5 = null, int pageIndex = 0, int pageSize = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetProjects");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@value", Value, "@value1", Value1, "@value2", Value2,
                    "@value3", Value3, "@value4", Value4, "@value5", Value5, "@pageIndex", pageIndex, "@pageSize", pageSize));
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

        public DataTable Get(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetClients");
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

        public DataTable GetCitiesMarkeet(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinations");
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

        public DataTable GetScopeOrStatus(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinations");
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

        public DataTable GetCitiesMarket(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinations");
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

        public bool Insert(DataTable data)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageProjectConfiguration");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@List", data));
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