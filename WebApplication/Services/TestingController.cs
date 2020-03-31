using LumenWorks.Framework.IO.Csv;
using SWI.Libraries.AirView.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication.Services
{
    public class TestingController : ApiController
    {

        [ApiFilter]
        [Route("swi/Test/Files"), HttpPost]
        public string TestFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    iUploadedCnt = iUploadedCnt + 1;
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "File not received\n";
            }
        }


        [Route("swi/Test/Files"), HttpPost]
        public async Task<IHttpActionResult>  TestFiles1(string mystream)
        {
            try
            {
                string result = string.Empty;
                string input = string.Empty;
                string output = string.Empty;
                string ffmpegFilePath = "~" + System.Web.HttpContext.Current.Request.ApplicationPath + "\\ffmpeg\\ffmpeg.exe";
                var processInfo = new ProcessStartInfo(System.Web.HttpContext.Current.Server.MapPath(ffmpegFilePath), "-i " + mystream + " -f mpeg1video -b:v 500k -r 21 -q:v 10 -s 640x480 http://127.0.0.1:8082/pass123/640/480/")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true

                };



                Process process = System.Diagnostics.Process.Start(processInfo);
                result = process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();

                return await Task.Run(() => Ok());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost, Route("swi/Test/fileupload")]
        public HttpResponseMessage Fileupload()
        {
            var random = new Random();
            //var color = String.Format("#{0:X6}", random.Next(0x1000000));
            //RepositoryContext rep = new RepositoryContext();
            HttpResponseMessage response = new HttpResponseMessage();
            List<string> sitecode = new List<string>();
            List<AV_Site> listLatLong = new List<AV_Site>();
            List<AV_Sector> secList = new List<AV_Sector>();
            var httpRequest = HttpContext.Current.Request;
            DataTable FileRecord = new DataTable();
            //string filePath = null;
            //string docPath = null;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {

                    var postedFile = httpRequest.Files[file];

                    Stream stream = httpRequest.Files[file].InputStream;
                    using (CsvReader csvReader =
                      new CsvReader(new StreamReader(stream), true))
                    {
                        FileRecord.Load(csvReader);
                    }



                    //filePath = HttpContext.Current.Server.MapPath("~/files/" + postedFile.FileName);
                    //postedFile.SaveAs(filePath);
                }


                //docPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                //var result = rep.GetDataTableFromCsv(docPath, true);

                foreach (DataRow item in FileRecord.Rows)
                {
                    AV_Site lat = new AV_Site();
                    lat.Latitude = Convert.ToDouble(item["siteLatitude"].ToString());
                    lat.Longitude = Convert.ToDouble(item["siteLongitude"].ToString());
                    lat.SiteCode = item["siteCode"].ToString();
                    listLatLong.Add(lat);
                    sitecode.Add(item["siteCode"].ToString());
                    AV_Sector s = new AV_Sector();
                    //s.Id = Convert.ToInt64(item["SectorId"].ToString());
                    s.SectorCode = item["siteCode"].ToString();
                    //s.sectorColor = "red";
                    s.BeamWidth = Convert.ToInt32(item["BeamWidth"].ToString());
                    s.Azimuth = Convert.ToInt32(item["Azimuth"].ToString());
                    s.Latitude = Convert.ToDouble(item["siteLatitude"].ToString());
                    s.Longitude = Convert.ToDouble(item["siteLongitude"].ToString());
                    s.PCI = Convert.ToInt32(item["PCI"].ToString());
                    secList.Add(s);
                }

            }

            //var uniquePci = secList.Select(x=>x.Pci).Distinct().ToList();



            var list = listLatLong.Distinct().Take(10).ToList();


            var list1 = (from x in list
                         join y in secList on x.SiteCode equals y.SectorCode into data
                         from d in data.DefaultIfEmpty()
                         select d).ToList();

            var uniq = list1.GroupBy(x => x.PCI).ToList();
            foreach (var item in uniq)
            {
                 var color = String.Format("#{0:X6}", random.Next(0x1000000));
                var q = list1.Where(x => x.PCI == item.Key).FirstOrDefault();
                q.sectorColor = color.ToString();

                foreach (var item1 in list1.Where(x=>x.PCI == item.Key).ToList())
                {
                    item1.sectorColor = q.sectorColor;
                }
            }

            //var a = list1.Where(x => x.sectorColor != null);

         


            return Request.CreateResponse(HttpStatusCode.OK, new { list, list1 });
        }
    }
}
