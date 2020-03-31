using SWI.Libraries.AD.Entities;
using System;


namespace SWI.Libraries.AirView.Entities
{
    public class SectorsVM
    {
        public SectorsVM()
        {
            NetworkMode = new AD_Defination();
            Scope = new AD_Defination();
            Band = new AD_Defination();
            Carrier = new AD_Defination();
            TestResult = new TestResultVm();
        }
        public Int64 SectorId { get; set; }
        public string SectorCode { get; set; }
        public AD_Defination NetworkMode { get; set; }
        public AD_Defination Scope { get; set; }
        public AD_Defination Band { get; set; }
        public AD_Defination Carrier { get; set; }
        public string Antenna { get; set; }
        public float BeamWidth { get; set; }
        public double Azimuth { get; set; }

        public decimal PingKpi { get; set; }
        public decimal DLKpi { get; set; }
        public decimal ULKpi { get; set; }
        public decimal AvgPing { get; set; }
        public decimal MaxDL { get; set; }
        public decimal MaxUL { get; set; }

        public int PCI { get; set; }

        /*----MoB!----*/
        public string Client { get; set; }
        public string NetType { get; set; }
        public TestTypeVM Test { get; set; }
        public TestResultVm TestResult { get; set; }

        public string SiteCode { get; set; }
        public string ClientPrefix { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 CityId { get; set; }




    }
}