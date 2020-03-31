using AirView.DBLayer.Survey.BLL;
using AirView.DBLayer.Survey.Model;
using Library.SWI.Survey.BLL;
using Library.SWI.Survey.DAL;
using Library.SWI.Survey.Model;
using SWI.AirView.Models;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using SWI.AirView.Common;
using WebApplication.Areas.Survey.Controllers;
using System.Text;
using System.IO;
using System.Net;

namespace WebApplication.Services
{
   
    public class  TSSController : ApiController
    {

        WebConfig wc = new WebConfig();

        [Route("swi/Site/ContactInfo/{Id}"), HttpPost]
        public List<TSS_SiteContact> ContactInfo(Int64 Id)
        {
            try
            {
                TSS_SiteContactDL scd = new TSS_SiteContactDL();
                DataTable dt = scd.GetContactInfo("Get_Site_Contact", Id);
                List<TSS_SiteContact> Contacts = dt.ToList<TSS_SiteContact>();
                return Contacts;

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("swi/Site/AccessInfo/{Id}"), HttpPost]
        public List<AccessInfo> AccessInfo(Int64 Id)
        {
            try
            {
                TSS_SiteContactDL scd = new TSS_SiteContactDL();
                DataTable dt = scd.GetAccessInfo("Get_Site_AccessInfo_API", Id);
                List<AccessInfo> AccessInfo = dt.ToList<AccessInfo>();
                return AccessInfo;

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("swi/Survey/GetAllCOP"), HttpGet,Route("swi/Survey/GetAllSurveys")]
        public List<TSS_COPList> GetAllCOP()
        {
            try
            {
                TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
                return sb.GetAllSurveys("GetAllCOP", null, true);

                    

            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("swi/Survey/Document"), HttpPost]
        public List<TSS_SurveyDocument> SurveyDocument(Int64 Id,int UserId,string IMEI=null)
        {
            try
            {
                TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
                return sb.ToList("SURVEY_BY_WO", Id.ToString(),true,UserId,0,IMEI);
                //TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
                //return  new List<TSS_SurveyDocument>() { new TSS_SectionBL().SurveyBySiteSurveyId(Id)};
                //return sb.ToList("SURVEY_BY_WO", Id.ToString(), true);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [Route("swi/Survey/SiteSectionBySurveyId"), HttpPost]
        public List<TSS_Section> SiteSectionBySurveyId(Int64 Id)
        {
            try
            {
                TSS_SurveyDocumentBL sb = new TSS_SurveyDocumentBL();
                return sb.GetSectionBySurveyId("GET_SECTIONS_BY_SurveyId", Id.ToString());
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("swi/Survey/DeleteRepeatable"),HttpPost]
        public Response DeleteRepeatables(int PSectionId, int SectionId)
        {
            try
            {
                new TSS_SectionDL().DeleteSectionsWithChild("DELETE_SECTIONS_WITH_CHILD", PSectionId, SectionId);
                return new Response()
                {
                    Message = "Section Deleted Successfully",
                    Status = "200",
                    Value = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "400",
                    Value = false
                };
            }

        }

        [Route("swi/Survey/SaveSurveySignature"), HttpPost]
        public Response SaveSurveySignature([FromBody] SignatureRequestDto sig)
        {
            string base64String = sig.base64String;
            Int64 Id = sig.Id;
            try
            {
                new TSS_SurveyResponseDL().SaveSingleResponse(Id, null, null, 0, 0,false,"",17,0, base64String, "Save_Section_Signature");
                return new Response()
                {
                    Message = "Signature Save Successfully",
                    Status = "200",
                    Value = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "400",
                    Value = false
                };
            }

        }

        [Route("swi/Survey/PdfDocument"), HttpPost]
        public Response GetPdfBytes(Int64 Id)
        {
            try
            {
                var sur = new TSS_SectionBL().SurveyBySiteId(Id);
                var response = new Response();
                response.Value = new PdfController().PDFViewBytes(sur, "~/Areas/Survey/Views/Document/SurveyReport.cshtml");
                return response;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<TSS_Section> GetTreeView(List<TSS_Section> sec)
        {
            TSS_SectionBL sb = new TSS_SectionBL();
            foreach (var s in sec)
            {
                if (s.PSectionId > 0)
                {
                    var sec2 = sb.ToList("By_SectionId", s.PSectionId.ToString()).ToList();
                    if (sec2 != null)
                    {
                        s.Sections.AddRange(sec2);
                        GetTreeView(sec2);
                    }
                }

            }

            return sec;
        }

        [Route("swi/Survey/Sections"), HttpPost]
        public List<TSS_Section> SurveySections(Int64 Id)
        {
            try
            {
                TSS_SectionBL sb = new TSS_SectionBL();
                var sec = sb.ToList("By_SurveyId", Id.ToString()).ToList();

                return GetTreeView(sec);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Route("swi/Survey/UpdateSectionStatus"), HttpPost]
        public Response UpdateSectionStatus(Int64 sectionId , int status)
        {
            try
            {
               DataTable result= new TSS_SurveyResponseDL().UpdateSectionStatus(sectionId, status);
                return new Response()
                {
                    Message = result.Rows[0][0].ToString(),
                    Status = "200",
                    Value = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "400",
                    Value = false
                };
            }

        }

        [Route("swi/Section/Iterations"), HttpPost]
        public Response SectionIterations(List<TSS_SectionIteration> secIter)
        {
            Response res = new Response();
            try
            {
                dbDataTable dbdt = new dbDataTable();
                TSS_SectionIterationBL secIterBL = new TSS_SectionIterationBL();

                DataTable dt = dbdt.List();
                foreach (var itr in secIter)
                {
                    myDataTable.AddRow(dt, "Value1", itr.SurveyId, "Value2", itr.SectionId, "Value3", itr.PSectionId, "Value4", itr.IterationId, "Value5", itr.PIterationId, "Value6", itr.StatusId);
                }
                if (secIterBL.Manage("Insert", dt))
                {
                    res.Value = true;
                    res.Status = "Success";
                    res.Message = "Success";
                }
                else
                {
                    res.Value = false;
                    res.Status = "error";
                    res.Message = "record not save.";
                }

            }
            catch (Exception ex)
            {
                res.Status = "error";
                res.Message = ex.Message;
            }
            return res;
        }

        [Route("swi/Section/Questions"), HttpPost]
        public List<TSS_Question> SectionQuestions(Int64 Id)
        {
            try
            {
                TSS_QuestionBL qb = new TSS_QuestionBL();
                TSS_ResponseBL rb = new TSS_ResponseBL();
                TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
                var que =
                    new TSS_QuestionBL().GetQuestionsWithOptions("GET_Questions_BY_SECTIONID_API", 0, Id.ToString()).OrderBy(s => s.SortOrder).ToList();
                //var que = qb.ToList("GET_QUESTION_BY_SECTION", Id.ToString());
                //foreach (var q in que)
                //{
                //    //q.Responses = rb.ToList("GET_BY_QUESTIONID", q.QuestionId.ToString());
                //    q.Responses = rb.ToList("GET_RESPONSE_BY_SITEQUESTION", q.QuestionId.ToString());
                //}

                foreach (var q in que)
                {
                    q.QuestionLogics = qlb.ToList("GET_BY_QUESTIONID", q.QuestionId.ToString());
                }
                return que;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("swi/Section/GenerateChildSections"), HttpPost]
        public List<TSS_Section> GenerateChildSections(int parentId, int childCount = 0)
        {
            try
            {
                return new TSS_SectionBL().GenerateChildSections(parentId,childCount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //----------------------------------

        [Route("swi/Section/QuestionLogic"), HttpPost]
        public List<TSS_QuestionLogic> SectionQuestionLogic(Int64 sectionId)
        {
            try
            {
                TSS_QuestionLogicBL qlb = new TSS_QuestionLogicBL();
                var qLogic = qlb.ToList("GET_BY_SECTIONID", sectionId.ToString());
                return qLogic;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("swi/Survey/SurveyResponse"), HttpPost]
        public Response SurveyResponse(List<TSS_SurveyResponse> response)
        {
            Response r = new Response();
            try
            {
                TSS_SurveyResponseDL srd = new TSS_SurveyResponseDL();

                dbDataTable dbdt = new dbDataTable();
                DataTable dt = dbdt.Survey_List();
                foreach (var res in response)
                {

                    string ImageBase = "";
                    if (!res.IsSectorLocation)
                    {
                        if (res.QuestionType == "Direction & GPS Based Images")
                        {
                            if (res.ResponseValue != "" && res.ResponseValue != null)
                            {
                                ImageBase = ConvertImageURLToBase64(res.ResponseValue, res.MapZoom);
                            }
                        }
                    }
                    else
                    {
                        ImageBase = GetLineMap(res.ResponseValue,res.MapZoom,res.Azimuth);
                    }
                    myDataTable.AddRow(dt, "Value1", res.SiteId, "Value2", res.SurveyId, "Value3", res.SectionId, "Value4", res.QuestionId, "Value5", res.ResponseId, "Value6", res.IterationId, "Value7", res.ResponseText, "Value8", res.pIterationId, "Value9", res.MinValue, "Value10", res.MaxValue, "Value11", res.IsGps,"Value12",res.ResponseValue,"Value13",res.MapZoom,"Value14",ImageBase,"Value15",res.Signature,"Value16",res.Azimuth);
                }
                srd.Manage("SurveyResponse", dt);
                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;

        }

        [Route("swi/Survey/SurveyAction"), HttpPost]
        public Response SurveyAction(List<TSS_SurveyAction> action)
        {
            Response r = new Response();
            try
            {
                TSS_SurveyResponseDL srd = new TSS_SurveyResponseDL();

                dbDataTable dbdt = new dbDataTable();
                DataTable dt = dbdt.Survey_List();

                foreach (var res in action)
                {
                    var NewAzimuth = res.Azimuth;
                    var NewAltitude = res.Altitude;
                    var NewGPSAccuracy = res.GPSAccuracy;
                    if ( NewAzimuth != "")
                    {
                        NewAzimuth = Math.Round(Convert.ToDecimal(NewAzimuth), 2).ToString();
                    }
                    if( NewAltitude!="")
                    {
                        NewAltitude = Math.Round(Convert.ToDecimal(res.Altitude), 2).ToString();
                    }
                    if(NewGPSAccuracy != "")
                    {
                        NewGPSAccuracy = Math.Round(Convert.ToDecimal(res.GPSAccuracy), 2).ToString();
                    }

                    myDataTable.AddRow(dt,  "Value1", res.SiteId, 
                                            "Value2", res.SurveyId, 
                                            "Value3", res.SectionId, 
                                            "Value4", res.QuestionId, 
                                            "Value5", res.ActionType, 
                                            "Value6", res.Remarks, 
                                            "Value7", res.IterationId, 
                                            "Value8", res.PIterationId,
                                            "Value9",res.Latitude,
                                            "Value10",res.Longitude,
                                            "Value11", NewAzimuth,
                                            "Value12",res.ObjectView,
                                            "Value13", NewAltitude,
                                            "Value14", NewGPSAccuracy,
                                            "Value15", res.ActionValue);
                }


                srd.Manage("SurveyAction", dt);
                r.Status = "success";
                r.Message = "success";
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;

        }

        [Route("swi/Survey/SiteSurvey"), HttpPost]
        public TSS_SurveyDocument SiteSurvey(Int64 SiteSurveyId)
        {
            try
            {
                TSS_SectionBL s = new TSS_SectionBL();
                var sur = s.SurvayBySite(SiteSurveyId);
                return sur;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //public List<TSS_SurveyDocument> SiteSurvey1(Int64 SiteSurveyId)
        //{
        //    try
        //    {
        //        TSS_SurveyDocumentDL sd = new TSS_SurveyDocumentDL();
        //        DataTable dt = sd.Get("By_SiteSurveyId", SiteSurveyId.ToString());

        //        List<TSS_SurveyDocument> surlst = new List<TSS_SurveyDocument>();

        //        List<List<object>> surveyDocList = new List<List<object>>();

        //        List<object> surveyList = new List<object>();
        //        List<object> sectionsList = new List<object>();
        //        List<object> questionsList = new List<object>();

        //        List<object> responsesList = new List<object>();
        //        List<object> reqActionsList = new List<object>();

        //        List<object> sectionIterationsList = new List<object>();

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            TSS_SurveyDocument sur = new TSS_SurveyDocument();

        //            sur.SiteId = int.Parse(dt.Rows[0]["SiteId"].ToString());
        //            sur.SiteCode = dt.Rows[0]["SiteCode"].ToString();

        //            sur.Status = dt.Rows[0]["Status"].ToString();
        //            sur.ClientName = dt.Rows[0]["ClientName"].ToString();

        //            sur.CityName = dt.Rows[0]["CityName"].ToString();
        //            sur.Scope = dt.Rows[0]["Scope"].ToString();

        //            sur.Category = dt.Rows[0]["Category"].ToString();
        //            sur.SubCategory = dt.Rows[0]["SubCategory"].ToString();

        //            sur.Latitude = dt.Rows[0]["Latitude"].ToString();
        //            sur.Longitude = dt.Rows[0]["Longitude"].ToString();

        //            sur.SurveyTitle = dt.Rows[0]["SurveyTitle"].ToString();
        //            sur.Description = dt.Rows[0]["Sur Descp"].ToString();
        //            surveyList.Add(sur);
        //        }

        //        var temp = dt.AsEnumerable()
        //                          .GroupBy(r => new { Col1 = r["SiteSectionId"] })
        //                          .Select(g => g.OrderBy(r => r["SiteSectionId"]).First())
        //                          .CopyToDataTable();
        //        var sec = temp.ToList<object>();

        //        //List<long> SiteSecId = sec.Select(x => x.SiteSectionId).Distinct().ToList();
        //        //foreach (DataRow item in dt.Rows)
        //        //{
        //        //    if (SiteSecId.Contains(Convert.ToInt64(item["SiteSectionId"].ToString())))
        //        //    {
        //        //        var Sections = new
        //        //        {
        //        //            SiteSecId = item["SiteSectionId"],
        //        //            SetionTitle = item["SectionTitle"]
        //        //        };
        //        //        sectionsList.Add(Sections);
        //        //    }
        //        //}

        //        //var distinctsections = sectionsList.Distinct();

        //        ////--------------------------

        //        surveyDocList.Add(surveyList);
        //        surveyDocList.Add(sec);

        //        foreach (List<object> surList in surveyDocList)
        //        {
        //            foreach (List<object> sections in surList)
        //            {
        //                foreach (List<object> questions in sections)
        //                {
        //                    foreach (object responses in questions)
        //                    {

        //                    }

        //                    foreach (object reqActions in questions)
        //                    {

        //                    }
        //                }
        //            }
        //        }



        //        //-------------------------------
        //        return surlst;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [Route("swi/Survey/SaveSiteAttendees"), HttpPost]
        public Response SiteAttendees(List<TSS_SiteAttendees> siteAttendeeList,int SiteId,int SiteSurveyId)
        {
            try
            {
                TSS_DashboardBL s = new TSS_DashboardBL();
                var att = s.Manage(siteAttendeeList, SiteId, SiteSurveyId);
                return new Response()
                {
                    Message ="Saved Successfully",
                    Status = "200",
                    Value = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "400",
                    Value = false
                };
            }
        }

        [Route("swi/Survey/GetSiteAttendees"), HttpPost]
        public List<TSS_SiteAttendees> GetSiteAttendees(int SiteId, int SiteSurveyId)
        {
            try
            {
                TSS_DashboardBL s = new TSS_DashboardBL();
                var att = s.GetSiteAttendees("Get_Site_Attendees_For_Dashboard", SiteId, SiteSurveyId);
                return att;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("swi/Survey/DeleteRequiredAction"), HttpPost]
        public Response DeleteRequiredAction(string ActionId,string Path)
        {
            try
            {
                try
                {
                    DocumentController dc = new DocumentController();
                    dc.DeleteImage(Path, ActionId, "true");
                }
                catch
                {
                    new RequiredActionsBL().DeleteImage(ActionId);
                }

                return new Response()
                {
                    Message = "Action Deleted Successfully",
                    Status = "200",
                    Value = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Message = ex.Message,
                    Status = "400",
                    Value = false
                };
            }

        }

        public String ConvertImageURLToBase64(string latlng,int Zoom)
        {
            string[] newlatlongs = latlng.Split('|');
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            var APIKey = MapKey;
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
            string url = "https://maps.googleapis.com/maps/api/staticmap?zoom="+mapZoom+"&size=400x350&maptype=roadmap" + sb + "&key=" + APIKey;

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
        public String GetLineMap(string Latlng, int Zoom, double azimuth)
        {
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            GeoLocation g = new GeoLocation();
            string[] latlng = Latlng.Split(',');
            g.Latitude = Convert.ToDouble(latlng[0]);
            g.Longitude = Convert.ToDouble(latlng[1]);
            var OldLatLng = g.Latitude + "," + g.Longitude;
            var MarkerLatLng = g.Latitude - 0.00002 + "," + g.Longitude;
            GeoLocation newLatLng = Offset(g, azimuth, 50);
            var latlngChanged = newLatLng.Latitude + "," + newLatLng.Longitude;
            string sb = "&path=color:red|weight:6|" + OldLatLng + "|" + latlngChanged + "&markers=icon:http://122.129.80.106:90/Content/Images/circle.ico|" + MarkerLatLng;
            string url = "https://maps.googleapis.com/maps/api/staticmap?zoom=17&size=400x350" + sb + "&key="+ MapKey;

            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = this.GetImg(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return string.Format(@"data:image/jpg;base64, {0}", _sb.ToString());

        }
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


    }
}
