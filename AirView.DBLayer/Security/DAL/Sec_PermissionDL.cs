using System;
using System.Data;
using System.Data.SqlClient;

using SWI.Libraries.Common;
namespace SWI.Libraries.Security.DAL
{
    /*----MoB!----*/
    public class Sec_PermissionDL
    {
        public bool Save(DataTable dtSectors)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Insert_Psermissions");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result= DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@SECTORS", dtSectors));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public bool Manage(string Filter, Int64 id, Int64 parentId,string Title,string URL,string code,string icon,bool IsMenu=false,bool IsUsed=false,Int64 ModuleId=0, bool IsModule = false)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                if (string.IsNullOrEmpty(icon))
                {
                    icon = DBNull.Value.ToString();
                }

                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_ManagePsermissions");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filter,"@Id",id,"@ParentId",parentId,"@Title",Title,"@URL",URL,
                                                          "@IsMenuItem",IsMenu, "@Code",code, "@Icon", icon, "@IsUsed", IsUsed, "@ModuleId",ModuleId, "@IsModule",IsModule));
                DataContext.EndTransaction(loCommand);
                return result;
            }
            catch 
            {
                DataContext.CancelTransaction(loCommand);
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        //public bool Manage2(string Filer, int Id = 0, string value = null)
        //{
        //    SqlCommand loCommand = DataContext.OpenConnection();

        //    try
        //    {
        //        loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_Permission_Manage");
        //        loCommand = DataContext.StartTransaction(loCommand);
        //        bool result = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand, "@Filter", Filer, "@Id", Id, "@Value", value == null ? DBNull.Value.ToString() : value));
        //        DataContext.EndTransaction(loCommand);
        //        return result;
        //    }
        //    catch
        //    {
        //        DataContext.CancelTransaction(loCommand);
        //        throw;
        //    }
        //    finally
        //    {
        //        DataContext.CloseConnection(loCommand);
        //    }
        //}


        public DataTable Get(string filter, string value=null,string value2=null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetPermissions");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", value, "@Value2", value2));
            }
            catch(Exception )
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }


        public DataSet GetDataSet(string filter, string Value1 = null, string Value2 = null, string Value3 = null)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "Sec_GetPermissions");
                return DataContext.SelectMany(DataContext.AddParameters(loCommand, "@FILTER", filter, "@Value", Value1, "@Value2", Value2, "@Value3", Value3));
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
