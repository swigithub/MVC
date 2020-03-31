using AirView.DBLayer.Fleet.DAL;
using AirView.DBLayer.Fleet.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.BLL
{
    public class FM_VehicleBL
    {
        FM_VehicleDL model = new FM_VehicleDL();
        public FM_Vehicle Insert_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleGroupId, int IMEIID)
        {
            try
            {
                DataTable dataTableModel = model.Insert_Vehicle(Filter, TypeId, ManuId, ModelId, SubModelId, Year, ChassisNumber, RegistrationNumber, IsActive, VehicleGroupId, IMEIID);
                
                List <FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();
                return ListModel[0];
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public FM_Vehicle Insert_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, string Picture, int VehicleGroupId, int IMEIID)
        {
            try
            {
                DataTable dataTableModel = model.Insert_Vehicle(Filter, TypeId, ManuId, ModelId, SubModelId, Year, ChassisNumber, RegistrationNumber, IsActive, Picture, VehicleGroupId, IMEIID);
                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();
                return ListModel[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FM_Permissions Check_Vehicle_Permission(string Filter, int UserRoleId)
        {
            try
            {
                DataTable dataTableModel = model.Check_Vehicle_Permission(Filter, UserRoleId);

                List<FM_Permissions> ListModel = dataTableModel.ToList<FM_Permissions>();
                if (ListModel.Count != 0)
                {
                    return ListModel[0];
                }
                else
                {
                    FM_Permissions model = new FM_Permissions();
                    model.PermissionId = 0;
                    return model;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Delete_Vehicle(string Filter, int VechicleId)
        {
            try
            {
                return model.Delete_Vehicle(Filter, VechicleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Set_Vehicle_Status(string Filter, int VechicleId, int Status)
        {
            try
            {
                return model.Set_Vehicle_Status(Filter, VechicleId, Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<FM_Vehicle> Get_Record_Status(string Filter, int Status)
        {
            try
            {
                //return model.Get_Record_Status(Filter, Status);
                DataTable dataTableModel = model.Get_Record_Status(Filter, Status);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<FM_Vehicle> Get_Record_Status(string Filter, string Search)
        {
            try
            {
                //return model.Get_Record_Status(Filter, Status);
                DataTable dataTableModel = model.Get_Record_Status(Filter, Search);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_Vehicle> Get_Assigned_Record_Status(string Filter)
        {
            try
            {
                //return model.Get_Record_Status(Filter, Status);
                DataTable dataTableModel = model.Get_Assigned_Record_Status(Filter);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<FM_Vehicle> Get_Vehicle(string Filter)
        {
            try
            {
                DataTable dataTableModel = model.Get_Vehicle(Filter, true);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();
                
                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public FM_Vehicle Get_EditVehicle(string Filter, int Id)
        {
            try
            {
                DataTable dataTableModel = model.Get_EditVehicle(Filter, Id, true);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_Vehicle> Get_Vehicle(string Filter, int TypeId)
        {
            try
            {
                DataTable dataTableModel = model.Get_Vehicle(Filter, TypeId);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_Vehicle> Get_Vehicle(string Filter, int ManuId, bool IsActive)
        {
            try
            {
                DataTable dataTableModel = model.Get_Vehicle(Filter, ManuId, IsActive);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        
        public List<FM_Vehicle> Get_ListModel(string Filter, int ManuId, int TypeId)
        {
            try
            {
                DataTable dataTableModel = model.Get_ListModel(Filter, ManuId, TypeId);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<FM_Vehicle> Get_Vehicle(string Filter, int ModelId, int ManuId)
        {
            try
            {
                DataTable dataTableModel = model.Get_Vehicle(Filter, ModelId, ManuId, true);

                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public int Update_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleId, int VehicleGroupId)
        {
            try
            {
                return model.Update_Vehicle(Filter, TypeId, ManuId, ModelId, SubModelId, Year, ChassisNumber, RegistrationNumber, IsActive, VehicleId, VehicleGroupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleId, string VehicleImage, int VehicleGroupId)
        {
            try
            {
                return model.Update_Vehicle(Filter, TypeId, ManuId, ModelId, SubModelId, Year, ChassisNumber, RegistrationNumber, IsActive, VehicleId, VehicleImage, VehicleGroupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update_Vehicle(string Filter, int VehicleId, int IMEIId)
        {
            try
            {
                return model.Update_Vehicle(Filter, VehicleId, IMEIId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Assign_Single_Vehicle(string Filter, int UserId, int VehicleId, DateTime DateAssign, int IMEIId)
        {
            try
            {
                return model.Assign_Single_Vehicle(Filter, UserId, VehicleId, DateAssign,  IMEIId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Return_Single_Vehicle(string Filter, int VehicleId, DateTime DateReturn, int VehicleAssignmentId)
        {
            try
            {
                return model.Return_Single_Vehicle(Filter, VehicleId, DateReturn, VehicleAssignmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Transfer_Single_Vehicle(string Filter, int UserId, int VehicleId, DateTime DateAssign, int VehicleAssignmentId,int IMEIID)
        {
            try
            {
                return model.Transfer_Single_Vehicle(Filter, UserId, VehicleId, DateAssign, VehicleAssignmentId, IMEIID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<FM_Vehicle_Assignment> GET_DT_LIST(string Filter)
        {
            try
            {
                DataTable dataTableModel = model.GET_DT_LIST(Filter);

                List<FM_Vehicle_Assignment> ListModel = dataTableModel.ToList<FM_Vehicle_Assignment>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_VehicleGroup> GET_Group_LIST(string Filter)
        {
            try
            {
                DataTable dataTableModel = model.GET_Group_LIST(Filter);

                List<FM_VehicleGroup> ListModel = dataTableModel.ToList<FM_VehicleGroup>();

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public FM_VehicleGroup GET_Group_LIST(string Filter, int id)
        {
            try
            {
                DataTable dataTableModel = model.GET_Group_LIST(Filter, id);

                List<FM_VehicleGroup> ListModel = dataTableModel.ToList<FM_VehicleGroup>();

                return ListModel[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public int ValidateChassisNumber(string Filter, string ChassisNumber)
        {
            DataTable dataTableModel = model.ValidateChassisNumber(Filter, ChassisNumber);

            List<FM_Permissions> ListModel = dataTableModel.ToList<FM_Permissions>();
            if (ListModel.Count != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //ValidateRegistrationNumberOnUpdate
        public int ValidateChassisNumberOnUpdation(string Filter, string ChassisNumber, int VehicleId)
        {
            DataTable dataTableModel = model.ValidateChassisNumberOnUpdate(Filter, ChassisNumber, VehicleId);

            List<FM_Permissions> ListModel = dataTableModel.ToList<FM_Permissions>();
            if (ListModel.Count != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int ValidateRegistrationNumber(string Filter, string ChassisNumber)
        {
            DataTable dataTableModel = model.ValidateRegistrationNumber(Filter, ChassisNumber);

            List<FM_Permissions> ListModel = dataTableModel.ToList<FM_Permissions>();
            if (ListModel.Count != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int ValidateRegistrationNumberOnUpdate(string Filter, string ChassisNumber, int VehicleID)
        {
            DataTable dataTableModel = model.ValidateRegistrationNumberOnUpdate(Filter, ChassisNumber,VehicleID);

            List<FM_Permissions> ListModel = dataTableModel.ToList<FM_Permissions>();
            if (ListModel.Count != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int List_Vehicle_Group(string Filter, string Title, string Description, bool IsActive, bool IsDelete)
        {
            try
            {
                return model.List_Vehicle_Group(Filter, Title, Description, IsActive, IsDelete);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int List_Vehicle_Group(string Filter, string Title, string Description, bool IsActive, bool IsDelete, int VehicleGroupId)
        {
            try
            {
                return model.List_Vehicle_Group(Filter, Title, Description, IsActive, IsDelete, VehicleGroupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Delete_VehicleGroup(string Filter, int VehicleGroupId)
        {
            try
            {
                return model.Delete_VehicleGroup(Filter, VehicleGroupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /* == Start Insert Tracker Logs == */           
        public int Insert_Tracker(string Filter, string TrackerID, double Latitude, double Longitude, double Speed, double Odometer, double Direction, string Rotation, double Altitude, string TrackerStream, string GPSSignalStatus, DateTime UTCTimeAndDate, FM_Tracker_InputOutputStatus ParameterObject, bool IsOfflineData, string DeviceState, string ExtendState, string Address, double FuelConsumedPercentage, double CurrentTripFuelConsumed, double Temperature, double CurrentTripMileage, string GSMSignal,int TripNO)
        {
            try
            {
                return model.Insert_Tracker(Filter, TrackerID, Latitude, Longitude, Speed, Odometer, Direction, Rotation, Altitude, TrackerStream, GPSSignalStatus, UTCTimeAndDate, ParameterObject, IsOfflineData, DeviceState, ExtendState, Address, FuelConsumedPercentage, CurrentTripFuelConsumed, Temperature, CurrentTripMileage, GSMSignal, TripNO);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /* == Start Insert Tracker VStates == */
        public int Insert_VState(string Filter, string TrackerID, int VID, double Latitude, double Longitude, FM_VehicleStates objTrackerAlarms)
        {
            try
            {
                return model.Insert_VState(Filter,TrackerID, VID, Latitude,Longitude, objTrackerAlarms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* == End Insert Tracker Logs == */

        /* == Start Insert VehicleTracker Logs == */
        public int Insert_VehicleTracker(string Filter, int UEId, int VehicleId)
        {
            try
            {
                return model.Insert_Tracker(Filter, UEId, VehicleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Tracker Alarms
        public int Insert_TrackingAlarms(string Filter,string TrackerID,int VID,double Latitude, double Longitude,FM_Tracker_Alarms objTrackerAlarms)
        {
            return model.Insert_TrackingAlarms(Filter,TrackerID,VID,Latitude,Longitude, objTrackerAlarms);
        }
        /* == End Insert VehicleTracker Logs == */

        public FM_RouteKML Get_KML(string Filter, int VehicleId, DateTime FromDate, DateTime ToDate, Nullable<bool> tripStatus =null)
        {
            try
            {
                DataTable dataTableModel = model.Get_KML(Filter,  VehicleId,  FromDate,  ToDate, tripStatus);

                List<FM_RouteKML> obj  = dataTableModel.ToList<FM_RouteKML>();

                return obj[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_TrackerAlarmConfiguration> GetAlarmsToSendNotification(string Filter, string TrackerIMEI)
        {
            DataTable datatableModel = model.GetAlarmsToSendNotification(Filter, TrackerIMEI);
            
            List<FM_TrackerAlarmConfiguration> lstAlarmConfig = new List<FM_TrackerAlarmConfiguration>();

            if (datatableModel == null) { return null; }
            foreach (DataRow dr in datatableModel.Rows)
            {
                FM_TrackerAlarmConfiguration objAlarmConfig = new FM_TrackerAlarmConfiguration();
                objAlarmConfig.AlrmCodes = dr["AlarmCode"].ToString();
                objAlarmConfig.IMEI = dr["RegistrationNumber"].ToString();
                objAlarmConfig.AssignedUserID =dr["AssignTo"].ToString();
                lstAlarmConfig.Add(objAlarmConfig);
            }                            

                return lstAlarmConfig;
        }

  

        public List<FM_RouteKML> Get_DrivePlayDate(string Filter, int VehicleId)
        {
            try
            {
                DataTable dataTableModel = model.Get_DrivePlayDate(Filter, VehicleId);

                List<FM_RouteKML> obj = dataTableModel.ToList<FM_RouteKML>();

                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public List<FM_Vehicle> Get_KML_details(string Filter, int VehicleId, DateTime FromDate, DateTime ToDate)
        {
            List<FM_Vehicle> ListModel = new List<FM_Vehicle>();
            List<FM_Tracker_InputOutputStatus> TrackerListModel = new List<FM_Tracker_InputOutputStatus>();
            try
            {
                DataTable dataTableModel = model.Get_KML(Filter, VehicleId, FromDate, ToDate);

               ListModel = dataTableModel.ToList<FM_Vehicle>();
               TrackerListModel = dataTableModel.ToList<FM_Tracker_InputOutputStatus>();
               
                for(var i = 0; i< ListModel.Count; i++)
                {
                    ListModel[i].ObjInpOutStatus = TrackerListModel[i];
                }

                return ListModel;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_Vehicle> Get_List_IMEI(string Filter)
        {
            try
            {
                DataTable dataTableModel = model.Get_List_IMEI(Filter);
                List < FM_Vehicle > LstVehicle = new List<FM_Vehicle>();

                foreach (DataRow rows in dataTableModel.Rows)
                {
                    FM_Vehicle objFMVehcile = new FM_Vehicle();
                    objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                    objFMVehcile.IMEI = rows["SerialNo"].ToString() + " (" + rows["UENumber"].ToString() + ")";

                    objFMVehcile.TypeName = rows["UERefNo"].ToString();
                    objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();
                    

                    LstVehicle.Add(objFMVehcile);
                }               

                return LstVehicle;
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_Vehicle> GetTrackersByUserID(string Filter, int id)
        {
            try
            {
                DataTable dataTableModel = model.GetTrackersByUserID(Filter, id);
                List<FM_Vehicle> LstVehicle = new List<FM_Vehicle>();

                foreach (DataRow rows in dataTableModel.Rows)
                {
                    FM_Vehicle objFMVehcile = new FM_Vehicle();
                    objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                    objFMVehcile.IMEI = rows["SerialNo"].ToString();
                    objFMVehcile.IMEI = rows["SerialNo"].ToString() + " (" + rows["UENumber"].ToString() + ")";

                    objFMVehcile.TypeName = rows["UERefNo"].ToString();
                    objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();


                    LstVehicle.Add(objFMVehcile);
                }

                return LstVehicle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FM_Tracker_VM Get_List_IMEI(string Filter, int id)
        {
            try
            {
                DataSet ds = model.Get_List_IMEI(Filter, id);
                //DataTable dataTableModel = model.Get_List_IMEI(Filter, id);
                //DataTable dataTableModelHistory = model.Get_List_IMEI(Filter, id);

                FM_Tracker_VM LstVehicle = new FM_Tracker_VM();
                List<FM_VehicleTrackerHistory> HistoryObjectList = new List<FM_VehicleTrackerHistory>();
                DataTable dataTableModel = ds.Tables[0];
                DataTable dataTableModelHistory = ds.Tables[1];
                foreach (DataRow rows in dataTableModel.Rows)
                {
                    FM_Vehicle objFMVehcile = new FM_Vehicle();

                    objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                    objFMVehcile.IMEI = rows["SerialNo"].ToString() + " ("+ rows["UENumber"].ToString()+")";

                    objFMVehcile.TypeName = rows["UERefNo"].ToString();
                    objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();

                    LstVehicle.Vehicle = objFMVehcile;

                }

                foreach (DataRow rows in dataTableModelHistory.Rows)
                {
                    FM_VehicleTrackerHistory HistoryObject = new FM_VehicleTrackerHistory();

                    HistoryObject.TypeName = rows["UERefNo"].ToString();
                    HistoryObject.ManuName = rows["ManufacturerModel"].ToString();
                    HistoryObject.IMEI = rows["SerialNo"].ToString() + " (" + rows["UENumber"].ToString() + ")";
                    HistoryObject.TrackerDate = Convert.ToDateTime(rows["TrackerDate"]);
                    HistoryObject.UENumber = rows["UENumber"].ToString();
                    HistoryObjectList.Add(HistoryObject);
                }
                LstVehicle.History = HistoryObjectList;
                return LstVehicle;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<FM_Vehicle> GetCurrentIMEIDetails(string Filter, int id)
        {
            try
            {
                DataTable dataTableModel = model.GetCurrentIMEIDetails(Filter, id);
                List<FM_Vehicle> LstFM_Vehicle = new List<FM_Vehicle>();

                foreach (DataRow rows in dataTableModel.Rows)
                {
                    FM_Vehicle objFMVehcile;
                    objFMVehcile = new FM_Vehicle();
                    objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                    objFMVehcile.IMEI = rows["SerialNo"].ToString() + " (" + rows["UENumber"].ToString() + ")";

                    objFMVehcile.TypeName = rows["UERefNo"].ToString();
                    objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();
                    LstFM_Vehicle.Add(objFMVehcile);
                }
                
                return LstFM_Vehicle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FM_Vehicle> Get_List_IMEI_ByUserID(string Filter, int id)
        {
            try
            {
                DataTable dataTableModel = model.Get_List_IMEI_ByUserID(Filter,id);
                List<FM_Vehicle> LstVehicle = new List<FM_Vehicle>();

                foreach (DataRow rows in dataTableModel.Rows)
                {
                    FM_Vehicle objFMVehcile = new FM_Vehicle();
                    objFMVehcile.IMEIId = Convert.ToInt32(rows["UEId"]);
                    objFMVehcile.IMEI = rows["SerialNo"].ToString() + " (" + rows["UENumber"].ToString() + ")";

                    objFMVehcile.TypeName = rows["UERefNo"].ToString();
                    objFMVehcile.ManuName = rows["ManufacturerModel"].ToString();


                    LstVehicle.Add(objFMVehcile);
                }

                return LstVehicle;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //Start Fleet Replay Record

        public List<FM_FleetReplay_VM> GetFleetReplayRecord(FleetReplayCriteria fleetreplaycriteria)
        {
            try
            {
                DataTable dataTableModel = model.Get_FleetReplayRecords(fleetreplaycriteria);
                List<FM_FleetReplay_VM> ListModel = dataTableModel.ToList<FM_FleetReplay_VM>();
                
                    int counter = 0;
                    foreach (DataRow row in dataTableModel.Rows)
                    {
                        var objVeh = new FM_Vehicle();
                        objVeh.RegistrationNumber = row["RegistrationNumber"].ToString();
                        objVeh.IMEI = row["IMEI"].ToString();
                        if (fleetreplaycriteria.Filter == "IdleEngineReport")
                        {
                            objVeh.Longitude = Convert.ToDouble(row["Longitude"]);
                            objVeh.Latitude = Convert.ToDouble(row["Latitude"]);
                        }
                        if (fleetreplaycriteria.Filter == "Alarms" || fleetreplaycriteria.Filter == "DetailTrack")
                        {
                            objVeh.Longitude = Convert.ToDouble(row["Longitude"]);
                            objVeh.Latitude = Convert.ToDouble(row["Latitude"]);
                            objVeh.Speed = Convert.ToDouble(row["Speed"]);
                            objVeh.Direction = Convert.ToDouble(row["Direction"]);
                            objVeh.Odometer = Convert.ToDouble(row["Odometer"]);
                            objVeh.Rotation = row["Rotation"].ToString();
                            objVeh.CurrentTripMileage = Convert.ToDouble(row["CurrentTripMileage"]);
                            objVeh.GPSSignalStatus = row["GPSSignalStatus"].ToString();
                            objVeh.GSMSignal = row["GSMSignal"].ToString();
                        }
                        ListModel[counter].LstVehicle = objVeh;
                        counter = counter + 1;
                    }
                var FatigueStatus = ListModel.Where(u => u.Status != null).Select(a => a.Status).ToList();
                var engineStatus = ListModel.Where(u => u.InEngineOnOff != null).Select(a => a.InEngineOnOff).ToList();
                var alarmState = ListModel.Where(u => u.AlarmState != null).Select(a => a.AlarmState).ToList();
                if (FatigueStatus.Count > 0 || engineStatus.Count > 0)
                {
                    foreach (var item in ListModel)
                    {
                        if (item.Status == "True")
                        {
                            item.Status = "Yes";
                        }
                        if(item.AlarmState == "True")
                        {
                            item.AlarmState = "On";
                        }
                        else
                        {
                            item.AlarmState = "Off";
                        }
                        if (item.InEngineOnOff == "True")
                        {
                            item.InEngineOnOff = "On";
                        }
                        else
                        {
                            item.InEngineOnOff = "Off";
                        }

                    }
                    return ListModel.ToList();
                }
               
                else
                {
                    return ListModel.ToList();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<FM_TrackerTrip> GetFleetReplayTripRecord(FleetReplayCriteria fleetreplaycriteria)
        {
            try
            {
                DataTable dataTableModel = model.Get_FleetReplayRecords(fleetreplaycriteria);
                List<FM_TrackerTrip> ListModel = dataTableModel.ToList<FM_TrackerTrip>();
                var tripDate = ListModel.Where(u => u.TripDate != null).Select(a => a.TripDate).ToList();
                if(tripDate.Count > 0)
                {
                    foreach(var item in ListModel)
                    {
                        DateTime replaytripDate = Convert.ToDateTime(item.TripDate);
                        item.TripDate = replaytripDate.ToShortDateString();
                    }
                   
                }
                return ListModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Insert_UpdateTripIdleConfig
        public FM_Vehicle Insert_TripIdleTime(string Filter,string IMEI,long ModifiedBy, string TimeInterval )
        {
            try
            {
                DataTable dataTableModel = model.Insert_TripIdleTime(Filter, IMEI,ModifiedBy,  TimeInterval);
                List<FM_Vehicle> ListModel = dataTableModel.ToList<FM_Vehicle>();
                return ListModel[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int Insert_WifiSettings(string Filter, FM_TrackerWifiSetting fm_trackerwifisettings)
        {
            try
            {
                return model.Insert_WifiSettings(Filter, fm_trackerwifisettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public FM_TrackerWifiSetting GetWifiSettings(string Filter, string trackerSerialNo)
        {
            try
            {
                DataTable dataTableModel = model.GetWifiSettings(Filter, trackerSerialNo);
                List<FM_TrackerWifiSetting> ListModel = dataTableModel.ToList<FM_TrackerWifiSetting>();
                return ListModel[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //End Fleet Replay Record

        /*public List<AD_Help> Read(string filter, int CID)
        {
            try
            {
                DataTable dt = help.Read(filter, CID);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();
                return model;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<AD_Help> Read(string filter, int CID, int MID)
        {
            try
            {
                DataTable dt = help.Read(filter, CID, MID);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();
                return model;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<AD_Help> List(string filter, bool listFilter)
        {
            try
            {
                DataTable dt = help.Read(filter, listFilter);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> jkj = dt.ToList<AD_Help>();
                return jkj;

            }
            catch (Exception)
            {
                return null;
            }

        }


        public int ListRow(string filter, bool IsActive, int ModuleId)
        {

            try
            {
                return help.UpdateRow(filter, IsActive, ModuleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<AD_Help> ReadPost(string filter, int id)
        {
            try
            {
                DataTable dt = help.ReadPost(filter, id);
                AD_Help helpFile = new AD_Help();
                List<AD_Help> model = dt.ToList<AD_Help>();


                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        model[i].ModuleId = int.Parse(dt.Rows[i]["ModuleId"].ToString());
                    }
                    return model;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public int EditPost(string filter, int HelpId, string title, string description)
        {
            try
            {
                return help.EditPost(filter, HelpId, title, description);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }*/

    }
}
