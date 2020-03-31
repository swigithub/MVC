using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Fleet.Model
{
    public class FM_Vehicle
    {
        public int VehicleId { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        [Required]
        public int ManuId { get; set; }
        public string ManuName { get; set; }
        [Required]
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        [Required]
        public int SubModelId { get; set; }
        public string SubModelName { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string ChassisNumber { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int VehicleAssignmentId { get; set; }
        public string VehicleImage { get; set; }
        [Required]
        public int VehicleGroupId { get; set; }
        public string VehicleGroupName { get; set; }        
        public int IMEIId { get; set; }        
        public string IMEI { get; set; }
        public double Milage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double FuelMeter { get; set; }

        public string TrackerID { get; set; }
        public string TrackerStream { get; set; }
        public double Altitude { get; set; }
        public string Rotation { get; set; }
        public double Direction { get; set; }
        public double Odometer { get; set; }
        public double Speed { get; set; }
        public string GPSSignalStatus { get; set; }
        public DateTime UTCTimeAndDate { get; set; }

        public string Location { get; set; }
        public FM_Tracker_InputOutputStatus ObjInpOutStatus { get; set; }
        public bool IsOfflineData { get; set; }
        public string DeviceState { get; set; }        
        public FM_Tracker_AssetStatus ObjAssetStatus { get; set; }
        public string ExtendState { get; set; }
        public string Address { get; set; }
        public double FuelConsumedPercentage { get; set; }
        public Double CurrentTripFuelConsumed { get; set; }
        public double Temperature { get; set; }
        public double CurrentTripMileage { get; set; }
        public string GSMSignal { get; set; }
        public List<FM_Tracker_Alarms> LstAlarms { get; set; }
        public List<FM_VehicleStates> LstVState { get; set; }
        public FM_TrackerTrip objTrackerTrip { get; set; }
    }   

}
