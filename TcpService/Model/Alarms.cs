using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class Alarms
    {
        public bool IsExhaustEmission { get; set; } = false;
        public bool IsIdleEngine { get; set; } = false;
        public bool IsHardAcceleration { get; set; } = false;
        public bool IsHardDecelration { get; set; } = false;

        public bool IsEngineCooltemp { get; set; } = false;
        public bool IsSpeeding { get; set; } = false;
        public bool IsTowing { get; set; } = false;
        public bool IsLowVoltage { get; set; } = false;
        public bool IsTemper { get; set; } = false;
        public bool IsCrash { get; set; }= false;
        public bool IsEmergency { get; set; } = false;

        public bool IsFatigueDriving { get; set; } = false;
        public bool IsSharpTurn { get; set; } = false;
        public bool IsQuickLaneChange { get; set; } = false;
        public bool IsPowerOn { get; set; } = false;
        public bool IsHighRPM { get; set; } = false;

        public bool IsMIL { get; set; } = false;

        public bool IsOBDCommunicationErr { get; set; } = false;
        public bool IsPowerOff { get; set; } = false;

        public bool IsNOGps { get; set; }= false;

        public bool IsPrivacyStatus { get; set; } = false;

        public bool IsIgnitionOn { get; set; } = false;

        public bool IsillegalIgnition { get; set; } = false;

        public bool IsIllegalEnter { get; set; } = false;

        public bool IsVibration { get; set; } = false;

        public bool IsDangerousDriving { get; set; } = false;

        public bool IsNocard { get; set; } = false;

        public bool IsUnlock { get; set; } = false;

        public bool IsGeoFenceAlarm { get; set; } = false;
        

        public bool IsIgnitionOff { get; set; } = false;

    }
}
