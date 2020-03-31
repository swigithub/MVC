using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.DAL
{
 public   class AD_SurveyDL
    {
        public DataTable GetSurvey()
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_GetSurveyDocuments");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter","Get_All", "@Value",""));
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
