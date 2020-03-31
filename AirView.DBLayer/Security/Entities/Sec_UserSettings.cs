using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Security.Entities
{
   public  class Sec_UserSettings
    {
         public Int64 UserSettingId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 TypeId { get; set; }
        public Int64 TypeValue { get; set; }
        public int EmailPIN { get; set; }
        public int MobilePIN { get; set; }
        public DateTime PinGenerateDate { get; set; }
        public bool IsRequested { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsRequestApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime DownloadDate { get; set; }
        public Int64 UEId { get; set; }                      
    }
}
