using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace SWI.Libraries.AD.DAL
{
    /*----MoB!----*/
    public class AD_DefinationDL
    {

        public bool Manage(string Filter, DataTable Table)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageDefinations");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", Table));
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
        public DataTable GetRegion(string value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Get_Region");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@value", value));
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

        public DataTable Get(string filter, string value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinations");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@value", value));
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

        public DataTable GetColumn(string filter, string value = null, string select = null, string group = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_rptTesterSites");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@where", value, "@Column", select, "@Group", group));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataTable GetCitiesByRegionId(int Id, bool status)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Get_Cities_ByRegionId");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Id", Id, "@status", status));
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

        public int List_Work_Group(string Filter, string WorkgroupName)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManangeWorkGroup");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@WorkgroupName", WorkgroupName, "@UserId"));
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

        public DataTable List_Work_Group(string Filter, int WorkgroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManangeWorkGroup");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@WorkgroupId", WorkgroupId));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int List_Work_Group(string Filter, string WorkgroupName, int WorkgroupId, Int32 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManangeWorkGroup");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@WorkgroupName", WorkgroupName, "@WorkgroupId", WorkgroupId, "@UserId", UserId));
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

        public int Delete_Group(string Filter, int WorkGroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManangeWorkGroup");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@WorkGroupId", WorkGroupId));
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

        public DataTable List_Work_Group(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManangeWorkGroup");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter));
            }
            catch (Exception ex)
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
