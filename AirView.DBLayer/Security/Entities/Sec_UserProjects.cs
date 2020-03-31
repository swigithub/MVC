using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Security.Entities
{
  public  class Sec_UserProjects
    {
        public Int64 UserId { get; set; }
        public Int64 ProjectId { get; set; }
        public string Plural { get; set; }
        public string Title { get; set; }
    }
}
