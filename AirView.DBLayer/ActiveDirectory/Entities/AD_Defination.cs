using System;
using System.Collections.Generic;

namespace SWI.Libraries.AD.Entities
{
    /*----MoB!----*/
    public class AD_Defination
    {
        public AD_Defination()
        {
            Definations = new List<AD_Defination>();
        }
        public Int64 DefinationId { get; set; }
        public string DefinationName { get; set; }
        public Int64 PDefinationId { get; set; }
        public Int64 DefinationTypeId { get; set; }
        public string KeyCode { get; set; }
        public string DisplayType { get; set; }
        public string ColorCode { get; set; }
        public string InputType { get; set; }
        public int MaxLength { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string DisplayText { get; set; }
        public string DefinationType { get; set; }
        public string PDefinationName { get; set; }
        public string MapColumn { get; set; }
          
        public List<AD_Defination> Definations { get; set; }
    }
}
