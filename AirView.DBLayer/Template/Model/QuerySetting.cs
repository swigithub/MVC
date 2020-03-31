using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Template.Model
{
    public class QuerySetting
    {
       public string Table { get; set; }
       public List<MetaData> MetaData { get; set; }
    }
}
