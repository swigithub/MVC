using System;

namespace SWI.Libraries.AirView.Entities
{
  public  class TestConfiguration
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public int RevisionId { get; set; }
        public DateTime ConfigurationDate { get; set; }
        public int TestTypeId { get; set; }
        public int KpiId { get; set; }
        public string KpiValue { get; set; }
        public bool IsActive { get; set; }
        public int NetworkModeId { get; set; }
        public int BandId { get; set; }
        public int CarrierId { get; set; }

    }
}
