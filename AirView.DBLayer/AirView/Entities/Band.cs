using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class Band
    {
        public Band() {
            Carriers = new List<Carrier>();
        }

        public Int64 BandId { get; set; }
        public string BandName { get; set; }

        public List<Carrier> Carriers { get; set; }
    }
}
