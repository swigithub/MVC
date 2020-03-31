using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AV_SiteScriptScannerConfigurations
    {
        public int SrId { get; set; }
        public int SiteScriptId { get; set; }
        public int MeasurementId { get; set; }
        public int KpiId { get; set; }
        public string KpiValue { get; set; }
    }
}
