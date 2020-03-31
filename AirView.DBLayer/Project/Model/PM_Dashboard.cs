using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Dashboard
    {
        public Int64 projectId { get; set; }
        public string Market { get; set; }
        public string Scope { get; set; }
        public string SiteCode{ get; set; }
        public string Schedule { get; set; }
        public string SSV { get; set; }

    }
}
