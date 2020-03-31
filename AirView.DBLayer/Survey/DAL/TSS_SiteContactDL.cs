using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SWI.Survey.DAL
{
    /*----MoB!----*/
    /*----07-09-2017----*/
    public class TSS_SiteContactDL
    {
        public bool Manage(string Filter, DataTable List,bool IsSpecialAccess,DateTime? DateTime,string Instruction)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteContacts");
                loCommand = DataContext.StartTransaction(loCommand);
                var result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter, "@List", List, "@IsSpecialAccess",IsSpecialAccess, "@DateTime",DateTime, "@Instruction",Instruction));
                DataContext.EndTransaction(loCommand);
                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable GetContactInfo(string Filter, long SiteId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteContacts");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }
        public DataTable GetAccessInfo(string Filter, long SiteId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "TSS_ManageSiteContacts");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@SiteId", SiteId));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

    }
}
