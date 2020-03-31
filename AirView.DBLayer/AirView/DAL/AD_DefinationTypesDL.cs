using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;
using AirView.DBLayer.AirView.Entities;
using System;
using SWI.Libraries.AirView.Entities;

namespace SWI.Libraries.AirView.DAL
{
    //----King--Coder--Safi-UK-----//
    public class AD_DefinationTypesDL
    {
        public DataTable Get(string filter, string Value=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinationTypes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@filter", filter,"@Value",Value));
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
        public DataTable GetSingle(string filter,string Value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinationTypes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@filter", filter, "@Value", Value));
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
        public bool Delete(string filter, string Value)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinationTypes");
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@filter", filter, "@Value", Value));
                return result;
            }
            catch
            {
                return false;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool add(string filter, AD_DefinationTypes dt)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageDefinationTypes");
                    bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@FILTER", filter, "@DefinationType", dt.DefinationType, "@DefinationTypeId", dt.DefinationTypeId, "PDefinationTypeId", dt.PDefinationTypeId, "Status", dt.IsActive));
                return result;
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

        public DataSet GetDataSet(string filter, string Value1 = null, string Value2 = null, string Value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetDefinationTypes");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", Value1, "@Value2", Value2, "@Value3", Value3));
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
    }
}
