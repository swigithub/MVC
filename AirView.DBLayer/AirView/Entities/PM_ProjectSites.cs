using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class ProjectSites
    {
     
        public string Color { get; set; }
        public string ColorId { get; set; }
        public string clusterId { get; set; }
        public string clusterName { get; set; }
        public string networkModes { get; set; }
        public string networkmodename { get; set; }
        public string filename { get; set; }
        /// <summary>
        /// //
        /// </summary>
        public Int64? SiteId { get; set; }
        public DateTime? SiteDate { get; set; }
        public Int64? ProjectId { get; set; }
        public string PMRefId { get; set; }
        public string CommonId { get; set; }
        public string Project { get; set; }
        //public Int64? ProjectSiteId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        //public string clusterCode { get; set; }
        public string ClusterCode { get; set; }
        public string FACode { get; set; }
        public Int64? CityId { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Client { get; set; }

        public Int64? ClientId { get; set; }
        public string ClientPrefix { get; set; }
       // public string siteCode { get; set; }
       public string SiteCode { get; set; }
        public string siteLatitude { get; set; }
        public string siteLongitude { get; set; }
        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Int64? CreatedBy { get; set; }
        public string sectorCode { get; set; }
        public string networkMode { get; set; }

        public string Scope { get; set; }
    //    public Int64 ScopeId { get; set; }
        public string ScopeId { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Antenna { get; set; }
        public string BeamWidth { get; set; }
        public string Azimuth { get; set; }
        public string PCI { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public string BandWidth { get; set; }
        public string CellId { get; set; }
        public int RFHeight { get; set; }
        public int MTilt { get; set; }
        public int ETilt { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string Address { get; set; }
        public string SiteTypeId { get; set; }
        public string SiteType { get; set; }
        public Int64? SiteClassId { get; set; }
        public string SiteClass { get; set; }
        public string MRBTS { get; set; }
        public Int64? UserId { get; set; }
        public string IMEI { get; set; }
        public Int64? RevisionId { get; set; }
        public Int64? RedriveCount { get; set; }
        public double? SectorLatitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? SectorLongitude { get; set; }
        public bool IsActive { get; set; }


        public string USID { get; set; }
        public string SubMarket { get; set; }
        public bool vMME { get; set; }
        public bool ControlledIntro { get; set; }
        public bool SuperBowl { get; set; }
        public string isDASInBuild { get; set; }
        public string FirstNetRAN { get; set; }
        public string IPlanJob { get; set; }
        public string PaceNo { get; set; }
        public string IPlanIssueDate { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }

    }
}
