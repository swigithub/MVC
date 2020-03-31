using System;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class AV_Widgets
    {
        public Int64 WidgetId { get; set; }
        public string WidgetName { get; set; }
        public string Tilte { get; set; }
        public string WidgetType { get; set; }
        public string Category { get; set; }
        public string SqlQuery { get; set; }
        public bool InPanel { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }

    public class AV_WidgetCategory
    {
        public AV_WidgetCategory() {
            Widgets = new List<AV_Widgets>();
        }
        public string Category { get; set; }
        public List<AV_Widgets> Widgets { get; set; }
    }
}
