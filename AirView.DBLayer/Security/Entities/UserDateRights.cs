using System;


namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class UserDateRights
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DaysForward { get; set; }
        public int DaysBack { get; set; }
        public DateTime AssignDate { get; set; }
        public bool IsActive { get; set; }
    }
}
