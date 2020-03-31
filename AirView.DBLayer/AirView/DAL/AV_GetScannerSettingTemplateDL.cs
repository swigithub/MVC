using SWI.Libraries.Common;
using System.Data;
using System.Data.SqlClient;

namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
  public  class AV_GetScannerSettingTemplateDL
    {
        public DataTable Get(string Filter = null, string value1 = null, string value2=null, string value3 = null, string value4 = null, string value5 = null, string value6 = null, string value7 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_GetScannerSettingTemplate");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@value1", value1, "@value2", value2, "@value3", value3, "@value4", value4, "@value5", value5, "@value6", value6, "@value7", value7));
            }
            catch
            {
                return null;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
