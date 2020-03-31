using Library.SWI.Survey.Model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Survey.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Survey/Exce
        int GlobalRows = 0;
        int StartCol = 1;
        int NameCounter = 1;
        public FileResult ExportToExcel(TSS_SurveyDocument sur)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sur.SurveyTitle);
                ws.Column(StartCol + 1).Width = 35;
                ws.Column(StartCol + 2).Width = 30;
                ws.Column(StartCol + 3).Width = 30;

                ws.Column(StartCol + 1).Style.WrapText = true;
                ws.Column(StartCol + 2).Style.WrapText = true;
                ws.Column(StartCol + 3).Style.WrapText = true;

                var title = ws.Cells[1, StartCol + 1].RichText.Add("Wo Ref# : ");
                title.Bold = true;
                var info = ws.Cells[1, StartCol + 1].RichText.Add(sur.WoRefId);
                info.Bold = false;

                var title3 = ws.Cells[1, StartCol + 3].RichText.Add("Site Code : ");
                title3.Bold = true;
                var info3 = ws.Cells[1, StartCol + 3].RichText.Add(sur.SiteCode);
                info3.Bold = false;

                var title4 = ws.Cells[2, StartCol + 1].RichText.Add("Client : ");
                title4.Bold = true;
                var info4 = ws.Cells[2, StartCol + 1].RichText.Add(sur.ClientName);
                info4.Bold = false;

                var title5 = ws.Cells[2, StartCol + 3].RichText.Add("Survey : ");
                title5.Bold = true;
                var info5 = ws.Cells[2, StartCol + 3].RichText.Add(sur.SurveyTitle);
                info5.Bold = false;

                var title6 = ws.Cells[3, StartCol + 1].RichText.Add("Region : ");
                title6.Bold = true;
                var info6 = ws.Cells[3, StartCol + 1].RichText.Add(sur.Region);
                info6.Bold = false;

                var title2 = ws.Cells[3, StartCol + 3].RichText.Add("Market : ");
                title2.Bold = true;
                var info2 = ws.Cells[3, StartCol + 3].RichText.Add(sur.CityName);
                info2.Bold = false;

                ApplyBordersMultiple(ws, 1, 1, 3, 4);

                GlobalRows = 3;


                int iteration = 0;
                foreach (var sec in sur.Sections)
                {
                    int EmptyRowsCount = GetEmptyRowsCount(ws, GlobalRows + iteration, StartCol + 1);
                    GlobalRows = GlobalRows - EmptyRowsCount;
                    iteration = iteration + 1;
                    StyleFunction(GlobalRows + iteration, ws);
                    ws.Cells[GlobalRows + iteration, StartCol + 2].Value = sec.SectionTitle;
                    ws.Row(GlobalRows + iteration).Height = 15;
                    ws.Row(GlobalRows + iteration).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //ws.Cells[GlobalRows + iteration, StartCol + 2].Style.HorizontalAlignment =ExcelHorizontalAlignment.Center;

                    int questionIteration = 0;
                    foreach (var qu in sec.Questions)
                    {
                        questionIteration = questionIteration + 1;

                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, 1);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 1);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 2);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 3);

                        //string questionText = qu.Question + "\n" + qu.Description;
                        var Quest = ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].RichText.Add(qu.Question+"\n");   
                        var Description = ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].RichText.Add(qu.Description);
                        Description.Italic = true;
                        Description.Size = 9;
                       // ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].Value = questionText;


                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Value = questionIteration;
                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        if (qu.QuestionType != "Table")
                        {
                            if (qu.QuestionType == "Direction & GPS Based Images" || qu.QuestionType == "DateTime")
                            {
                                foreach (var r in qu.Responses)
                                {
                                    r.IsChecked = true;
                                }
                            }
                            var Res = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                            if (Res != null)
                            {
                                if (Res.ResponseId == 0)
                                {
                                    string value = Res.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                                else
                                {
                                    string value = Res.ResponseText;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if(qu.QuestionType=="Multi Select")
                            {
                                var AllOptions = qu.Responses.Where(r => r.IsChecked == true).ToList();
                                if (AllOptions != null)
                                {
                                    List<string> Responses=new List<string>();
                                    
                                    foreach(var option in AllOptions)
                                    {
                                        if (option.ResponseId == 0)
                                        {
                                            Responses.Add(option.ResponseValue.ToString());
                                        }
                                        else
                                        {
                                            Responses.Add(option.ResponseText.ToString());
                                        }
                                    }
                                    string[] arrRes = new string[Responses.Count];
                                    for (int i = 0; i < Responses.Count; i++)
                                    {
                                        arrRes[i] = Responses[i].ToString();
                                    }
                                        string value = string.Join(",", arrRes);
                                        value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Image Options")
                            {
                                var sres = qu.Responses.FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Rating" || qu.QuestionType == "Scale")
                            {
                                var sres = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Range")
                            {
                                var sres = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value = "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = sres.MinValue + "," + sres.MaxValue;
                                }
                            }
                            if (qu.QuestionType == "Direction & GPS Based Images")
                            {

                                if (qu.MapImage != "" && qu.MapImage != null)
                                {

                                    ws.Cells[GlobalRows + iteration + questionIteration, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    Image logo = Base64ToImage(qu.MapImage);

                                    var size = new Size();
                                   
                                        size.Height = 750;
                                        size.Width = 800;

                                        var newimg = new Bitmap(logo, size);
                                    Random rand = new Random();
                                    var randomId = rand.Next(0, 999999999);
                                    dynamic picture;
                                    try
                                    {
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    catch
                                    {
                                        randomId = rand.Next(0, 88888888);
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString()+sec.SiteSectionId.ToString(), newimg);
                                    }
                                        picture.SetPosition(GlobalRows + iteration + questionIteration - 1, 5, 2, 5);
                                        ws.Row(GlobalRows + iteration + questionIteration).Height = 300;
                                        picture.SetSize(410, 340);
                                  
                                }

                            }
                            if (qu.QuestionType == "Signature")
                            {

                                var sigres = qu.Responses.Where(x => x.Signature != null && x.Signature != "").FirstOrDefault();
                                if (sigres != null)
                                {
                                    ws.Row(GlobalRows + iteration + questionIteration).Height = 90;
                                    ws.Cells[GlobalRows + iteration + questionIteration, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    Image logo = Base64ToImage(sigres.Signature);
                                    var size = new Size();
                                    size.Height = 600;
                                    size.Width = 600;
                                    var newimg = new Bitmap(logo, size);
                                    Random rand = new Random();
                                    var randomId = rand.Next(0, 999999999);
                                    dynamic picture;
                                    try
                                    {
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    catch
                                    {
                                        randomId = rand.Next(0, 88888888);
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    picture.SetPosition(GlobalRows + iteration + questionIteration - 1, 5, 2, 5);
                                    picture.SetSize(200, 100);
                                }


                            }

                        }
                        if (qu.QuestionType == "Table")
                        {
                            GlobalRows = GlobalRows + 1;
                            int getRowsCount = 0;
                            int col = qu.Responses.Count;
                            StyleFunctionTable(GlobalRows + iteration + questionIteration, StartCol + col, ws);
                            int colNo = StartCol + 1;
                            int row = GlobalRows + iteration + questionIteration;
                            foreach (var tres in qu.Responses)
                            {
                                ws.Cells[row, colNo].Value = tres.ResponseText;
                                ws.Cells[row, colNo].Style.Font.Color.SetColor(Color.Black);
                                ws.Cells[row, colNo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[row, colNo].Style.Font.Bold = true;
                                string[] values = new string[] { };
                                if (tres.IsReadOnly)
                                {
                                    values = tres.UserValues.Split(',');
                                }
                                else
                                {
                                    values = tres.SelectedRawResponse.Split('|');
                                }
                                int counter = 1;
                                if (values.Length > getRowsCount)
                                {
                                    getRowsCount = values.Length;
                                }
                                foreach (var val in values)
                                {
                                    ws.Cells[row + counter, colNo].Value = val.ToString();
                                    counter = counter + 1;
                                }

                                colNo = colNo + 1;

                            }
                            ApplyBordersMultiple(ws, GlobalRows + iteration + questionIteration, 1, GlobalRows + iteration + questionIteration + getRowsCount, colNo-1);
                            MergeRange(ws, (GlobalRows + iteration + questionIteration) - 1, 1, GlobalRows + iteration + questionIteration + getRowsCount, 1);

                            GlobalRows = GlobalRows + getRowsCount;

                        }

                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        if (qu.QuestionType == "Direction & GPS Based Images")
                        {
                            if (qu.IsNoteRequired)
                            {
                                foreach (var req in qu.ReqActions)
                                {
                                    if (req.ActionType == "Notes_Required")
                                    {
                                        if (!string.IsNullOrEmpty(req.RequiredAction))
                                        {
                                            ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = req.RequiredAction + " (" + qu.Azimuth + ")";
                                        }
                                        else
                                        {
                                            ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = qu.Azimuth;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = qu.Azimuth;
                            }
                        }
                        else
                        {
                            if (qu.IsNoteRequired)
                            {
                                foreach (var req in qu.ReqActions)
                                {
                                    if (req.ActionType == "Notes_Required")
                                    {
                                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = req.RequiredAction;
                                    }

                                }
                            }
                        }
                        
                    }
                    GlobalRows = GlobalRows + questionIteration;
                    // For Signature

                    if (sec.Signature != "")
                    {
                        ws.Cells[GlobalRows + iteration + 1, StartCol].Value = "Signature";
                        ws.Cells[GlobalRows + iteration + 1, StartCol].Style.Font.Bold = true;

                        Image logo = Base64ToImage(sec.Signature);
                        var size = new Size();
                        size.Height = 200;
                        size.Width = 200;
                        var newimg = new Bitmap(logo, size);
                        Random rand = new Random();
                        var randomId = rand.Next(0, 999999999);
                        dynamic picture;
                        try
                        {
                             picture= ws.Drawings.AddPicture(sec.SiteSectionId + iteration.ToString() + randomId.ToString(), newimg);
                        }
                        catch
                        {
                            randomId = rand.Next(0, 88888888);
                            picture = ws.Drawings.AddPicture(sec.SiteSectionId + iteration.ToString() + randomId.ToString()+sec.SiteQuestionId.ToString(), newimg);
                        }
                        picture.SetPosition(GlobalRows + iteration + 2, 0, StartCol, 0);
                        picture.SetSize(200, 200);

                        GlobalRows = GlobalRows + 13;
                    }
                    // End For Signature 
                    if (sec.Sections.Count > 0)
                    {
                        GlobalRows = GlobalRows + iteration;
                        List<TSS_Section> TempNewSections = sec.Sections;
                    LoopThroug:
                        List<TSS_Section> childs = ManageChildSections(ws, TempNewSections);
                        if (childs.Count > 0)
                        {
                            TempNewSections = childs;
                            childs = new List<TSS_Section>();
                            goto LoopThroug;
                        }
                        else
                        {
                            iteration = 0;
                        }
                    }
                }
                //Creating Attandee List
                GlobalRows = GlobalRows + 1;
                int Increment = 2;
                int Loop = 0;
                if (sur.SiteAttendeesList.Count > 0)
                {
                    StyleFunction(GlobalRows + Increment, ws);
                    ws.Cells[GlobalRows + Increment, StartCol + 1].Value = "Attandee Info";
                    ws.Cells[GlobalRows + Increment, StartCol + 1].Style.Font.Bold = true;
                    ws.Cells[GlobalRows + Increment, StartCol + 2].Value = "Signature";
                    ws.Cells[GlobalRows + Increment, StartCol + 2].Style.Font.Bold = true;

                    foreach (var attandees in sur.SiteAttendeesList)
                    {
                        Loop = Loop + 1;

                        ws.Cells[GlobalRows + Increment + Loop + 0, StartCol].Value = "Name";
                        ws.Cells[GlobalRows + Increment + Loop + 0, StartCol].Style.Font.Bold = true;
                        ws.Cells[GlobalRows + Increment + Loop + 1, StartCol].Value = "Designation";
                        ws.Cells[GlobalRows + Increment + Loop + 1, StartCol].Style.Font.Bold = true;
                        ws.Cells[GlobalRows + Increment + Loop + 2, StartCol].Value = "Comapany";
                        ws.Cells[GlobalRows + Increment + Loop + 2, StartCol].Style.Font.Bold = true;

                        ws.Cells[GlobalRows + Increment + Loop + 0, StartCol + 1].Value = attandees.Name;
                        ws.Cells[GlobalRows + Increment + Loop + 1, StartCol + 1].Value = attandees.Designation;
                        ws.Cells[GlobalRows + Increment + Loop + 2, StartCol + 1].Value = attandees.Company;

                        // Signature For Attandee List

                        if (attandees.Signature != "")
                        {
                            Image logo = Base64ToImage(attandees.Signature);
                            Random rand = new Random();
                            var randomId = rand.Next(0, 999999999);
                            dynamic picture;
                            try
                            {
                                picture = ws.Drawings.AddPicture(attandees.SiteAttendeeId + iteration.ToString() + randomId.ToString() + (attandees.RowId == null ? "1" : attandees.RowId).ToString(), logo);
                            }
                            catch
                            {
                                randomId = rand.Next(0, 88888888);
                                picture = ws.Drawings.AddPicture(attandees.SiteAttendeeId + iteration.ToString() + randomId.ToString() + (attandees.RowId == null ? "1" : attandees.RowId).ToString(), logo);
                            }
                            picture.SetPosition(GlobalRows + Increment + Loop + (-1), 0, StartCol + 1, 0);
                            picture.SetSize(93, 57);

                        }

                        GlobalRows = GlobalRows + Increment;
                    }


                }

                // Creating Status Sheet

                CreateStatusSheet(pck, sur);

                // Creating Section Images Sheets 

                CreateImagesSheetForSections(pck, sur);

                CreateSheetForSiteInformation(pck,sur);

                string[] partOfWoRef = sur.WoRefId.Split('-');
                var LastPart = partOfWoRef[partOfWoRef.Length - 1];
                var FullName = sur.SiteCode + "-" + LastPart;

                return File(pck.GetAsByteArray(), "application/vnd-ms.excel", FullName + ".xlsx");
            }

        }

        public List<TSS_Section> ManageChildSections(ExcelWorksheet ws, List<TSS_Section> Sections)
        {
            List<TSS_Section> list = new List<TSS_Section>();
            int iteration = 0;
            foreach (var sec in Sections.ToList())
            {
                int EmptyRowsCount = GetEmptyRowsCount(ws, GlobalRows + iteration, StartCol + 1);
                if (EmptyRowsCount == 13)
                {
                    EmptyRowsCount = 0;
                }
                GlobalRows = GlobalRows - EmptyRowsCount;
                if (sec.Questions.Count > 0)
                {
                    
                    iteration = iteration + 1;

                    StyleFunctionChild(GlobalRows + iteration, ws);
                    ws.Cells[GlobalRows + iteration, StartCol + 1].Value = sec.SectionTitle;

                    int questionIteration = 0;
                    foreach (var qu in sec.Questions)
                    {
                        questionIteration = questionIteration + 1;

                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, 1);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 1);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 2);
                        ApplyBorders(ws, GlobalRows + iteration + questionIteration, StartCol + 3);

                        //string questionText = qu.Question + "\n" + qu.Description;
                        var Quest = ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].RichText.Add(qu.Question + "\n");
                        var Description = ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].RichText.Add(qu.Description);
                        Description.Italic = true;
                        Description.Size = 9;
                        // ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 1].Value = questionText;

                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Value = questionIteration;
                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[GlobalRows + iteration + questionIteration, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        if (qu.QuestionType != "Table")
                        {
                            if (qu.QuestionType == "Direction & GPS Based Images" || qu.QuestionType == "DateTime")
                            {
                                foreach (var r in qu.Responses)
                                {
                                    r.IsChecked = true;
                                }
                            }
                            var Res = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                            if (Res != null)
                            {
                                if (Res.ResponseId == 0)
                                {
                                    string value = Res.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                                else
                                {
                                    string value = Res.ResponseText;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Multi Select")
                            {
                                var AllOptions = qu.Responses.Where(r => r.IsChecked == true).ToList();
                                if (AllOptions != null)
                                {
                                    List<string> Responses = new List<string>();

                                    foreach (var option in AllOptions)
                                    {
                                        if (option.ResponseId == 0)
                                        {
                                            Responses.Add(option.ResponseValue.ToString());
                                        }
                                        else
                                        {
                                            Responses.Add(option.ResponseText.ToString());
                                        }
                                    }
                                    string[] arrRes = new string[Responses.Count];
                                    for (int i = 0; i < Responses.Count; i++)
                                    {
                                        arrRes[i] = Responses[i].ToString();
                                    }
                                    string value = string.Join(",", arrRes);
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Image Options")
                            {
                                var sres = qu.Responses.FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Rating" || qu.QuestionType == "Scale")
                            {
                                var sres = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = value;
                                }
                            }
                            if (qu.QuestionType == "Range")
                            {
                                var sres = qu.Responses.Where(r => r.IsChecked == true).FirstOrDefault();
                                if (sres != null)
                                {
                                    string value = sres.ResponseValue;
                                    value += "\n" + qu.Description;
                                    ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 2].Value = sres.MinValue + "," + sres.MaxValue;
                                }
                            }
                            if (qu.QuestionType == "Direction & GPS Based Images")
                            {
                                if (qu.MapImage != "" && qu.MapImage != null)
                                {

                                    ws.Cells[GlobalRows + iteration + questionIteration, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    Image logo = Base64ToImage(qu.MapImage);

                                    var size = new Size();


                                    size.Height = 750;
                                    size.Width = 800;

                                    var newimg = new Bitmap(logo, size);
                                    Random rand = new Random();
                                    var randomId = rand.Next(0, 999999999);
                                    dynamic picture;
                                    try
                                    {
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    catch
                                    {
                                        randomId = rand.Next(0, 88888888);
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    picture.SetPosition(GlobalRows + iteration + questionIteration - 1, 5, 2, 5);
                                    ws.Row(GlobalRows + iteration + questionIteration).Height = 300;
                                    picture.SetSize(410, 340);
                                
                                   
                                }

                            }
                            if (qu.QuestionType == "Signature")
                            {

                                var sigres = qu.Responses.Where(x => x.Signature != null && x.Signature != "").FirstOrDefault();
                                if (sigres != null)
                                {
                                    ws.Row(GlobalRows + iteration + questionIteration).Height = 90;
                                    ws.Cells[GlobalRows + iteration + questionIteration, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    Image logo = Base64ToImage(sigres.Signature);
                                    var size = new Size();
                                    size.Height = 600;
                                    size.Width = 600;
                                    var newimg = new Bitmap(logo, size);
                                    Random rand = new Random();
                                    var randomId = rand.Next(0, 999999999);
                                    dynamic picture;
                                    try
                                    {
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    catch
                                    {
                                        randomId = rand.Next(0, 88888888);
                                        picture = ws.Drawings.AddPicture(qu.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                                    }
                                    picture.SetPosition(GlobalRows + iteration + questionIteration - 1, 5, 2, 5);
                                    picture.SetSize(200, 100);
                                }


                            }

                        }
                        if (qu.QuestionType == "Table")
                        {
                            GlobalRows = GlobalRows + 1;
                            int getRowsCount = 0;
                            int col = qu.Responses.Count;
                            StyleFunctionTable(GlobalRows + iteration + questionIteration, StartCol + col, ws);
                            int colNo = StartCol + 1;
                            int row = GlobalRows + iteration + questionIteration;
                            foreach (var tres in qu.Responses)
                            {
                                ws.Cells[row, colNo].Value = tres.ResponseText;
                                ws.Cells[row, colNo].Style.Font.Color.SetColor(Color.Black);
                                ws.Cells[row, colNo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[row, colNo].Style.Font.Bold = true;

                                string[] values = new string[] { };
                                if (tres.IsReadOnly)
                                {
                                    values = tres.UserValues.Split(',');
                                }
                                else
                                {
                                    values = tres.SelectedRawResponse.Split('|');
                                }
                                int counter = 1;
                                if (values.Length > getRowsCount)
                                {
                                    getRowsCount = values.Length;
                                }
                                foreach (var val in values)
                                {
                                    ws.Cells[row + counter, colNo].Value = val.ToString();
                                    counter = counter + 1;
                                }

                                colNo = colNo + 1;

                            }
                            ApplyBordersMultiple(ws, GlobalRows + iteration + questionIteration, 1, GlobalRows + iteration + questionIteration + getRowsCount, colNo-1);
                            MergeRange(ws, (GlobalRows + iteration + questionIteration) - 1, 1, GlobalRows + iteration + questionIteration + getRowsCount, 1);

                            GlobalRows = GlobalRows + getRowsCount;

                        }
                        //ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = qu.Description;
                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        if (qu.QuestionType == "Direction & GPS Based Images" )
                        {
                            if (qu.IsNoteRequired)
                            {
                                foreach (var req in qu.ReqActions)
                                {
                                    if (req.ActionType == "Notes_Required")
                                    {
                                        if (!string.IsNullOrEmpty(req.RequiredAction))
                                        {
                                            ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = req.RequiredAction + " (" + qu.Azimuth + ")";
                                        }
                                        else
                                        {
                                            ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = qu.Azimuth;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value =qu.Azimuth;
                            }
                        }
                        else
                        {
                            if (qu.IsNoteRequired)
                            {
                                foreach (var req in qu.ReqActions)
                                {
                                    if (req.ActionType == "Notes_Required")
                                    {
                                        ws.Cells[GlobalRows + iteration + questionIteration, StartCol + 3].Value = req.RequiredAction;
                                    }

                                }
                            }
                        }

                    }

                    GlobalRows = GlobalRows + questionIteration;
                }
                // Sec Signature
                if (sec.Signature != "")
                {
                    ws.Cells[GlobalRows + iteration + 1, StartCol].Value = "Signature";
                    ws.Cells[GlobalRows + iteration + 1, StartCol].Style.Font.Bold = true;

                    Image logo = Base64ToImage(sec.Signature);
                    var size = new Size();
                    size.Height = 200;
                    size.Width = 200;
                    var newimg = new Bitmap(logo, size);
                    Random rand = new Random();
                    var randomId = rand.Next(0, 999999999);
                    dynamic picture;
                    try
                    {
                        picture = ws.Drawings.AddPicture(sec.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                    }
                    catch
                    {
                        randomId = rand.Next(0, 88888888);
                        picture = ws.Drawings.AddPicture(sec.SiteQuestionId + iteration.ToString() + randomId.ToString() + sec.SiteSectionId.ToString(), newimg);
                    }
                    picture.SetPosition(GlobalRows + iteration + 2, 0, StartCol, 0);
                    picture.SetSize(200, 200);

                    GlobalRows = GlobalRows + 13;
                }
                // End For Signature 
                if (sec.Sections.Count > 0)
                {
                    foreach (var s in sec.Sections)
                    {
                        list.Add(s);
                    }
                }
                if (list.Count > 0)
                {
                    GlobalRows = GlobalRows + iteration;
                    list= ManageChildSections(ws, list);
                }
            }
            GlobalRows = GlobalRows + iteration;
            return new List<TSS_Section>();
        }
        public void CreateStatusSheet(ExcelPackage pck, TSS_SurveyDocument sur)
        {
            ExcelWorksheet cs = pck.Workbook.Worksheets.Add("Checklist Status");
            cs.Column(1).Width = 35;
            cs.Column(2).Width = 35;
            cs.Column(3).Width = 35;

            cs.Column(1).Style.WrapText = true;
            cs.Column(2).Style.WrapText = true;
            cs.Column(3).Style.WrapText = true;

            StyleFunction(1, cs);
            cs.Cells[1, 1].Value = "Section Name";
            cs.Cells[1, 2].Value = "Completion %";
            cs.Cells[1, 3].Value = "Status";

            cs.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cs.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cs.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            int csIterations = 1;
            foreach (var status in sur.CompletetionSections)
            {

                cs.Cells[1 + csIterations, 1].Value = status.SectionTitle;


                if (Convert.ToString(Math.Round((Convert.ToDouble(status.TotalAnswered) / Convert.ToDouble(status.TotalQuestions)) * 100, 2)) != "NaN")
                {
                    if (Math.Round((Convert.ToDouble(status.TotalAnswered) / Convert.ToDouble(status.TotalQuestions)) * 100, 2) == 0)
                    {
                        cs.Cells[1 + csIterations, 2].Value = "";
                    }
                    else
                    {
                        if (status.DefinationName == "Completed")
                        {
                            cs.Cells[1 + csIterations, 2].Value =100 + "%";
                        }
                        else
                        {
                            cs.Cells[1 + csIterations, 2].Value = Math.Round((Convert.ToDouble(status.TotalAnswered) / Convert.ToDouble(status.TotalQuestions)) * 100, 2) + "%";
                        }
                    }

                }
                else
                {
                    cs.Cells[1 + csIterations, 2].Value = "";
                }

                cs.Cells[1 + csIterations, 3].Value = status.DefinationName;


                csIterations = csIterations + 1;
            }
        }
        public void CreateSheetForSiteInformation(ExcelPackage pck,TSS_SurveyDocument sur)
        {
         
            if (sur.AllFanImage != "" & sur.AllFanImage!=null)
            {
                ExcelWorksheet SiteInfo;
                try
                {
                    SiteInfo = pck.Workbook.Worksheets.Add("Site Information");
                }
                catch
                {
                    SiteInfo = pck.Workbook.Worksheets.Add(NameCounter+"_"+"Site Information");
                    NameCounter++;
                }
                Image logo = Base64ToImage(sur.AllFanImage);
                var size = new Size();
                size.Height = 800;
                size.Width = 800;
                var newimg = new Bitmap(logo, size);
                Random rand = new Random();
                var randomId = rand.Next(0, 999999999);
                dynamic picture;
                try
                {
                    picture = SiteInfo.Drawings.AddPicture((sur.SurveyId + sur.SurveyTitle).ToString() + randomId.ToString()+sur.WoRefId.ToString(), newimg);
                }
                catch
                {
                    randomId = rand.Next(0, 88888888);
                    picture = SiteInfo.Drawings.AddPicture((sur.SurveyId + sur.SurveyTitle).ToString() + randomId.ToString()+sur.SiteSurveyId.ToString(), newimg);
                }
                
                picture.SetSize(800,450);

            }
        }

        public void CreateImagesSheetForSections(ExcelPackage pck, TSS_SurveyDocument sur)
        {
            // Code Here For Images Sheets 

            bool SameSection = false;
            foreach (var sec in sur.Sections.ToList())
            {
                int CurrentRow = 1;
                int ImagesPerRow = 3;
                SameSection = false;
                bool flag = CheckIfImageExists(sec);
                if (flag)
                {
                    ExcelWorksheet secWorkSheet;
                    try
                    {
                        secWorkSheet = pck.Workbook.Worksheets.Add(sec.SectionTitle);
                    }
                    catch
                    {
                        secWorkSheet = pck.Workbook.Worksheets.Add(NameCounter+"_"+sec.SectionTitle);
                        NameCounter++;
                    }
                    CurrentRow = DrawImages(secWorkSheet, sec.Questions, 0, ImagesPerRow, CurrentRow, SameSection,"");
                    List<TSS_Section> TempSections = sec.Sections;
                    if (sec.Sections.Count > 0)
                    {
                       CurrentRow= DrawImagesForChild(secWorkSheet, TempSections, SameSection, ImagesPerRow, CurrentRow, "");
                    }
                }


            }

            //
        }
        public int DrawImagesForChild(ExcelWorksheet secWorkSheet, List<TSS_Section> TempSections,bool SameSection,int ImagesPerRow,int CurrentRow,string ParentName)
        {
            string ParentTitle = ParentName;
            List<TSS_Section> ChildSection = new List<TSS_Section>();
            if (TempSections.Count > 0)
            {
                foreach (var cs in TempSections.ToList())
                {
                    if (!cs.IsRepeatable)
                    {
                        ParentTitle = cs.SectionTitle;
                    }
                    bool check = CheckIfImageExists(cs);
                    if (check)
                    {
                        SameSection = true;
                        int val = (ImagesPerRow * 5) / 2;
                        int CheckIfParentHaveImages = 0;
                        foreach (var secques in cs.Questions)
                        {
                            var images = secques.ReqActions.Where(r => r.ActionTypeId == 2 && r.ActionId > 0).ToList();
                            if (images.Count > 0)
                            {
                                CheckIfParentHaveImages = CheckIfParentHaveImages + 1;
                            }
                        }
                        if (CheckIfParentHaveImages > 0)
                        {
                            StyleFunctionForImageSheetSections(CurrentRow, 5 * ImagesPerRow, secWorkSheet);
                            secWorkSheet.Cells[CurrentRow, val].Value = ParentName+" "+cs.SectionTitle;
                            CurrentRow = DrawImages(secWorkSheet, cs.Questions, 2, ImagesPerRow, CurrentRow, SameSection, ParentName + " " + cs.SectionTitle);
                        }

                    }
                    if (cs.Sections.Count > 0)
                    {
                        ChildSection.AddRange(cs.Sections);
                    }
                    if (ChildSection.Count > 0)
                    {
                        CurrentRow = DrawImagesForChild(secWorkSheet, ChildSection, SameSection, ImagesPerRow, CurrentRow,ParentTitle);
                        ChildSection = new List<TSS_Section>();
                    }
                }
            }
            return CurrentRow;
        }
        public int DrawImages(ExcelWorksheet secWorkSheet, List<TSS_Question> que, int TopSpace, int ImagesPerRow, int CurrentRow, bool SameSection,string CompleteSectionTitle)
        {
            int iteration = 0;
            int PicCount = 2;
            int GlobalRowPosition = 0;
            int ColSpacing = 5;
            int RowSpacing = 25;
            int CurrentColumn = 0;
            int QuestionId = 0;
            foreach (var q in que)
            {
                if (q.Prefix != "")
                {
                    q.Prefix = GetCompletePrefix(q.Prefix, CompleteSectionTitle);
                }
                QuestionId = QuestionId + 1;
                if (q.ReqActions.Count > 0)
                {
                    var ImagesOnly = q.ReqActions.Where(r => r.ActionTypeId == 2 && r.ActionId > 0).ToList();


                    if (ImagesOnly.Count > 0)
                    {
                        foreach (var img in ImagesOnly.ToList())
                        {
                            if (System.IO.File.Exists(img.RequiredAction))
                            {
                                CurrentColumn = CurrentColumn + 1;
                                iteration = iteration + 1;
                                Image logo = Image.FromFile(img.RequiredAction);
                                Random rand = new Random();
                                var nameExtension = rand.Next(0, 99999999);
                                dynamic picture;
                                try
                                {
                                    picture = secWorkSheet.Drawings.AddPicture((img.Name + img.ActionId + q.SectionId + q.SiteSectionId + q.SiteQuestionId + nameExtension).ToString(), logo);
                                }
                                catch
                                {
                                    nameExtension = rand.Next(0, 88888888);
                                    picture = secWorkSheet.Drawings.AddPicture((img.Name + img.ActionId + q.SectionId + q.SiteSectionId + q.SiteQuestionId + nameExtension).ToString(), logo);
                                }
                                if (iteration > 1 && (iteration - 1) % ImagesPerRow != 0)
                                {

                                    picture.SetPosition(CurrentRow + (RowSpacing * GlobalRowPosition) + TopSpace, 0, (CurrentColumn - 1) * ColSpacing, 0);
                                    PicCount = PicCount + 1;
                                    if (GlobalRowPosition != 0)
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value =(q.Prefix==""?'Q'+QuestionId.ToString():q.Prefix) + img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 3].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks;
                                        }
                                    }
                                    else
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 3].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks;
                                        }
                                    }
                                }
                                if (iteration > 2 && (iteration - 1) % ImagesPerRow == 0)
                                {
                                    int row = GlobalRowPosition + 1;
                                    picture.SetPosition(CurrentRow + TopSpace + ((iteration + 1) - (PicCount)) * RowSpacing * row, 0, 0, 0);
                                    PicCount = PicCount + 1;
                                    GlobalRowPosition = GlobalRowPosition + 1;
                                    CurrentColumn = 1;
                                    if (GlobalRowPosition != 0)
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) +  img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 3].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks ;
                                        }
                                    }
                                    else
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) +  img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 2].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks;
                                        }
                                    }
                                }
                                if (iteration == 1)
                                {
                                    picture.SetPosition(CurrentRow + TopSpace, 0, 0, 0);
                                    if (GlobalRowPosition != 0)
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) +  img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing * GlobalRowPosition * 2) + CurrentRow) - 3, CurrentColumn * ColSpacing - 3].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks;
                                        }
                                    }
                                    else
                                    {
                                        if (img.Azimuth != "")
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 4].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) +  img.Remarks + " " + "(" + img.Azimuth + ")";
                                        }
                                        else
                                        {
                                            secWorkSheet.Cells[((RowSpacing) + CurrentRow) - 3, CurrentColumn * ColSpacing - 3].Value = (q.Prefix == "" ? 'Q' + QuestionId.ToString() : q.Prefix) + img.Remarks;
                                        }
                                    }
                                }
                                string getSize = CheckOrientation(img.RequiredAction);
                                if (getSize == "L")
                                {
                                    picture.SetSize(350, 250);
                                    ColSpacing = 6;
                                }
                                else if (getSize == "P")
                                {
                                    picture.SetSize(250, 350);
                                    ColSpacing = 5;
                                }
                                else
                                {

                                    picture.SetSize(250, 250);
                                    ColSpacing = 5;
                                }

                            }
                        }
                    }


                }
            }
            if (SameSection)
            {
                if (GlobalRowPosition == 0)
                {
                    return CurrentRow + RowSpacing;
                }
                else
                {
                    return (CurrentRow + RowSpacing) + (GlobalRowPosition*RowSpacing);
                }
            }
            else if ((CurrentRow * GlobalRowPosition * RowSpacing) == 0)
            {
                return 1;
            }
            else
            {

                Decimal d = Convert.ToDecimal(PicCount);
                Decimal f = Convert.ToDecimal(ImagesPerRow);
                Decimal result = d / f;
                int RowsToMultiply = Convert.ToInt32(Math.Round(result));

                return CurrentRow * RowSpacing * RowsToMultiply;
            }
        }
        public bool CheckIfImageExists(TSS_Section sec)
        {
            List<TSS_Section> childSections = new List<TSS_Section>();

            foreach (var q in sec.Questions)
            {
                if (q.ReqActions.Count > 0)
                {

                    var rq = q.ReqActions.Where(r => r.ActionTypeId == 2 && r.ActionId > 0).ToList();
                    if (rq.Count > 0)
                    {
                        foreach (var img in rq)
                        {
                            if (System.IO.File.Exists(img.RequiredAction))
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            if (sec.Sections.Count > 0)
            {
                childSections.AddRange(sec.Sections);
                bool GetChildSectionStatus = ifChildHasImages(childSections);
                if (GetChildSectionStatus)
                {
                    return true;
                }
            }
            else
            {
                childSections = new List<TSS_Section>();
            }
            return false;
        }
        public bool ifChildHasImages(List<TSS_Section> sec)
        {
            List<TSS_Section> childSections = new List<TSS_Section>();
            List<TSS_Section> temp = sec;
        CheckAgian:
            foreach (var sections in temp.ToList())
            {
                foreach (var q in sections.Questions)
                {
                    if (q.ReqActions.Count > 0)
                    {

                        var rq = q.ReqActions.Where(r => r.ActionTypeId == 2 && r.ActionId > 0).ToList();
                        if (rq.Count > 0)
                        {
                            foreach (var img in rq)
                            {
                                if (System.IO.File.Exists(img.RequiredAction))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (sections.Sections.Count > 0)
                {
                    childSections.AddRange(sections.Sections);
                }
            }
            if (childSections.Count > 0)
            {
                temp = childSections;
                childSections = new List<TSS_Section>();
                goto CheckAgian;
            }

            return false;
        }
        public string CheckOrientation(string path)
        {
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(path);
            if (img.Width > img.Height)
                return "L";
            else if (img.Width < img.Height)
                return "P";
            else
                return "S";
        }
        public string CheckOrientationByBase64String(string base64String)
        {
            var pieces = base64String.Split(new[] { ',' }, 2);
            byte[] image = Convert.FromBase64String(pieces[1]);
            using (var ms = new MemoryStream(image))
            {
                Image img = Image.FromStream(ms);
                if (img.Height>1000)
                    return "P";
                else if (img.Height<1000)
                {
                    if (img.Width > img.Height)
                        return "L";
                    else if (img.Width < img.Height)
                        return "P";
                    else
                        return "S";
                }
                else
                    return "S";
            }
        }
        public void StyleFunction(int FromRow, ExcelWorksheet ws)
        {
            using (ExcelRange rng = ws.Cells[FromRow, 1, FromRow, StartCol + 3])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);  //Set color to dark blue
                rng.Style.Font.Color.SetColor(Color.White);
            }
        }
        public void StyleFunctionChild(int FromRow, ExcelWorksheet ws)
        {
            using (ExcelRange rng = ws.Cells[FromRow, 1, FromRow, StartCol + 3])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.Silver);  //Set color to silver
                rng.Style.Font.Color.SetColor(Color.White);
            }
        }
        public void StyleFunctionTable(int FromRow, int col, ExcelWorksheet ws)
        {
            using (ExcelRange rng = ws.Cells[FromRow, 1, FromRow, col])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);  //Set color to white Smoke
                rng.Style.Font.Color.SetColor(Color.White);
            }
        }

        public void StyleFunctionForImageSheetSections(int FromRow, int ToColumn, ExcelWorksheet ws)
        {
            using (ExcelRange rng = ws.Cells[FromRow, 1, FromRow, ToColumn])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);  //Set color to gray
                rng.Style.Font.Color.SetColor(Color.White);
            }
        }
        public Image Base64ToImage(string base64String)
        {
            var pieces = base64String.Split(new[] { ',' }, 2);
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(pieces[1]);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
        public void ApplyBorders(ExcelWorksheet ws, int row, int column)
        {
            ws.Cells[row, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;

        }

        public void ApplyBordersMultiple(ExcelWorksheet ws, int FromRow, int FromColumn, int ToRow, int ToColumn)
        {
            ws.Cells[FromRow, FromColumn, ToRow, ToColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[FromRow, FromColumn, ToRow, ToColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[FromRow, FromColumn, ToRow, ToColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[FromRow, FromColumn, ToRow, ToColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        public void MergeRange(ExcelWorksheet ws, int FromRow, int FromColumn, int ToRow, int ToColumn)
        {
            ws.Cells[FromRow, FromColumn, ToRow, ToColumn].Merge = true;
        }
        public int GetEmptyRowsCount(ExcelWorksheet ws, int rowsNumber, int ColumnNumber)
        { int rowToReturn = 0;
            string Values = "";
            try
            {

                while (Values == "")
                {
                    var value1 = ws.Cells[rowsNumber, 0].Value;
                    var value2 = ws.Cells[rowsNumber, 1].Value;
                    var value3 = ws.Cells[rowsNumber, 2].Value;
                    var value4 = ws.Cells[rowsNumber, 3].Value;
                    var value5 = ws.Cells[rowsNumber, 4].Value;
                    var value6 = ws.Cells[rowsNumber, 5].Value;
                    Values = Values + value1 + value2+value2+value3+value4+value5;

                    rowsNumber--;
                    rowToReturn++;
                }
                var CheckHeader = ws.Cells[GlobalRows - rowToReturn - 1, ColumnNumber].Value;
                var CheckHeader2 = ws.Cells[GlobalRows - rowToReturn - 1, ColumnNumber + 1].Value;
                if (CheckHeader ==null || CheckHeader2 == null)
                {
                    return 0;
                }
                else
                {
                    var RetrunRows = rowToReturn - 1;
                    return RetrunRows <= 0?0: RetrunRows;
                }
            }
            catch(Exception ex)
            {
                var RetrunRows = rowToReturn - 1;
                return RetrunRows <= 0 ? 0 : RetrunRows;
            }
        }
        public string GetCompletePrefix(string Prefix, string SectionTitle)
        {
            StringBuilder NewPrefix = new StringBuilder();
            if (SectionTitle != "")
            {
                List<int> p = new List<int>();
                string[] values = SectionTitle.Split(' ');
                foreach (var s in values)
                {
                    try
                    {
                        p.Add(Convert.ToInt32(s));
                    }
                    catch
                    {
                    }
                }
                string[] sp = Prefix.Split(new[] { ' ' }, 2);
                string[] CompletePrefix = sp[0].Split('-');
                int pid = 0;
                foreach (var i in CompletePrefix)
                {
                    try
                    {
                        if (pid == 0)
                        {
                            NewPrefix.Append(i + p[pid].ToString());
                        }
                        else
                        {
                            NewPrefix.Append("-" + i + p[pid].ToString());
                        }
                        if (CompletePrefix.Length < p.Count)
                        {
                            if (pid == CompletePrefix.Length - 1)
                            {
                                NewPrefix.Append(":");
                            }
                        }
                        else
                        {
                            if (pid == p.Count - 1)
                            {
                                NewPrefix.Append(":");
                            }

                        }

                        pid++;
                    }
                    catch
                    {

                    }
                }
                var lastpart = "";
                try
                {
                    lastpart = sp[1];
                }
                catch
                {

                }
                return NewPrefix.ToString() + lastpart+" ";
            }
            else
            {

                return Prefix + " ";
            }
        }
    }
}