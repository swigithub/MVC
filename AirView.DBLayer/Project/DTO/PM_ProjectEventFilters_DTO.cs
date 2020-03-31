using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DTO
{
   public class PM_ProjectEventFilters_DTO
    {
        public List<Status> Statuses { get; set; }
        public List<PM_Task_DTO> Task{ get; set; }
        public List<PM_Entity_DTO> Entities { get; set; }
        public List<PM_Type_DTO> Types { get; set; }
        public List<Security.Entities.Sec_UserProjects> UserProjects { get; set; }
    }
    public class PM_Task_DTO
    {
        public Int32 EntityTaskId { get; set; }
        public string EntityTaskName { get; set; }
        public Int32 EntityId { get; set; }
        public Int32 PreviousSitesCount { get; set; }
        public Int32 CurrentSitesCount { get; set; }
    }
    public class PM_Entity_DTO
    {
        public int EntityId { get; set; }
        public string EntityCode { get; set; }
        public string EntityName { get; set; }

    }
    public class PM_Type_DTO
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }

    }
}
