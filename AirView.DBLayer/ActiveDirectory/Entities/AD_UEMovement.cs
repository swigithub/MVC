using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.Entities
{
    /*----MoB!----*/
    public class AD_UEMovement
    {
        public Int64 UEMovementId { get; set; }
        public Int64 UEId { get; set; }
        public Int64 FromUserId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 UEStatusId { get; set; }
        public Int64 UETypeId { get; set; }
        public DateTime Date { get; set; }

    }
}
