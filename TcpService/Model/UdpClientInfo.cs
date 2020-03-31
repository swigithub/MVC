using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpService
{
    class UdpClientInfo
    {
        public string TrackerID { get; set; }
        public IPAddress IPAddress { get; set; }
        public int PortNo { get; set; }
        public TrackerManufacturer Manufacturer { get; set; }
        public DateTime LstPktRcvdTime { get; set; }
    }
}
