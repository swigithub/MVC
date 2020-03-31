using Library.SWI.Survey.BLL;
using Library.SWI.Survey.Model;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.SWI.Survey.DAL;
using SWI.AirView.Common;
using SWI.Libraries.Common;
using AirView.DBLayer.Survey.BLL;
using System.Text;
using System.Net;
using System.Drawing;
using AirView.DBLayer.Survey.Model;
using System.Web.Configuration;
using SWI.Libraries.Security.Entities;

namespace WebApplication.Areas.Survey.Controllers
{
     
    public class DocumentController : PdfController
    {
        // GET: Survey/Document

        private string MapAPIKey = WebConfigurationManager.AppSettings["ApiMapKey"].ToString();

        [IsLogin(CheckPermission = false)]
        public ActionResult List()
        {

            return View();
        }

        public ActionResult Preview()
        {

            return View();
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult CheckList_UI()
        {

            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult CheckList(int id)
        {
            ViewBag.SiteSurveyId = id;
            ViewBag.MapApiKey = MapAPIKey;
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetSections(int id)
        {
            var res = new TSS_SurveyDocumentBL().GetSections("SURVEY_SECTIONS_BY_SITESURVEYID", id.ToString(), true);
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false)]
        public ActionResult GetSectionQuestons(int id, int surveyId)
        {
            var rec = new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID", surveyId, id.ToString());
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetSectionsForDashboard(int id)
        {
            var res = new TSS_SurveyDocumentBL().GetSectionsForDashboard("SURVEY_SECTIONS_BY_SITESURVEYID_For_Dashboard", id.ToString(), true);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult CreateNewWorkOrder(int SurveyId)
        {
            var res = new TSS_SurveyDocumentBL().CreateNewWOrkOrder("CREATE_DEMO_WORK_ORDER", SurveyId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveSectionsOrder(List<TSS_Section> sections)
        {
            new TSS_SectionBL().SaveSectionsOrder(sections);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveQuestionsOrder(List<TSS_Question> questions)
        {
            new TSS_QuestionBL().SaveQuestionsOrder(questions);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetSectionQuestonsForDashboard(int surveyId)
        {
            var rec = new TSS_QuestionBL().GetQuestionsWithOptionsForDashboard("GET_Questions_BY_SECTIONID_For_Dashboard", surveyId, "");
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult UpdateCheckListSectionsQuestions(string sections, string questions, string surveyid, string sitesectionids)
        {
            var rec = new TSS_QuestionBL().UpdateCheckListSectionsQuestions(sections, questions, surveyid, sitesectionids);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false)]
        public ActionResult GetSectionQuestions_For_Preview(int id, int surveyId)
        {
            var rec = new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_For_Preview", surveyId, id.ToString());
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult MarkSectionCompleted(int id, string base64String)
        {
            new TSS_SurveyResponseDL().SaveSingleResponse(id, null, null, 0, 0, false, "", 17, 0, base64String, "Save_Section_Signature");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult MarkSectionStatus(int sectionId, int status)
        {
            DataTable dt = new TSS_SurveyResponseDL().UpdateSectionStatus(sectionId, status);
            Response res = new Response();
            if (dt.Rows[0][0].ToString() == "STATUS_CHANGED")
            {
                res.Message = "Status Changed Successfully";
                res.Status = "green";
            }
            else
            {
                res.Message = "Required Questions are Pendings";
                res.Status = "red";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult CloneSurvey(Int64 SurveyId, string SurveyTitle,long ClientId)
        {
            var rec = new TSS_SurveyResponseDL().CloneSurvey(SurveyId, SurveyTitle,ClientId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult SaveSiteQuestionResponse(TSS_Question question, bool IsFinished = false)
        {
            var dt = new dbDataTable().Survey_List();
            var dtra = new dbDataTable().Survey_List();
            //if (question.QuestionType == "Image Options")
            //{
            //    myDataTable.AddRow(dt, "Value1", question.SiteQuestionId, "Value2", "Image", "Value3", "Just Generating Response");
            //}
            foreach (var res in question.Responses.Where(s => s.IsChecked).ToList())
            {
                myDataTable.AddRow(dt, "Value1", question.SiteQuestionId, "Value2", res.ResponseText, "Value3", question.QuestionType == "Table" ? res.SelectedRawResponse : res.ResponseValue, "Value4", res.ResponseId, "Value5", res.MinValue, "Value6", res.MaxValue, "Value7", res.IsGps, "Value15", res.Signature);
            }

            foreach (var questionReqAction in question.ReqActions.Where(s => !string.IsNullOrEmpty(s.RequiredAction)).ToList())
            {
                myDataTable.AddRow(dtra, "Value1", questionReqAction.SiteSectionId, "Value2",
                    question.SiteQuestionId, "Value3", questionReqAction.ActionTypeId, "Value4",
                    questionReqAction.Remarks, "Value5", questionReqAction.RequiredAction, "Value6",
                    questionReqAction.Azimuth, "Value7", questionReqAction.Latitude, "Value8", questionReqAction.Longitude,
                    "Value9", questionReqAction.Altitude, "Value10", questionReqAction.GPSAccuracy, "Value11", questionReqAction.ObjectView
                    );
            }
            if (question.Azimuth == 0)
            {
                if (question.QuestionType == "Direction & GPS Based Images")
                {
                    if (question.Responses[0].ResponseValue != "" && question.Responses[0].ResponseValue != null)
                    {
                        question.MapImage = ConvertImageURLToBase64(question.Responses[0].ResponseValue, question.MapZoom);
                    }
                }
            }
            else
            {
                question.MapImage = GetLineMap(question.Responses[0].ResponseValue, question.MapZoom, question.Azimuth);
            }
            //string SavedPath= SaveByteArrayAsImage(question.MapImage, question.SiteQuestionId.ToString());
            // question.MapImage = SavedPath;
            var rec = new TSS_SurveyResponseDL().SaveSingleResponse(question.SiteQuestionId, dt, dtra, question.QuestionId, question.TotalRows, IsFinished, question.MapImage, question.MapZoom, question.Azimuth);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false), HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files,string SiteCode,string SurveyTitle,string SectionTitle)
        {
            var fileNames = new List<object>();
            foreach (var httpPostedFileBase in files)
            {

                var tempPath = Guid.NewGuid().ToString().Substring(0, 6);
                var tempPath1 = Guid.NewGuid().ToString().Substring(0, 6);
                var fileext = Path.GetExtension(httpPostedFileBase.FileName);
                var name = Path.GetFileNameWithoutExtension(httpPostedFileBase.FileName);

                string ServerPath = "/Content/AirViewLogs/TMO/WEB/" + SiteCode + "/" + SurveyTitle + "/" + SectionTitle + "/";
                if (!Directory.Exists(Server.MapPath(ServerPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(ServerPath));
                }
                var releativePath = ServerPath + name + "-" + tempPath + tempPath1 + fileext;
                var path = Server.MapPath(releativePath);

                httpPostedFileBase.SaveAs(path);
                fileNames.Add(new
                {
                    path,
                    name = httpPostedFileBase.FileName,
                    releativePath
                });
            }
            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteImage(string path, string actionId, string isdbexist)
        {

            var filePath = Server.MapPath("~" + path);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
            }

            if (isdbexist == "true")
            {
                var delimg = new RequiredActionsBL().DeleteImage(actionId);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GenerateChildSections(int parentId, int childCount)
        {
            var generateChildSections = new TSS_SectionBL().GenerateChildSections(parentId, childCount);
            foreach (var generateChildSection in generateChildSections)
            {
                if (!string.IsNullOrEmpty(generateChildSection.ChildTitle) || generateChildSection.ChildTitle == "0")
                    generateChildSection.SectionTitle = $"{generateChildSection.SectionTitle} {generateChildSection.ChildTitle}";
            }
            var rec = generateChildSections;
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult DeleteSectionsWithChild(int PSectionId, int SectionId)
        {
            var rec = new TSS_SectionBL().DeleteSectionsWithChild(PSectionId, SectionId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult Index(Int64 Id = 0)
        {
            ViewBag.SurveyId = Id;
            return View();
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(Int64 Id)
        {

            TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
            var rec = sb.ToSingle("By_SurveyId", Id.ToString());
            return Json(rec, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult New(Int64 Id)
        {
            ViewBag.SurveyId = Id;

            return PartialView("~/Areas/Survey/Views/Document/_New.cshtml");
        }

        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult New(TSS_SurveyDocument sd)
        {
            Response res = new Response();
            try
            {
                TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
                sd.CreatedBy = ViewBag.UserId;
                sd.IsActive = true;
                if (sd.SurveyId > 0)
                {
                    res.Value = sdb.Manage("Update", sd);
                }
                else
                {
                    res.Value = sdb.Manage("Insert", sd);
                }
                res.Status = "success";
                res.Message = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult ManageStatus(Int64 SurveyId, bool Status)
        {
            Response res = new Response();
            try
            {
                TSS_SurveyDocument sd = new TSS_SurveyDocument();
                TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
                sd.SurveyId = SurveyId;
                sd.IsActive = Status;

                res.Value = sdb.Manage("Set_IsActive", sd);

                res.Status = "success";
                res.Message = "save successfully";
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [IsLogin(CheckPermission = false)]
        public JsonResult ToList(string Filter, string Value, int pageIndex, int pageSize)
        {
            TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
            var user = Session["user"] as LoginInformation;
            var rec = sb.ToList(Filter, Value, false, pageIndex, pageSize,null,user.UserId);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetIsActive(string Filter, string Value)
        //{
        //    TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
        //    var rec = sb.ToList(Filter, Value);
        //    return Json(rec, JsonRequestBehavior.AllowGet);
        //}



        [IsLogin(CheckPermission = false)]
        public JsonResult ToSurveyList(string Filter, string Value)
        {
            TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
            var rec = sb.ToList(Filter, Value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SubCategories(Int64 Id)
        {
            Id = (Id == 0) ? -1 : Id;
            AD_DefinationBL db = new AD_DefinationBL();
            var rec = db.SelectedList("PDefinationId", Id.ToString(), "-Sub Category-");
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        public List<TSS_Section> GetFlatSections(List<TSS_Section> FlatSect, List<TSS_Section> sections)
        {
            List<TSS_Section> FlatSections = FlatSect;
            foreach (var secc in sections)
            {
                FlatSections.Add(secc);
                if (secc.Sections.Count > 0)
                {
                    GetFlatSections(FlatSections,secc.Sections);
                }
            }
            return FlatSections;

        }

        [IsLogin(Return = "NoCheck", CheckPermission = false)]
        public ActionResult SurveyReport(int id)
        {
            TSS_SectionBL s = new TSS_SectionBL();
            var sur = s.SurveyBySiteId(id);

            List<TSS_Section> FlatSections = new List<TSS_Section>();
           FlatSections= GetFlatSections(FlatSections,sur.Sections);
            

            foreach (var secs in FlatSections)
            {
                foreach (var ques in secs.Questions)
                {
                    if(ques.QuestionType=="Table" && ques.DynamicRows)
                    {
                        ques.TotalRows = ques.DynamicRowsCount;
                    }
                    if(ques.QuestionType== "Direction & GPS Based Images" && ques.IsMultiLocation)
                    {
                        string[] latslongs = ques.Responses[0].ResponseValue.Split('|');
                        if (latslongs.Length == 2)
                        {
                            string[] first = latslongs[0].Split(',');
                            string[] second = latslongs[1].Split(',');
                            try
                            {
                                ques.MapImage = GetLineBetweenTwoSites(Convert.ToDouble(first[0]), Convert.ToDouble(first[1]), Convert.ToDouble(second[0]), Convert.ToDouble(second[1]), "Sites", "400x350", "Sites");
                                ques.Azimuth = Math.Round(DegreeBearing(Convert.ToDouble(first[0]), Convert.ToDouble(first[1]), Convert.ToDouble(second[0]), Convert.ToDouble(second[1])),0);
                            }
                            catch { }
                        }
                    }
                    // ques.MapImage = Server.MapPath(ques.MapImage);
                    foreach (var req in ques.ReqActions.ToList())
                    {
                        if (req.ActionTypeId == 5)
                        {
                            var ext = Path.GetExtension(req.Name).ToLower();
                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".bmp" || ext == ".gif" || ext == ".svg")
                            {
                                RequiredActions rq = new RequiredActions();
                                rq.ActionTypeId = 2;
                                rq.ActionId = req.ActionId;
                                rq.ActionType = "Video_Required";
                                rq.IsDBExist = true;
                                rq.Name = req.Name;
                                rq.RequiredAction = Server.MapPath(req.RequiredAction);
                                rq.SiteQuestionId = req.SiteQuestionId;
                                rq.Remarks = req.Remarks;
                                rq.SiteSectionId = req.SiteSectionId;
                                rq.ObjectView = req.ObjectView;
                                rq.Altitude = req.Altitude;
                                rq.Azimuth = req.Azimuth;
                                rq.GPSAccuracy = req.GPSAccuracy;
                                rq.Latitude = req.Latitude;
                                rq.Longitude = req.Longitude;
                                ques.ReqActions.Add(rq);
                            }
                        }
                    }
                }
            }
            // Logic To Get All Fan Images
            int colorindex = 0;
            if (sur.SectorLocationList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var sector in sur.SectorLocationList)
                {
                    GeoLocation g = new GeoLocation();
                    string[] latlng = sector.LatLng.Split(',');
                    g.Latitude = Convert.ToDouble(latlng[0]);
                    g.Longitude = Convert.ToDouble(latlng[1]);

                    GeoLocation newLatLng = Offset(g, sector.Azimuth, 250);
                    var latlngChanged = newLatLng.Latitude + "," + newLatLng.Longitude;
                    string color = GetRandomColor(colorindex);
                    colorindex++;
                    sb.Append("&path=color:" + color + "|weight:6|" + sector.LatLng + "|" + latlngChanged + "&markers=icon:http://122.129.80.106:90/Content/Images/circle.ico|" + sector.LatLng);


                }
                string url = "https://maps.googleapis.com/maps/api/staticmap?zoom=15&size=600x300&maptype=hybrid" + sb + "&key="+ MapAPIKey;

                StringBuilder _sb = new StringBuilder();
                Byte[] _byte = this.GetImg(url);
                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                sur.AllFanImage = string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());
            }


            // End Logic to Get All Fan
            //return View(sur);

            return PDFView(sur, "~/Areas/Survey/Views/Document/SurveyReport.cshtml");
        }

        [IsLogin(Return = "NoCheck", CheckPermission = false)]
        public ActionResult SurveyReportExcel(int id)
        {
            ExcelController excel = new ExcelController();
            TSS_SectionBL s = new TSS_SectionBL();
            var sur = s.SurveyBySiteId(id);
            List<TSS_Section> FlatSections = new List<TSS_Section>();
            FlatSections = GetFlatSections(FlatSections, sur.Sections);
            //foreach (var secc in sur.Sections)
            //{
            //    FlatSections.Add(secc);
            //    if (secc.Sections.Count > 0)
            //    {
            //        foreach (var isec in secc.Sections)
            //        {
            //            FlatSections.Add(isec);
            //        }
            //    }
            //}

            foreach (var secs in FlatSections)
            {
                foreach (var ques in secs.Questions)
                {
                    if (ques.QuestionType == "Table" && ques.DynamicRows)
                    {
                        ques.TotalRows = ques.DynamicRowsCount;
                    }
                    if (ques.QuestionType == "Direction & GPS Based Images" && ques.IsMultiLocation)
                    {
                        string[] latslongs = ques.Responses[0].ResponseValue.Split('|');
                        if (latslongs.Length == 2)
                        {
                            string[] first = latslongs[0].Split(',');
                            string[] second = latslongs[1].Split(',');
                            try
                            {
                                ques.MapImage = GetLineBetweenTwoSites(Convert.ToDouble(first[0]), Convert.ToDouble(first[1]), Convert.ToDouble(second[0]), Convert.ToDouble(second[1]), "Sites", "400x350", "Sites");
                                ques.Azimuth = Math.Round(DegreeBearing(Convert.ToDouble(first[0]), Convert.ToDouble(first[1]), Convert.ToDouble(second[0]), Convert.ToDouble(second[1])),0);
                            }
                            catch { }
                        }
                    }
                    foreach (var req in ques.ReqActions.ToList())
                    {
                        if (req.ActionTypeId == 5)
                        {
                            var ext = Path.GetExtension(req.Name).ToLower();
                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".bmp" || ext == ".gif" || ext == ".svg")
                            {
                                RequiredActions rq = new RequiredActions();
                                rq.ActionTypeId = 2;
                                rq.ActionId = req.ActionId;
                                rq.ActionType = "Video_Required";
                                rq.IsDBExist = true;
                                rq.Name = req.Name;
                                rq.RequiredAction = Server.MapPath(req.RequiredAction);
                                rq.SiteQuestionId = req.SiteQuestionId;
                                rq.Remarks = req.Remarks;
                                rq.SiteSectionId = req.SiteSectionId;
                                rq.ObjectView = req.ObjectView;
                                rq.Altitude = req.Altitude;
                                rq.Azimuth = req.Azimuth;
                                rq.GPSAccuracy = req.GPSAccuracy;
                                rq.Latitude = req.Latitude;
                                rq.Longitude = req.Longitude;
                                ques.ReqActions.Add(rq);
                            }
                        }
                    }
                }
            }
            int colorindex = 0;
            // Logic To Get All Fan Images
            if (sur.SectorLocationList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var sector in sur.SectorLocationList)
                {
                    GeoLocation g = new GeoLocation();
                    string[] latlng = sector.LatLng.Split(',');
                    g.Latitude = Convert.ToDouble(latlng[0]);
                    g.Longitude = Convert.ToDouble(latlng[1]);

                    GeoLocation newLatLng = Offset(g, sector.Azimuth, 250);
                    var latlngChanged = newLatLng.Latitude + "," + newLatLng.Longitude;
                    string color = GetRandomColor(colorindex);
                    colorindex++;
                    sb.Append("&path=color:" + color + "|weight:6|" + sector.LatLng + "|" + latlngChanged + "&markers=icon:http://122.129.80.106:90/Content/Images/circle.ico|" + sector.LatLng);

                }
                string url = "https://maps.googleapis.com/maps/api/staticmap?zoom=15&size=600x300&maptype=hybrid" + sb + "&key="+ MapAPIKey;

                StringBuilder _sb = new StringBuilder();
                Byte[] _byte = this.GetImg(url);
                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                sur.AllFanImage = string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());
            }


            // End Logic to Get All Fan
            return excel.ExportToExcel(sur);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult DeleteSection(int sectionId)
        {
            DataTable dt = new TSS_SurveyResponseDL().DeleteSection(sectionId);
            Response res = new Response();
            if (dt.Rows[0][0].ToString() == "SECTION_DELETED")
            {
                res.Message = "Section Deleted Successfully";
                res.Status = "green";
            }
            else
            {
                res.Message = "Section is in use";
                res.Status = "red";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public String GetLineMap(string Latlng, int Zoom, double azimuth)
        {
            var mapZoom = Zoom == 0 ? 17 : Zoom;
            GeoLocation g = new GeoLocation();
            string[] latlng = Latlng.Split(',');
            g.Latitude = Convert.ToDouble(latlng[0]);
            g.Longitude = Convert.ToDouble(latlng[1]);
            var OldLatLng = g.Latitude + "," + g.Longitude;
            var MarkerLatLng = g.Latitude - 0.00002 + "," + g.Longitude;
            GeoLocation newLatLng = Offset(g, azimuth, 50);
            var latlngChanged = newLatLng.Latitude + "," + newLatLng.Longitude;
            string sb = "&path=color:red|weight:6|" + OldLatLng + "|" + latlngChanged + "&markers=icon:http://122.129.80.106:90/Content/Images/circle.ico|" + MarkerLatLng;
            string url = "https://maps.googleapis.com/maps/api/staticmap?zoom=+" + mapZoom + "&size=400x350" + sb + "&key="+ MapAPIKey;

            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = this.GetImg(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());

        }
        // Image URL To Base64

        public String ConvertImageURLToBase64(string latlng, int Zoom, string size = "400x350", string maptype = "roadmap")
        {
            string[] newlatlongs = latlng.Split('|');
            var APIKey = MapAPIKey;
            var mapZoom = Zoom;
            if (mapZoom <= 0)
            {
                mapZoom = 1;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var ll in newlatlongs)
            {
                sb.Append("&markers=color:red%7C" + ll);
            }
            string url = "https://maps.googleapis.com/maps/api/staticmap?zoom=" + mapZoom + "&size=" + size + "&maptype=" + maptype + "" + sb + "&key=" + APIKey;

            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = this.GetImg(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());

        }
        private byte[] GetImg(string url)
        {
            Stream stream = null;
            byte[] buf;
            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }
                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }
            return (buf);
        }

        private string SaveByteArrayAsImage(string base64String, string QuestionId)
        {
            string path = "/Content/" + "MapImage" + QuestionId + ".png";
            string fullOutputPath = Server.MapPath("~" + path);
            String kept = base64String.Substring(0, base64String.IndexOf(","));
            String remainder = base64String.Substring(base64String.IndexOf(",") + 1, base64String.Length - kept.Length - 1);
            byte[] bytes = Convert.FromBase64String(remainder);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            image.Save(fullOutputPath, System.Drawing.Imaging.ImageFormat.Png);
            return path;
        }
        /// Get Map Coardinates
        public struct GeoLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public GeoLocation Offset(GeoLocation point, double angle, double distanceInMeters)
        {
            double rad = Math.PI * angle / 180;

            double xRad = Math.PI * point.Latitude / 180; // convert to radians
            double yRad = Math.PI * point.Longitude / 180;

            double R = 6378100; //Radius of the Earth in meters
            double x = Math.Asin(Math.Sin(xRad) * Math.Cos(distanceInMeters / R)
                                  + Math.Cos(xRad) * Math.Sin(distanceInMeters / R) * Math.Cos(rad));

            double y = yRad + Math.Atan2(Math.Sin(rad) * Math.Sin(distanceInMeters / R) * Math.Cos(xRad), Math.Cos(distanceInMeters / R) - Math.Sin(xRad) * Math.Sin(x));

            x = x * 180 / Math.PI; // convert back to degrees
            y = y * 180 / Math.PI;

            GeoLocation g = new GeoLocation();
            g.Latitude = x;
            g.Longitude = y;
            return g;
        }
        /// 
        [IsLogin(Return = "NoCheck", CheckPermission = false)]
        public ActionResult SurveyReportImages(int id)
        {
            var SiteCodeDefault = "";
            TSS_SectionBL s = new TSS_SectionBL();
            var surveyids = s.GetSurveyBySiteId(id);
            string LatitudeFromQuestion = "";
            string LongitudeFromQuestion = "";
            string DonorLatitudeFromQuestion = "";
            string DonorLongitudeFromQuestion = "";
            StringBuilder  TempInstruction = new StringBuilder();
            List<TSS_SurveyDocument> surveyList = new List<TSS_SurveyDocument>();
            try
            {

                List<SiteInfo> sinfo = new List<SiteInfo>();
                for (int su = 0; su < surveyids.Rows.Count; su++)
             {
                    var sur = s.SurveyBySiteId(Convert.ToInt64(surveyids.Rows[su]["SiteSurveyId"]));
                    if (su == 0)
                    {
                        SiteCodeDefault = sur.SiteCode;
                    }
                    sur.DefaultSitecode = sur.SiteCode;
                    List<TSS_Section> FlatSections = new List<TSS_Section>();

                    foreach (var secc in sur.Sections)
                    {
                        FlatSections.Add(secc);
                        var tempSec = secc;
                    Loop:
                        if (tempSec.Sections.Count > 0)
                        {
                            foreach (var isec in tempSec.Sections)
                            {
                                FlatSections.Add(isec);
                                if (isec.Sections.Count > 0)
                                {
                                    tempSec = isec;
                                    goto Loop;
                                }

                            }
                        }
                    }
                    foreach (var secs in FlatSections)
                    {
                        foreach (var ques in secs.Questions)
                        {
                            foreach (var req in ques.ReqActions.ToList())
                            {
                                if (req.ActionTypeId == 5)
                                {
                                    var ext = Path.GetExtension(req.Name).ToLower();
                                    if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".bmp" || ext == ".gif" || ext == ".svg")
                                    {
                                        RequiredActions rq = new RequiredActions();
                                        rq.ActionTypeId = 2;
                                        rq.ActionId = req.ActionId;
                                        rq.ActionType = "Video_Required";
                                        rq.IsDBExist = true;
                                        rq.Name = req.Name;
                                        rq.RequiredAction = Server.MapPath(req.RequiredAction);
                                        rq.SiteQuestionId = req.SiteQuestionId;
                                        rq.Remarks = req.Remarks;
                                        rq.SiteSectionId = req.SiteSectionId;
                                        rq.ObjectView = req.ObjectView;
                                        rq.Altitude = req.Altitude;
                                        rq.Azimuth = req.Azimuth;
                                        rq.GPSAccuracy = req.GPSAccuracy;
                                        rq.Latitude = req.Latitude;
                                        rq.Longitude = req.Longitude;
                                        ques.ReqActions.Add(rq);
                                    }
                                }
                            }

                        }
                    }
                    foreach (var item in FlatSections)
                    {
                        TSS_Question DQuest = item.Questions.Where(x => x.QuestionType == "Direction & GPS Based Images").FirstOrDefault();
                        if (DQuest != null && DQuest.Responses[0].ResponseValue != "")
                        {
                                string[] DLatLng = DQuest.Responses[0].ResponseValue.Split(',');
                                DonorLatitudeFromQuestion = DLatLng[0].ToString();
                                DonorLongitudeFromQuestion = DLatLng[1].ToString();
                        }
                        else
                        {
                            DonorLatitudeFromQuestion = "";
                            DonorLongitudeFromQuestion = "";
                        }

                        var Tables = 0;
                        foreach (var itemQuestion in item.Questions)
                        {
                            if (itemQuestion.QuestionType == "Table" && itemQuestion.DynamicRows)
                            {
                                itemQuestion.TotalRows = itemQuestion.DynamicRowsCount;
                            }
                            if (string.IsNullOrEmpty(item.TempSectionTitle))
                            {
                                item.TempSectionTitle = item.SectionTitle + " " + item.SiteCode;
                            }
                            if (itemQuestion.QuestionType == "Table")
                            {
                                Tables++;
                                if (Tables == 1)
                                {
                                    var columnNames = new string[] { };
                                    var columnLatLong = new string[] { };
                                    var columnSiteId = new string[] { };
                                    var columnAddress = new string[] { };
                                    for (int row = 0; row < itemQuestion.TotalColumn; row++)
                                    {
                                        var TitleName = itemQuestion.Responses[row].ResponseText.ToUpper();
                                        if (TitleName == "D/R" || TitleName == "SITENAME" || TitleName == "SITE NAME" || TitleName == "SITE" || TitleName == "NAME")
                                        {
                                            if (itemQuestion.Responses[row].IsReadOnly)
                                            {
                                                columnNames = itemQuestion.Responses[row].UserValues.Split(',');
                                            }
                                            else
                                            {
                                                columnNames = itemQuestion.Responses[row].SelectedRawResponse.Split('|');
                                            }
                                        }
                                        if (TitleName == "LATLNG" || TitleName == "LATITUDELONGITUDE" || TitleName=="LATLONG" || TitleName.Contains("LAT") || TitleName.Contains("LNG"))
                                        {
                                            if (itemQuestion.Responses[row].IsReadOnly)
                                            {
                                                columnLatLong = itemQuestion.Responses[row].UserValues.Split(',');
                                            }
                                            else
                                            {
                                                columnLatLong = itemQuestion.Responses[row].SelectedRawResponse.Split('|');
                                            }
                                        }
                                        if (TitleName == "SITE ID" || TitleName == "SITEID")
                                        {
                                            if (itemQuestion.Responses[row].IsReadOnly)
                                            {
                                                columnSiteId = itemQuestion.Responses[row].UserValues.Split(',');
                                            }
                                            else
                                            {
                                                columnSiteId = itemQuestion.Responses[row].SelectedRawResponse.Split('|');
                                            }
                                        }
                                        if (TitleName == "ADDRESS" || TitleName == "SITEADDRESS" || TitleName == "SITE ADDRESS")
                                        {
                                            if (itemQuestion.Responses[row].IsReadOnly)
                                            {
                                                columnAddress = itemQuestion.Responses[row].UserValues.Split(',');
                                            }
                                            else
                                            {
                                                columnAddress = itemQuestion.Responses[row].SelectedRawResponse.Split('|');
                                            }
                                        }


                                    }
                                    for (int i = 0; i < itemQuestion.TotalRows; i++)
                                    {
                                        try
                                        {
                                            var checkColor = 0;
                                            int colorIndex = 0;
                                            SiteInfo sl = new SiteInfo();
                                            sl.SiteName = columnNames[i];
                                            string[] splittedLatLng = columnLatLong[i].Split(',');
                                            sl.Lat = Convert.ToDouble(splittedLatLng[0]);
                                            sl.Long = Convert.ToDouble(splittedLatLng[1]);
                                            sl.SiteId = columnSiteId[i];
                                            sl.Address = columnAddress[i];
                                            while (checkColor == 0)
                                            {
                                                var getColor = GetRandomColorCode(colorIndex);
                                                var checkExists= sinfo.Where(x => x.Color==getColor).FirstOrDefault();
                                                if (checkExists == null)
                                                {
                                                    checkColor = 1;
                                                    sl.Color = getColor;
                                                }
                                                colorIndex++;
                                            }
                                            sinfo.Add(sl);

                                            sur.SiteCode = columnSiteId[i].ToString();
                                            sur.SurveyTitle = columnNames[i].ToString();

                                            item.TempSectionTitle= columnNames[i].ToString()+ " "+ columnSiteId[i].ToString();

                                        }
                                        catch
                                        {
                                        }
                                    }

                                }
                            }
                            if(itemQuestion.QuestionType.ToUpper()=="MULTI LINE")
                            {
                                TempInstruction.Append(itemQuestion.Responses[0].ResponseValue);
                                TempInstruction.Append("<br/>");
                            }
                           if(itemQuestion.QuestionType == "Direction & GPS Based Images")
                            {
                                if (!string.IsNullOrEmpty(itemQuestion.Responses[0].ResponseValue) && !(sur.SurveyTitle.ToUpper().Contains("DONOR")))
                                {
                                    string[] LatLng = itemQuestion.Responses[0].ResponseValue.Split(',');
                                    LatitudeFromQuestion = LatLng[0].ToString();
                                    LongitudeFromQuestion = LatLng[1].ToString();
                                }
                            }
                        }
                    }
                    sur.Instruction = TempInstruction.ToString();
                    TempInstruction.Clear();
                    surveyList.Add(sur);
                }
                SiteInfo GetRecipient = sinfo.Where(x => x.SiteName.ToUpper().Contains("RECIPIENT")).FirstOrDefault();
                if (GetRecipient == null)
                {
                    SiteInfo si = new SiteInfo();
                    si.Address = surveyList[0].SiteAddress;
                    si.Lat = Convert.ToDouble(LatitudeFromQuestion ==""?surveyList[0].Latitude:LatitudeFromQuestion);
                    si.Long = Convert.ToDouble(LongitudeFromQuestion==""?surveyList[0].Longitude:LongitudeFromQuestion);
                    si.SiteId = SiteCodeDefault;
                    si.Color = GetRandomColorCode(sinfo.Count + 1);
                    si.SiteName = "Recipient";
                    sinfo.Add(si);
                }
                surveyList[0].SiteImage = GetLinesBetweenSites(sinfo, "640x400");
                surveyList[0].SurveySitesInfo = GetCompleteInfo(sinfo);
                for (int i = 1; i < surveyList.Count; i++)
                {
                    surveyList[i].SubCategory = null;
                }
                // Shift Instruction From All Surveys to Last so that it will only be shown
                string TempInstructionHolder = "";
                foreach (var surv in surveyList)
                {
                    if (surv.Instruction != "")
                    {
                        TempInstructionHolder = surv.Instruction;
                        surv.Instruction = "";
                    }
                }
                surveyList[surveyList.Count - 1].Instruction = TempInstructionHolder;
            // End Shifting 
                return PDFViewMultiple(surveyList, "~/Areas/Survey/Views/Document/ImagesSurveyReport.cshtml", "LandScape");
            }
            catch(Exception ex)
            {

                return PDFViewMultiple(surveyList, "~/Areas/Survey/Views/Document/ImagesSurveyReport.cshtml", "LandScape");
            }
        }
        public string GetLinesBetweenSites(List<SiteInfo> sites , string size="640x400",string MapType="hybrid")
        {
            string UrlForMapmarker = "http://chart.apis.google.com/chart%3Fchst%3Dd_map_pin_letter%26chld%3DR%257C";
            string ApiKey = MapAPIKey;
            StringBuilder path = new StringBuilder();
            SiteInfo Recipient = sites.Where(x => x.SiteName.ToUpper().Contains("RECIPIENT")).FirstOrDefault();
            string RecipientLatLng = Recipient.Lat + "," + Recipient.Long;
            string RecipienticonUrl = UrlForMapmarker + Recipient.Color + "%257Cffffff";
            int colorIndex = 0;
            foreach(var site in sites)
            {
                if (!site.SiteName.ToUpper().Contains("RECIPIENT") && site.SiteName.ToUpper().Contains("DONOR"))
                {
                    var currentSiteLatLng = site.Lat + "," + site.Long;
                    var currentColor = site.Color;
                    string color = GetRandomColor(colorIndex);
                    colorIndex++;
                    string iconUrl = UrlForMapmarker + site.Color+"%257Cffffff";
                    path.Append("&path=color:"+color+"|weight:5|" + RecipientLatLng + "|" + currentSiteLatLng + "&markers=icon:" + iconUrl + "|" + currentSiteLatLng);
                }
            }
            string RecipientMarkerPath = "&markers=icon:" + RecipienticonUrl + "|" + RecipientLatLng;
            string url = "https://maps.googleapis.com/maps/api/staticmap?size="+size+"&maptype="+MapType+path+RecipientMarkerPath + "&key="+ApiKey;
            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = this.GetImg(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());
        }
        public string GetLineBetweenTwoSites(double sLatitude, double sLongitude, double eLatitude, double eLongitude,string a="DonorRecipient",string size= "640x400",string Mcolor="DonorRecipient")
        {
            string ApiKey = MapAPIKey;
            string RecipientLatLng = sLatitude+","+sLongitude;
            string DonorLatLng = eLatitude+","+eLongitude;
            int colorIndex = 0;
            string color = GetRandomColor(colorIndex);
            colorIndex++;
            var RLabel = "label:O";
            var DLabel = "label:T";
            var RColor = "color:red";
            var DColor = "color:yellow";
            if (a== "DonorRecipient")
            {
                RLabel = "label:R";
                DLabel = "label:D";
            }
            if(Mcolor== "DonorRecipient")
            {
                 RColor = "color:green";
                 DColor = "color:yelow";
            }
            string path="&path=color:" + color + "|weight:5|" + RecipientLatLng + "|" + DonorLatLng + "&markers="+RColor+"|"+ RLabel + "|" + RecipientLatLng + "&markers="+DColor+"|"+ DLabel + "|" + DonorLatLng;
              
            string url = "https://maps.googleapis.com/maps/api/staticmap?size="+size+"&maptype=road" + path + "&key=" + ApiKey;
            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = this.GetImg(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());
        }
        public string GetRandomColor(int index = -1)
        {
            string[] Colors = new string[] { "red", "blue", "purple", "brown", "orange", "green", "yellow", "white"};
            Random Rand = new Random();
            string color = "";
            if (index != -1)
            {
                try
                {
                    color = Colors[index].ToString();
                }
                catch
                {
                    int indexOfColor = Rand.Next(0, Colors.Length);
                    color = Colors[indexOfColor].ToString();
                }
            }
            else
            {
                int indexOfColor = Rand.Next(0, Colors.Length);
                color = Colors[indexOfColor].ToString();
            }
            return color;
        }
        public string GetRandomColorCode(int index = -1)
        {
            string[] Colors = new string[] { "FF0000", "0000FF", "800080", "A52A2A", "FFA500", "008000", "FFFF00", "808000", "FFFFFF" };
            Random Rand = new Random();
            string color = "";
            if (index != -1)
            {
                try
                {
                    color = Colors[index].ToString();
                }
                catch
                {
                    int indexOfColor = Rand.Next(0, Colors.Length);
                    color = Colors[indexOfColor].ToString();
                }
            }
            else
            {
                int indexOfColor = Rand.Next(0, Colors.Length);
                color = Colors[indexOfColor].ToString();
            }
            return color;
        }
        public List<SurveySitesInfo> GetCompleteInfo(List<SiteInfo> sites)
        {
            List<SurveySitesInfo> SitesInfo = new List<SurveySitesInfo>();
            SiteInfo Recipient = sites.Where(x => x.SiteName.ToUpper().Contains("RECIPIENT")).FirstOrDefault();
           

            // loop here and find distance then fill SitesInfo

            foreach(var site in sites)
            {
                if (!site.SiteName.ToUpper().Contains("RECIPIENT"))
                {
                    SurveySitesInfo si = new SurveySitesInfo();
                    si.SSiteName = Recipient.SiteName;
                    si.ESiteName = site.SiteName;
                    si.Distance = Getdistance(Recipient.Lat, Recipient.Long, site.Lat, site.Long);
                    si.SLatitude = Recipient.Lat;
                    si.SLongitude = Recipient.Long;
                    si.ELatitude = site.Lat;
                    si.ELongitude = site.Long;
                    si.StartSiteId = Recipient.SiteId;
                    si.EndSiteId = site.SiteId;
                    si.SAddress = Recipient.Address;
                    si.EAddress = site.Address;
                    si.EAzimuth = DegreeBearing(Recipient.Lat, Recipient.Long, site.Lat, site.Long);
                    si.Image = GetLineBetweenTwoSites(Recipient.Lat, Recipient.Long, site.Lat, site.Long);
                    si.EColor = site.Color;
                    si.SColor = Recipient.Color;

                    SitesInfo.Add(si);
                }
            }
            
            return SitesInfo;
        }
        public double Getdistance(double sLatitude, double sLongitude, double eLatitude,double eLongitude)
        
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var sLatitudeRadians = sLatitude * radiansOverDegrees;
            var sLongitudeRadians = sLongitude * radiansOverDegrees;
            var eLatitudeRadians = eLatitude * radiansOverDegrees;
            var eLongitudeRadians = eLongitude * radiansOverDegrees;

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;//* 1.60934;  // Distance in KM
        }
        public double DegreeBearing(double lat1, double lon1,double lat2, double lon2)
        {
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }
    }
   
}