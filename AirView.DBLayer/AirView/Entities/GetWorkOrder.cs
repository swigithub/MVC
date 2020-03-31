using AirView.DBLayer.AirView.Entities;
using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/

    public class GetWorkOrder
    {
        public GetWorkOrder()
        {
            Site = new List<GetWorkOrderSite>();
          
    }
        public List<GetWorkOrderSite> Site { get; set; }
        public string Message { get; set; }

     

    }

    public class WoStatusCheck
    {
        public Int64 SiteId { get; set; }
       // public bool? IsDelete { get; set; }
        public string WoStatus { get; set; }
        public string LicenseType { get; set; }
        public string ScheduledOn { get; set; }
    }


    public class GetWorkOrderSite
    {

        public int SiteId { get; set; }
        public string SiteCode { get; set; }
        public int ClusterId { get; set; }
        public int TesterId { get; set; }
        public string Tester { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WoRefId { get; set; }
        public string WOStatus { get; set; }
        public string ColorCode { get; set; }
        public string Scope { get; set; }
        public string Market { get; set; }
        public string ScheduledOn { get; set; }
        public string SiteTypeId { get; set; }
        public string SiteType { get; set; }
        public string ProjectId { get; set; }
        public List<GetWorkOrderSector> Sector = new List<GetWorkOrderSector>();
        public List<AD_ClusterScheduleVM> Devices = new List<AD_ClusterScheduleVM>();
    }

    public class GetWorkOrderSector
    {

        public int SectorId { get; set; }
        public string SectorCode { get; set; }

        public string NetType { get; set; }
        public int NetTypeId { get; set; }
        public string Carrier { get; set; }
        public int CarrierId { get; set; }
        public int BandId { get; set; }
        public string Band { get; set; }
        public string Scope { get; set; }
        public int ScopeId { get; set; }
        public string Antenna { get; set; }
        public string BeamWidth { get; set; }
        public string Azimuth { get; set; }
        public string PCI { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RecieverDistance { get; set; }
        public double InnerDistance { get; set; }
        public double OuterDistance { get; set; }



        public string CellId { get; set; }
        public double RFHeight { get; set; }
        public double AntennaDownTilt { get; set; }
        public double VerticalBeamwidth { get; set; }
    }


    public class GetWorkExport
    {
        public string clusterCode { get; set; }
        public string Region { get; set; }
        public string Market { get; set; }
        public string Client { get; set; }
        public string siteCode { get; set; }
        public string SiteName { get; set; }
        public string Project { get; set; }
        public string SiteType { get; set; }
        public string SiteClass { get; set; }
        public string CellId { get; set; }
        public string siteLatitude { get; set; }
        public string siteLongitude { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string sectorCode { get; set; }
        public string networkMode { get; set; }
        public string Scope { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Antenna { get; set; }
        public string RFHeight { get; set; }
        public string BeamWidth { get; set; }
        public string VBeamWidth { get; set; }
        public string AntennaDowntilt { get; set; }
        public string Azimuth { get; set; }
        public string PCI { get; set; }
        public DateTime ReceivedOn { get; set; }
    }


    public class WorkOrderSearch
    {
        public string WoRefNo { get; set; }
        public decimal SiteId { get; set; }
        public string SiteCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal ClusterId { get; set; }
        public decimal ClientId { get; set; }
        public string Description { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime ScheduledOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public string Market { get; set; }
        public string Region { get; set; }
        public string Tester { get; set; }
        public string ClientName { get; set; }
        public decimal TesterId { get; set; }
        public bool IsDownloaded { get; set; }
        public decimal Status { get; set; }
    }

    public class GetClusterSchedule
    {
        public Int64 SiteClusterId { get; set; }
        public Int64 LayerStatusId { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 ScopeId { get; set; }
        public Int64 TesterId { get; set; }
        public Int64 UserDeviceId { get; set; }
        public Int64 SequenceId { get; set; }
        public Int64 Status { get; set; }
        public bool IsActive { get; set; }
    }
}
