using System;
using System.Collections.Generic;

namespace SWI.Libraries.AirView.Entities
{
    public class TestCategoryVM
    {
        public TestCategoryVM()
        {
            TestTypes = new List<TestTypeVM>();
        }
        public Int64 TestCategoryId { get; set; }
        public string TestCategoryName { get; set; }
        public List<TestTypeVM> TestTypes { get; set; }
    }
}
