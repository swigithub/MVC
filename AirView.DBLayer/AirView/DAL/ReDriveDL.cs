using SWI.Libraries.Common;
using System;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
  public  class ReDriveDL
    {
        public bool Manage(string Filter, Int64 ReDriveId, Int64 ReDriveTypeId, Int64 ReasonId, string Description, Int64 SiteId, Int64 NetworkModeId, Int64 BandId, Int64 CarrierId, Int64 ScopId, Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageReDrive");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ReDriveId", ReDriveId, "@ReDriveTypeId", ReDriveTypeId, "@ReasonId",ReasonId, "@Description", Description, "@SiteId", SiteId, "@NetworkModeId", NetworkModeId, "@BandId", BandId, "@CarrierId", CarrierId, "@ScopId", ScopId, "@UserId", UserId));
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
