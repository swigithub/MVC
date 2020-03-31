using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AL_UserHubModel
    {
        public string UserName { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}
