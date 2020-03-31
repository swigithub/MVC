using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirViewTracker.DBLayer.Fleet
{
    public class FM_VehicleBL
    {
        private FM_VehicleDAL model;

        public FM_VehicleBL(String ConnectionString)
        {
            model = new FM_VehicleDAL(ConnectionString);
        }
        public List<string> GetAllVehicleIMEI(string Filter)
        {
            try
            {
                DataTable dataTableModel = model.GetAllVehicleIMEI(Filter);
                List<string> lst = dataTableModel.AsEnumerable().Select(x => x["IMEI"].ToString()).ToList<string>();
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ValidateVehicleIMEI(string Filter, string IMEI)
        {
            try
            {
                DataTable dataTableModel = model.ValidateVehicleIMEI(Filter, IMEI);
                if (dataTableModel.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TrackerAlarmConfig> GetAllAlarmsToApply(string Filter, string IMEI)
        {
            try
            {
                DataTable dataTableModel = model.GetAllAlarmsToApply(Filter, IMEI);
                List<TrackerAlarmConfig> LstConfig = new List<TrackerAlarmConfig>();

                if (dataTableModel.Rows.Count > 0)
                {
                    foreach (DataRow rows in dataTableModel.Rows)
                    {
                        TrackerAlarmConfig obj = new TrackerAlarmConfig();
                        obj.AlrmCodes = rows["AlarmCode"].ToString();
                        obj.IsEnabled = Convert.ToBoolean(rows["IsEnabled"]);//+ " (" + rows["UENumber"].ToString() + ")";
                        obj.ThresholdValues = Convert.ToSingle(rows["ThresholdValues"].ToString());

                        LstConfig.Add(obj);
                    }
                    return LstConfig;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateAlarmsSettings_ByTID(string Filter, string IMEI,string AlarmCode)
        {
            try
            {
                return Convert.ToBoolean(model.UpdateAlarmsSettings_ByTID(Filter, IMEI, AlarmCode));                
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateWifiSettings_ByTID(string Filter, string IMEI, string AlarmCode)
        {
            try
            {
                return Convert.ToBoolean(model.UpdateWifiSettings_ByTID(Filter, IMEI, AlarmCode));
            }

            catch (Exception ex)
            {
                return false;
            }
        }
        public string GetTrackerManufacturer(string Filter, string IMEI)
        {
            try
            {
                string Manufacturer = "";
                Manufacturer = model.GetTrackerManufacturer(Filter, IMEI);
                return Manufacturer;
            }

            catch(Exception ex)
            {
                return null;
            }
        }

        public string GetCurrentTripCounterForClient(string Filter,string SerialNo ,string CurrentDate)
        {
            try
            {
                string Manufacturer = "";
                DataTable dataTableModel = model.GetCurrentTripCounterForClient(Filter, SerialNo, CurrentDate);

                if (dataTableModel.Rows.Count > 0)
                {

                    return dataTableModel.Rows[0]["TripName"].ToString();                  
                }

                return "";            
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetTripIdleValue(string Filter, string TrackerSerialNo)
        {
            try
            {
                
                DataTable dataTableModel = model.GetTripIdleValue(Filter, TrackerSerialNo);

                if (dataTableModel.Rows.Count > 0)
                {

                    return dataTableModel.Rows[0]["ThresholdValues"].ToString();
                }

                return "";
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }           
}

