using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_SiteTaskInventory
    {
        public long SiteTaskInventoryId { get; set; }
        public long SiteId { get; set; }
        public long SiteTaskId { get; set; }
        public long CategoryId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }

        public int IsModified { get; set; }
    }
}
