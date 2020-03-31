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
  public  class FloorPlan_DL
    {
        public bool Save(string Filter, DataTable dt)
        {
           
              SqlCommand loCommand = DataContext.OpenConnection();
                try
                {
                    loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageFloorPlans");
                    loCommand = DataContext.StartTransaction(loCommand);
                    bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", dt));
                    DataContext.EndTransaction(loCommand);
                    return result;

                }
                catch(Exception ex)
                {
                    DataContext.CancelTransaction(loCommand);
                    throw;
                }
                finally
                {
                    DataContext.CloseConnection(loCommand);
                }
            }

        public DataTable Get(string Filter,Int64 SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetFloorPlans");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", Filter, "@SiteId", SiteId));
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

        public bool Manage(string Filter, Int64 PlanId,bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageFloorPlans");
                 DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@FILTER", Filter, "@PlanId", PlanId, "@IsActive", IsActive,"@List",null));
                DataContext.EndTransaction(loCommand);
                return true;
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
