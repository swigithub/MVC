using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.DAL
{
 public   class CLS_VMDL
    {
        public DataTable GetDataTable(string filter, string Value = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetBands");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SITEID", Value, "@Filter", filter));
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
        public DataTable GetSectorDataTable(string filter,Int64 SiteId,int LayerId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetSectors");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@SITEID", SiteId, "@LayerId", LayerId, "@Filter", filter));
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
    }
}
