namespace Library.SWI.Template.Model
{
    public class TMP_Node
    {
        public int NodeId { get; set; }
        public int TemplateId { get; set; }
        public string NodeTitle { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int x_axis { get; set; }
        public int y_axis { get; set; }
        public int PageTyppeId { get; set; }
        public string NodeUrl { get; set; }
        public string NodeSQL { get; set; }
        public bool IsActive { get; set; }
    }
}
