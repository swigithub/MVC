using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
    public class PM_TaskStages
    {
        public int StageId { get; set; } = 0;
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; } = 0;
        public long ProjectId { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public long TaskId { get; set; } = 0;
    }
}
