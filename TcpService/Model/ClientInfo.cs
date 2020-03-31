using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    public class ClientInfo
    {
        public TcpClient client { get; set; }
        public string IMEI { get; set; }
        public TrackerManufacturer Manufacturer { get; set; }
        
        public TrackerTrip objTrackerTrip { get; set; }                
        public int CurrentTripCounter { get; set; }
        public bool IsEngineOn = false;
        public int TripIdleTime { get; set; }

        private DateTime _LastPktWithZeroSpeedTime;
        public DateTime LastPktWithZeroSpeedTime {
                get
                {
                    return _LastPktWithZeroSpeedTime;
                }

            set
                {
                    DateTime LstPktTime = value;
                _LastPktWithZeroSpeedTime = LstPktTime.ToUniversalTime();
                }
            }
        private DateTime _LstPktRcvdTime;
        public DateTime LstPktRcvdTime
        {
            get { return _LstPktRcvdTime; }
            set
            {
                DateTime LstPktTime = value;
                _LstPktRcvdTime = LstPktTime.ToUniversalTime();
            }
        }

        private DateTime _PktBeforeLastRcvdTime;
        public DateTime PktBeforeLastRcvdTime
        {
            get
            {
                return _PktBeforeLastRcvdTime;
            }

            set
            {
                DateTime LstPktTime = value;
                _PktBeforeLastRcvdTime = LstPktTime.ToUniversalTime();
            }
        }
        public bool IsNewTrip = false;
        public bool ContinuousZeroPkt = false;        
     
    }

}

    public enum TrackerManufacturer
    {
        Sinocastel=1,
        Top10=2, 
        Calamp =3     
    }


//* Vehicle Type
//* License Plate
//* IMEI
//* Milage
//* Latitude
//* Longitude
//* Fuel Meter