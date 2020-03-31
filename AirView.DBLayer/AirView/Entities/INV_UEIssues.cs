using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class INV_UEIssues
    {

        public Int64 IssueId { get; set; }
        public Int64 UEId { get; set; }
        public string Description { get; set; }
        public Int64 IssueUserId { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
