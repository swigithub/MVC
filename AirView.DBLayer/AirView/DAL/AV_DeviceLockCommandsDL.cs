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
    /*----MoB!----*/
    public class AV_DeviceLockCommandsDL
    {
        public DataTable Get(string MenuType,Int64 NetworkModeId, Int64 BandId,string DeviceModel)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetDeviceLockCommands");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@MenuType", MenuType, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@DeviceModel", DeviceModel));
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
