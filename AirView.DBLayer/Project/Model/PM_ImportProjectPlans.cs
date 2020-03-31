using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
  public  class PM_ImportProjectPlans
    {

        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedBy { get; set; }
        public string Completion { get; set; }
        public string FACode { get; set; }
       public string Cluster { get; set; }
        public string Market { get; set; }
       public string SiteName { get; set; }
        public int MyProperty { get; set; }
        public string Task { get; set; }
        public string PlanDate { get; set; }
        public string ForecastStartDate { get; set; }
        public string Priority { get; set; }
        public string ForecastEndDate { get; set; }
        public string TargetDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string Status { get; set; }
        public string Resources { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }
        public string SiteClass { get; set; }
        public string USID { get; set; }
        public string CommonID { get; set; }
        public string REGION { get; set; }
        public string MARKET { get; set; }

        public string SUBMarket { get; set; }
        public string StreetAddress { get; set; }
        public string CITY { get; set; }
        public string State { get; set; }


        public string ZIP { get; set; }
        public string COUNTY { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string vMME { get; set; }


        public string ControlledIntroduction { get; set; }
        public string SuperBowl { get; set; }
        public string SiteType { get; set; }
        public string DASorInbuilding { get; set; }
        public string FirstNetRAN { get; set; }


        public string iPlanJob { get; set; }
        public string iPlanStatus { get; set; }
        public string iPlanIssueDate { get; set; }
        public string PACENumber { get; set; }
        public string TSSPlan { get; set; }

        public string TSSForecast { get; set; }
        public string TSSSubmitted { get; set; }
        public string SiteSpecificMaterialAvailableForecast { get; set; }
        public string SiteSpecificMaterialAvailableActual { get; set; }
        public string PreInstallPlanned { get; set; }


        public string PreInstallFcst { get; set; }
        public string PreInstallActual { get; set; }
        public string MigDatePlanned { get; set; }
        public string MigDateForecast { get; set; }
        public string MigrationDate { get; set; }

        public string EPLOrdered { get; set; }
        public string EPLCalledOut { get; set; }
        public string EPLDelivered { get; set; }
        public string EPLStatus { get; set; }
        public string SiteRevisionType { get; set; }
        public string TaskRevisionType { get; set; }
        public long RevisionId { get; set; }
        public long ProjectPlanHistoryId { get; set; }
        public long ProjectSiteId { get; set; }
        public long SiteTaskId { get; set; }
        public string EstimatedStartDate { get; set; }
        public string EstimatedEndDate { get; set; }

        public string _difference { get; set; }
    }
    public class PM_ImportProjectPlansResult
    {

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Value6 { get; set; }
        public string Value7 { get; set; }
        public string Value8 { get; set; }
        public string Value9 { get; set; }

        public string Value20 { get; set; }
        public string Value21 { get; set; }
        public string Value22 { get; set; }
        public string Value15 { get; set; }
        public string Value16 { get; set; }

        public string Value28 { get; set; }
        public string Value29 { get; set; }

        public string Value30 { get; set; }
        public string Value31 { get; set; }

        public string Value32 { get; set; }
        public string _difference { get; set; }

    }
    public class MP_Import_WR_Ex
    {
        //34
        public string FACode { get; set; }
 
        public string ActivityType { get; set; }
        public string Alarms { get; set; }
        public string GNG { get; set; }
        public string Scheduled { get; set; }
        public string MW { get; set; }
        public string Attachment { get; set; }
        public string AttachmentType { get; set; }
        public string Notes { get; set; }
        public string eNB { get; set; }
        public string ExtendedENB { get; set; }
        public string EquipmentId { get; set; }
        public string AOTSCR { get; set; }
        public string USID { get; set; }
        public string Status { get; set; }
        public string IsAdditional { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
    }
    public class MP_Import_WR_Fail
    {
        //22
        public string eNB { get; set; }
        public string Task { get; set; }
        public string FACode { get; set; }
        public string ExtendedENB { get; set; }
        public string EquipmentId { get; set; }
        public string AOTSCR { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string WhoFix { get; set; }
        public string IsUnavoidable { get; set; }
        public string ActivityType { get; set; }
        public string Alarms { get; set; }
        public string Severity { get; set; }
        public string MW { get; set; }
        public string AttachmentType { get; set; }
        public string Attachment { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
        public string ScheduleDate { get; set; }
        public string ActualDate { get; set; }
        public string TargetDate { get; set; }
        public string RequestedBy { get; set; }
        public string RequestDate { get; set; }
        public string CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string SiteName { get; set; }
        //public string CC { get; set; }

        //public string Market { get; set; }
        //public string Schedule { get; set; }
        //public string Actual { get; set; }


        //public string Alarm { get; set; }
        //public string Issues { get; set; }
       
        //public string Notes { get; set; }
        //public string ContentType { get; set; }

        //public string AppCreatedBy { get; set; }
        //public string AppModifiedBy { get; set; }
        //public string Attachments { get; set; }
        //public string WorkflowInstanceID { get; set; }
        //public string FileType { get; set; }

        //public string PMO { get; set; }
        //public string Modified { get; set; }
      

    }
}
