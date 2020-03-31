using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.DAL
{
    public class AV_MarketConfigurationDL
    {
        public string SaveDateTemplate(string Filter, string MarketId, long ReportTypeId, long ClientId, long ScopeId, string TemplateName, string Files, long ProjectId, string ReportTemplateName, string jsonData, string keystring, int Value, long UserId = 0)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                var res = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@MarketId", MarketId,
                    "@ReportTypeId", ReportTypeId,
                    "@ClientId", ClientId,
                    "@ScopeId", ScopeId,
                    "@TemplateName", TemplateName,
                    "@Files", Files,
                    "@ProjectId", ProjectId,
                    "@JsonData", jsonData,
                    "@UserId", UserId,
                    "@ReportTemplateName", ReportTemplateName,
                    "@Keys", keystring,
                    "@Value", Value
                    ));
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SaveDataToDataBase(DataTable dt, string TableName)
        {
                string constr = ConfigurationManager.ConnectionStrings["DataTemplates"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        bulkCopy.DestinationTableName = TableName;
                        try
                        {
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(dt);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    con.Close();
                }
                return "";

        }
        public bool CreateTable(string query)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["DataTemplates"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckForColumns(string query)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["DataTemplates"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckExistence(string TemplateName)
        {
            bool res = false;
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(
                    DataContext.AddParameters(loCommand,
                        "@Filter", "CheckDuplication",
                        "@TemplateName", TemplateName));
                DataContext.EndTransaction(loCommand);
                res = false;

            }
            catch (Exception ex)
            {
                res = true;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
            //    string constr = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;
            //    string query = "SELECT 1 FROM AV_DataTemplates WHERE TemplateName ='" + TemplateName + "'";
            //    // Replace dots(.) with dashes('_');
            //    query = query.Replace('.', '_');
            //    using (SqlConnection con = new SqlConnection(constr))
            //    {
            //        con.Open();
            //        DataTable dt = new DataTable();
            //        SqlDataAdapter adot = new SqlDataAdapter(query, con);
            //        adot.Fill(dt);
            //        if (dt.Rows.Count > 0)
            //        {
            //            res = true;
            //        }
            //        else
            //        {
            //            res = false;
            //        }
            //        con.Close();
            //    }
            //    return res;
            //}
            //catch (Exception ex)
            //{
            //    return res;
            //}
            return res;
        }

        public string SavePlotLegends(string Filter, DataTable RFPLotLegends)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageRFPlotLegends");
                loCommand = DataContext.StartTransaction(loCommand);
                bool result = DataContext.ExecuteNonQuery(
                    DataContext.AddParameters(loCommand,
                        "@Filter", Filter,
                        "@List", RFPLotLegends));
                DataContext.EndTransaction(loCommand);
                return "success";

            }
            catch (Exception ex)
            {
                return "failed";
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public DataTable GetTemplateById(string Filter, int Id)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                return DataContext.Select(DataContext.AddParameters(loCommand, "@Filter", Filter, "@Value", Id));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }
        }

        public string DeleteDataTemplate(string Filter, int DataTemplateId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                var res = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@Value", DataTemplateId
                    ));
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public DataTable GetDataTemplate(string Filter, long ClientId, long ProjectId, long MarketId, long ScopeId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                return DataContext.Select(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@MarketId", MarketId,
                    "@ClientId", ClientId,
                    "@ScopeId", ScopeId,
                    "@ProjectId", ProjectId
                    ));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }

        }
        public DataTable GetWorkOrderById(string Filter, long SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                return DataContext.Select(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@Value", SiteId
                    ));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DataContext.CloseConnection(loCommand);
            }

        }
        public bool CheckFiles(string Filter, long SiteId, string FileName, string TemplateName)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                var res = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@Value", SiteId,
                    "@Keys", FileName,
                    "@TemplateName", TemplateName
                    ));
                return false; // File Not Exists
            }
            catch (Exception ex)
            {
                return true;// File true
            }
        }
        public DataTable Get(string Filter, string MarketId, long ClientId, long ProjectId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();

            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                return DataContext.Select(DataContext.AddParameters(loCommand,
                   "@Filter", Filter,
                    "@MarketId", MarketId,
                    "@ClientId", ClientId,
                    "@ProjectId", ProjectId
                    ));
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

        public bool CheckForReportTemplate(string Filter, int SiteId)
        {
            SqlCommand loCommand = DataContext.OpenConnection();
            try
            {
                loCommand = DataContext.SetStoredProcedure(loCommand, "AV_ManageDataTemplate");
                var res = DataContext.ExecuteNonQuery(DataContext.AddParameters(loCommand,
                    "@Filter", Filter,
                    "@Value", SiteId
                    ));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
