//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirView.DBLayer.EFModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AV_LogsInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AV_LogsInfo()
        {
            this.AV_NemoSiteLogs = new HashSet<AV_NemoSiteLogs>();
        }
    
        public int fileID { get; set; }
        public string fileName { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string pathFile { get; set; }
        public Nullable<int> siteID { get; set; }
        public Nullable<int> sectorID { get; set; }
        public Nullable<int> networkModeID { get; set; }
        public Nullable<int> bandID { get; set; }
        public Nullable<int> carrierID { get; set; }
        public Nullable<int> scopeID { get; set; }
        public string fileType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AV_NemoSiteLogs> AV_NemoSiteLogs { get; set; }
    }
}
