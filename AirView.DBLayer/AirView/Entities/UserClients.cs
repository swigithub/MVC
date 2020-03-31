
namespace AirView.DBLayer.AirView.Entities
{
   public class UserClients
    {
        public decimal UserId { get; set; }
        public decimal ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
