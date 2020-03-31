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
    public class AV_ParserDL
    {
        public DataTable Get(string filter, int templateid=0,int UserId=0 )
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Parser");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@tempid", templateid,"@UserId",UserId));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Insert(string Filter ,string ObjectJson,string tempname,string keys,int UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_Parser");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@FILTER", Filter, "@templatename", tempname, "@templatejson", ObjectJson,"@UserId",UserId,"@Keys",keys));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch(Exception e)
            {
                if (e.Message == "Template Name Already Exists")
                    return false;
                else 
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
