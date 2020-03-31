using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
 public   class MP_Import_WR_Issues
    {
        //15
        public string FACode { get; set; }
        public string eNB { get; set; }
        public string othereNB { get; set; }
        public string Schedule { get; set; }
        public string Actual { get; set; }
        public string MW { get; set; }
        public string Status { get; set; }
        public string Alarm { get; set; }
        public string Issues { get; set; }
        public string WhoFix { get; set; }
        public string Notes { get; set; }
        public string ContentType { get; set; }
        public string Attachments { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }

    }
}
