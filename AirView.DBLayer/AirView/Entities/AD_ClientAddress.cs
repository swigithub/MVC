

using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    /*----Mubashar----*/

    public class AD_ClientAddress
    {
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public int AddressCityId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int ZipCode { get; set; }
        public bool IsHeadOffice { get; set; }
        public int ClientId { get; set; }
        public bool IsActive { get; set; }
        public List<AD_ClientAddress> Addresses{get;set;}
       
    }
}
