using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class AssetStatus
    {
        public bool ExhaustEmission { get; set; }
        public bool IdleEngine { get; set; }
        public bool HardDeceleration { get; set; }

        public bool HardAcceleration { get; set; }
        public bool HighEngineCoolantTemp { get; set; }
        public bool Speeding { get; set; }
        public bool Towing { get; set; }

        public bool LowVoltage { get; set; }

        public bool Temper { get; set; }

        public bool Crash { get; set; }

        public bool Emergency { get; set; }
        public bool Fatiguedriving { get; set; }

        public bool SharpTurn { get; set; }

        public bool QuickLaneChange { get; set; }

        public bool PowerOn { get; set; }

        public bool HighRPM { get; set; }

        public bool MIL { get; set; }

        public bool OBDcommunicationerror { get; set; }

        public bool PowerOff { get; set; }

        public bool NoGPSdevice { get; set; }

        public bool PrivacyStatus { get; set; }

        public bool illegalIgnition { get; set; }

        public bool IllegalEnter { get; set; }

        public bool Vibration { get; set; }

        public bool DangerousDriving { get; set; }

        public bool NoCardPresented { get; set; }

        public bool UnLock { get; set; }
    }
}
