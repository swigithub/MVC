using Newtonsoft.Json;
using SWI.AirView.Common;
using SWI.AirView.Models;
using SWI.Common;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace WebApplication.Services
{
    public class ExchangeController : ApiController
    {


        [Route("swi/Idle/UE"), HttpPost]
        public Response IdleUE(string IMEI, int Duration, string TestType)
        {
            Response r = new Response();
            try
            {
                AV_UEPbxBL ueb = new AV_UEPbxBL();

                var rec = ueb.ToSingle("byIsIdle", true.ToString());
                if (rec != null)
                {
                    AD_UserEquipmentsBL ueqb = new AD_UserEquipmentsBL();
                    var uerec = ueqb.ToSingle("BySerialNo", IMEI);
                    if (uerec != null)
                    {
                        SendMessage(rec.DeviceToken, uerec.UENumber, Duration, TestType);
                        r.Status = "success";
                        r.Message = "success";
                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "Device number not found!.";

                    }
                }
                else
                {
                    r.Status = "error";
                    r.Message = "Server busy.";
                }


            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }

            return r;
        }


        [Route("swi/set/DeviceToken"), HttpPost]
        public Response SetDeviceToken(string IMEI, string token)
        {
            Response r = new Response();
            try
            {
                AV_UEPbxBL ueb = new AV_UEPbxBL();
                AV_UEPbx ue = new AV_UEPbx();
                ue.IMEI = IMEI;
                ue.DeviceToken = token;
                ueb.Manage("SetToken", ue);

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


        [Route("swi/set/DeviceIsIdle"), HttpPost]
        public Response DeviceIsIdle(string IMEI, bool status)
        {
            Response r = new Response();
            try
            {
                AV_UEPbxBL ueb = new AV_UEPbxBL();
                AV_UEPbx ue = new AV_UEPbx();
                ue.IMEI = IMEI;
                ue.IsIdle = status;
                ueb.Manage("SetIsIdle", ue);

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


        [Route("swi/set/DeviceNumber"), HttpPost]
        public Response SetDeviceNumber(string IMEI, string DeviceNumber)
        {
            Response r = new Response();
            try
            {
                AD_UserEquipmentsBL ueb = new AD_UserEquipmentsBL();
                AD_UserEquipment ue = new AD_UserEquipment();
                ue.SerialNo = IMEI;
                ue.UENumber = DeviceNumber.Trim();
                ueb.Manage("Set_DeviceNumber", ue);

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

        private void SendMessage(string DevieToken, string Number, int Ducation, string TestType)
        {

            try
            {
                WebConfig wc = new WebConfig();
                var FirebaseKey = wc.AppSettings("FirebaseKey");
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + FirebaseKey);
                httpWebRequest.Method = "POST";
                // VMT,MT,SMSMT
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //  string json = "{\"to\": \"dfk90rb_znE:APA91bHLegH5Ru1zVNj-aDnV6zrm7ewOFmjl9QvM7N9wL-wnInpqBlHWfWyICpcH6lJgRSux2M-EkVXhtRlGdb2REnwftdE-4xgXAOxm54U-c35PIB7k7Q-o-wepxhC0_30QQp3D2jhe\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
                    string json = "{ \"data\": {\"TestType\": \"" + TestType + "\",\"Number\":\"" + Number + "\",\"duration\":\"" + Ducation + "\",\"AnotherActivity\": \"True\"},\"to\" : \"" + DevieToken + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                // return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [Route("swi/Make/Stream"), HttpPost]
        public Response Stream(string data)
        {
            Response r = new Response();
            try
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Stream.txt");

                File.AppendAllText(mappedPath, data + Environment.NewLine);
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


        //[HttpPost()]
        [Route("swi/Layer/Log2"), HttpPost]
        public Response UploadFilesDemo()
        {
            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            //BinaryReader reader = new BinaryReader(file.InputStream);

            Response r = new Response();
            r.Status = "ok";
            r.Message = "File name ok";
            return r;
        }
        [Route("swi/Layer/Log"), HttpPost]
        public Response UploadFiles()
        {
            Response r = new Response();

            try
            {
                string sPath = "";
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/AirViewLogs/");
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        DirectoryHandler dh = new DirectoryHandler();
                        myFile file = new myFile();
                        string FileText = file.StreamToString(hpf.InputStream);


                        XML xml = new XML();

                        string asAscii = Encoding.ASCII.GetString(
                                Encoding.Convert(Encoding.UTF8,
                                    Encoding.GetEncoding(Encoding.ASCII.EncodingName,
                                        new EncoderReplacementFallback(string.Empty),
                                        new DecoderExceptionFallback()
                                        ),
                                    Encoding.UTF8.GetBytes(FileText)
                            )
                          );

                        string json = xml.ToJson(FileText);



                        string[] FileName = hpf.FileName.Split('_');
                        if (FileName.Length > 6)
                        {
                            AV_SitesBL sitb = new AV_SitesBL();
                            var SiteRecord = sitb.ToList("BySiteCodeWithLayer", FileName[3], null, null, null, null, null).FirstOrDefault();
                            if (SiteRecord != null)
                            {
                              //  var fileList = new DirectoryInfo(sPath + SiteRecord.ClientPrefix + "\\" + FileName[2]).GetFiles("*.xml", SearchOption.AllDirectories);


                                string BAlpha = System.Text.RegularExpressions.Regex.Replace(FileName[5], "[^a-zA-Z]", "");
                                string BNumaric = System.Text.RegularExpressions.Regex.Replace(FileName[5], "[^0-9]", "");
                                string Band = (!string.IsNullOrEmpty(BAlpha.Trim())) ? BAlpha.Trim() + " " + BNumaric : BNumaric;


                                //string Band = "";

                                //if (FileName[3].Equals("LTE"))
                                //{
                                //    Band = FileName[4].Substring(0, 3) + " " + FileName[4].Substring(3, FileName[4].Length - 3);
                                //}
                                //else if (FileName[3].Equals("WCDMA"))
                                //{
                                //    Band = FileName[4].Substring(0, 4) + " " + FileName[4].Substring(4, FileName[4].Length - 4);
                                //}
                                //else if (FileName[3].Equals("GSM"))
                                //{
                                //    Band = FileName[4].Substring(0, 3) + " " + FileName[4].Substring(3, FileName[4].Length - 3);
                                //}

                                string Alpha = System.Text.RegularExpressions.Regex.Replace(FileName[6], "[^a-zA-Z]", "");
                                string Numaric = System.Text.RegularExpressions.Regex.Replace(FileName[6], "[^0-9]", "");
                                string Carrier =(!string.IsNullOrEmpty(Alpha.Trim()))? Alpha.Trim() + " " + Numaric:Numaric;

                                sPath = sPath + SiteRecord.ClientPrefix + "\\" + FileName[3] + "\\" + FileName[4] + "_" + Band + "_" + Carrier + "\\";
                                dh.CreateDirectory(sPath);

                                string[] files = Directory.GetFiles(sPath, "*.xml");
                               string TempFileName = hpf.FileName.Replace(FileName[6], Carrier);
                               TempFileName = hpf.FileName.Replace(FileName[5], Band);

                                TempFileName= TempFileName.Replace("PSC1900", "PSC 1900");



                                file.Write(sPath, TempFileName, "json", json);

                                file.Write(sPath, TempFileName, "xml", FileText);

                                // hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));

                                r.Status = "success";
                                r.Message = "files save successfully.";
                            }
                            else
                            {
                                r.Status = "error";
                                r.Message = "Site not found.";
                            }
                        }
                        else
                        {
                            r.Status = "error";
                            r.Message = "File name not correct";
                        }

                    }
                    else
                    {
                        r.Status = "error";
                        r.Message = "File not found.";
                    }
                }
            }
            catch (Exception ex)
            {

                r.Status = "error";
                r.Message = ex.Message;
            }
            return r;
        }


    }
}
