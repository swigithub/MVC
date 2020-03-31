using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Survey.Model
{
 public class CLS_VM
    {
     
        public string ClusterId { get; set; }
        public Int64 SiteClusterId { get; set; }
        public string ClusterName { get; set; }
        public string ClusterScope { get; set; }
        public string clusterFile { get; set; }
       public Int64 SiteId { get; set; }
        public string clusterCode { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Client { get; set; }
        public string ClientPrefix { get; set; }
        public string siteCode { get; set; }
        public int SiteCount { get; set; }
        public string siteLatitude { get; set; }
        public string siteLongitude { get; set; }
        public string Description { get; set; }
        public string sectorCode { get; set; }
        public string networkMode { get; set; }

        public string Scope { get; set; }
        public Int64 ScopeId { get; set; }
          public DateTime ReceivedOn { get; set; }
        public DateTime SubmittedOn { get; set; }
        public int STATUS { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int TesterId { get; set; }
        public string SiteAddress { get; set; }
        public string SiteType { get; set; }
        public string TesterName { get; set; }
        public string RedriveType { get; set; }
        public string RedriveReason { get; set; }
        public Int64 PWoRefid { get; set; }
        public bool isRedrive { get; set; }
        public Int64 RedriveCount { get; set; }
        public string SiteCode { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public Int64 LayerStatusId { get; set; }
        public string ClientPOC { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public string NetworkMode { get; set; }
        public string BandName { get; set; }
        public string Carrier { get; set; }
    }
}
