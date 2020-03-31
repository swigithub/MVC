using SWI.Libraries.Common;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SWI.Libraries.AirView.DAL
{
    /*----MoB!----*/
  public  class AV_NetLayerReportExportDL
    {

        public DataTable Get(string DateFilter ,DateTime fromDate,DateTime toDate,string woStatus, string Panel1Filter, string Panel1Value, string Panel2Filter, string Panel2Value,string ReportFilter,Int64 UserId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_rptNetLayer");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@DateFilter", DateFilter, "@fromDate", fromDate, "@toDate", toDate, "@woStatus", woStatus, "@Panel1Filter",
                                                                    Panel1Filter, "@Panel1Value", Panel1Value, "@Panel2Filter", Panel2Filter, "@Panel2Value", Panel2Value,
                                                                    "@ReportFilter", ReportFilter, "@UserId", UserId));
            }
            catch
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
    }
}
