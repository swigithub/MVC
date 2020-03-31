namespace Library.SWI.Template.Model
{
    public class TMP_Templates
    {
        public int TemplateId { get; set; }
        public string TemplateTitle { get; set; }
        public int ProjectId { get; set; }
        public int ScopeId { get; set; }
        public string BackgroundColor { get; set; }
        public string PageType { get; set; }
        public string Parameters { get; set; }
        public bool IsActive { get; set; }

        public string TemplateType { get; set; }

        public bool? IsDefault { get; set; }
        public int ModuleId { get; set; }
        public string ModuleType { get; set; }
    }
}
