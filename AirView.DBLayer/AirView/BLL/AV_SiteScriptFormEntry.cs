using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
  public  class AV_SiteScriptFormEntry
    {

        public long SrId { get; set; }
        public decimal FormId { get; set; }
        public int IsDeleted { get; set; }
        public decimal NodeTypeId { get; set; }
        public string Title { get; set; }
        public string ControlType { get; set; }
        public string DataType { get; set; }
        public string DefaultValue { get; set; }

        public string ActualValue { get; set; }
        public string MaxLength { get; set; }
        public string Required { get; set; }
        public string IsAttachment { get; set; }
        public int SortOrder { get; set; }
        public string Comments { get; set; }
    }
}
