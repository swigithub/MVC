using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----Mubashar----*/
    public class AD_ClientContacts
    {
        public int ContactId { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public decimal ContactNo { get; set; }
        public string ContactType { get; set; }
        public bool IsPrimary { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int ContactCityId { get; set; }
        public bool IsActive { get; set; }
        public int? ReportToId { get; set; }
        public List<AD_ClientContacts> Client { get; set; }
    }
}
