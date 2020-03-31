using AirView.DBLayer.Survey.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class TSS_SurveyDocument
    {
        public TSS_SurveyDocument() {
        
            Sections = new List<TSS_Section>();
            CompletetionSections = new List<TSS_Section>();
            QuestionList = new List<TSS_Question>();
            RespondentList = new List<Respondents>();
            SiteAttendeesList = new List<TSS_SiteAttendees>();
            SectorLocationList = new List<TSS_SectorLocations>();
        }
        public Int64 SurveyId { get; set; }
        public Int64 SiteSurveyId { get; set; }
        public string SurveyTitle { get; set; } 
        public string Description { get; set; }
        public string CityName { get; set; }
        public string ClientName { get; set; }
        public Int64 CategoryId { get; set; }
        public string Category { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public Int64 SiteId { get; set; }
        public string WoRefId { get; set; }
        public Int64 UnitSystemId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int64 CreatedBy { get; set; }
      
        public Int64 totalCount { get; set; }
        public string Region { get; set; }
        public string SiteCode { get; set; }
        public string Status { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Scope { get; set; }

      
        public Int64 SiteSectionId { get; set; }

   
        public List<TSS_Section> Sections { get; set; }

        public List<TSS_Section> CompletetionSections { get; set; }
        public List<TSS_Question> QuestionList { get; set; }
        public List<Respondents> RespondentList { get; set; }
        public List<TSS_SiteAttendees> SiteAttendeesList { get; set; }
        public List<TSS_SectorLocations>  SectorLocationList { get; set; }

        public List<SurveySitesInfo> SurveySitesInfo { get; set; }
        public string AllFanImage { get; set; }
        public string SiteImage { get; set; }
        public string SiteAddress { get; set; }

        public bool IsPublished { get; set; }
        public bool IsGlobal { get; set; }
        public string ProjectName { get; set; }
        public string WODescription { get; set; }

        public string SiteTpye { get; set; }
        public string Instruction { get; set; }

        public int ChecklistCount { get; set; }

        public string DefaultSitecode { get; set; }

        public string SurveyCode { get; set; }
    }

    public class TSS_COPList
    {
        public Int64 SurveyId { get; set; }
        public string ClientName { get; set; }
        public string CityName { get; set; }
        public string SurveyTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublish { get; set; }
        public bool IsGlobal { get; set; }


    }
}
