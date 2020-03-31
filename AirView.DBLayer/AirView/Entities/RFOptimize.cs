using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class RFOptimize
    {
        public double rangeFrom { get; set; }
        public double rangeTo { get; set; }
        public string rangeColor { get; set; }
        public string PlotType { get; set; }
        public Int64 PciId { get; set; }
        public string pciColor { get; set; }
        public int PCI { get; set; }
        public string SectorCode { get; set; }
        public string sectorColor { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 NetworkLayerId { get; set; }
                
    }
}
