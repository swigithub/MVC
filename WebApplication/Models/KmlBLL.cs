using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWI.AirView.Models
{
    public class Cordinates
    {
        public string Color { get; set; }
        public Lanlng location { get; set; }
    }

    public class Lanlng
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }


    public class DriveRoute
    {
        public Int64 SiteId { get; set; }
        public string SiteCode { get; set; }
        public string TestType { get; set; }
        public int ScopeId { get; set; }
        public Int64 RouteId { get; set; }
        public Int64 UserId { get; set; }
        public string Filter { get; set; }
        public string ClientPrefix { get; set; }
        public string Delete { get; set; }

        public List<Cordinates> cordinates = new List<Cordinates>();
        public List<Cordinates> pathJson = new List<Cordinates>(); 
    }

}