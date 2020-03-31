using System;


namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class Sec_Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Update_at { get; set; }
        public bool IsActive { get; set; }
        public string DefaultUrl { get; set; }
    }
}