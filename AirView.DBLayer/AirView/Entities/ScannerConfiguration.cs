using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    public class ScannerConfiguration
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public int RevisionId { get; set; }
        public DateTime ConfigurationDate { get; set; }
        public int TestTypeId { get; set; }
        public int KpiId { get; set; }
        public string KpiValue { get; set; }
        public bool IsActive { get; set; }
        public int ManufacturerId { get; set; }
        public int ScannerModelId { get; set; }
        public int ProtocolId { get; set; }
        public int ScannerBandId { get; set; }
    }
}
