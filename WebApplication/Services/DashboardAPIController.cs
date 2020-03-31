using SWI.AirView.Models;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.IO;
using System.Text;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using System.Web;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net;
using AirView.DBLayer.Project.Model;
using AirView.DBLayer.Project.BLL;
using System.Web.Mvc;
using System.Web.Http;
using SWI.Libraries.AD.BLL;

namespace WebApplication.Services
{
    public class DashboardAPIController : ApiController
    {
        [System.Web.Http.Route("swi/DashboardAPI/DriveRoute"), System.Web.Http.HttpPost]
        public Response DriveRoute(DriveRoute driveRoute)
        {

            List<Cordinates> cordinates = driveRoute.cordinates;
            List<Cordinates> pathJson = driveRoute.pathJson;

            Response res = new Response();
            DirectoryHandler dh = new DirectoryHandler();

            string CompleteKml = null;

            string ClientPrefix = "";
            string SiteCode = "";
            long RouteId = 0;
            string Delete = "";

            long SiteId = 0;
            string TestType = "";
            int ScopeId = 0;
            long UserId = 0;
            string Filter = "";



            ClientPrefix = driveRoute.ClientPrefix;
            SiteCode = driveRoute.SiteCode;
            RouteId = driveRoute.RouteId;
            Delete = "";

            SiteId = driveRoute.SiteId;
            TestType = driveRoute.TestType;
            ScopeId = driveRoute.ScopeId;
            UserId = driveRoute.UserId;
            Filter = "Insert";

            KML km = new KML();
            try
            {
                CompleteKml += km.Open("Routes", "Routes For Site");

                int count = 1;
                #region Existing File Coordinates
                string Path = "/Content/AirViewLogs/" + ClientPrefix + "/" + SiteCode;
                if (dh.FileExist(HttpContext.Current.Server.MapPath("~" + Path + "/route-" + RouteId + ".kml")))
                {
                    string text;
                    var fileStream = new FileStream(HttpContext.Current.Server.MapPath("~" + Path + "/route-" + RouteId + ".kml"), FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }

                    MyString ms = new MyString();
                    string MyPlacemarks = "";
                    string[] MyPlacemarksSplit;
                    int MyIndex = 5782;
                    if (Delete != "")
                    {
                        MyPlacemarks = ms.BetweenTag(text, "Placemark", "&");
                        MyPlacemarksSplit = MyPlacemarks.Split('&');
                        int counter = 0;

                        foreach (var item in MyPlacemarksSplit)
                        {
                            if (item.Contains(Delete))
                                MyIndex = counter;
                            counter++;
                        }
                    }

                    string Cordinate = ms.BetweenTag(text, "coordinates", "&");
                    string Colors = ms.BetweenTag(text, "color", "&");
                    string[] ColorsArr = Colors.Split('&');
                    //Cordinate = Cordinate.Replace(",0", ",");
                    //Cordinate = Cordinate.Replace(",-", "-");
                    //Cordinate = Cordinate.Replace("-", ",-");
                    //Cordinate = Cordinate.Replace(",,", ",");
                    string[] Tags = Cordinate.Split('&');
                    string cords; ;
                    string[] Cordinates;

                    for (int i = 0; i < Tags.Length; i++)
                    {
                        if (Delete == "" || (i != MyIndex && MyIndex != 5782))
                        {
                            if (Tags[i].Trim().Length > 0)
                            {
                                cords = null;
                                Cordinates = Tags[i].Split('\n');
                                for (int j = 0; j < Cordinates.Length; j++)
                                {
                                    if (Cordinates[j].Trim().Length > 0)
                                    {
                                        cords += Cordinates[j] + "0\n";
                                    }
                                }
                                CompleteKml += km.Style("LineId" + count, "LineStyle", "color", "FFA9A9A9", "width", "4");
                                CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);
                                count++;
                            }
                        }
                    }
                }
                #endregion


                if (cordinates != null || Delete != "")
                {
                    if (Delete == "")
                    {
                        //  foreach (var jk in cordinates)
                        // {
                        string cords = null;

                        string plotColor = "FFA9A9A9"; //(!string.IsNullOrEmpty(r.Color)) ? r.Color.Replace("#", "") :
                        plotColor = "ff" + plotColor.Substring(4, 2) + plotColor.Substring(2, 2) + plotColor.Substring(0, 2);

                        CompleteKml += km.Style("LineId" + count, "LineStyle", "color", plotColor, "width", "4");
                        foreach (var cor in cordinates)
                        {
                            cords += cor.location.lng + "," + cor.location.lat + ",0\n";
                        }
                        CompleteKml += km.Placemark("LineId" + count, "LineId" + count, "LineString", "relative", cords);
                        // CompleteKml += km.Placemarks("LineId" + count, "#LineStyle", "relativeToGround", cords);
                        count++;
                        // }
                    }

                    AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                    AV_DriveRoutes dr = new AV_DriveRoutes();
                    dr.CreatedDate = DateTime.Now;
                    dr.RouteId = RouteId;
                    dr.SiteId = Convert.ToInt64(SiteId);
                    dr.CreatedBy = UserId;
                    dr.RoutePath = Path;
                    if (TestType.Length > 1)
                    {
                        dr.TestType = TestType; //.Remove(TestType.Length - 1);
                    }

                    dr.ScopeId = ScopeId;
                    RouteId = drb.Manage(Filter, dr);

                    if (RouteId > 0)
                    {
                        res.Status = "success";
                        res.Value = RouteId;

                        CompleteKml += km.Close();
                        dh.CreateDirectory(HttpContext.Current.Server.MapPath(Path));
                        km.SaveKml2(CompleteKml, "route-" + RouteId, HttpContext.Current.Server.MapPath(Path));
                        if (pathJson != null)
                        {
                            string jsonpath = "~" + Path;
                            string fname = HttpContext.Current.Server.MapPath(jsonpath);
                            if (!Directory.Exists(fname))
                            { // if it doesn't exist, create

                                System.IO.Directory.CreateDirectory(fname);
                            }
                            string json = JsonConvert.SerializeObject(pathJson.ToArray());

                            //write string to file
                            System.IO.File.WriteAllText(fname + "/route-" + RouteId + ".txt", json);
                        }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();


                    if (Delete == "")
                    {
                        res.Message = "Drive Route Planned Successfully.";
                    }
                    else { res.Message = "Drive Route Deleted Successfully."; }
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "No Route Selected.";
                }
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }

            //return Json(res, JsonRequestBehavior.AllowGet);
            return res;
        }

