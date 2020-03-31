using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Areas.Project.View_Models
{
    public class PM_SandBox
    {
        public Int64 ProjectId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public string eNB { get; set; }
        public string FACode { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string SubMarket { get; set; }
        public string Status { get; set; }
        public string ColorCode { get; set; }
        public bool IsSelected { get; set; }
    }
}