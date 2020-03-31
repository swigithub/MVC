using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Template.Model
{
    public class MetaData
    {
        public string ColumnName { get; set; }
        public string Alias { get; set; }
        public string Funtion { get; set; }
        public bool GroupBy { get; set; }
        public string SortBy { get; set; }
        public string DataType { get; set; }
    }
}
