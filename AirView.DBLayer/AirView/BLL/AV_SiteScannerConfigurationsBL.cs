using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System.Collections.Generic;
using System.Data;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
  public  class AV_SiteScannerConfigurationsBL
    {
        public List<AV_SiteScannerConfigurations> ToList(string Filter, string Value1 = null, string Value2 = null, string Value3 = null, string Value4 = null, string Value5 = null, string Value6 = null, string Value7 = null)
        {

            AV_SiteScannerConfigurationsDL st = new AV_SiteScannerConfigurationsDL();
            DataTable dt = st.Get(Filter, Value1, Value2, Value3, Value4,Value5,Value6,Value7);
            return dt.ToList<AV_SiteScannerConfigurations>();
        }
    }
}
