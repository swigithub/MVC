using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class NetworkMode
    {
        public NetworkMode()
        {
            Bands = new List<Band>();
        }
        public Int64 NetworkModeId { get; set; }
        public string NetworkModeName { get; set; }
        public List<Band> Bands { get; set; }
    }
}
