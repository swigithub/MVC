using AirView.DBLayer.Project.Model;
using SWI.Libraries.AD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_KPI
    {
        public Int64 KPI { get; set; }
        public bool IsActive { get; set; }
        public Int64 TaskId { get; set; }
        public Int64 Technology { get; set; }
        public Int64 BandId { get; set; }
        public Int64 DataType { get; set; }
        public Int64 Level { get; set; }
        public string LevelName { get; set; }
        public string Kpi_Name { get; set; }
        public Int64 Kpi_Type { get; set; }
        public Int64 ComputedValue { get; set; }
        public string Formula { get; set; }
        public Int64 Weightage { get; set; }
        public string DataTypeName { get; set; }

     public   List<AD_Defination> Bands = new List<AD_Defination>();
    }
}
