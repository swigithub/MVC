using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.DAL
{
    /*----MoB!----*/
    public class AD_UEMovementDL
    {
     
        public bool Manage(string Filter, Int64 UEMovementId, Int64 UEId,Int64 FromUserId, Int64 UserId, Int64 UEStatusId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_ManageUEMovement");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UEMovementId", UEMovementId, "@UEId", UEId, "@FromUserId", FromUserId, "@UserId", UserId, "@UEStatusId", UEStatusId));
                DataContext.EndTransaction(loCommand);
                return result;

            }
            catch
            {
                //DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
