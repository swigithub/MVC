using AirView.DBLayer.Fleet.DAL;
using AirView.DBLayer.Fleet.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AirView.DBLayer.Fleet.BLL
{
    public class FM_TrackerAlarmConfigurationBL
    {
        FM_TrackerAlarmConfigurationDL model = new FM_TrackerAlarmConfigurationDL();            

        public bool Insert_TrackingAlarmConfig(string Filter, string AlrmCodes, int TrackerID, bool IsEnabled, bool IsNotification, float ThresholdValues, int ModifiedBy, DateTime ModifiedOn )
        {
            try
            {
                FM_TrackerAlarmConfiguration a = new FM_TrackerAlarmConfiguration();                
               // DataTable dataTableModel = model.Insert_TrackingAlarmConfig(Filter, AlrmCodes, TrackerID, IsEnabled, IsNotification, ThresholdValues, ModifiedBy, ModifiedOn);
                return Convert.ToBoolean(model.Insert_TrackingAlarmConfig(Filter, AlrmCodes, TrackerID, IsEnabled, IsNotification, ThresholdValues, ModifiedBy, ModifiedOn));
                //return true;//ListModel[0];
            }
            catch (Exception ex)
            {
                return false;                
            }
        }

        public List<FM_Vehicle> GetAllTrackers(string Filter)
        {
            DataTable dataTableModel = model.Get_List_IMEI(Filter);
            List<FM_Vehicle> LstVehicle = new List<FM_Vehicle>();

            foreach (DataRow rows in dataTableModel.Rows)
            {
                FM_Vehicle objFMVehcile = new FM_Vehicle();
                objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                objFMVehcile.IMEI = rows["SerialNo"].ToString();//+ " (" + rows["UENumber"].ToString() + ")";

                objFMVehcile.TypeName = rows["UERefNo"].ToString();
                objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();
                objFMVehcile.RegistrationNumber = rows["RegistrationNumber"].ToString();

                LstVehicle.Add(objFMVehcile);
            }

            return LstVehicle;
        }

        public List<FM_TrackerAlarmConfiguration> GetTrackerAlarmConfig(string Filter, int TrackerID)
        {
            DataTable dataTableModel = model.GetTrackerAlarmConfig(Filter, TrackerID);
            List<FM_TrackerAlarmConfiguration> LstConfig = new List<FM_TrackerAlarmConfiguration>();

            foreach (DataRow rows in dataTableModel.Rows)
            {
                FM_TrackerAlarmConfiguration objFMConfig = new FM_TrackerAlarmConfiguration();
                objFMConfig.AlrmCodes = rows["AlarmCode"].ToString();
                objFMConfig.TrackerId = Convert.ToInt32(rows["TrackerID"]);//+ " (" + rows["UENumber"].ToString() + ")";
                objFMConfig.IsEnabled = Convert.ToBoolean(rows["IsEnabled"]);
                objFMConfig.ThresholdValues = Convert.ToSingle(rows["ThresholdValues"]);
                objFMConfig.IsApplied = Convert.ToBoolean(rows["IsApplied"]);
                objFMConfig.IsNotification= Convert.ToBoolean(rows["SendAlert"]);

                //IsEnabled,ThresholdValues,IsApplied,ModifiedOn,ModifiedOn                 
                LstConfig.Add(objFMConfig);
            }

            return LstConfig;
        }
    }
}
