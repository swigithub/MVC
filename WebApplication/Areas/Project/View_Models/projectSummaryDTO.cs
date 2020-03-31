using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Areas.Project.View_Models
{
    public class projectSummaryDTO
    {
        public projectSummaryDTO()
        {
            SiteStatus = new List<SiteStatusDTO>();    
            IssueDistribution = new List<IssueDistributionDTO>();
            IssueAccountibility = new List<IssueAccountibilityyDTO>();
        }
        public List<SiteStatusDTO> SiteStatus { get; set; }  
        public List<IssueDistributionDTO> IssueDistribution { get; set; }
        public List<IssueAccountibilityyDTO> IssueAccountibility { get; set; }
    }

    public class SiteStatusDTO
    {
        public string SiteStatus { get; set; }
        public string ColorCode { get; set; }
        public string StatusType { get; set; }
    }

    public class IssueDistributionDTO
    {    
        public string IssueType { get; set; }
        public Int64 TotalIssues { get; set; }
        public string ColorCode { get; set; }
    }

    public class IssueAccountibilityyDTO
    {
        public string IssueOwner { get; set; }
        public Int64 TotalIssues { get; set; }
        public string ColorCode { get; set; }
    }
}