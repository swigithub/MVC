using System;
using System.IO;
using System.Collections.Generic;


namespace SWI.Libraries.Common
{
  public  class KML
    {
        //KML km = new KML();
        //AV_TestBL tb = new AV_TestBL();
        //var dt = tb.ToList("11109");
        //string kml = string.Empty;
        //kml = km.Open("test", "test");
        //foreach (var item in dt)
        //{
        //    kml += km.Style(item.Sector, "LineStyle", "color", item.ColorCode, "width", "4");
        //    kml += km.Placemark(item.Site, item.Sector, "LineString", "relative", item.Coordinates);
        //}

        //kml += km.Close();
        //bool b = km.SaveKml(kml, "11109", Server.MapPath("~/Content"));

        //string r = km.ReadKmlFile(Server.MapPath("~/Content/km.kml"));

        //if (! HttpContext.IsDebuggingEnabled) {
        //    ViewBag.IsDebuggingEnabled = "Remote";
        //}
        //else
        //{
        //    ViewBag.IsDebuggingEnabled = "develop";
        //}
        public string Open(string Name,string Description) {

            //return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml xmlns=\"http://earth.google.com/kml/2.1\">\n<Document>\n<name>"+ Name + "</name>\n"+ 
            //"<description>"+ Description + "</description>\n";
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml xmlns=\"http://www.opengis.net/kml/2.2\">\n<Document>\n<name>" + Name + "</name>\n" +
            "<description>" + Description + "</description>\n";
        
        }

        public string Close()
        {
            return "\n</Document>\n</kml>";
        }

        public string Style(string Id,string Type, params object[] parameters) {

            string Options = string.Empty;

            for (int liX = 0, liY = 1; liY < parameters.Length; liX = liX + 2, liY = liY + 2)
            {
                Options +="<"+parameters[liX].ToString()+">"+ parameters[liY].ToString() + "</" + parameters[liX].ToString() + ">\n";
            }

            return "<Style id=\""+Id+"\">\n<"+ Type + ">\n"+ Options + "\n</"+ Type + ">\n</Style>";
        }
        public string Placemark(string Name,string StyleUrl,string Type,string AltitudeMode,List<KMLCoordinates> coordinates) {

            string Open = "<Placemark>\n<name>" + Name + "</name>\n<styleUrl>#" + StyleUrl + "</styleUrl>\n<" + Type + ">\n<altitudeMode>" + AltitudeMode +
                          "</altitudeMode>\n<coordinates>\n";
            string Close = "</coordinates>\n </" + Type + ">\n </Placemark > ";
            string coordi=string.Empty;
            foreach (var item in coordinates)
            {
                coordi += item.Latitude + "," + item.Longitude + ",0\n";
            }
            return Open + coordi + Close;
        }

        public string Placemark(string Name, string StyleUrl, string Type, string AltitudeMode, string coordinates)
        {

            return "<Placemark>\n<name>" + Name + "</name>\n<styleUrl>#" + StyleUrl + "</styleUrl>\n<" + Type + ">\n<altitudeMode>" + AltitudeMode +
                   "</altitudeMode> \n<coordinates>\n" + coordinates + "</coordinates>\n</" + Type + ">\n</Placemark>";
        }


        public string PlacemarkMarker(string StyleId,string IconUrl,string PlacemarkName,string Latitude,string Longitude,string Altitude) {
            string style = "<Style id=\""+ StyleId + "\"><IconStyle><Icon><href>"+ IconUrl + "</href></Icon></IconStyle></Style>";
            string StyleMap = "<StyleMap id=\"Map"+ StyleId + "\"><Pair><key>normal</key><styleUrl>#"+ StyleId + "</styleUrl></Pair>";
            string Placemark = "<Placemark><name>"+ PlacemarkName + "</name><styleUrl>#Map"+ StyleId + "</styleUrl><Point><coordinates>"+ Longitude + ","+ Latitude + ","+ Altitude + "</coordinates></Point> </Placemark>";

            return style+ StyleMap+ Placemark;

        }

        public bool SaveKml(string xml,string FileName,string Path) {
            try
            {
                DirectoryHandler dh = new DirectoryHandler();
                dh.CreateDirectory(Path);
                string Text = xml + Environment.NewLine;
                File.WriteAllText(Path+"/"+ FileName+".kml", Text);
                return true;
            }
            catch (Exception)
            {

                //throw;
               return false;
            }
            
        }

        public string Placemarks(string Name, string StyleUrl, string AltitudeMode, string coordinates)
        {

            return "<Placemark>" +
                    "\n<name>" + Name + "</name>" +
                        "\n<styleUrl>#" + StyleUrl + "</styleUrl>" +
                            "<LineString>" +
                                "<extrude>1</extrude>" +
                                "\n<altitudeMode>" + AltitudeMode + "</altitudeMode>" +
                                "\n<coordinates>\n" + coordinates + "</coordinates>" +
                             "</LineString>" +
                   "\n</Placemark>";
        }
        public bool SaveKml2(string xml, string FileName, string Path)
        {
            try
            {
                string Text = xml + Environment.NewLine;
                File.WriteAllText(Path + "/" + FileName + ".kml", Text);
                return true;
            }
            catch (Exception)
            {

                //throw;
                return false;
            }

        }
        public string ReadKmlFile(string path) {
            return File.ReadAllText(path);
        }

        public string MarkerOpen() {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><kml xmlns =\"http://earth.google.com/kml/2.0\" ><Document>";
        }

        public string MarkerCoordinates(string Name,string Description, string styleId, string IconUrl,string Coordinates)
        {
            string style= "<Style id =\"" + styleId + "\"><IconStyle>\n<Icon><href>" + IconUrl + "</href></Icon>\n</IconStyle></Style>\n";
            string coord = "<Placemark><name>"+ Name + "</name ><description>"+ Description + "</description>\n"+
                           "<styleUrl>#"+ styleId + "</styleUrl><Point><coordinates>";
            
            return style+ coord+ Coordinates+"</coordinates></Point></Placemark>";
        }

        public string MarkerEnd()
        {
            return "</Document></kml>";
        }
    }

    public class KMLCoordinates {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
