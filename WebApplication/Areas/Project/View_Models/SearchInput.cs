using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Areas.Project.View_Models
{
    public class SearchInput
    {
        public Int64[] ProjectId { get; set; }
        public Int64[] CompanyId { get; set; }
        public Int64[] VendorId { get; set; }
        public Int64[] StatusId { get; set; }
        public DateTime? FromDate{ get; set; }
        public DateTime? EndDate { get; set; }
        public string date { get; set; }

    }
}