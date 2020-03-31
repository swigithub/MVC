using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.Model
{
    /*----MoB!----*/
    /*----11-08-2017----*/
    public class RequiredActions
    {
        public Int64 ActionId { get; set; }
        public Int64 SiteQuestionId { get; set; }
        public Int64 SiteSectionId { get; set; }
        public Int64 ActionTypeId { get; set; }
        public string RequiredAction { get; set; }
        public string Remarks { get; set; }
        public string ActionType { get; set; }
        public string Name { get; set; }
        public bool IsDBExist { get; set; }
        
        public string Azimuth { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        
        public string Altitude { get; set; }
        public string GPSAccuracy { get; set; }
        public string ObjectView { get; set; }
    }

    public enum ActionType
    {
        Notes = 1,
        Image =2,
        Barcode =3,
        Video=4,
        Document=5,
        Audio=6,
        Signature =99,
    }
}
