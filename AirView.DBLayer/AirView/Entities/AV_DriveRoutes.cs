using System;


namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_DriveRoutes
    {
        public decimal RouteId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RoutePath { get; set; }
        public decimal CreatedBy { get; set; }
        public string UserName { get; set; }
        public decimal SiteId { get; set; }
        public decimal ScopeId { get; set; }
        public string Scope { get; set; }
        public string TestType { get; set; }
        public bool IsSelected { get; set; }
        public string FileName { get; set; }
    }
}
