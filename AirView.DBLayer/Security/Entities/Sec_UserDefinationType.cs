using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Security.Entities
{
   public class Sec_UserDefinationType
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public Int64 DefinationTypeId { get; set; }
        public string DefinationType { get; set; }
        public Int64 PDefinationTypeId { get; set; }
    }
}
