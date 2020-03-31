using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_SiteContact
    {
        public Int64 SrId { get; set; }
        public Int64 SiteId { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string GateNo { get; set; }
        public string ContactNo { get; set; }
        public Int64 ContactTypeID { get; set; }
        public Int64 DesignationID { get; set; }
        public bool IsHoldingKeys { get; set; }
        public string Comment { get; set; }
        public string ContactType { get; set; }
        public string Designation { get; set; }
    }

    public class AccessInfo
    {
        public bool IsSpecialAccess { get; set; }
        public DateTime? AccessDateTime { get; set; }
        public string AccessInstructions { get; set; }
    }
}
