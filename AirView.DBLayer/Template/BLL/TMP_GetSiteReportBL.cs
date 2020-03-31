using AirView.DBLayer.Template.DAL;
using AirView.DBLayer.Template.Model;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;
using AirView.DBLayer.Common;

namespace AirView.DBLayer.Template.BLL
{


    public class TMP_GetSiteReportBL
    {
        public string ExeCustomQuery { get; set; }
        public string FilterClause { get; set; }
        public string TableName { get; set; }
        public string xAxis { get; set; }
        public string yAxis { get; set; }
        public string LSiteIdKML { get; set; }
        public string LBandIdKML { get; set; }
        public string LNetworkModeIdKML { get; set; }
        public string LCarrierKML { get; set; }

        TMP_GetSiteReportDL srd = new TMP_GetSiteReportDL();
        TMP_GetProjectScopeDL projectScopeDl = new TMP_GetProjectScopeDL();
        public TMP_GetSiteReportVM ToObject(string Filter, int NodeId = 0, int SiteId = 0, int BandId = 0, int CarrierId = 0, int NetworkModeId = 0, int ScopeId = 0, int PlotId = 0, int UserId = 0, string ControlDetailType = null, string ChartType = null, string WhereClause = null, int projectId = 0, int? page = 1, int StartIndex = 1, int EndIndex = 10, string draw = null, string start = null, string length = null, string sortColumn = null, string ColumnDir = null, string CustomQuery = null)
        {
            draw = "55";
            if (string.IsNullOrEmpty(WhereClause) || WhereClause == "()")
            {
                WhereClause = "  '' = ''  ";
            }
            string IsPagingEnable = "enable";
            DataSet ds = new DataSet();
            TMP_NodeSettingsBL NodeSettingsBL = new TMP_NodeSettingsBL();
            TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
            List<TMP_NodeSettings> NodeSetting = new List<TMP_NodeSettings>();
            NodeSettingsBL.ToList("GET_BY_NODEID", Convert.ToString(NodeId));
            NodeSetting = NodeSettingsBL.ToList("GET_BY_NODEID", NodeId.ToString());
            var MapLongitude = NodeSetting.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "longitude").FirstOrDefault();
            var MapLatitude = NodeSetting.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "latitude").FirstOrDefault();
            var MapPlotColor = NodeSetting.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "color").FirstOrDefault();

            string TemplateType = TemplatesBL.ToList("GetTemplateTypeByNodeId", NodeId.ToString()).FirstOrDefault()?.TemplateType;

          
            if (TemplateType == "dashboard")
            {
                ds = projectScopeDl.GetDs(Filter, projectId, NodeId, ScopeId, UserId, ControlDetailType, ChartType, WhereClause, ExeCustomQuery);
            }
            else if(TemplateType == "report")
            {
                if (!string.IsNullOrEmpty(FilterClause))
                {
                    if(FilterClause == "()" || FilterClause == null)
                    {
                        FilterClause = "";
                    }
                    WhereClause = FilterClause;
                }
                ds = projectScopeDl.GetDs(Filter, projectId, NodeId, ScopeId, UserId, ControlDetailType, ChartType, WhereClause, ExeCustomQuery, "report", SiteId, BandId, NetworkModeId, CarrierId, TableName, MapPlotColor?.Value, MapLongitude?.Value, MapLatitude?.Value, LSiteIdKML, LBandIdKML, LNetworkModeIdKML, LCarrierKML);
                // ds = srd.GetDs(Filter, NodeId, SiteId, BandId, CarrierId, NetworkModeId, ScopeId, PlotId, UserId, ControlDetailType, ChartType, WhereClause);
            }

            TMP_GetSiteReportVM siteRpt = new TMP_GetSiteReportVM();

            if (Filter == "GET_PAGE_DATA")
            {
                DataTable ReportData = ds.Tables[0];
                DataTable NodeSettingsDataLst = ds.Tables[1];

                if (NodeSettingsDataLst.Rows.Count > 0)
                {
                    siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                }

                if (ReportData.Rows.Count > 0)
                {
                    siteRpt.NetworkMode = ReportData.Rows[0]["Network Mode"].ToString();
                    siteRpt.Band = ReportData.Rows[0]["Band"].ToString();
                    siteRpt.Site = ReportData.Rows[0]["Site"].ToString();
                    //siteRpt.Region = ReportData.Rows[0]["Region"].ToString();
                    //siteRpt.Scope = ReportData.Rows[0]["Scope"].ToString();
                    //siteRpt.City = ReportData.Rows[0]["City"].ToString();
                    //siteRpt.Carrier = ReportData.Rows[0]["Carrier"].ToString();
                    //siteRpt.Sector = ReportData.Rows[0]["Sector"].ToString();
                    //siteRpt.SiteScheduleDate = ReportData.Rows[0]["SiteScheduleDate"].ToString();
                }
            }
            else if (Filter == "GET_MAP_DATA")
            {

                if(TemplateType == "dashboard")
                {
                    DataTable NodeSettingsDataLst = ds.Tables[1];
                    var MapData = ds.Tables[0];
                    siteRpt.DashbardMapData = GetTableJSON(MapData);
                    siteRpt.MapMarkerData = new JavaScriptSerializer().Serialize(MapData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList());
                    if (NodeSettingsDataLst.Rows.Count > 0)
                    {
                        siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                    }
                }
                else
                {
                    int NoOfTablesInDS = ds.Tables.Count;
                    DataTable PlotData = ds.Tables[0];
                    DataTable AzmuthSummary = ds.Tables[1];
                    DataTable LegendsData = ds.Tables[2];
                    DataTable NodeSettingsDataLst = ds.Tables[3];
                   /* foreach (DataRow row in AzmuthSummary.Rows)
                    {
                        row["SectorColor"] = MapPlotColor?.Value;
                    }*/
                    if (PlotData.Rows.Count > 0)
                    {
                        siteRpt.MapPlotKML = PlotData.Rows[0]["Plot"].ToString();
                        siteRpt.MapType = PlotData.Rows[0]["PlotName"].ToString();
                        siteRpt.Site = PlotData.Rows[0]["SiteCode"].ToString();
                    }

                    if (AzmuthSummary.Rows.Count > 0)
                    {
                        siteRpt.AzmuthData = GetTableJSON(AzmuthSummary);
                    }

                    if (LegendsData.Rows.Count > 0)
                    {
                        siteRpt.LegendsList = LegendsData.ToList<Legends>();
                    }

                    if (NodeSettingsDataLst.Rows.Count > 0)
                    {
                        siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                    }
                }
                siteRpt.Latitude = MapLatitude?.Value != "-1" ? MapLatitude?.Value : "0";
                siteRpt.Longitude = MapLongitude?.Value != "-1" ? MapLongitude?.Value : "0";

            }
            else if (Filter == "GET_TABLE_DATA")
            {

                DataTable SiteTestSummary = ds.Tables[0];
                DataTable NodeSettingsDataLst = ds.Tables[1];

                if (NodeSettingsDataLst.Rows.Count > 0)
                {
                    siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                    length = siteRpt.NodeSettingList.FirstOrDefault(x => x.MappedId == "PAGE_SIZE")?.Value;
                    IsPagingEnable = siteRpt.NodeSettingList.FirstOrDefault(x => x.MappedId == "IS_PAGING_ENABLE")?.Value?.ToLower();
                    if (IsPagingEnable == "enable")
                    { IsPagingEnable = "true"; }
                    else
                    { IsPagingEnable = "false"; }
                }

                int? tableRowsCount = SiteTestSummary.Rows.Count;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var ListSummaryData = SiteTestSummary.AsEnumerable();
                int? take = length != null ? Convert.ToInt32(length) : 0;
                if (IsPagingEnable == "false")
                    take = tableRowsCount;
                var SummaryData = ListSummaryData.Skip(skip).Take(take.Value).ToList();

                siteRpt.TotalNoOfRecords = tableRowsCount;

                siteRpt.SiteDataListColumns = SiteTestSummary.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                siteRpt.SiteDataTableJSON = DataRowsToJSON(SummaryData);


            }
            else if (Filter == "GET_TABLE_WITH_MAP_DATA")
            {
                DataTable AzmuthSummary = new DataTable();
                DataTable SiteTestSummary = new DataTable();
                DataTable NodeSettingsDataLst = new DataTable();

                if (TemplateType == "dashboard")
                {
                    SiteTestSummary = ds.Tables[0];
                    NodeSettingsDataLst = ds.Tables[1];
                }
                else
                {
                    AzmuthSummary = ds.Tables[0];
                    SiteTestSummary = ds.Tables[1];
                    NodeSettingsDataLst = ds.Tables[2];

                    //foreach (DataRow row in AzmuthSummary.Rows)
                    //{
                    //    row["SectorColor"] = MapPlotColor?.Value;
                    //}
                }

                int? d = AzmuthSummary.Rows.Count;
                if (AzmuthSummary.Rows.Count > 0)
                {
                    siteRpt.AzmuthData = GetTableJSON(AzmuthSummary);
                }

                
                if (NodeSettingsDataLst.Rows.Count > 0)
                {
                    siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                    length = siteRpt.NodeSettingList.FirstOrDefault(x => x.MappedId == "PAGE_SIZE")?.Value;
                    IsPagingEnable = siteRpt.NodeSettingList.FirstOrDefault(x => x.MappedId == "IS_PAGING_ENABLE")?.Value?.ToLower();
                    if (IsPagingEnable == "enable")
                    { IsPagingEnable = "true"; }
                    else
                    { IsPagingEnable = "false"; }
                }

                int? tableRowsCount = SiteTestSummary.Rows.Count;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var ListSummaryData = SiteTestSummary.AsEnumerable();
                int? take = length != null ? Convert.ToInt32(length) : 0;
                if (IsPagingEnable == "false")
                    take = tableRowsCount;
                var SummaryData = ListSummaryData.Skip(skip).Take(take.Value).ToList();

                siteRpt.TotalNoOfRecords = SiteTestSummary.Rows.Count;

                siteRpt.SiteDataListColumns = SiteTestSummary.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                siteRpt.SiteDataTableJSON = DataRowsToJSON(SummaryData);
                siteRpt.Latitude = MapLatitude?.Value;
                siteRpt.Longitude = MapLongitude?.Value;
            }
            else if (Filter == "GET_OOKLA_DATA")
            {
                DataTable SiteTestSummary = ds.Tables[0];
                DataTable NodeSettingsDataLst = ds.Tables[1];


                if (SiteTestSummary.Rows.Count > 0)
                {
                    siteRpt.OoklaTestDataList = SiteTestSummary.ToList<OOKLATest>();
                }

                if (NodeSettingsDataLst.Rows.Count > 0)
                {
                    siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
                }
            }
            else if (Filter == "GET_CHART_DATA")
            {
                DataTable ChartSummary = ds.Tables[0];
                DataTable NodeSettingsDataLst = ds.Tables[1];
                string x_axis = xAxis;
                string y_axis = yAxis;
                //System.Data.DataView view = new System.Data.DataView(ChartSummary);
                //System.Data.DataTable selected = view.ToTable(false, x_axis, yAxis);
                DataTable tb = new DataTable();
                tb.Columns.Add("x");
                tb.Columns.Add("y");
                if(x_axis == y_axis)
                {
                    foreach (var item in ChartSummary.Select())
                    {
                        tb.Rows.Add(item[x_axis], item[x_axis]);
                    }
                }
                else
                {
                    foreach (var item in ChartSummary.Select())
                    {
                        tb.Rows.Add(item[x_axis], item[y_axis]);
                    }
                }

             //   tb.Columns[x_axis].ColumnName = "x";
              //  tb.Columns[y_axis].ColumnName = "y";
                if (ChartSummary.Rows.Count > 0)
                {
                    siteRpt.SiteDataListColumns = tb.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                    tb.DefaultView.Sort = "x ASC";

                    siteRpt.SiteDataTableJSON = GetTableJSON(tb);
                }
                siteRpt.NodeSettingList = NodeSettingsDataLst.ToList<TMP_NodeSettings>();
            }
            else
            {
                siteRpt = null;
            }


            return siteRpt;
        }


        public string GetTableJSON(DataTable dt)
        {
            string tableJSON = string.Empty;

            if (dt.Rows.Count > 0)
            {

                var JSONString = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    JSONString.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JSONString.Append("{");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j < dt.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                            }
                            else if (j == dt.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                            }
                        }
                        if (i == dt.Rows.Count - 1)
                        {
                            JSONString.Append("}");
                        }
                        else
                        {
                            JSONString.Append("},");
                        }
                    }
                    JSONString.Append("]");
                }

                tableJSON = JSONString.ToString();

            }

            return tableJSON;
        }

        public string GetTableJSON(List<DataRow> dt)
        {
            string tableJSON = string.Empty;

            if (dt.Count > 0)
            {
               
                var JSONString = new StringBuilder();
                if (dt.Count > 0)
                {
                    JSONString.Append("[");
                    for (int i = 0; i < dt.Count; i++)
                    {
                       
                        JSONString.Append("{");
                        for (int j = 0; j < dt[j].Table.Columns.Count - 1; j++)
                        {
                            var totalColumn = dt[j].Table.Columns.Count - 1;
                            if (j < dt[j].Table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + Convert.ToString(dt[i].Table.Columns[j].ColumnName) + "\":" + "\"" + Convert.ToString(dt[i][j]) + "\",");
                            }
                            else if (j == dt[j].Table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + Convert.ToString(dt[i].Table.Columns[j].ColumnName) + "\":" + "\"" + Convert.ToString(dt[i][j]) + "\"");
                            }
                        }
                        if (i == dt.Count - 1)
                        {
                            JSONString.Append("}");
                        }
                        else
                        {
                            JSONString.Append("},");
                        }
                    }
                    JSONString.Append("]");
                }

                tableJSON = JSONString.ToString();

            }

            return tableJSON;
        }

        public string DataRowsToJSON(List<DataRow> rw)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = (Int32)900000000;
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in rw)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in row.Table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}
