using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_Entity
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Plural { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
