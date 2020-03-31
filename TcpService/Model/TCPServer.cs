using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class InputOutputStatus
    {
        public bool OutLockthedoor { get; set; }
        public bool OutSirenSound { get; set; }
        public bool OutUnlockthedoor  { get; set; }
        public bool OutRelyToStopCar { get; set; }

        public bool InSOS { get; set; }
        public bool InAntiTemper { get; set; }
        public bool InDoorOpenClose { get; set; }
        public bool InUnlockDoor { get; set; }
        public bool InEngineOnOff { get; set; }

        public bool ExhaustEmission { get; set; }
        public bool IdleEngine { get; set; }
        public bool HardDecleration { get; set; }
        public bool HardAccelration { get; set; }
        public bool HighEngineCoolentTemp { get; set; }
    }

}
