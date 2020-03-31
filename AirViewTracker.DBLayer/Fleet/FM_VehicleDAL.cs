using AirViewTracker.DBLayer.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirViewTracker.DBLayer.Fleet
{
    class FM_VehicleDAL
    {
        string _CONNECTIONSTRING = "";
        public FM_VehicleDAL(string ConnString)
        {
            _CONNECTIONSTRING = ConnString;
        }

        internal DataTable GetAllVehicleIMEI(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
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

        internal DataTable ValidateVehicleIMEI(string Filter, string IMEI)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", IMEI));
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
        internal DataTable GetAllAlarmsToApply(string Filter, string TrackerSerialNo)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", TrackerSerialNo));
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

        internal int UpdateAlarmsSettings_ByTID(string Filter, string TrackerSerialNo, string AlarmCode)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", TrackerSerialNo, "@AlarmCode", AlarmCode));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);

            }

            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        internal int UpdateWifiSettings_ByTID(string Filter, string TrackerSerialNo, string AlarmCode)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", TrackerSerialNo, "@AlarmCode", AlarmCode));
                DataContext.EndTransaction(loCommand);
                return Convert.ToInt32(result);

            }

            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        internal string GetTrackerManufacturer(string Filter, string IMEI)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", IMEI));
                DataContext.EndTransaction(loCommand);
                return "";

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

        public DataTable GetCurrentTripCounterForClient(string Filter,string TrackerSerialNo, string CurrentDate)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", TrackerSerialNo, "@CurrentDate", CurrentDate));            
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

        public DataTable GetTripIdleValue(string Filter, string TrackerSerialNo)
        {
            SqlCommand loCommand = DataContext.OpenConnection(_CONNECTIONSTRING);

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", TrackerSerialNo));
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
    }
}
