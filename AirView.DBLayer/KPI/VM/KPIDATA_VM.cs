using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.KPI.VM
{
    public class KPIDATA_VM
    {
        public Int64 Site { get; set; }

        public Int64 TaskId { get; set; }
        public Int64 Carrier { get; set; }
        public Int64 Sector { get; set; }
        public Int64 KPI_Id { get; set; }
        public DateTime Date { get; set; }
        public dynamic KPI_Value { get; set; }
        public string DataTypeName { get; set; }

        public string KpiType { get; set; }
        public string KpiName { get; set; }
        public Int64 KPIData_Id { get; set; }

        public string Formula { get; set; }
    }
}
