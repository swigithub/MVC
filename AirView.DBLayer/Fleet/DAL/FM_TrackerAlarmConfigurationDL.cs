using System;
using System.Data;
using System.Data.SqlClient;
using SWI.Libraries.Common;

namespace AirView.DBLayer.Fleet.DAL
{
    class FM_TrackerAlarmConfigurationDL
    {
        public int Insert_TrackingAlarmConfig(string Filter,string AlrmCodes, int TrackerID, bool IsEnabled,bool IsNotification, float ThresholdValues,int  ModifiedBy, DateTime ModifiedOn )
        {

            SqlCommand loCommand = DataContext.OpenConnection();
        
             try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@AlarmCode", AlrmCodes, "@TrackerId", TrackerID, "@IsEnabled", Convert.ToInt16(IsEnabled), "@SendAlert", Convert.ToInt16(IsNotification), "@ThresholdValues", ThresholdValues, "@ModifiedBy", ModifiedBy, "@ModifiedOn", ModifiedOn));
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

        public DataTable Get_List_IMEI(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
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

        public DataTable GetTrackerAlarmConfig(string Filter, int TrackerID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerId", TrackerID));
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


        //@TrackerId
    }
}
