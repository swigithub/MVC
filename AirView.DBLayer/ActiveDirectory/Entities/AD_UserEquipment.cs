using System;
namespace SWI.Libraries.AD.Entities
{
    /*----MoB!----*/
    public class AD_UserEquipment
    {
        public Int64 UEId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 UETypeId { get; set; }
        public string Device { get; set; }
        public string IMEI { get; set; }     
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string MAC { get; set; }
        public string UENumber { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }

        public string UserFullName { get; set; }
        public string UERefNo { get; set; }
        public Int64 UEOwnerId { get; set; }
    }
}
