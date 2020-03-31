using System;
using System.Collections.Generic;


namespace SWI.Libraries.AirView.Entities
{
    public class TestTypeVM
    {
        public TestTypeVM()
        {
            TestKpi = new List<TestKpiVM>();
        }
        public string TestTypeName { get; set; }
        public Int64 TestTypeId { get; set; }
        public List<TestKpiVM> TestKpi { get; set; }
    }
}
