using System;
using System.Collections.Generic;
using System.Data;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;


namespace SWI.Libraries.AirView.BLL
{
    public class DashboardBL
    {
        public  DashboardVM GetDashboardVM(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, DataTable dtCiteies, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string CountryId, string Client, string Scopes, string Markets, string Projects)
        {
            try
            {
                DashboardDL db = new DashboardDL();
                DataSet ds = db.GetDashboardData(ParentFilter, ChildFilter, fromDate, toDate, dtCiteies, Panel1Option, Panel1Value, Panel2Option, Panel2Value, UserId, CountryId, Client, Scopes, Markets,Projects);
                // DataTable TotalSites =;
                DataTable DashboardData = ds.Tables[1];
                DashboardVM vm = new DashboardVM();

                ClientSitesVM ClientSites = new ClientSitesVM();
                ClientSites.Sites = GetSitesFromTable(ds.Tables[0]);

                vm.ClientSites = ClientSites;

                ClientSites.Markers = GetSitesFromTable(ds.Tables[4]);



                DashboardStatusVM SiteStatuses = new DashboardStatusVM();

                SiteStatuses.TotalSites = Convert.ToInt32(DashboardData.Rows[0]["TotalSites"] is DBNull ? 0 : DashboardData.Rows[0]["TotalSites"]);
                SiteStatuses.PendingSites = Convert.ToInt32(DashboardData.Rows[0]["PendingSites"] is DBNull ? 0 : DashboardData.Rows[0]["PendingSites"]);
                SiteStatuses.InProcessSites = Convert.ToInt32(DashboardData.Rows[0]["InProcessSites"] is DBNull ? 0 : DashboardData.Rows[0]["InProcessSites"]);
                SiteStatuses.CompletedSites = Convert.ToInt32(DashboardData.Rows[0]["CompletedSites"] is DBNull ? 0 : DashboardData.Rows[0]["CompletedSites"]);
                SiteStatuses.DriveCompleted = Convert.ToInt32(DashboardData.Rows[0]["DriveCompletedSites"] is DBNull ? 0 : DashboardData.Rows[0]["DriveCompletedSites"]);
                SiteStatuses.PendingWithIssues = Convert.ToInt32(DashboardData.Rows[0]["PendingWithIssuesSites"] is DBNull ? 0 : DashboardData.Rows[0]["PendingWithIssuesSites"]);
                SiteStatuses.InProgress = Convert.ToInt32(DashboardData.Rows[0]["InProgress"] is DBNull ? 0 : DashboardData.Rows[0]["InProgress"]);
                SiteStatuses.ReportSubmitted = Convert.ToInt32(DashboardData.Rows[0]["ReportSubmitted"] is DBNull ? 0 : DashboardData.Rows[0]["ReportSubmitted"]);

                vm.SiteStatuses = SiteStatuses;

                List<RegionsVM> regionVm = GetRegionalSitesFromTable(ds.Tables[2]);
                List<RegionsVM> testerVm = GetTesterSitesFromTable(ds.Tables[3]);

                //  List<RegionsVM> driveTesterVm = GetTesterSitesFromTable(ds.Tables[3]);// GetDriveTesterSitesFromTable(ds.Tables[4]);

                vm.Regions = regionVm;
                vm.TesterSites = testerVm;
                vm.DriveTesterSites = testerVm;


                return vm;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public DashboardVM GetProjectDashboardVM(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, DataTable dtCiteies, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string CountryId, string Client, string Scopes, string Markets)
        {
            try
            {
                DashboardDL db = new DashboardDL();
                DataSet ds = db.GetProjectDashboardData(ParentFilter, ChildFilter, fromDate, toDate, dtCiteies, Panel1Option, Panel1Value, Panel2Option, Panel2Value, UserId, CountryId, Client, Scopes, Markets);

                DashboardVM vm = new DashboardVM();

                DataTable DashboardData = ds.Tables[0];
                DataTable dtSiteWO = ds.Tables[1];


                DashboardStatusVM SiteStatuses = new DashboardStatusVM();
                SiteStatuses.TotalSites = Convert.ToInt32(DashboardData.Rows[0]["TotalSites"] is DBNull ? 0 : DashboardData.Rows[0]["TotalSites"]);
                SiteStatuses.PendingSites = Convert.ToInt32(DashboardData.Rows[0]["PendingSites"] is DBNull ? 0 : DashboardData.Rows[0]["PendingSites"]);
                SiteStatuses.InProcessSites = Convert.ToInt32(DashboardData.Rows[0]["InProcessSites"] is DBNull ? 0 : DashboardData.Rows[0]["InProcessSites"]);
                SiteStatuses.CompletedSites = Convert.ToInt32(DashboardData.Rows[0]["CompletedSites"] is DBNull ? 0 : DashboardData.Rows[0]["CompletedSites"]);
                SiteStatuses.DriveCompleted = Convert.ToInt32(DashboardData.Rows[0]["DriveCompletedSites"] is DBNull ? 0 : DashboardData.Rows[0]["DriveCompletedSites"]);
                SiteStatuses.PendingWithIssues = Convert.ToInt32(DashboardData.Rows[0]["PendingWithIssuesSites"] is DBNull ? 0 : DashboardData.Rows[0]["PendingWithIssuesSites"]);
                SiteStatuses.InProgress = Convert.ToInt32(DashboardData.Rows[0]["InProgress"] is DBNull ? 0 : DashboardData.Rows[0]["InProgress"]);
                SiteStatuses.ReportSubmitted = Convert.ToInt32(DashboardData.Rows[0]["ReportSubmitted"] is DBNull ? 0 : DashboardData.Rows[0]["ReportSubmitted"]);
                vm.SiteStatuses = SiteStatuses;


                if (dtSiteWO != null && dtSiteWO.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSiteWO.Rows.Count; i++)
                    {
                        SitesVM sv = new SitesVM();                 
                        sv.SubmittedOn = Convert.ToDateTime(dtSiteWO.Rows[i]["Date"].ToString());          
                        sv.SiteCount = int.Parse(dtSiteWO.Rows[i]["SiteCount"].ToString());
                        vm.lstSiteWO.Add(sv);
                    }
                }
                return vm;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public DashboardVM GetDashboardSiteVM(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string FilterOption, int Offset, int PageSize,string CountryId, string Client=null, string Scopes=null, string Projects = null)
        {
            try
            {
                DashboardDL db = new DashboardDL();
                DataSet ds = db.GetDashboardSites(ParentFilter, ChildFilter, fromDate, toDate, Panel1Option, Panel1Value, Panel2Option, Panel2Value, UserId, FilterOption, Offset, PageSize, CountryId, Client,Scopes,Projects);
                // DataTable TotalSites =;
                DataTable DashboardData = ds.Tables[0];
                DashboardVM vm = new DashboardVM();

                ClientSitesVM ClientSites = new ClientSitesVM();
                ClientSites.Sites = GetSitesFromTable(ds.Tables[0]);

                vm.ClientSites = ClientSites;
                if (ds.Tables.Count == 2)
                {
                    DataTable Count = ds.Tables[1];
                    vm.Count = (!string.IsNullOrEmpty(Count.Rows[0]["Count"].ToString())) ? Convert.ToInt32(Count.Rows[0]["Count"].ToString()) : 0;
                }

                return vm;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SitesVM> GetSitesFromTable(DataTable TotalSites)
        {
            DirectoryHandler dh = new DirectoryHandler();
            List<SitesVM> lstSites = new List<SitesVM>();
            string ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8);
            string AppPath = ApplicationPath.Remove(ApplicationPath.Length - 24);
            string Profile = ApplicationPath.Remove(ApplicationPath.Length - 24);

            if (TotalSites.Rows.Count > 0)
            {
                SitesVM loSite;
                for (int i = 0; i < TotalSites.Rows.Count; i++)
                {
                    loSite = new SitesVM();
                    loSite.SiteId = Convert.ToInt64(TotalSites.Rows[i]["SiteId"]);
                    loSite.SiteCode = TotalSites.Rows[i]["SiteCode"].ToString();
                    loSite.ClusterId = TotalSites.Rows[i]["ClusterId"].ToString();
                    if (TotalSites.Columns.Contains("Scope"))
                    {
                        loSite.Scope = TotalSites.Rows[i]["Scope"].ToString();
                    }
                    if (TotalSites.Columns.Contains("ScopeId"))
                    {
                        loSite.ScopeId = Convert.ToInt64(TotalSites.Rows[i]["ScopeId"].ToString());
                    }




                    loSite.Latitude = (!string.IsNullOrEmpty(TotalSites.Rows[i]["Latitude"].ToString())) ? Convert.ToDouble(TotalSites.Rows[i]["Latitude"]) : 0;
                    loSite.Longitude = (!string.IsNullOrEmpty(TotalSites.Rows[i]["Longitude"].ToString())) ? Convert.ToDouble(TotalSites.Rows[i]["Longitude"]) : 0;
                    loSite.Tester = TotalSites.Rows[i]["Tester"] == DBNull.Value ? "" : TotalSites.Rows[i]["Tester"].ToString();
                    loSite.TesterId = (TotalSites.Columns.Contains("TesterId")) ? TotalSites.Rows[i]["TesterId"] == DBNull.Value ? 0 : Convert.ToInt32(TotalSites.Rows[i]["TesterId"]) : 0;
                    if (!string.IsNullOrEmpty(TotalSites.Rows[i]["SubmittedOn"].ToString()))
                    {
                        loSite.SubmittedOn = Convert.ToDateTime(TotalSites.Rows[i]["SubmittedOn"]);

                    }
                    if (!string.IsNullOrEmpty(TotalSites.Rows[i]["ReceivedOn"].ToString()))
                    {
                        loSite.ReceivedOn = Convert.ToDateTime(TotalSites.Rows[i]["ReceivedOn"]);

                    }
                    loSite.WoRefNo = TotalSites.Rows[i]["WoRefNo"].ToString();
                    if (TotalSites.Rows[i]["AssignedOn"] != DBNull.Value)
                    { loSite.AssignedOn = Convert.ToDateTime(TotalSites.Rows[i]["AssignedOn"]); }
                    if (TotalSites.Rows[i]["CompletedOn"] != DBNull.Value)
                    { loSite.CompletedOn = Convert.ToDateTime(TotalSites.Rows[i]["CompletedOn"]); }



                    // break point
                    if (TotalSites.Columns.Contains("DriveCompletedOn"))
                    {
                        if (TotalSites.Rows[i]["DriveCompletedOn"] != DBNull.Value)
                        {
                            loSite.DriveCompletedOn = Convert.ToDateTime(TotalSites.Rows[i]["DriveCompletedOn"]);
                        }
                    }

                    if (TotalSites.Rows[i]["ScheduledOn"] != DBNull.Value)
                    { loSite.ScheduledOn = Convert.ToDateTime(TotalSites.Rows[i]["ScheduledOn"]); }

                    string TesterPicture = TotalSites.Rows[i]["Status"].ToString();

                    loSite.Status = TotalSites.Rows[i]["Status"].ToString();
                    loSite.StatusName = TotalSites.Rows[i]["StatusName"].ToString();
                    loSite.StatusKeyCode = TotalSites.Rows[i]["StatusKeyCode"].ToString();

                    if (TotalSites.Columns.Contains("IsDownloaded") && !string.IsNullOrEmpty(TotalSites.Rows[i]["IsDownloaded"].ToString()))
                    {
                        loSite.IsDownloaded = Convert.ToBoolean(TotalSites.Rows[i]["IsDownloaded"].ToString());

                    }
                    if (TotalSites.Columns.Contains("ReportSubmittedOn") && !string.IsNullOrEmpty(TotalSites.Rows[i]["ReportSubmittedOn"].ToString()))
                    {
                        loSite.ReportSubmittedOn = Convert.ToDateTime(TotalSites.Rows[i]["ReportSubmittedOn"].ToString());

                    }

                    //ReportSubmittedOn
                    loSite.Market = TotalSites.Rows[i]["Market"].ToString();
                    loSite.Region = TotalSites.Rows[i]["Region"].ToString();
                    loSite.Client = TotalSites.Rows[i]["ClientName"].ToString();
                    if (TotalSites.Columns.Contains("ClientPrefix") && !string.IsNullOrEmpty(TotalSites.Rows[i]["ClientPrefix"].ToString()))
                    {
                        loSite.ClientPrefix = TotalSites.Rows[i]["ClientPrefix"].ToString();
                    }
                    if (TotalSites.Columns.Contains("IsActive") && !string.IsNullOrEmpty(TotalSites.Rows[i]["IsActive"].ToString()))
                    {
                        loSite.IsActive = Convert.ToBoolean(TotalSites.Rows[i]["IsActive"].ToString());

                    }

                    //

                    loSite.ZIndex = 1;

                    string cnt = string.Empty;
                    if (loSite.StatusKeyCode == "N/A")
                    {
                        Profile = "/Content/Images/Profile/home-" + loSite.TesterId + ".png";
                        //  string   NewProfilePath = ProfilePath + Profile;
                        if (dh.FileExist(AppPath + Profile))
                        {
                            loSite.MarkerImagePath = Profile;
                        }
                        else
                        {
                            loSite.MarkerImagePath = "/Content/Images/Profile/home-default.png";
                        }

                        cnt = "<b>Tester: </b> " + loSite.Tester + "</br>";

                    }
                    else
                    {
                        if (loSite.StatusKeyCode == "PENDING_SCHEDULED")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/PENDING_SCHEDULED.png";//"http://maps.google.com/mapfiles/ms/icons/yellow-dot.png";
                            loSite.MarkerTitle = "PENDING_SCHEDULED";

                        }
                        else if (loSite.StatusKeyCode == "COMPLETED")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/COMPLETED_MARKER.png";// "http://maps.google.com/mapfiles/ms/icons/green-dot.png";
                            loSite.MarkerTitle = "COMPLETED";

                        }
                        else if (loSite.StatusKeyCode == "IN_PROGRESS")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/IN_PROGRESS_MARKER.png";
                            loSite.MarkerTitle = "IN_PROGRESS";

                        }
                        else if (loSite.StatusKeyCode == "REPORT_SUBMITTED")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/REPORT_SUBMITTED_MARKER.png";// "http://maps.google.com/mapfiles/ms/icons/green-dot.png";
                            loSite.MarkerTitle = "REPORT_SUBMITTED";

                        }
                        else if (loSite.StatusKeyCode == "DRIVE_COMPLETED")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/DRIVE_COMPLETED_MARKER.png";// "http://maps.google.com/mapfiles/ms/icons/green-dot.png";
                            loSite.MarkerTitle = "DRIVE_COMPLETED";


                        }
                        else if (loSite.StatusKeyCode == "PENDING_WITH_ISSUE")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/PENDING_WITH_ISSUE_MARKER.png"; //"http://maps.google.com/mapfiles/ms/icons/red-dot.png";
                            loSite.MarkerTitle = "PENDING_WITH_ISSUE";


                        }
                        else if (loSite.StatusKeyCode == "SCHEDULED")
                        {
                            loSite.MarkerImagePath = "/Content/Images/Common/SCHEDULED_MARKER.png";
                            loSite.MarkerTitle = "SCHEDULED";

                            if (!string.IsNullOrEmpty(TotalSites.Rows[i]["TesterPicture"].ToString()))
                            {
                                string imgUrl = TotalSites.Rows[i]["TesterPicture"].ToString().Replace("u-", "thumb-");
                                if (imgUrl == "/Content/Images/Profile/Default.svg")
                                {
                                    loSite.MarkerImagePath = "/Content/Images/Profile/defaultThumb.jpg";
                                }
                                else
                                {
                                    loSite.MarkerImagePath = imgUrl;
                                }
                            }

                        }
                        else
                        {
                            loSite.MarkerImagePath = "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png";
                            loSite.MarkerTitle = loSite.SiteCode;

                        }

                        //cnt = "<div>" + "<b>Site Code: </b> " + loSite.SiteCode + "</br>" + "<b>Region: </b>" + loSite.Region + "</br>" + "<b>Market: </b> " + loSite.Market + "</br>" + "<b>Tester: </b> " + loSite.Tester + "</br>";
                        //cnt = (loSite.SubmittedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00") ? cnt + "<b>Submitted: </b> " + loSite.SubmittedOn.ToString("MM/dd/yyyy HH:mm") + "</br>" : cnt + "<b>Submitted: </b> " + loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        //if (loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        //{
                        //    cnt = cnt + "<b>Scheduled: </b> " + loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        //}
                        //else
                        //{
                        //    cnt = cnt + "<b>Scheduled: </b> " + "</br>";
                        //}
                        //if (loSite.CompletedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        //{
                        //    cnt = cnt + "<b>Completed: </b> " + loSite.CompletedOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        //}
                        //else
                        //{
                        //    cnt = cnt + "<b>Completed: </b> " + "</br>";
                        //}
                        //cnt = cnt + "</div>";

                        //---------------------------------------------
                        cnt = "<div>" +
                               "<b>Site Code: </b> " + loSite.SiteCode + "</br>" +
                               "<b>Site Status: </b> " + loSite.StatusName + "</br>" +
                               "<b>Site Type: </b> " + loSite.SiteType + "</br>" +
                               "<b>Scope: </b> " + loSite.Scope + "</br>" +
                               "<b>Region: </b>" + loSite.Region + "</br>" +
                               "<b>Market: </b> " + loSite.Market + "</br>" +
                               "<b>Tester: </b> " + loSite.Tester + "</br>";

                        /*Received Date Start*/
                        if (loSite.ReceivedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        {
                            cnt = cnt + "<b>Received: </b> " + loSite.ReceivedOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        }
                        else
                        {
                            cnt = cnt + "<b>Received: </b> " + "</br>";
                        }
                        /*Received Date End*/
                        cnt = (loSite.SubmittedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00") ? cnt + "<b>Submitted: </b> " + loSite.SubmittedOn.ToString("MM/dd/yyyy HH:mm") + "</br>" : cnt + "<b>Submitted: </b> " + loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") + "</br>";

                        if (loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        {
                            cnt = cnt + "<b>Scheduled: </b> " + loSite.ScheduledOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        }
                        else
                        {
                            cnt = cnt + "<b>Scheduled: </b> " + "</br>";
                        }
                        var cd = loSite.DriveCompletedOn.ToString("MM/dd/yyyy HH:mm");
                        /*Drive Completed Date Start*/
                        if (loSite.DriveCompletedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        {
                            cnt = cnt + "<b>Drive Completed: </b> " + loSite.DriveCompletedOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        }
                        else
                        {
                            cnt = cnt + "<b>Drive Completed: </b> " + "</br>";
                        }
                        /*Drive Completed Date End*/

                        /*Report Submitted Date Start*/
                        if (loSite.ReportSubmittedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        {
                            cnt = cnt + "<b>Report Submitted: </b> " + loSite.ReportSubmittedOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        }
                        else
                        {
                            cnt = cnt + "<b>Report Submitted: </b> " + "</br>";
                        }
                        /*Report Submitted Date End*/
                        if (loSite.CompletedOn.ToString("MM/dd/yyyy HH:mm") != "01/01/0001 00:00")
                        {
                            cnt = cnt + "<b>Completed: </b> " + loSite.CompletedOn.ToString("MM/dd/yyyy HH:mm") + "</br>";
                        }
                        else
                        {
                            cnt = cnt + "<b>Completed: </b> " + "</br>";
                        }
                        cnt = cnt + "</div>";

                    }


                    loSite.InfoWindowContent = cnt;

                    lstSites.Add(loSite);
                }
            }
            return lstSites;
        }

        public List<RegionsVM> GetRegionalSitesFromTable(DataTable TotalSites)
        {
            List<RegionsVM> lstSites = new List<RegionsVM>();

            if (TotalSites.Rows.Count > 0)
            {
                RegionsVM loSite;
                for (int i = 0; i < TotalSites.Rows.Count; i++)
                {
                    loSite = new RegionsVM();
                    loSite.ID = Convert.ToInt32(TotalSites.Rows[i]["RegionId"]);
                    loSite.RegionName = TotalSites.Rows[i]["Region"].ToString();
                    loSite.TotalSites = Convert.ToInt32(TotalSites.Rows[i]["RegionTotalSites"]);
                    loSite.PendingSites = Convert.ToInt32(TotalSites.Rows[i]["RegionPendingSites"]);
                    loSite.InProcessSites = Convert.ToInt32(TotalSites.Rows[i]["RegionInProcessSites"]);
                    loSite.CompletedSites = Convert.ToInt32(TotalSites.Rows[i]["RegionCompletedSites"]);
                    loSite.DriveCompleted = Convert.ToInt32(TotalSites.Rows[i]["RegionDriveCompletedSites"]);
                    loSite.PendingWithIssues = Convert.ToInt32(TotalSites.Rows[i]["RegionPendingWithIssuesSites"]);
                    loSite.InProgress = Convert.ToInt32(TotalSites.Rows[i]["RegionInProgress"]);
                    loSite.ReportSubmited = Convert.ToInt32(TotalSites.Rows[i]["RegionReportSubmitted"]);
                    lstSites.Add(loSite);
                }
            }
            return lstSites;
        }
        public List<RegionsVM> GetTesterSitesFromTable(DataTable dtTestersites)
        {
            List<RegionsVM> lstSites = new List<RegionsVM>();

            if (dtTestersites.Rows.Count > 0)
            {
                RegionsVM loSite;
                for (int i = 0; i < dtTestersites.Rows.Count; i++)
                {
                    loSite = new RegionsVM();
                    loSite.ID = Convert.ToInt64(dtTestersites.Rows[i]["TesterId"]);
                    loSite.RegionName = dtTestersites.Rows[i]["TesterName"].ToString();
                    loSite.TesterImage = dtTestersites.Rows[i]["Picture"].ToString();
                    loSite.TotalSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterTotalSites"]);
                    loSite.PendingSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingSites"]);
                    loSite.InProcessSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProcessSites"]);
                    loSite.CompletedSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterCompletedSites"]);
                    loSite.DriveCompleted = Convert.ToInt32(dtTestersites.Rows[i]["TesterDriveCompletedSites"]);
                    loSite.PendingWithIssues = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingWithIssuesSites"]);
                    loSite.DtWoCount = Convert.ToInt32(dtTestersites.Rows[i]["DtWoCount"]);
                    loSite.InProgress = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProgress"]);
                    loSite.ReportSubmited = Convert.ToInt32(dtTestersites.Rows[i]["TesterReportSubmitted"]);

                    lstSites.Add(loSite);
                }
            }
            return lstSites;
        }

        public List<RegionsVM> GetDriveTesterSitesFrmomTable(DataTable dtTestersites)
        {
            List<RegionsVM> lstSites = new List<RegionsVM>();

            if (dtTestersites.Rows.Count > 0)
            {
                RegionsVM loSite;
                for (int i = 0; i < dtTestersites.Rows.Count; i++)
                {
                    loSite = new RegionsVM();
                    loSite.ID = Convert.ToInt64(dtTestersites.Rows[i]["TesterId"]);
                    loSite.RegionName = dtTestersites.Rows[i]["TesterName"].ToString();
                    loSite.TesterImage = dtTestersites.Rows[i]["Picture"].ToString();
                    loSite.TotalSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterTotalSites"]);
                    loSite.PendingSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingSites"]);
                    loSite.InProcessSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProcessSites"]);
                    loSite.CompletedSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterCompletedSites"]);
                    loSite.DriveCompleted = Convert.ToInt32(dtTestersites.Rows[i]["TesterDriveCompletedSites"]);
                    loSite.PendingWithIssues = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingWithIssuesSites"]);
                    loSite.InProgress = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProgress"]);
                    loSite.ReportSubmited = Convert.ToInt32(dtTestersites.Rows[i]["TesterReportSubmitted"]);
                    lstSites.Add(loSite);
                }
            }
            return lstSites;
        }

        public List<RegionsVM> GetPartialRegionalSites(string ParentFilter, string ChildFilter, DateTime StartDate, DateTime EndDate, string RegionFilter, Int64 UserId, Int64 FilterValue, string FilterType2, string FilterValue2)
        {
            List<RegionsVM> lstRegions = new List<RegionsVM>();
            try
            {
                DashboardDL db = new DashboardDL();
                DataTable TotalSites = db.GetPartialRegionalSites(ParentFilter, ChildFilter, StartDate, EndDate, RegionFilter, UserId, FilterValue, FilterType2, FilterValue2);

                if (TotalSites.Rows.Count > 0)
                {
                    RegionsVM loRegion;
                    for (int i = 0; i < TotalSites.Rows.Count; i++)
                    {
                        loRegion = new RegionsVM();
                        loRegion.RegionName = TotalSites.Rows[i]["Region"].ToString();
                        loRegion.ID = Convert.ToInt64(TotalSites.Rows[i]["RegionId"].ToString());
                        loRegion.TotalSites = Convert.ToInt32(TotalSites.Rows[i]["RegionTotalSites"]);
                        loRegion.PendingSites = Convert.ToInt32(TotalSites.Rows[i]["RegionPendingSites"]);
                        loRegion.InProcessSites = Convert.ToInt32(TotalSites.Rows[i]["RegionInProcessSites"]);
                        loRegion.CompletedSites = Convert.ToInt32(TotalSites.Rows[i]["RegionCompletedSites"]);
                        loRegion.DriveCompleted = Convert.ToInt32(TotalSites.Rows[i]["RegionDriveCompletedSites"]);
                        loRegion.PendingWithIssues = Convert.ToInt32(TotalSites.Rows[i]["RegionPendingWithIssuesSites"]);
                        loRegion.InProgress = Convert.ToInt32(TotalSites.Rows[i]["RegionInProgress"]);
                        loRegion.ReportSubmited = Convert.ToInt32(TotalSites.Rows[i]["RegionReportSubmitted"]);
                        lstRegions.Add(loRegion);
                    }
                }
                return lstRegions;
            }
            catch (Exception)
            {


            }

            return lstRegions;
        }

        public List<RegionsVM> GetPartialDriveTesterSites(string ParentFilter, string ChildFilter, DateTime StartDate, DateTime EndDate, string RegionFilter, Int64 UserId, Int64 FilterValue, string FilterType2, string FilterValue2)
        {
            List<RegionsVM> lstSites = new List<RegionsVM>();
            try
            {
                DashboardDL db = new DashboardDL();
                DataTable dtTestersites = db.GetPartialRegionalSites(ParentFilter, ChildFilter, StartDate, EndDate, RegionFilter, UserId, FilterValue, FilterType2, FilterValue2);

                if (dtTestersites.Rows.Count > 0)
                {
                    RegionsVM loSite;
                    for (int i = 0; i < dtTestersites.Rows.Count; i++)
                    {
                        loSite = new RegionsVM();
                        loSite.ID = Convert.ToInt64(dtTestersites.Rows[i]["TesterId"]);
                        loSite.RegionName = dtTestersites.Rows[i]["TesterName"].ToString();
                        loSite.TesterImage = dtTestersites.Rows[i]["Picture"].ToString();
                        loSite.TotalSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterTotalSites"]);
                        loSite.PendingSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingSites"]);
                        loSite.InProcessSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProcessSites"]);
                        loSite.CompletedSites = Convert.ToInt32(dtTestersites.Rows[i]["TesterCompletedSites"]);
                        loSite.DriveCompleted = Convert.ToInt32(dtTestersites.Rows[i]["TesterDriveCompletedSites"]);
                        loSite.PendingWithIssues = Convert.ToInt32(dtTestersites.Rows[i]["TesterPendingWithIssuesSites"]);
                        loSite.InProgress = Convert.ToInt32(dtTestersites.Rows[i]["TesterInProgress"]);
                        loSite.ReportSubmited = Convert.ToInt32(dtTestersites.Rows[i]["TesterReportSubmitted"]);
                        lstSites.Add(loSite);
                    }
                }
                return lstSites;
            }
            catch (Exception)
            {


            }

            return lstSites;
        }


        public List<SitesVM> GetSiteLocationsForSchedule(string ParentFilter, string ChildFilter, DateTime fromDate, DateTime toDate, string Panel1Option, Int64 Panel1Value, string Panel2Option, Int64 Panel2Value, Int64 UserId, string CountryId, string Client, string Scopes, string Markets)
        {
            List<SitesVM> lstSites = new List<SitesVM>();
            DashboardDL db = new DashboardDL();
            DataTable dt = db.GetSiteLocationsForSchedule(ParentFilter, ChildFilter, fromDate, toDate, Panel1Option, Panel1Value, Panel2Option, Panel2Value, UserId, CountryId, Client, Scopes, Markets);

            lstSites = GetSitesFromTable(dt);

            return lstSites;
        }
    }
}