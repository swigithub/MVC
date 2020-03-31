using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class PM_SiteTaskInventoryAttachments
    {
        public long SiteTaskInventoryAttachmentId { get; set; }
        public string FileNameWithExtension { get; set; }

        public string SubDirectory { get; set; }
        public long SiteTaskInventoryId { get; set; }

        public int ContentLength { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
