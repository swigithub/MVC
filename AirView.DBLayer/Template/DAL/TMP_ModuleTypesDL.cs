using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Template.DAL
{
    public class TMP_ModuleTypesDL
    {
        public DataTable GetTb(string ModuleType)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TMP_ModuleTypes");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@ModuleType", ModuleType));
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
