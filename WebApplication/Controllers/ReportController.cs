using iTextSharp.text;
using iTextSharp.text.pdf;

using SWI.Libraries.AirView.BLL;

using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SWI.Libraries.AD.BLL;

namespace WebApplication.Controllers
{
    [IsLogin]
    public class ReportController : Controller
    {
        // GET: Report

           
        public ActionResult Index1()
        {
            AD_DefinationBL db = new AD_DefinationBL();
         
            ViewBag.Region = db.ToList("Regions") .GroupBy(l => l.DefinationName)
                                        .Select(cl => new  {
                                             Name = cl.First().DefinationName
                                        }).ToList();

            return View();
        }
        public ActionResult Inde2x()
        {
            AD_DefinationBL db = new AD_DefinationBL();
            //
            var rec = db.ToList("ReportTree");
            return View(rec);
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FilterQuery(string value)
        {
            AD_FilterQueryBL fqb = new AD_FilterQueryBL();
            var rec = fqb.ToList("GET_QUERY_RESULT_ByFilterId", value);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportFilters(Int64 ReportId)
        {
            AD_DefinationBL db = new AD_DefinationBL();
            var rec = db.ToList("ReportFilters_byReportId", ReportId.ToString());
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Definations(string Filter) {
            try
            {
                AD_DefinationBL db = new AD_DefinationBL();
                return Json(db.ToList(Filter), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        

        public ActionResult ToPDF()
        {
            if (TempData["Report"] != null)
            {
                var rec = TempData["Report"] as List<AV_SiteTestLog>;
                TempData["Report"] = rec;
                var dt = rec.Select(m=>new {m.Site,m.TestType,m.NetworkMode,m.Band,m.Carrier,m.Scope }).ToList().ToDataTable();
                Document pdfDoc = new Document();
                MemoryStream pdfStream = new MemoryStream();
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);

                pdfDoc.Open();//Open Document to write
                pdfDoc.NewPage();

                Font font8 = FontFactory.GetFont("ARIAL", 7);

                PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                PdfPCell PdfPCell = null;

                //Add Header of the pdf table
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[column].Caption, font8)));
                    PdfTable.AddCell(PdfPCell);
                }

                //How add the data from datatable to pdf table
                for (int rows = 0; rows < dt.Rows.Count; rows++)
                {
                    for (int column = 0; column < dt.Columns.Count; column++)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                        PdfTable.AddCell(PdfPCell);
                    }
                }

                PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table            
                pdfDoc.Add(PdfTable); // add pdf table to the document
                pdfDoc.Close();
                pdfWriter.Close();


                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=gridexport.pdf");
                Response.BinaryWrite(pdfStream.ToArray());
                Response.End();
            }

            return null;
        }
        public ActionResult ToExcel() {
            var grid = new GridView();

            if (TempData["Report"] != null)
            {
                var rec = TempData["Report"] as List<AV_SiteTestLog>;
                TempData["Report"] = rec;
                grid.DataSource = rec;
                grid.DataBind();

                //grid.HeaderRow.Cells[0].Style.Add("background-color", "green");
                //grid.HeaderRow.Cells[1].Style.Add("background-color", "green");
                //grid.HeaderRow.Cells[2].Style.Add("background-color", "green");
                //grid.HeaderRow.Cells[3].Style.Add("background-color", "green");

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Purchase History.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }



            return null;
        }
        public ActionResult data(string filter,string RemovCols, string sord, int page, int rows)
        {
            try
            {
                List<AV_SiteTestLog> Results=new List<AV_SiteTestLog>();
                AV_ReportBL rb = new AV_ReportBL();
                Results = rb.ToList(filter, RemovCols);
                TempData["Report"] = Results;



                int pageIndex = Convert.ToInt32(page) - 1;
                int pageSize = rows;
                //var Results = rec.ToList();
                    //.Select(
                //        a => new
                //        {
                //            a.Site,
                //            a.Region,
                //            a.City,
                //            a.TestType,
                //            a.Sector,
                //            a.NetworkMode,
                //        });
                int totalRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                if (sord.ToUpper() == "DESC")
                {
                    Results =Results.OrderByDescending(s => s.TestType).ToList();
                    Results = Results.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    Results = Results.OrderBy(s => s.TestType).ToList();
                    Results = Results.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = Results
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return null;
            }
            
        }

       


        




    }
}