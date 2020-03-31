using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class AD_Projects
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectScopeID { get; set; }
        public string Scope { get; set; }
        public string Market { get; set; }
        public int CompanyID { get; set; }
        public string Company { get; set; }
        public int VendorID { get; set; }
        public string Vendor { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }


        public int StatusID { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ClientName { get; set; }
   
        public string DefinationName { get; set; }
        public int TypeId { get; set; }
        public int TypeValue { get; set; }
        public int totalCount { get; set; }

        //public IEnumerable<AD_Clients> listClients { get; set; }

        public Int64[] ProjectIds { get; set; }
        public Int64[] CompanyIds { get; set; }
        public Int64[] VendorIds { get; set; }
        public Int64[] StatusIds { get; set; }

    }
}
