using AirView.DBLayer.Fleet.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.DAL
{
    public class FM_VehicleDL
    {

        public DataTable Insert_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleGroupId, int IMEIID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TypeId", TypeId, "@ManuId", ManuId, "@ModelId", ModelId, "@SubModelId", SubModelId, "@Year", Year, "@ChassisNumber", ChassisNumber, "@RegistrationNumber", RegistrationNumber, "@IsActive", IsActive, "@VehicleGroupId", VehicleGroupId, "@IMEIID", IMEIID));
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

        public DataTable Insert_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, string Picture, int VehicleGroupId, int IMEIID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TypeId", TypeId, "@ManuId", ManuId, "@ModelId", ModelId, "@SubModelId", SubModelId, "@Year", Year, "@ChassisNumber", ChassisNumber, "@RegistrationNumber", RegistrationNumber, "@IsActive", IsActive, "@Picture", Picture, "@VehicleGroupId", VehicleGroupId, "@IMEIID", IMEIID));
                
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

        public DataTable Check_Vehicle_Permission(string Filter, int UserRoleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_VehiclesSchedule");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserRoleId", UserRoleId));

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

        public int VehicleAssignTesterCLS(string Filter, int UserId, int? VehicleId, int SiteId, int NetworkId, int BandId, int CarrierId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_VehiclesSchedule");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", UserId, "@VehicleId", VehicleId, "@SiteId", SiteId, "@NetworkId", NetworkId, "@BandId", BandId, "@CarrierId", CarrierId));
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

        public int Delete_Vehicle(string Filter, int VechicleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VechicleId, "@IsDeleted", 1, "@IsActive", 0));
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

        public int Delete_VehicleGroup(string Filter, int VehicleGroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleGroupId", VehicleGroupId, "@IsDeleted", 1));
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

        public int Set_Vehicle_Status(string Filter, int VechicleId, int Status)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VechicleId, "@IsActive", Status));
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

        public DataTable Get_Record_Status(string Filter, int Status)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IsActive", Status));
               
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
        public DataTable Get_Record_Status(string Filter, string Search)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Search", Search));

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
        public DataTable Get_Assigned_Record_Status(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter));

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

        public DataTable Get_Vehicle(string Filter, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IsActive", IsActive));
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


        
        public DataTable Get_Vehicle(string Filter, int ManuId, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ManuId", ManuId, "@IsActive", IsActive));
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

        public DataTable Get_Vehicle(string Filter, int TypeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TypeId", TypeId));
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
        public DataTable Get_EditVehicle(string Filter, int VehicleId, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VehicleId, "@IsActive", IsActive));
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
        

        public DataTable Get_ListModel(string Filter, int ManuId, int TypeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ManuId", ManuId, "@TypeId", TypeId));
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
        public DataTable Get_Vehicle(string Filter, int ModelId, int ManuId, bool IsActive)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ParentId", ModelId, "@ManuId", ManuId, "@IsActive", IsActive));
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

        public DataTable Get_Manufacturer(string Filter, int ManuId, int TypeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ManuId", ManuId, "@TypeId", TypeId));
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

        public int Update_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleId, string VehicleImage, int VehicleGroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TypeId", TypeId, "@ManuId", ManuId, "@ModelId", ModelId, "@SubModelId", SubModelId, "@Year", Year, "@ChassisNumber", ChassisNumber, "@RegistrationNumber", RegistrationNumber, "@IsActive", IsActive, "@VehicleId", VehicleId, "@Picture", VehicleImage, "@VehicleGroupId", VehicleGroupId));
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
        public int Update_Vehicle(string Filter, int VehicleId, int IMEIId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VehicleId, "@IMEIId", IMEIId));
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
        public int Update_Vehicle(string Filter, int TypeId, int ManuId, int ModelId, int SubModelId, string Year, string ChassisNumber, string RegistrationNumber, bool IsActive, int VehicleId, int VehicleGroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TypeId", TypeId, "@ManuId", ManuId, "@ModelId", ModelId, "@SubModelId", SubModelId, "@Year", Year, "@ChassisNumber", ChassisNumber, "@RegistrationNumber", RegistrationNumber, "@IsActive", IsActive, "@VehicleId", VehicleId, "@VehicleGroupId", VehicleGroupId));
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

        public int Assign_Single_Vehicle(string Filter, int UserId, int VehicleId, DateTime DateAssign, int IMEIId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles_Assignment");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", UserId, "@VehicleId", VehicleId, "@DateAssign", DateAssign,"@IMEIId",IMEIId ));
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

        public int Return_Single_Vehicle(string Filter, int VehicleId, DateTime DateReturn, int VehicleAssignmentId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles_Assignment");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VehicleId,  "@DateReturn", DateReturn, "@VehicleAssignmentId", VehicleAssignmentId));
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

        public int Transfer_Single_Vehicle(string Filter, int UserId, int VehicleId, DateTime DateAssign, int VehicleAssignmentId,int IMEIId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles_Assignment");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserId", UserId, "@VehicleId", VehicleId, "@DateAssign", DateAssign, "@DateReturn", DateAssign, "@VehicleAssignmentId", VehicleAssignmentId, "@IMEIId", IMEIId));
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

        public int List_Vehicle_Group(string Filter, string Title, string Description, bool IsActive, bool IsDelete)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Title", Title, "@Description", Description, "@IsActive", IsActive, "@IsDeleted", IsDelete));
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

        public int List_Vehicle_Group(string Filter, string Title, string Description, bool IsActive, bool IsDelete, int VehicleGroupId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Title", Title, "@Description", Description, "@IsActive", IsActive, "@IsDeleted", IsDelete, "@VehicleGroupId", VehicleGroupId));
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

        public DataTable GET_DT_LIST(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles_Assignment");
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

        public DataTable GET_Group_LIST(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
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

        public DataTable GET_Group_LIST(string Filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleGroupId", id));
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

        public DataTable Get_List_IMEI(string Filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
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

        public DataTable GetTrackersByUserID(string Filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserID",id));
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
        public DataSet Get_List_IMEI(string Filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", id));
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

        public DataTable GetCurrentIMEIDetails(string Filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", id));
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

        public DataTable Get_List_IMEI_ByUserID(string Filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UserID",id));
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
        public DataTable Get_KML(string Filter, int VehicleId, DateTime FromDate, DateTime ToDate, Nullable<bool> tripStatus = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VehicleId, "@FromDate", FromDate, "@ToDate", ToDate , "@TripStatus", tripStatus));
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

        public DataTable Get_DrivePlayDate(string Filter, int VehicleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@VehicleId", VehicleId));
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


        public DataTable ValidateChassisNumber(string Filter, string ChassisNumber)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ChassisNumber", ChassisNumber));

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

        public DataTable ValidateChassisNumberOnUpdate(string Filter, string ChassisNumber, int VehicleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@ChassisNumber", ChassisNumber, "@VehicleId", VehicleId));

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

        public DataTable ValidateRegistrationNumber(string Filter, string RegistrationNumber)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@RegistrationNumber", RegistrationNumber));

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

        public DataTable ValidateRegistrationNumberOnUpdate(string Filter, string RegistrationNumber, int VehicleID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_Vehicles");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@RegistrationNumber", RegistrationNumber, "@VehicleId", VehicleID));

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

        /* == Start Insert Tracker Logs == */
        public int Insert_Tracker(string Filter, string TrackerID, double Latitude, double Longitude, double Speed, double Odometer, double Direction, string Rotation, double Altitude, string TrackerStream, string GPSSignalStatus, DateTime UTCTimeAndDate, FM_Tracker_InputOutputStatus ParameterObject, bool IsOfflineData, string DeviceState, string ExtendState, string Address, double FuelConsumedPercentage, double CurrentTripFuelConsumed, double Temperature, double CurrentTripMileage, string GSMSignal,int TripNO)
        {                                                                                                                                                                                                                                
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", TrackerID, "@Latitude", Latitude, "@Longitude", Longitude, "@Speed", Speed, "@Odometer", Odometer, "@Direction", Direction, "@Rotation", Rotation, "@Altitude", Altitude, "@TrackerStream", TrackerStream, "@GPSSignalStatus", GPSSignalStatus, "@UTCTimeAndDate", UTCTimeAndDate, "@OutLockthedoor", ParameterObject.OutLockthedoor, "@OutSirenSound", ParameterObject.OutSirenSound, "@OutUnlockthedoor", ParameterObject.OutUnlockthedoor, "@OutRelyToStopCar", ParameterObject.OutRelyToStopCar, "@InSOS", ParameterObject.InSOS, "@InAntiTemper", ParameterObject.InAntiTemper, "@InDoorOpenClose", ParameterObject.InDoorOpenClose, "@InUnlockDoor", ParameterObject.InUnlockDoor, "@InEngineOnOff", ParameterObject.InEngineOnOff, "@IsOfflineData", IsOfflineData, "@DeviceState", DeviceState, "@ExtendState", ExtendState, "@Address", Address,  "@FuelLiter", CurrentTripFuelConsumed, "@FuelPercent", FuelConsumedPercentage, "@Temperature", Temperature, "@CurrentTripMileage", CurrentTripMileage,"@GSMSignal", GSMSignal, "@TripName", TripNO.ToString()));
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

        // Insert Vehicle States/ Alarms
        public int Insert_VState(string Filter, string TrackerID, int VID, double Latitude, double Longitude, FM_VehicleStates objTrackerAlarms)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {                
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", TrackerID, "@Latitude", Latitude, "@Longitude", Longitude, "@Status", objTrackerAlarms.Status, "@AlarmCode", objTrackerAlarms.VStatusCode.ToString()));
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

        public int Insert_TrackingAlarms(string Filter, string TrackerID, int VID, double Latitude, double Longitude,FM_Tracker_Alarms objAlarms)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {                                
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", TrackerID, "@VehicleId", VID, "@Latitude", Latitude, "@Longitude",Longitude, "@AlarmCode",objAlarms.AlrmCodes.ToString(), "@AlarmThreshodVal",objAlarms.ThresholdVal.ToString(), "@AlarmCurrentVal", objAlarms.CurrentVal.ToString(), "@Status", objAlarms.Status));
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
        public int Insert_Tracker(string Filter, int UEId, int VehicleId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracker");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@UEId", UEId, "@VehicleId", VehicleId));
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

        //Fleet Replay Records Start

        public DataTable Get_FleetReplayRecords(FleetReplayCriteria fleetreplaycriteria)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_GetFleetReplayRecords");
                //loCommand = DataContext.StartTransaction(loCommand);
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", fleetreplaycriteria.Filter, "@resultDate", fleetreplaycriteria.resultDate, "@Device", fleetreplaycriteria.Device));
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

        public DataTable GetAlarmsToSendNotification(string Filter, string TrackerIMEI)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageVehicleTracking");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IMEI", TrackerIMEI));
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

        public DataTable Insert_TripIdleTime(string Filter, string IMEI,long ModifiedBy, string TimeInterval)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");                
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", IMEI , "@ModifiedBy", Convert.ToInt32(ModifiedBy), "@ThresholdValues", TimeInterval));
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

        //Fleet Replay Records End 
        public int Insert_WifiSettings(string Filter, FM_TrackerWifiSetting fm_wifistngs)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@IsAppliedStatus", fm_wifistngs.IsAppliedStatus, "@WifiStatus", fm_wifistngs.WifiStatus, "@SSID", fm_wifistngs.SSID,"@Password",fm_wifistngs.WifiPassword, "@IsAppliedSSID",fm_wifistngs.IsAppliedSSID, "@IsAppliedPwd",fm_wifistngs.IsAppliedPwd, "@TrackerSerialNo", fm_wifistngs.TrackerID));
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

        public DataTable GetWifiSettings(string Filter, string trackerSerialNo)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "FM_ManageFleetSettings");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@TrackerSerialNo", trackerSerialNo));
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

        /* == End Insert Tracker Logs == */
        /*
        public DataTable Read(string filter)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Read(string filter, int CID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ModuleID", CID));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable Read(string filter, int CID, int MID)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@ComponentID", CID, "@ModuleID", MID));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable ReadPost(string filter, int id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", filter, "@FeatureID", id));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public int EditPost(string filter, int HelpId, string title, string description)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {

                loCommand = DataContext.SetStoredProcedure(loCommand, "AD_GetHelp");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", filter, "@HelpID", HelpId, "@Title", title, "@Description", description));
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
        }*/

    }
}
