using AirView.DBLayer.Project.Model;
using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Areas.Project.View_Models
{
    public class VM_ProjectsDetail
    {
        public AD_Projects Project { get; set; }
        
        public DashboardVM vm { get; set; }
       

        public IEnumerable<AD_Projects> projectsList { get; set; }
        public IEnumerable<AD_Clients> vendorList { get; set; }

    }
}