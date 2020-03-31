using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class VehicleStates
    {
        public AlarmCodes VStatusCode { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }        
    }
}
