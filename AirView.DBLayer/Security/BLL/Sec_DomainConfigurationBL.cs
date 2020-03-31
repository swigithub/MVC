using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System.Data;


namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
    public class Sec_DomainConfigurationBL
    {
        Sec_DomainConfigurationDL dcd = new Sec_DomainConfigurationDL();
        public Sec_DomainConfiguration Single(string filter, string Value = null)
        {

            DataTable dt = dcd.Get(filter);
            Sec_DomainConfiguration dc = new Sec_DomainConfiguration();

            if (dt != null && dt.Rows.Count > 0)
            {
                dc.DomainUrl = dt.Rows[0]["DomainUrl"].ToString();
                dc.FtpUrl = dt.Rows[0]["FtpUrl"].ToString();
                dc.FtpPort = dt.Rows[0]["FtpPort"].ToString();
                dc.FtpUsername = dt.Rows[0]["FtpUsername"].ToString();
                dc.FtpPassword = dt.Rows[0]["FtpPassword"].ToString();
                dc.FtpUploadPath = dt.Rows[0]["FtpUploadPath"].ToString();
                dc.VideoURL = dt.Rows[0]["VideoURL"].ToString();
                dc.MessageService = dt.Rows[0]["MessageService"].ToString();

                dc.MediaServer = dt.Rows[0]["MediaServer"].ToString();
                dc.LTServer = dt.Rows[0]["LTServer"].ToString();
                dc.LTServerFilePath = dt.Rows[0]["LTServerFilePath"].ToString();
                dc.SecDomainURL = dt.Rows[0]["SecDomainURL"].ToString();
                dc.IperfServerIP = dt.Rows[0]["IperfServerIP"].ToString();
                dc.IperfServerPort = dt.Rows[0]["IperfServerPort"].ToString();
                

            }

            return dc;
        }
    }
}
