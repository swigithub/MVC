
namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class Sec_UserDevices
    {
        public decimal DeviceId { get; set; }
        public decimal UserId { get; set; }
        public string IMEI { get; set; }
        public string MAC { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public bool IsActive { get; set; }
        public string UserFullName { get; set; }
        public string Password { get; set; }
        public int TranferToId { get; set; }
    }
}
