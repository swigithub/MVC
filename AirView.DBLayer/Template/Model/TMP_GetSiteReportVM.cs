using AirView.DBLayer.Common;
using System.Collections.Generic;
using System.Data;

namespace AirView.DBLayer.Template.Model
{
    public class TMP_GetSiteReportVM
    {
        public string BaseURL { get; set; }
        public string ControlType { get; set; }

        #region PAGE_DATA
        public string Region { get; set; }
        public string City { get; set; }
        public string Site { get; set; }
        public string NetworkMode { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Scope { get; set; }
        public string SiteScheduleDate { get; set; }
        public string Sector { get; set; }

        public List<TMP_NodeSettings> NodeSettingList { get; set; }

        #endregion

        #region MAP_DATA

        public string MapType { get; set; }
        public string MapPlotKML { get; set; }
        public string KMLFilePath { get; set; }
        public List<Legends> LegendsList { get; set; }
        public List<TMP_DashboardMap> DashboardMap { get; set; }
        public string DashbardMapData { get; set; }
        public string MapMarkerData { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        #endregion

        #region TABLE_DATA

        public string Title { get; set; }
        public string SiteDataTableJSON { get; set; }
        public List<string> SiteDataListColumns { get; set; }
        public List<DataTableColumns> DataTableColumns { get; set; }

        public string IsTablePagingEnabled { get; set; }
        #endregion

        #region TABLE_MAP_DATA

        public string AzmuthData { get; set; }

        public string AzmuthDataJSON { get; set; }

        #endregion

        #region IMAGES_DATA (OOKLA)

        public List<OOKLATest> OoklaTestDataList { get; set; }

        #endregion

        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public double? TotalNoOfRecords { get; set; }

    }

    public class OOKLATest
    {
        public string OoklaTestFilePath { get; set; }
    }

    public class Legends
    {
        public string Legend { get; set; }
        public string Color { get; set; }
    }

    public class SiteData
    {
        public string Site { get; set; }
        public string SectorId { get; set; }
        public string NetworkMode { get; set; }
        public string Band { get; set; }
        public string Carrier { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Azimuth { get; set; }
        public string AngleToSite { get; set; }
    }

}
