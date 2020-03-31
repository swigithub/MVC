using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SWI.AirView.Common
{
    /*----MoB!----*/
    /*----19-10-2017----*/
    public class ExceptionHandling
    {
        public static bool ErrorLogs(string url,string Message) {
            try
            {
                string Environment = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/");
                string path = Environment + "Logs/";
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                }
                using (var tw = new StreamWriter(path + Regex.Replace(DateTime.Now.ToShortDateString(), @"[^0-9a-zA-Z]+", "") + ".log", true))
                {
                    string error = "URL: " + url +" -> Error: "+Message+" -> Time: "+DateTime.Now;

                    tw.WriteLine(error);
                    tw.Close();
                    return true;
                }
            }
            catch
            {

                return false;
            }

        }
    }
}