using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Configuration;
namespace WebApplication.Report
{
    public partial class TesterSites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                UpdatePanel2.Visible = true;
            }
            else
            {
                UpdatePanel2.Visible = false;
            }
        }
        private void ShowReport()
        {

            ReportViewer1.Reset();

            string where = txtwhere.Text;
            DataTable dt = GetData(where);

            ReportDataSource rds = new ReportDataSource("TesterSitesDataSet", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/TesterSites.rdlc");
            ReportParameter[] rptpara = new ReportParameter[] {

            new ReportParameter("Where",where)
        };
            ReportViewer1.LocalReport.SetParameters(rptpara);
            ReportViewer1.LocalReport.Refresh();
        }
        private DataTable GetData(string where)
        {
            where = txtwhere.Text;
            DataTable dt = new DataTable();

            string con = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand("AV_rptTesterSites", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Where", SqlDbType.NVarChar).Value = where;
                cmd.Parameters.Add("@Filter", SqlDbType.VarChar).Value = "Tester_Sites";
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }

        protected void GetReports_Click(object sender, EventArgs e)
        {
            ShowReport();
        }
    }
}