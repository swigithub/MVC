using AirView.DBLayer.Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.DTO
{
   public class PM_ProjectEntityFilters_DTO
    {
        public List<Client> clients { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Type> Types { get; set; }
        public List<Market> Markets { get; set; }


    }
    public class Status
    {
        public Int64 StatusId { get; set; }
        public string StatusName { get; set; }

    }
    public class Type
    {
        public Int64 TypeId { get; set; }
        public string TypeName { get; set; }
    }
    public class Market
    {
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public string SubMarket { get; set; }
        public string SubMarketParentId { get; set; }

    }
    
}