        [System.Web.Http.Route("swi/DashboardAPI/DriveRoutesList"), System.Web.Http.HttpPost]
        public HttpResponseMessage DriveRouteList(int SiteId, string Scope)//  538461
        {
            //ViewBag.Scope = Scope;
            //ViewBag.SiteId = SiteId;
            AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
            var result = drb.ToList("BySiteId", SiteId.ToString());

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Record with Id " + SiteId.ToString() + " not found");
            }

            // return View("~/views/Dashboard/_DriveRoute.cshtml", rec);
        }

        [System.Web.Http.Route("swi/DashboardAPI/RouteIsSelected"), System.Web.Http.HttpPost]
        public HttpResponseMessage ManageDriveRoute(Int64 RouteId, bool IsSelected)
        {
            Response res = new Response();

            try
            {
                AV_DriveRoutesBL drb = new AV_DriveRoutesBL();
                AV_DriveRoutes dr = new AV_DriveRoutes();
                dr.RouteId = RouteId;
                dr.IsSelected = IsSelected;
                int Id = drb.Manage("UpdateIsSelected", dr);
                if (Id > 0)
                {
                    if(dr.IsSelected == true) { 
                    res.Status = "success";
                    res.Message = "Drive Route Active Successfully.";
                    res.Value = Id;
                    }
                    else
                    {
                        res.Status = "success";
                        res.Message = "Drive Route Deactived.";
                        res.Value = Id;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, res);
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Record not found.";
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                       "Record with Id " + RouteId.ToString() + " not found");
                }

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error Occured");
            }


        }

        [System.Web.Http.Route("swi/DashboardAPI/IssueTicket"), System.Web.Http.HttpPost]
        public HttpResponseMessage InsertUpdateIssueTicket(PM_Issues Issu)
        {
            PM_IssueBL bal = new PM_IssueBL();

            string Message = string.Empty;
            string fileName = string.Empty;
            string actualFileName = string.Empty;

            //var issues = HttpContext.Current.Request["Issue"];
            //var File = HttpContext.Current.Request["file"];

            //// if data is simple json object then no need to deserilized
            //PM_Issues Issue = JsonConvert.DeserializeObject<PM_Issues>(issues);

            //// HttpFileCollectionBase files = HttpContext.Request.Files;

            ////upload the file to server
            //if (HttpContext.Current.Request.Files != null)
            //{
            //    if (HttpContext.Current.Request.Files.Count > 0)
            //    {
            //        var file = HttpContext.Current.Request.Files[0];
            //        actualFileName = file.FileName;
            //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            //        int size = file.ContentLength;
            //        try
            //        {
            //            string FilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Files/") + fileName).ToString();
            //            file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Files/"), fileName));
            //            Issue.FilePath = FilePath;
            //        }
            //        catch (Exception)
            //        {
            //            Message = "File upload failed! Please try again";
            //        }
            //    }
            //}

            // insert/update issue ticket
            if (Issu.IssueId != 0)
            {
                var resUpdate = bal.Manage(bal.Filter_Update, Issu);
                if (resUpdate)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Issue Ticket Updated", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ticket Not Updated");
                }
            }
            else
            {
                var resSave = bal.Manage(bal.Filter_Insert, Issu);
                if (resSave)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "New Ticket Created", Value = 1 });
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ticket Not Created");
                }
            }
        }

        //-------------------------------------------
       


    }
}