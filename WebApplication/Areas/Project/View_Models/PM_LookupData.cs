using AirView.DBLayer.Project.Model;
using SWI.Libraries.AD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Areas.Project.View_Models
{
    public class PM_LookupData
    {
        public List<AD_Defination> ProjectStatus { get; set; }
        public List<AD_Defination> Priorities { get; set; }
        public List<PM_TaskEntry> GroupResources { get; set; }
        public List<PM_TaskEntry> ProjectResources { get; set; }
    }
}