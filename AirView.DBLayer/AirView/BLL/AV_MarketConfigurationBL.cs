using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AirView.DBLayer.AirView.BLL
{
    public class AV_MarketConfigurationBL
    {
        AV_MarketConfigurationDL dd = new AV_MarketConfigurationDL();
        public string SaveDataTemplate(string Filter, string MarketId, long ReportTypeId, long ClientId, long ScopeId, string TemplateName, string Files, long ProjectId, string ReportTemplateName, string jsonData, string keystring, int Value, long UserId = 0)
        {
            return dd.SaveDateTemplate(Filter, MarketId, ReportTypeId, ClientId, ScopeId, TemplateName, Files, ProjectId, ReportTemplateName, jsonData, keystring, Value, UserId);
        }

        public bool CreateDataTemplateTable(bool IsUpdate, string TemplateName, DataColumnCollection dtColumns)
        {
            if (IsUpdate)
            {
                return CheckForColumns(dtColumns, TemplateName);
            }
            else
            {
                string Columns = getColumns(dtColumns);
                string query = "";
                query += "IF OBJECT_ID('" + TemplateName + "', 'U') IS NULL ";
                query += "BEGIN ";
                query += "CREATE TABLE " + TemplateName + "(";
                query += Columns;
                query += ")";
                query += " END";
                // Replace dots(.) with dashes('_');
                query = query.Replace('.', '_');

                return dd.CreateTable(query);
            }
        }
        public string SaveDataToDataBase(DataTable dt, string TemplateName, long SiteId)
        {
            DataColumn newCol = new DataColumn("SiteId", typeof(long));
            dt.Columns.Add(newCol);
            dt.Columns["SiteId"].SetOrdinal(0);
            foreach (DataRow row in dt.Rows)
            {
                row["SiteId"] = SiteId;
            }
            // Change Dt Columns / Replace for Dots with _;
            foreach (var a in dt.Columns)
            {
                var newColumnName = a.ToString().Replace('.', '_'); // Replacing Dot with UnderScore
                dt.Columns[a.ToString()].ColumnName = newColumnName;
            }
            bool res = CheckForColumns(dt.Columns, TemplateName);
            if (res)
            {
                return dd.SaveDataToDataBase(dt, TemplateName);
            }
            else
            {
                return "Failed To Alter Columns";
            }
        }

        public bool CheckForColumns(DataColumnCollection columns, string TableName)
        {
            // Check if Columns Exists Add if Not Exist
            string query = "";
            foreach (var c in columns)
            {
                query += "IF COL_LENGTH('" + TableName + "', '" + c.ToString() + "') IS NULL ";
                query += "BEGIN ";
                query += "ALTER TABLE " + TableName + " ADD " + c.ToString() + " NVARCHAR(1000)";
                query += "END;";
            }
            return dd.CheckForColumns(query);
        }


        public bool checkExistence(string TemplateName)
        {
            return dd.CheckExistence(TemplateName);
        }

        public string SaveRFPlotLegend(FormCollection frm)
        {
            // Do Stuff Here 
            var msg = "";
            List<string> cities = frm["CityId"].ToString().Split(',').ToList();
            foreach (var city in cities)
            {
                DataTable RFPlotLegends = new DataTable();
                #region Datatable Columns
                RFPlotLegends.Columns.AddRange(new DataColumn[7]
                {
                                    new DataColumn("ClientId", typeof (int)),
                                    new DataColumn("CityId", typeof (int)),
                                    new DataColumn("NetworkModeId", typeof (int)),
                                    new DataColumn("PlotTypeId", typeof (int)),
                                    new DataColumn("RangeFrom", typeof (float)),
                                    new DataColumn("RangeTo", typeof (float)),
                                    new DataColumn("Color", typeof (string))
                });
                #endregion
                try
                {
                    #region Form Keys
                    string ClientId = frm["ClientId"].ToString();
                    string NetworkModeId = frm["NetworkModeId"].ToString();
                    string CityId = city.ToString();
                    string PlotTypeId = frm["PlotTypeId"].ToString();

                    List<string> RangeFrom = frm["RangeFrom"].ToString().Split(',').ToList();
                    List<string> RangeTo = frm["RangeTo"].ToString().Split(',').ToList();
                    List<string> Color = frm["Color"].ToString().Split(',').ToList();

                    string Count = frm["Count"].ToString();
                    #endregion

                    for (int i = 0; i < Convert.ToInt32(Count); i++)
                    {
                        #region Add Row In RFPlotLegends
                        DataRow row;
                        row = RFPlotLegends.NewRow();
                        row["ClientId"] = ClientId;
                        row["CityId"] = CityId;
                        row["NetworkModeId"] = NetworkModeId;
                        row["PlotTypeId"] = PlotTypeId;
                        row["RangeFrom"] = RangeFrom[i];
                        row["RangeTo"] = RangeTo[i];
                        row["Color"] = Color[i];
                        RFPlotLegends.Rows.Add(row);
                        #endregion
                    }
                    AV_MarketConfigurationDL mcdl = new AV_MarketConfigurationDL();
                    msg = mcdl.SavePlotLegends("INSERT", RFPlotLegends);


                }
                catch (Exception ex)
                {
                    msg = "Failed! Please Fill the form Correctly.";
                }
            }
            return msg;
        }

        public DataTable GetTemplateById(string Filter, int Id)
        {
            return dd.GetTemplateById(Filter, Id);
        }
        public string DeleteDataTemplate(string Filter, int DataTemplateId)
        {
            return dd.DeleteDataTemplate(Filter, DataTemplateId);
        }

        public DataTable GetDataTemplate(string Filter, long ClientId, long ProjectId, long MarketId, long ScopeId)
        {
            // Parse File Here
            return dd.GetDataTemplate(Filter, ClientId, ProjectId, MarketId, ScopeId);
        }
        public DataTable GetWorkOrderById(string Filter, long SiteId)
        {
            return dd.GetWorkOrderById(Filter, SiteId);
        }
        public bool CheckFiles(string Filter, long SiteId, string FileName, string TemplateName)
        {
            return dd.CheckFiles(Filter, SiteId, FileName, TemplateName);
        }
        public List<SelectedList> SelectedList(string Filter, string MarketId, long ClientId, long ProjectId)
        {
            string Message = null;
            SelectedList sl = new SelectedList();

            var rec = ToList(Filter, MarketId, ClientId, ProjectId).Select(m => new SelectedList { Text = m.DefinationName, Value = m.DefinationId.ToString() }).ToList();
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }

            return rec;
        }

        public List<AD_Defination> ToList(string Filter, string MarketId, long ClientId, long ProjectId)
        {

            DataTable dt = dd.Get(Filter, MarketId, ClientId, ProjectId);

            List<AD_Defination> lst = dt.ToList<AD_Defination>();


            return lst;
        }

        public bool CheckForReportTemplate(int SiteId)
        {
            return dd.CheckForReportTemplate("GetReportStatus", SiteId);
        }
        public string getColumns(DataColumnCollection dtColumns)
        {
            var columns = "SiteId NUMERIC(18,0),";
            foreach (var col in dtColumns)
            {
                columns += col + " Nvarchar(1000),";
            }
            return columns;
        }
    }
}
