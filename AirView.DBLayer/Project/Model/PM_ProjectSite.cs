
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_ProjectSite
    {
        public PM_ProjectSite()
        {
            SelectedListStatus = new List<SelectedList>();
            //SelectedListUsers = new List<SelectedList>();
            Clients = new List<Client>();
        }
        
        public Int64 MyProperty { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public string WoRefId { get; set; }
        public Int64 ProjectId { get; set; }
        [Display(Name = "FA Code")]
        public string SiteCode { get; set; }
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }
        public string ExtendedeNB { get; set; }
        public string eNB { get; set; }
        public string EquipmentId { get; set; }
        public string AOTSCR { get; set; }
        public string FilePath { get; set; }
        public DateTime SiteDate { get; set; }
        public Int64 SiteTypeId { get; set; }
        public Int64 SiteClassId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int RevisionId { get; set; }
        public int PMCode { get; set; }
        public Int64 PMRefId { get; set; }
        public Int64 ClusterId { get; set; }
        public Int64 ClusterCode { get; set; }
        public Int64 GNGId { get; set; }
        public Int64 ActivityTypeId { get; set; }
        public Int64 ItemTypeId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 StatusId { get; set; }
        public string Status { get; set; }
        public Int64 MSWindowId { get; set; }
        public Int64 AlarmId { get; set; }
        public Int64 PriorityId { get; set; }
        public Int64 ColorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public float BudgetCost { get; set; }
        public float ActualCost { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Notes { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 ScopeId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string Address { get; set; }



        [Display(Name = "Plan Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PlannedDate { get; set; }


        [Display(Name = "Target Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TargetDate { get; set; }



        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ActualStartDate { get; set; }



        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ActualEndDate { get; set; }



        [Display(Name = "Forecast Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EstimatedStartDate { get; set; }




        public DateTime? EstimatedEndDate { get; set; }



        public bool IsAddionalSite { get; set; }
        public string Tasks { get; set; }
        public Int64 TaskId { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Markets { get; set; }

        public bool CheckAll { get; set; }
        public List<SelectedList> SelectedListStatus { get; set; }
        //public List<SelectedList> SelectedListUsers { get; set; }
        public List<Client> Clients { get; set; }
        
    }



    public class PM_ProjectSiteDto
    {
        public PM_ProjectSiteDto()
        {
            SelectedListStatus = new List<SelectedList>();
            SelectedListUsers = new List<string>();
        }
        public Int64? MyProperty { get; set; }
        public Int64? ProjectSiteId { get; set; }
        public string WoRefId { get; set; }
        public Int64? ProjectId { get; set; }
      
        public string SiteCode { get; set; }
        
        public string SiteName { get; set; }
        public string ExtendedeNB { get; set; }
        public string eNB { get; set; }
        public string EquipmentId { get; set; }
        public string AOTSCR { get; set; }
        public string FilePath { get; set; }
        public DateTime SiteDate { get; set; }
        public Int64? SiteTypeId { get; set; }
        public Int64? SiteClassId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int RevisionId { get; set; }
        public int PMCode { get; set; }
        public Int64? PMRefId { get; set; }
        public Int64? ClusterId { get; set; }
        public Int64? ClusterCode { get; set; }
        public Int64? GNGId { get; set; }
        public Int64? ActivityTypeId { get; set; }
        public Int64? ItemTypeId { get; set; }
        public Int64? CityId { get; set; }
        public Int64? StatusId { get; set; }
        public string Status { get; set; }
        public Int64? MSWindowId { get; set; }
        public Int64? AlarmId { get; set; }
        public Int64? PriorityId { get; set; }
        public Int64? ColorId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int64? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public float? BudgetCost { get; set; }
        public float? ActualCost { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Notes { get; set; }
        public Int64? ClientId { get; set; }
        public Int64? ScopeId { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public string Address { get; set; }

       
        public DateTime? PlannedDate { get; set; }


       
        public DateTime? TargetDate { get; set; }


      
        public DateTime? ActualStartDate { get; set; }

        


        public DateTime? ActualEndDate { get; set; }

      
        public DateTime? EstimatedStartDate { get; set; }


      
        public DateTime? EstimatedEndDate { get; set; }
        public bool IsAddionalSite { get; set; }
        public string Tasks { get; set; }
        public Int64? TaskId { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Markets { get; set; }

        public bool CheckAll { get; set; }
        public List<SelectedList> SelectedListStatus { get; set; }
        public List<string> SelectedListUsers { get; set; }
    }
}
