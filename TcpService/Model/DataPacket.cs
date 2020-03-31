using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class DataPacket
    {
        public DateTime UTCTimeAndDate { get; set; }        
        public double Speed { get; set; }
        public double Odometer { get; set; } //in meters       
        public double Latitude { get; set; } 
        public string LatitudeDirection { get; set; }
        public double Longitude { get; set; }
        public string LongitudeDirection { get; set; }
        public double Direction { get; set; }
        public string Rotation { get; set; }
        public double Altitude { get; set; }
        public Double TotalMilage { get; set; }
        public Double TotalFuelConsumed { get; set; }                
        public string TrackerID { get; set; }
        public string TrackerStream { get; set; }        
        public string VehicleId { get; set; }
        public string IMEI { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlate { get; set; }        
        public string FuelMeter { get; set; }
        public double InternalBatteryLvl { get; set; }
        public double ExternalBatteryLvl { get; set; }
        public bool IsOfflineData {get;set;}

        public string DeviceState { get; set; }
        public InputOutputStatus ObjInpOutStatus { get; set; }
        public AssetStatus ObjAssetStatus { get; set; }
        public string ExtendState { get; set; }
        public string Address { get; set; }
        public double FuelConsumedPercentage { get; set; }
        public Double CurrentTripFuelConsumed { get; set; }
        public double Temperature { get; set; }
        public string GPSSignalStatus { get; set; }//valid or Invalid
        public double CurrentTripMileage { get; set; }
        public double GSMSignal { get; set; }
        public List<TrackerAlarms> LstAlarms { get; set; }

        public List<VehicleStates> LstVState { get; set; }
        public TrackerTrip objTrackerTrip { get; set; }
    }
}
