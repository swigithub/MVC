using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Attachments
    {
        public Int64 SiteDocId { get; set; }
        public string Tag { get; set; }

        public Int64 Category { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public Int64 TaskId { get; set; }
        public string DocFilepath { get; set; }
        public string Description { get; set; }
        public Int64 UploadedById { get; set; }
        public DateTime UploadedOn { get; set; }
        public NameValueCollection Files { get; set; }
        public NameValueCollection Files1 { get; set; }
        public NameValueCollection Files2 { get; set; }
        public NameValueCollection Files3 { get; set; }
    }
}
