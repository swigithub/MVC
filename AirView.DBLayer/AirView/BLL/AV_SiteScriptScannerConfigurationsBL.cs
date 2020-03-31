using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Template.DAL;
using AirView.DBLayer.Template.Model;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
    public class AV_SiteScriptScannerConfigurationsBL
    {
        AV_SiteScriptScannerConfigurationsDL scDL = new AV_SiteScriptScannerConfigurationsDL();
        public int Manage(string filter, AV_SiteScriptScannerConfigurations sc)
        {
            return scDL.Manage(filter, sc);
        }

        public List<AV_SiteScriptScannerConfigurations> ToList(string Filter, string Value = null)
        {
            DataTable dt = scDL.Get(Filter, Value);
            return dt.ToList<AV_SiteScriptScannerConfigurations>();
        }
    }
}
