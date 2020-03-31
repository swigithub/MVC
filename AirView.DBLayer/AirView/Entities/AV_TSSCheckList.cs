using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
   public class AV_TSSCheckList
    {
        public decimal CheckListId { get; set; }
        public decimal SiteId { get; set; }
        public decimal LayerId { get; set; }
        public decimal CheckListTypeId { get; set; }
        public decimal CheckListPId { get; set; }
        public int CheckCount { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
