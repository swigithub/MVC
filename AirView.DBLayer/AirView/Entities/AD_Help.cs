using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
    public class AD_Help
    {
        public int HelpId { get; set; }
        public int ParentId { get; set; }
        [Required]
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        [Required]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        [Required]
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
