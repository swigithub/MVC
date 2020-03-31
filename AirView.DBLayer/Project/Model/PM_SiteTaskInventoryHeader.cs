using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTaskInventoryHeader
    {
        public int Id { get; set; }
        public string BusCode { get; set; }
        public string TaskName { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastStatus { get; set; }
    }
}
