﻿using AirView.DBLayer.AirView.Entities;
using Library.SWI.Survey.Model;
using SWI.AirView.Common;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class Workorder
    {
        public string SurveyId { get; set; }

        public string clusterId { get; set; }
        public string clusterName { get; set; }
        public string networkModes { get; set; }
        public string networkmodename { get; set; }
        public string filename { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 ProjectId { get; set; }
        public string clusterCode { get; set; }
        public Int64 Region { get; set; }
        public string Market { get; set; }
        public string Client { get; set; }
        public Int64 State { get; set; }
        public string ClientPrefix { get; set; }
        public string siteCode { get; set; }

        public string siteLatitude { get; set; }
        public string siteLongitude { get; set; }
        public string Description { get; set; }
        public string sectorCode { get; set; }
        public string networkMode { get; set; }

        public string Scope { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Antenna { get; set; }
        public string BeamWidth { get; set; }
        public string VerticalBeamWidth { get; set; }
        public string Azimuth { get; set; }
        public string PCI { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string BandWidth { get; set; }
        public string CellId { get; set; }
        public int RFHeight { get; set; }
        public int MTilt { get; set; }
        public int ETilt { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string SiteTypeId { get; set; }
        public Int64 SiteClassId { get; set; }
        public string MRBTS { get; set; }
        public Int64 UserId { get; set; }
        public string IMEI { get; set; }
        public Int64 RevisionId { get; set; }
        public Int64 RedriveCount { get; set; }
        public double SectorLatitude { get; set; }
        public double SectorLongitude { get; set; }
        public Int64 SiteSurveyId { get; set; }
        public Int64 SiteClusterId { get; set; }

        public Int64 SectorId { get; set; }
        public List<AV_Sector> Sectors { get; set; }
        
        ///keepr
        ///
        public AV_Site Site { get; set; }

       

    }
    public class Changefoldername
    {
        public string Path { get; set; }
        public Int64 Id { get; set; }
    }
    
}
