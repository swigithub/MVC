using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
   public class AV_SectorColor
    {
        public Int64 SectorColorId { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 ScopeId { get; set; }
        public string Scope { get; set; }
        public Int64 SectorId { get; set; }
        public string SectorCode { get; set; }
        public string SectorColor { get; set; }
  
    }
}
