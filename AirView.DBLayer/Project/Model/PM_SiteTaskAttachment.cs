using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTaskAttachment
    {
        public long AttachmentId { get; set; }
        public Int64 ProjectId { get; set; }
        public string FileExtension { get; set; }
        public long SiteTaskId { get; set; }
        public string FileName { get; set; }
        public string file { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
