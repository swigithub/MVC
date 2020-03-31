using System;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace WebApplication.Report
{
    public partial class NetLayerCrystal : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{

            //    UpdatePanel2.Visible = true;
            //}
            //else
            //{
            //    UpdatePanel2.Visible = false;
            //}
        }



        //protected void PDFbtn_Click(object sender, ImageClickEventArgs e)
        //{
        //    //string where = txtwhere.Text;
        //    ReportDocument reportDocument = new ReportDocument();


        //    string reportPath = Server.MapPath("~/Report/NetLayer.rpt");

        //    reportDocument.Load(reportPath);
        //    reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
        //    CrystalViewer1.ReportSource = reportDocument;


        //    reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime(txtfrom.Text));
        //    reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime(txtto.Text)); 
        //    reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
        //    reportDocument.SetParameterValue("@Where", "2017-01-01");
        //    reportDocument.ExportToHttpResponse
        //   (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NetworkSummary");
        //}

        //protected void Excelbtn_Click(object sender, ImageClickEventArgs e)
        //{

        //    ReportDocument reportDocument = new ReportDocument();


        //    string reportPath = Server.MapPath("~/Report/NetLayer.rpt");

        //    reportDocument.Load(reportPath);
        //    reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
        //    CrystalViewer1.ReportSource = reportDocument;


        //    reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime(txtfrom.Text));
        //    reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime(txtto.Text));
        //    reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
        //    reportDocument.SetParameterValue("@Where", "2017-01-01");
        //    reportDocument.ExportToHttpResponse
        //   (CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "NetworkSummary");
        //}

        //protected void Wordbtn_Click(object sender, ImageClickEventArgs e)
        //{
        //    try {
        //        ReportDocument reportDocument = new ReportDocument();


        //        string reportPath = Server.MapPath("~/Report/NetLayer.rpt");

        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
        //        CrystalViewer1.ReportSource = reportDocument;


        //        reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime(txtfrom.Text));
        //        reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime(txtto.Text));

        //        reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
        //        reportDocument.SetParameterValue("@Where", "2017-01-01");
        //        reportDocument.ExportToHttpResponse
        //       (ExportFormatType.WordForWindows, Response, true, "NetworkSummary");
        //    }
        //    catch (Exception) { }

        //}

        protected void PDF_Click(object sender, EventArgs e)
        {
            string where = txtwhere.Text;
            ReportDocument reportDocument = new ReportDocument();
            string reportPath = Server.MapPath("~/Report/NetLayer.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
            CrystalViewer1.ReportSource = reportDocument;
            reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime("2017-01-01"));
            reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime("2017-03-03"));
            
            reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
            reportDocument.SetParameterValue("@Where", where);
            reportDocument.ExportToHttpResponse
      (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NetworkSummary");

        }

        protected void EXCEL_Click(object sender, EventArgs e)
        {
            string where = txtwhere.Text;
            ReportDocument reportDocument = new ReportDocument();
            string reportPath = Server.MapPath("~/Report/NetLayer.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
            CrystalViewer1.ReportSource = reportDocument;
            reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime("2017-01-01"));
            reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime("2017-03-03"));
            reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
            reportDocument.SetParameterValue("@Where", where);
            reportDocument.ExportToHttpResponse
           (CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "NetworkSummary");
        }

        protected void WORD_Click(object sender, EventArgs e)
        {
            string where = txtwhere.Text;
            ReportDocument reportDocument = new ReportDocument();     
            string reportPath = Server.MapPath("~/Report/NetLayer.rpt");
            reportDocument.Load(reportPath);
            reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");
            CrystalViewer1.ReportSource = reportDocument;
            reportDocument.SetParameterValue("@FromDate", Convert.ToDateTime("2017-01-01"));
            reportDocument.SetParameterValue("@ToDate", Convert.ToDateTime("2017-03-03"));    
            reportDocument.SetParameterValue("@Filter", "NetLayer_WO_Status");
            reportDocument.SetParameterValue("@Where", where);
            reportDocument.ExportToHttpResponse
           (CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, "NetworkSummary");
        }
    }
}