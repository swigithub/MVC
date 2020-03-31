using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;


namespace AirView.DBLayer.Schema.DAL
{
    public class DatabaseSchemaDL
    {
        public DataTable GetSchemaInfo(string Filter, string TableName = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "GetDatabaseSchemaInfo");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TableName", TableName));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
                loCommand.Dispose();
            }
        }
    }
}
