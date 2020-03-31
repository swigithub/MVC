using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Security.Entities
{
   public class Summary
    {
       
        public int UserId { get; set; }
        public string SiteCode { get; set; }
        public String Status { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string Color { get; set; }
        public string DefinationName { get; set; }
        public int DefinationId { get; set; }
        public string PDefinationName { get; set; }
        public int PDefinationId { get;  set; }
    }
}
