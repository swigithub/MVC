using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Project.DAL
{
    /*----MoB!----*/
    /*----15-15-2017----*/
    public class PM_ProjectResource
    {
        public Int64 ProjectAssignId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 AssignedById { get; set; }
        public Int64 AssignToId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ForecastDate { get; set; }
        public DateTime PlanDate { get; set; }
        public bool IsActive { get; set; }

        
    }
}
