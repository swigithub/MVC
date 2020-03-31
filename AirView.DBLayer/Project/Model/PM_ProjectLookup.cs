using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class PM_ProjectLookup
    {

      public  List<Status> Statuses { get; set; }
       public List<Priority> Priorities { get; set; }
        public List<Client> clients { get; set; }
    }

    public class Status
    {
       public Int64 StatusId { get; set; }
       public string StatusName { get; set; }
       public int count { get; set; }

    }
    public class Priority
    {
        public Int64 PriorityId { get; set; }
        public string PriorityName { get; set; }
    }

    
}
