using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class ChartSettings
    {
       public Int64 SrId  { get; set; }
        public Int64 ProjectId { get; set; }
        public string PanelName { get; set; }
        public string ChartName { get; set; }
        public string ChartType { get; set; }
        public string DataSeries { get; set; }
        public string ColorCode { get; set; }
        public string SeriesType { get; set; }



        public Int64 TaskId { get; set; }
        public string Color { get; set; }
    }
  
}
