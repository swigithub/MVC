

using System;

namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class Sec_Permission
    {

        public Int64 Id  { get; set; }
        public Int64 ParentId { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Code { get; set; }
        public bool IsMenuItem { get; set; }
        public string Icon { get; set; }
        public bool IsUsed { get; set; }
        public Int64 ModuleId { get; set; }
        public int SortOrder { get; set; }
        public bool IsModule { get; set; }

    }
}