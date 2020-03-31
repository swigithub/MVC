namespace WebApplication.Areas.Project.View_Models
{
    public class TemplateRDViewModel
    {
        public int NodeId { get; set; }
        public int ScopeId { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public int NetworkModeId { get; set; }
        public int BandId { get; set; }
        public int CarrierId { get; set; }
        public string PageType { get; set; }
        public bool WithPartialView { get; set; }
        public string TemplateType { get; set; }
        public int? Page { get; set; }
        public int? PageStartIndex { get; set; }
        public int? PageEndIndex { get; set; }

    }
}