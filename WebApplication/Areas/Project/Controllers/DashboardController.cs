using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.Model;
using AirView.DBLayer.Survey.BLL;
using AirView.DBLayer.Template.BLL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using SWI.AirView.Common;
using SWI.AirView.Models;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication.Areas.Project.View_Models;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class DashboardController : Controller
    {
        private TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
        // GET: Project/Dashboard

        private PM_DashboardBL bl = new PM_DashboardBL();
        [IsLogin(CheckPermission = true)]
        public ActionResult Index(Int64 id = 0)
        {
            ViewBag.ApiMapKey = ApiMapKey();
            string ModuleKeyCode = "MD_PROJECT";
            var templateData = TemplatesBL.ToList("IsTemplateExist", ModuleKeyCode).Where(x => x.ProjectId == id).FirstOrDefault();
            if (templateData != null)
            {
                return Redirect($"/Project/Template/Dashboard?Id={templateData.TemplateId}&ProjectId={templateData.ProjectId}");
            }

            if (id > 0)
            {
                var oob = Permission.AllowProject(id);
                if (oob == null)
                {
                    TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                    return RedirectToAction("index", "error", new { Area = "Project" });
                }
                else
                {
                    TempData["ProjectEntity"]  = oob;
                    TempData.Keep("ProjectEntity");
                }
                ViewBag.Id = id;
                ViewBag.SelectProjectId = id;
                TempData["ProjectId"] = Convert.ToString(id);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        private string ApiMapKey()
        {
            WebConfig wc = new WebConfig();
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            return MapKey;
        }
        #region SandBox
        [IsLogin(CheckPermission = true)]
        public ActionResult SandBox(Int64 Id = 0)
        {
            ViewBag.Id = Id;
            //  TempData["ProjectId"] = Convert.ToString(id);
            if(Id > 0) { 
            var oob = Permission.AllowProject(Convert.ToInt64(Id));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            }
            return View();
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult GetSandbox(string Filter, Int64 ProjectId, string Value, string Category, string Name, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, string MapStatus = null, string MapType = null)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetSandbox(Filter, ProjectId, Value, Category, Name, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption, 0, 0, MapStatus, MapType);
            ViewBag.Result = dt.ToList<PM_SandBox>();
            List<PM_SandBox> ListCount = ViewBag.Result;
            ViewBag.Count = ListCount.Where(x => x.IsSelected == true).Count();
            //   IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
            //                                                                         .ToDictionary(a => a.Name, a => a.Value));
            //  return Json(result, JsonRequestBehavior.AllowGet);
            return PartialView("~/Areas/Project/Views/Dashboard/_Sandbox.cshtml");
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult SaveSandbox(string Filter, Int64 ProjectId, string TaskIds, string Value, string Category, string Name, string Selected, string Unselected)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            var result = pd.SaveSites(Filter, ProjectId, TaskIds, Value, Category, Name, Selected, Unselected);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        [IsLogin(CheckPermission = false)]
        public ActionResult GetSingleSite()
        {
            DashboardVM vm = new DashboardVM();
            DashboardBL loDashboardBL = new DashboardBL();

            vm = loDashboardBL.GetDashboardSiteVM(null, null, DateTime.Now, DateTime.Now, null, 10096, null, 0, ViewBag.UserId, "SINGLE_SITE", 0, 5, "0");
            var result = vm.ClientSites;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult TableResult(Int64 ProjectId, int Page, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string Searchoption = null, bool IsActive = true)
        {
            try
            {
                TempData["CProjectID"] = ProjectId;
                int Offset = (Page - 1) * 5;
                PM_DashboardBL pm = new PM_DashboardBL();
                PM_DashboardDL pd = new PM_DashboardDL();
                DataSet ds = pd.GetDashboardWO("Get_Project_WO", ProjectId, 5, Offset, Searchoption, TaskIds, LocationIds, FromDate, ToDate, ViewBag.UserId.ToString(), IsActive);
                if (ds.Tables.Count == 2)
                {
                    DataTable Count = ds.Tables[1];
                    ViewBag.Count = (!string.IsNullOrEmpty(Count.Rows[0]["Count"].ToString())) ? Convert.ToInt32(Count.Rows[0]["Count"].ToString()) : 0;
                }
                ViewBag.activeAndinative = IsActive;
                ViewBag.TableResult = ds.Tables[0];
                //pm.GetWorkOrder("Get_Project_WO", 10007);
                var Entity = TempData["ProjectEntity"] as AirView.DBLayer.Security.Entities.Sec_UserProjects;
                TempData.Keep("ProjectEntity");

               
               return PartialView("~/Areas/Project/Views/Dashboard/_PMGrid.cshtml", ViewBag.TableResult);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SetSiteStatus(Int64 ProjectSiteId, bool IsActive)
        {
            Response res = new Response();

            try
            {
                PM_ProjectSitesDL ue = new PM_ProjectSitesDL();
                var isActive = ue.ManageStatus("SetIsActive", null, ProjectSiteId, IsActive);
                string St = IsActive == true ? "Activated" : "Deactivated";
                if(isActive == true)
                {
                    res.Status = "success";
                    res.Message = "Site " + St + " Successfully!";
                }
                else
                {
                    res.Status = "not";
                    res.Message = "Site can not be " + St + "";
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }

        [IsLogin(CheckPermission = false)]
        public ActionResult TLGrid(Int64 ProjectId, Int64 SiteId, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string FACode = null,Int64 MilestoneId=0)
        {
            //PM_DashboardBL pm = new PM_DashboardBL();

            ViewBag.SiteId = SiteId;
                 ViewBag.TaskId = MilestoneId;
            ViewBag.FACode = FACode;
            PM_DashboardDL pd = new PM_DashboardDL();
            ViewBag.TLGrid = pd.GetDataTable("Get_WO_Milestones", ProjectId, 1, 0, SiteId, TaskIds, LocationIds, FromDate, ToDate,MilestoneId,ViewBag.UserId).DefaultView.ToTable(true);
            // int thisWeekNumber = SWI.Libraries.Common.WeekDays.GetIso8601WeekOfYear(DateTime.Today);
            // DateTime firstDayOfWeek = SWI.Libraries.Common.WeekDays.FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            // DateTime lastdayofweek = firstDayOfWeek.AddDays(7);
            // var obj = Enumerable.Range(0, 1 + DateTime.Now.Subtract(firstDayOfWeek).Days)
            //.Select(offset => firstDayOfWeek.AddDays(offset))
            // .ToList();
            List<DateTime> obj = new List<DateTime>();
            int day = (int)DateTime.Now.DayOfWeek;
            for(var i = 1; i <= day; i++)
            {
                obj.Add(DateTime.Now.AddDays(-i));
            }
            obj.Add(DateTime.Now);
            string dayss = "";
            foreach (var item in obj)
            {
                dayss = dayss + "," + item.Date.ToShortDateString();
            }
            ViewBag.worklogdays = dayss;
           


            //List<PM_Task> list = new List<PM_Task>() {
            //    new PM_Task { Status="Staus",TargetDate= DateTime.Now,Title="SSV",PredecessorId=123 }
            //};
            return PartialView("~/Areas/Project/Views/Dashboard/_TLGrid.cshtml");
        }
       
        [IsLogin(CheckPermission = false)]
        public ActionResult STGrid(Int64 ProjecId, Int64 MilestoneId, Int64 SiteId, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            ViewBag.TLGrid = pd.GetStages("Get_WO_Stages", ProjecId, MilestoneId, SiteId, TaskIds, LocationIds, FromDate, ToDate);

            return PartialView("~/Areas/Project/Views/Dashboard/_STGrid.cshtml", ProjecId);
        }


        [IsLogin(CheckPermission = false)]
        public ActionResult GetStages(Int64 ProjecId, Int64 MilestoneId, Int64 SiteId)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetStages("Get_WO_Stages", ProjecId, MilestoneId, SiteId,"","",null,null,Convert.ToInt64(ViewBag.UserId)  );
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult ToList(Int64 projectId, int Page)
        {
            int Offset = (Page - 1) * 5;
            PM_DashboardBL pm = new PM_DashboardBL();
            var result = pm.GetWorkOrder("Get_Project_WO", projectId, Page, Offset);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetMileStoneValues(Int64 projectId, string filterOption,Int64 ParentId = 0)
        {
            List<MilestoneModel> Mlist = new List<MilestoneModel>();
            List<MilestoneModel> Tlist = new List<MilestoneModel>();
            PM_DashboardDL pd = new PM_DashboardDL();
            ViewBag.ProjectId= projectId;

            DataTable dt = pd.GetMileStoneValues("Get_Project_Tasks_Site", projectId, ParentId, filterOption);
            var dtToMilestone = dt.ToList<MilestoneModel>();
            if (filterOption == "" )
            {
                List<MilestoneModel> MyList = new List<MilestoneModel>();
                foreach (var item in dtToMilestone)
                {
                    MilestoneModel mil = new MilestoneModel();
                    mil.PTaskId = item.PTaskId;
                    mil.Task = item.Task;
                    mil.Title = item.Title;
                    mil.TaskId = item.TaskId;
                    mil.Color = item.Color;
                    mil.TotalSites = item.TotalSites;
                    mil.ActualSites = item.ActualSites;
                    MyList.Add(mil);
                }
                ViewBag.Withouthierarchy = MyList;
                foreach (var item in MyList.ToList().Where(x=>x.PTaskId ==0))
                {
                    MilestoneModel mil = new MilestoneModel();
                    mil.Tasks = FlatToHierarchy(MyList, item.TaskId);
                    mil.TaskId = item.TaskId;
                    mil.Task = item.Task;
                    mil.Title = item.Title;
                    mil.Color = item.Color;
                    mil.TotalSites = item.TotalSites;
                    mil.ActualSites = item.ActualSites;
                    Mlist.Add(mil);
                }
                ViewBag.Tasks = Mlist;
                return PartialView("~/Areas/Project/Views/Dashboard/_Milestone.cshtml", Mlist);

            }




            foreach (DataRow item in dt.Rows)
            {
                MilestoneModel mil = new MilestoneModel();
                MilestoneModel tsk = new MilestoneModel();
                tsk.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                tsk.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                tsk.Task = item["Task"].ToString();
                tsk.Color = item["Color"].ToString();
                tsk.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                tsk.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                tsk.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                mil.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                mil.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                mil.Task = item["Task"].ToString();
                mil.Color = item["Color"].ToString();
                mil.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                mil.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                mil.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                if (tsk.PTaskId == 0)
                {
                    Mlist.Add(mil);
                }
                else
                {
                    Tlist.Add(tsk);
                }
            }

            foreach (var mile in Mlist)
            {
                foreach (var task in Tlist)
                {
                    if (task.PTaskId == mile.TaskId)
                    {
                        mile.Tasks.Add(task);// = Tlist;
                    }
                }
            }

            ViewBag.Tasks = Mlist;
            return PartialView("~/Areas/Project/Views/Dashboard/_Milestone.cshtml", Mlist);
        }
        [IsLogin(CheckPermission = false)]
        private List<MilestoneModel> FlatToHierarchy(IEnumerable<MilestoneModel> list, long parentId = 0)
        {

            return (from i in list
                    where i.PTaskId == parentId
                    select new MilestoneModel
                    {
                        TaskId = i.TaskId,
                        Task = i.Task,
                        Title = i.Title,
                        PTaskId = i.PTaskId,
                        Color = i.Color,
                   TotalSites = i.TotalSites,
                    ActualSites = i.ActualSites,
            Tasks = FlatToHierarchy(list, Convert.ToInt64(i.TaskId))
                    }).ToList();
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetTaskList(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null)
        {
            List<MilestoneModel> Mlist = new List<MilestoneModel>();
            List<MilestoneModel> Tlist = new List<MilestoneModel>();
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetDashboardCharts(Filter, ProjectId, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption);

            foreach (DataRow item in dt.Rows)
            {
                MilestoneModel mil = new MilestoneModel();
                MilestoneModel tsk = new MilestoneModel();

                tsk.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                tsk.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                tsk.Task = item["TaskType"].ToString();
                tsk.Color = item["Color"].ToString();
                tsk.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                tsk.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                tsk.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                mil.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                mil.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
               mil.Task = item["TaskType"].ToString();
                mil.Color = item["Color"].ToString();
                mil.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                mil.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                mil.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                if (tsk.PTaskId == 0)
                {
                    Mlist.Add(mil);
                }
                else
                {
                    Tlist.Add(tsk);
                }
            }
            List<MilestoneModel> Record = new List<MilestoneModel>();
            foreach (var mile in Mlist)
            {
                //foreach (var task in Tlist)
                //{
                //    if (task.PTaskId == mile.TaskId)
                //    {
                //        mile.Tasks.Add(task);// = Tlist;
                //    }
                //}
                MilestoneModel mil = new MilestoneModel();
                mil = mile;
                mil.Tasks = FlatToHierarchy(Mlist, mile.TaskId);
                Record.Add(mil);
            }

            return Json(Record, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        private List<MilestoneModel> DefinationTree(Int64 TaskId, List<MilestoneModel> def)
        {
            return def.Where(m => m.PTaskId == TaskId).ToList();
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetMilstonesBarchart(Int64 projectId, string filter)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues(filter, projectId);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetProjectStages(Int64 ProjectId, Int64 MilestoneId, string filterOption)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Project_Tasks", ProjectId, MilestoneId, filterOption);
            return PartialView("~/Areas/Project/Views/Dashboard/_ProjectStages.cshtml", dt);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ProjectAttachment(Int64 ProjectId = 0)
        {
            ViewBag.ProjectId = ProjectId;
           return PartialView("~/Areas/Project/Views/Dashboard/_ProjectAttachments.cshtml");
        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult SaveAttachment(List<PM_SiteTaskAttachment> Attachments, object obj)
        {

            Response res = new Response();
            try
            {
                dbDataTable dbtd = new dbDataTable();
                DataTable dt = dbtd.List();
                PM_SiteTaskAttachment_DL SiteTaskDL = new PM_SiteTaskAttachment_DL();
                for (int i = 0; i < Attachments.Count; i++)
                {
                    string fname="",Path = "";
                    #region File Save
                    if (Attachments[i].file != null && Attachments[i].file != "" && (Attachments[i].AttachmentId > 0 || Attachments[i].IsDeleted ==false))
                    {
                        if(Attachments[i].AttachmentId == 0)
                        {
                             Path = "/Content/SiteTaskAttachments/"+Attachments[i].ProjectId;
                            string Extension = "." + Attachments[i].FileExtension.ToLower();
                            if (!Directory.Exists(HttpContext.Server.MapPath("~" + Path)))
                            {
                                // if it doesn't exist, create
                                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~" + Path));
                            }
                            var Time = DateTime.Now.ToString("yyyyMMddHHmmss");
                            var splitarea = Attachments[i].file.Split(',');
                            var _File = Convert.FromBase64String(splitarea[1]);
                            fname = Attachments[i].FileName + "_" + ViewBag.UserId + "_" + Time + Extension; //HttpContext.Server.MapPath("~" + Path + "/" + ViewBag.UserId);
                            System.IO.File.WriteAllBytes(HttpContext.Server.MapPath("~" + Path + "/" + fname), _File);
                        }
                         myDataTable.AddRow(dt, "Value1", Attachments[i].AttachmentId, "Value2", Attachments[i].SiteTaskId, "Value3", fname, "Value4", Attachments[i].Description, "Value5", Attachments[i].Tags,"Value6", Attachments[i].CategoryId,"Value7",ViewBag.UserId,"Value8",Attachments[i].IsDeleted);
                    }

                    #endregion

                }
                SiteTaskDL.Save("Save", dt);
                res.Status = "success";
                res.Message = "success";
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;
             //   ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);








            //var length = Request.ContentLength;
            //var bytes = new byte[length];
            //Request.InputStream.Read(bytes, 0, length);

            //var fileName = Request.Headers["X-File-Name"];
            //var fileSize = Request.Headers["X-File-Size"];
            //var fileType = Request.Headers["X-File-Type"];

            //var saveToFileLoc = "\\\\adcyngctg\\HRMS\\Images\\" + fileName;

            //// save the file.
            //var fileStream = new FileStream(saveToFileLoc, FileMode.Create, FileAccess.ReadWrite);
            //fileStream.Write(bytes, 0, length);
            //fileStream.Close();
            //foreach (var item in Attachments)
            //{
            //    var postedFile = item.Files.FileName;
            //    Stream stream = item.Files.InputStream;
            //      //string Path =item.Files.SaveAs()
            //    string path= System.Web.HttpContext.Current.Request.ApplicationPath + "\\files"+ item.Files.FileName;
            //    item.Files.SaveAs(path);


            //}
            //var httpRequest = HttpContext.Current.Request;


            return null;
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult SiteTaskAttachments(Int64? SiteTaskId=0)
        {
            Response res = new Response();
            try
            {
                PM_SiteTaskAttachment_BL db = new PM_SiteTaskAttachment_BL();
              res.Value = db.ToList("GetAttachments", SiteTaskId.ToString());

                res.Status = "success";
                res.Message = "Success";
                return Json(res, JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = ex.Message;

            }
            return Json(res, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost, IsLogin(CheckPermission = false)]
  public virtual string UploadFiles(object obj)
        {
            var length = Request.ContentLength;
            var bytes = new byte[length];
            Request.InputStream.Read(bytes, 0, length);

            var fileName = Request.Headers["X-File-Name"];
            var fileSize = Request.Headers["X-File-Size"];
            var fileType = Request.Headers["X-File-Type"];

            var saveToFileLoc = "\\\\adcyngctg\\HRMS\\Images\\" + fileName;

            // save the file.
            var fileStream = new FileStream(saveToFileLoc, FileMode.Create, FileAccess.ReadWrite);
            fileStream.Write(bytes, 0, length);
            fileStream.Close();

            return string.Format("{0} bytes uploaded", bytes.Length);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetProjectIssue(Int64 projectId, int Page, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string Searchoption = null)
        {
            int Offset = (Page - 1) * 5;
            PM_DashboardBL pm = new PM_DashboardBL();
            List<PM_Issues> result = new List<PM_Issues>();

            result = pm.GetProjectIssue("Get_ProjectIssue_WO", projectId, 5, Offset, Searchoption, TaskIds, LocationIds, FromDate, ToDate, ViewBag.UserId.ToString());
            if (result.Count != 0)
            {
                ViewBag.IssueCount = result.FirstOrDefault().Count;
            }
            List<DateTime> obj = new List<DateTime>();
            int day = (int)DateTime.Now.DayOfWeek;
            for (var i = 1; i <= day; i++)
            {
                obj.Add(DateTime.Now.AddDays(-i));
            }
            obj.Add(DateTime.Now);
            string dayss = "";
            foreach (var item in obj)
            {
                dayss = dayss + "," + item.Date.ToShortDateString();
            }
            ViewBag.worklogdays = dayss;

            return PartialView("~/Areas/Project/Views/Dashboard/_ProjectIssue.cshtml", result);
        }
        [HttpGet]
        public FileResult File(string path)
        {
            try { 
            if (!string.IsNullOrEmpty(path))
            {
                var vFullFileName = HostingEnvironment.MapPath(path);

                var file = new FileInfo(vFullFileName);
                if (file.Exists)
                {
                    //return File(vFullFileName, "application/binary");
                    return new FileStreamResult(new FileStream(vFullFileName, FileMode.Open), "content-disposition");
                }
            }
            }catch(Exception ex)
           { return null; }
            return null;
            //file is empty, so return null

        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetMarkets(string filter, Int64 ProjectId, string Value = null)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            //DataTable dt = pd.GetMileStoneValues(filter, ProjectId, 0,null,RegionId);
            ViewBag.ProjectId = ProjectId;
            if (filter == "Get_MarketView_WO")
            {
                return PartialView("~/Areas/Project/Views/Dashboard/_MarketView.cshtml");
            }
            else if (filter == "Get_RegionView_WO")
            {
                return PartialView("~/Areas/Project/Views/Dashboard/_RegionView.cshtml");
            }
            else if (filter == "Get_DateWise_WO")
            {
                return PartialView("~/Areas/Project/Views/Dashboard/_DatewiseMarket.cshtml");
            }
            else
            {
                return PartialView("~/Areas/Project/Views/Dashboard/_IssueChart.cshtml");
            }
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetMarketRegionMap(string filter, Int64 ProjectId, string Value = null)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues(filter, ProjectId, 0, null, Value);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetDefination()
        {
            var result = bl.GetDefination("Get_Defination_Issue");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetSiteLatLong(Int64 ProjectId)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Site_LatLong", ProjectId, 0);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetRegionIssue(string Filter, Int64 ProjectId)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues(Filter, ProjectId, 0);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetMarketIssue(Int64 ProjectId, Int64 Value1)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Market_Issue", ProjectId, 0, null, Value1.ToString());
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetResourceIssue(Int64 ProjectId, string Value1)
        {
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetMileStoneValues("Get_Datewise_Issue", ProjectId, 0, null, Value1);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetList(Int64 projectId)
        {
            int User = Convert.ToInt16(ViewBag.UserId);
            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            List<SelectListItem> Users = new List<SelectListItem>();
            List<Sec_UserDevices> UserDevices = new List<Sec_UserDevices>();
            sl.UserAssinged_Testers_Devices(User, ref Users, ref UserDevices);
            ViewBag.Users = Users;
            ViewBag.UserDevices = UserDevices;

            AV_WoDevicesBL wdb = new AV_WoDevicesBL();
            ViewBag.SelectedLayer = wdb.ToList("BySiteId", 528435, 0, 0, 0, 0, 0);
            CLS_VMBL abc = new CLS_VMBL();
            List<AV_SiteScript> lstSectors = abc.ToListSectors("GetSectorsCLS", 528435, 544433);
            ViewBag.SelectedLayer = wdb.ToList("BySiteId", 528435, 0, 0, 0, 0, 0);

            return PartialView("~/Views/Dashboard/_SiteGridSectorsCLS.cshtml", lstSectors);
        }
        [IsLogin(CheckPermission = false)]
        public ActionResult GetDashboardCharts(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, string MapStatus = null, string MapType = null, string DateRange = null)
        {

            ViewBag.TaskIds = TaskIds;
            ViewBag.LocationIds = LocationIds;
            ViewBag.ToDate = ToDate;
            ViewBag.FromDate = FromDate;
            if (!string.IsNullOrEmpty(DateRange))
            {
                var spDate = DateRange?.Split('-');
                if (spDate?.Count() == 2)
                {
                    FromDate = Convert.ToDateTime(spDate[0]);
                    ToDate = Convert.ToDateTime(spDate[1]);
                }
            }
           
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetDashboardCharts(Filter, ProjectId, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption, 0, 0, MapStatus, MapType);
            IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
                                                                                    .ToDictionary(a => a.Name, a => a.Value));

            return new JsonResult { Data = result, MaxJsonLength = Int32.MaxValue, JsonRequestBehavior=JsonRequestBehavior.AllowGet, ContentType = "Application/json; charset=UTF-8" };
            //return Json(result, JsonRequestBehavior.AllowGet);
           
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetTasks(Int64 ProjectId, string Filter, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null)
        {
            //PM_DashboardDL pd = new PM_DashboardDL();
            //DataTable dt = pd.GetMileStoneValues(Filter, ProjectId, 0, null, Value1);
            //IEnumerable<Dictionary<string, object>> result = dt.Select().Select(x => x.ItemArray.Select((a, i) => new { Name = dt.Columns[i].ColumnName, Value = a })
            //.ToDictionary(a => a.Name, a => a.Value));
            List<MilestoneModel> Mlist = new List<MilestoneModel>();
            List<MilestoneModel> Tlist = new List<MilestoneModel>();
            PM_DashboardDL pd = new PM_DashboardDL();
            DataTable dt = pd.GetDashboardCharts(Filter, ProjectId, MilestoneId, Value1, TaskIds, LocationIds, FromDate, ToDate, SearchFilter, FilterOption);

            foreach (DataRow item in dt.Rows)
            {
                MilestoneModel mil = new MilestoneModel();
                MilestoneModel tsk = new MilestoneModel();

                tsk.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                tsk.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                tsk.Task = item["Task"].ToString();
                tsk.Color = item["Color"].ToString();
                tsk.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                tsk.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                tsk.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
                mil.TaskId = Convert.ToInt64(item["TaskId"].ToString());
                mil.PTaskId = Convert.ToInt64(item["PTaskId"].ToString());
                mil.Task = item["Task"].ToString();
                mil.Color = item["Color"].ToString();
                mil.ProjectId = Convert.ToInt64(item["ProjectId"].ToString());
                mil.ActualSites = Convert.ToInt32(item["ActualSites"].ToString());
                mil.TotalSites = Convert.ToInt32(item["TotalSites"].ToString());
               // if (tsk.PTaskId == 0)
               // {
                    Mlist.Add(mil);
                //}
                //else
                //{
                //    Tlist.Add(tsk);
                //}
            }

            foreach (var mile in Mlist)
            {
                foreach (var task in Tlist)
                {
                    if (task.PTaskId == mile.TaskId)
                    {
                        mile.Tasks.Add(task);// = Tlist;
                    }
                }
            }
            return Json(Mlist, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetSummary(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, string Request = "")
        {
            if (Request == "JSON")
            {
                TempData.Keep("t1");
                TempData.Keep("t2");
                TempData.Keep("t3");
                var a = TempData["t1"] as IEnumerable<Dictionary<string, object>>;
                var b = TempData["t2"] as IEnumerable<Dictionary<string, object>>;
                var c = TempData["t3"] as IEnumerable<Dictionary<string, object>>;
                return Json(new { t1 = a, t2 = b, t3 = c }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                PM_DashboardDL pd = new PM_DashboardDL();
                DataSet ds = pd.GetDashboardWO(Filter, ProjectId, 5, 0, "", TaskIds, LocationIds, FromDate, ToDate, ViewBag.UserId.ToString());
                IEnumerable<Dictionary<string, object>> result1 = ds.Tables[0].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[0].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                IEnumerable<Dictionary<string, object>> result2 = ds.Tables[1].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[1].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                IEnumerable<Dictionary<string, object>> result3 = ds.Tables[2].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[2].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                TempData["t1"] = result1;
                TempData["t2"] = result2;
                TempData["t3"] = result3;
                return PartialView("~/Areas/Project/Views/Dashboard/_Summary.cshtml");

            }
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult GetKPI(string Filter, Int64 ProjectId = 0, Int64 MilestoneId = 0, string Value1 = "0", string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null, string SearchFilter = null, string FilterOption = null, string Request = "")
        {
            if (Request == "JSON")
            {
                TempData.Keep("t1");
                TempData.Keep("t2");
                TempData.Keep("t3");
                var a = TempData["t1"] as IEnumerable<Dictionary<string, object>>;
                var b = TempData["t2"] as IEnumerable<Dictionary<string, object>>;
                var c = TempData["t3"] as IEnumerable<Dictionary<string, object>>;
                return Json(new { t1 = a, t2 = b, t3 = c }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                PM_DashboardDL pd = new PM_DashboardDL();
                DataSet ds = pd.GetDashboardWO(Filter, ProjectId, 5, 0, "", TaskIds, LocationIds, FromDate, ToDate, ViewBag.UserId.ToString());
                IEnumerable<Dictionary<string, object>> result1 = ds.Tables[0].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[0].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                IEnumerable<Dictionary<string, object>> result2 = ds.Tables[1].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[1].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                IEnumerable<Dictionary<string, object>> result3 = ds.Tables[2].Select().Select(x => x.ItemArray.Select((a, i) => new { Name = ds.Tables[2].Columns[i].ColumnName, Value = a })
                                                                          .ToDictionary(a => a.Name, a => a.Value));
                TempData["t1"] = result1;
                TempData["t2"] = result2;
                TempData["t3"] = result3;
                return PartialView("~/Areas/Project/Views/Dashboard/_KPI.cshtml");

            }
        }



        [IsLogin(CheckPermission = false)]
        [HttpPost]


        //Int64 ProjectId, string Where = "", string Column = "", string Group = "", string Filter = "", DateTime? FromDate = null, DateTime? ToDate = null, string Markets = "", string Tasks = ""
        public CrystalReportPdfResult Pdf(Int64 ProjectId = 0, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {



            if (ProjectId > 0)
            {
                TempData["ProjectId"] = ProjectId;
                TempData.Keep("ProjectId");

                TempData["TaskIds"] = TaskIds;
                TempData.Keep("TaskIds");

                TempData["LocationIds"] = LocationIds;
                TempData.Keep("LocationIds");

                TempData["FromDate"] = FromDate;
                TempData.Keep("FromDate");

                TempData["ToDate"] = ToDate;
                TempData.Keep("ToDate");
            }


            string reportPath = Path.Combine(Server.MapPath("~/Report"), "rptProjectSummary.rpt");
            return new CrystalReportPdfResult(reportPath, Convert.ToInt64(TempData["ProjectId"] as Int64?), Convert.ToString(TempData["TaskIds"] as string), Convert.ToString(TempData["LocationIds"] as string),
            Convert.ToDateTime(TempData["FromDate"]), Convert.ToDateTime(TempData["ToDate"]));

            //return new CrystalReportPdfResult(reportPath, 20021, "50078", "163408,163409,163410,163411,163412,163413,163414,163415,163416,163405,163406,163407", DateTime.Parse("2017-02-02"), DateTime.Parse("2018-02-15"));
        }
        [ IsLogin(CheckPermission = false)]
        public class CrystalReportPdfResult : ActionResult
        {

            private readonly byte[] _contentBytes;

            public static Int64 _ProjectId;
            public static string _TaskIds;
            public static string _LocationIds;
            public static DateTime? _FromDate;
            public static DateTime? _ToDate;

            //public static Int64 ProjectId {
            //    get { return ProjectId; }
            //    set { ProjectId = value; }
            //}

            //public static string TaskIds {
            //    get { return TaskIds; }
            //    set { TaskIds = value; }
            //}
            //public static string LocationIds {
            //    get { return LocationIds; }
            //    set { LocationIds = value; }
            //}
            //public static DateTime FromDate {
            //    get { return FromDate; }
            //    set { FromDate = value; }
            //}
            //public static DateTime ToDate {
            //    get { return ToDate; } 
            //    set { ToDate = value; }
            //}


            public CrystalReportPdfResult(string reportPath, Int64 ProjectId = 0, string TaskIds = null, string LocationIds = null, DateTime? FromDate = null, DateTime? ToDate = null)
            {

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                _ProjectId = ProjectId;
                _TaskIds = TaskIds;
                _LocationIds = LocationIds;
                _FromDate = FromDate;
                _ToDate = ToDate;


                reportDocument.SetParameterValue("@ProjectId", _ProjectId);
                reportDocument.SetParameterValue("@Where", "");
                reportDocument.SetParameterValue("@Column", "*");
                reportDocument.SetParameterValue("@Group", "");
                reportDocument.SetParameterValue("@Filter", "CUSTOM_STATUS_REPORT");
                reportDocument.SetParameterValue("@FromDate", _FromDate);
                reportDocument.SetParameterValue("@ToDate", _ToDate);
                reportDocument.SetParameterValue("@Markets", _LocationIds);
                reportDocument.SetParameterValue("@Tasks", _TaskIds);

                reportDocument.SetParameterValue("@ProjectId", _ProjectId, "rptIssueChart");
                reportDocument.SetParameterValue("@Where", "", "rptIssueChart");
                reportDocument.SetParameterValue("@Column", "*", "rptIssueChart");
                reportDocument.SetParameterValue("@Group", "", "rptIssueChart");
                reportDocument.SetParameterValue("@Filter", "ISSUE_BY_REPORT", "rptIssueChart");
                reportDocument.SetParameterValue("@FromDate", _FromDate, "rptIssueChart");
                reportDocument.SetParameterValue("@ToDate", _ToDate, "rptIssueChart");
                reportDocument.SetParameterValue("@Markets", _LocationIds, "rptIssueChart");
                reportDocument.SetParameterValue("@Tasks", _TaskIds, "rptIssueChart");

                reportDocument.SetParameterValue("@ProjectId", _ProjectId, "rptNationalIssues");
                reportDocument.SetParameterValue("@Where", "", "rptNationalIssues");
                reportDocument.SetParameterValue("@Column", "*", "rptNationalIssues");
                reportDocument.SetParameterValue("@Group", "", "rptNationalIssues");
                reportDocument.SetParameterValue("@Filter", "NATIONAL_ISSUES", "rptNationalIssues");
                reportDocument.SetParameterValue("@FromDate", _FromDate, "rptNationalIssues");
                reportDocument.SetParameterValue("@ToDate", _ToDate, "rptNationalIssues");
                reportDocument.SetParameterValue("@Markets", _LocationIds, "rptNationalIssues");
                reportDocument.SetParameterValue("@Tasks", _TaskIds, "rptNationalIssues");

                reportDocument.SetDatabaseLogon("dev", "Sublime123", "192.168.3.8", "AirView");

                //reportDocument.SetDataSource(dataSet);

                _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));

            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.ApplicationInstance.Response;
                response.Clear();
                response.Buffer = false;
                response.ClearContent();
                response.ClearHeaders();
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.ContentType = "application/pdf";

                using (var stream = new MemoryStream(_contentBytes))
                {

                    stream.WriteTo(response.OutputStream);
                    stream.Flush();
                }
            }
            private static byte[] StreamToBytes(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;


                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }


        }

        //[IsLogin(CheckPermission = false)]
        //public ActionResult ManageWorkLogs()
        //{
        //    return PartialView("~/Areas/Project/Views/Issue/_WorkLogResources.cshtml");
        //}


        [IsLogin(CheckPermission = false)]
        public ActionResult WorkLogResources()
        {
            return View();
        }




        #region Configuration

        [IsLogin(CheckPermission = true)]
        public ActionResult Configuration(string Id)
        {
            ViewBag.ProjectId = Id;
            var oob = Permission.AllowProject(Convert.ToInt64(Id));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }
            AD_ChartSettings_BL dl = new AD_ChartSettings_BL();
            TempData.Keep("ProjectId");
            string ProjectId = TempData["ProjectId"] as string;
            // if (ProjectId == "")
            ProjectId = Id;
            /* == Setup ProjectId for Charts == */
            TempData["ProjectId"] = Id;
            List<ChartSettings> Data = dl.Get("By_ProjectId", ProjectId);
            ViewBag.Data = Data;
            ViewBag.SeriesTypes = new List<SelectListItem>
{
   new SelectListItem{ Text="line", Value = "line" },
   new SelectListItem{ Text="column", Value = "column" },

};
            return View();
        }

        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult Configurations(List<ChartSettings> wo)
        {
            Response res = new Response();

            try
            {
                AD_ChartSettings_BL bl = new AD_ChartSettings_BL();
                if (wo.Count > 0)
                {
                    for (int i = 0; i < wo.Count; i++)
                    {

                    }
                    bl.Insert("Insert", wo, Convert.ToString(ViewBag.UserId));
                    res.Status = "success";
                    res.Message = "save successfully";
                }
                else
                {
                    res.Status = "danger";
                    res.Message = "Atleast Enter one Project Site";
                }

            }
            catch (Exception ex)
            {
                res.Status = "danger";
                res.Message = ex.Message;
            }
            return Json(new { response = res }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public string IsNullCheck(string data)
        {
            if (data == null)
            {
                return "";
            }
            else
            {
                return data;
            }
            
        }
        #region Export
        [IsLogin(CheckPermission = false)]
        public FileStreamResult Export(Int64 ProjectId, string Where = "", string Column = "", string Group = "", Int64 UserId = 0)
        {
    
            MemoryStream memStream;
            string FileName = DateTime.Now.ToString();

            PM_Report_DL pd = new PM_Report_DL();
            DataTable dt = pd.GetDataTable_Exports(Where, ProjectId, Where, Column, Group, UserId);

            //var temp = dt.AsEnumerable()
            //                      //.GroupBy(r => new { Col1 = r["SiteType"] })
            //                      //.Select(g => g.OrderBy(r => r["SiteType"]).First())
            //                      .CopyToDataTable();

            var temp = dt;


            //if (dt.Rows.Count > 0) Remove this If started from here
            //{

                //  dt.Columns.Remove("ActualNetMode");
                //dt.Columns.Remove("ActualBand");
                //dt.Columns.Remove("ActualCarrier");
                using (var package = new ExcelPackage())
                {
                    //  FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();
                    FileName = Where + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                    var Sheet1 = package.Workbook.Worksheets.Add(Where.ToString());
                    //for (int t = 0; t < temp.Rows.Count; t++)
                    //{
                    //string TestType = (temp.Columns.Contains("SiteType")) ? temp.Rows[t]["SiteType"].ToString() : "";


                    //dt.DefaultView.RowFilter = "SiteType='" + TestType + "'";
                    DataTable CCW = dt;
                    // CCW.Columns.Remove("SiteType");

                    //if (t==0)
                    //{ 
                    // Sheet1 = package.Workbook.Worksheets.Add("ProjectSummary");
                    //}


                    for (int i = 0; i < CCW.Columns.Count; i++)
                    {
                        Sheet1.Cells[1, i + 1].Value = CCW.Columns[i].ColumnName;

                        if (CCW.Columns[i].DataType == typeof(System.DateTime))
                        {
                            Sheet1.Column(i + 1).Style.Numberformat.Format = "mm/dd/yyyy HH:mm";
                        }
                    }

                    string[,] TempDT = new string[CCW.Rows.Count+1, CCW.Columns.Count+1];

                    for (int i = 0; i < CCW.Rows.Count; i++)
                    {
                        for (int j = 0; j < CCW.Columns.Count; j++)
                        {
                            if(CCW.Rows[i][j].ToString() != "2" && CCW.Rows[i][j].ToString() != "1/1/1900 12:00:00 AM")
                            {
                                
                                if (i == 0) {
                                    
                                    Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                }
                                else
                                {
                                    if (j<9) {

                                        
                                        if (CCW.Rows[i - 1][0].ToString() != CCW.Rows[i][0].ToString() || CCW.Rows[i - 1][1].ToString() != CCW.Rows[i][1].ToString() || CCW.Rows[i - 1][2].ToString() != CCW.Rows[i][2].ToString() || CCW.Rows[i - 1][3].ToString() != CCW.Rows[i][3].ToString() || CCW.Rows[i - 1][4].ToString() != CCW.Rows[i][4].ToString() || CCW.Rows[i - 1][5].ToString() != CCW.Rows[i][5].ToString() || CCW.Rows[i - 1][6].ToString() != CCW.Rows[i][6].ToString() || CCW.Rows[i - 1][7].ToString() != CCW.Rows[i][7].ToString() || CCW.Rows[i - 1][8].ToString() != CCW.Rows[i][8].ToString())
                                        {
                                            Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                        }else
                                        {
                                            if (CCW.Rows[i - 1][j].ToString() != CCW.Rows[i][j].ToString())
                                            {
                                                Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                            }

                                        }
                                    }
                                    else
                                    {
                                        Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j];
                                    }
                                    
                                } 
                            }
                        }
                    }

                    //}

                    memStream = new MemoryStream(package.GetAsByteArray());
                }
                //return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
                //} Remove this If ended here

                return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            //return null;

            //using (var package = new ExcelPackage())
            //{
            //    // var CCW = rec.Where(m => m.TestType == "CCW").ToList().ToDataTable();



            //    if (dt.Rows.Count > 0)
            //    {
            //        FileName = dt.Rows[0][4] + "_" + dt.Rows[0][2] + "_" + dt.Rows[0][3] + "_" + dt.Rows[0][5] + "_" + DateTime.Now.ToString();


            //        #region CCW
            //        dt.DefaultView.RowFilter = "TestType='CCW'";
            //        DataTable CCW = (dt.DefaultView).ToTable();
            //        CCW.Columns.Remove("TestType");
            //        CCW.Columns.Remove("ActualNetMode");
            //        CCW.Columns.Remove("ActualBand");
            //        CCW.Columns.Remove("ActualCarrier");
            //        var Sheet1 = package.Workbook.Worksheets.Add("CCW");
            //        for (int i = 0; i < CCW.Columns.Count; i++)
            //        {
            //            Sheet1.Cells[1, i + 1].Value = CCW.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < CCW.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < CCW.Columns.Count; j++)
            //            {
            //                Sheet1.Cells[i + 2, j + 1].Value = CCW.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region CW
            //        dt.DefaultView.RowFilter = "TestType='CW'";
            //        DataTable CW = (dt.DefaultView).ToTable();
            //        CW.Columns.Remove("TestType");
            //        CW.Columns.Remove("ActualNetMode");
            //        CW.Columns.Remove("ActualBand");
            //        CW.Columns.Remove("ActualCarrier");
            //        // var CW = rec.Where(m => m.TestType == "CW").ToList().ToDataTable();
            //        var Sheet2 = package.Workbook.Worksheets.Add("CW");

            //        for (int i = 0; i < CW.Columns.Count; i++)
            //        {
            //            Sheet2.Cells[1, i + 1].Value = CW.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < CW.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < CW.Columns.Count; j++)
            //            {
            //                Sheet2.Cells[i + 2, j + 1].Value = CW.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion


            //        #region DL
            //        dt.DefaultView.RowFilter = "TestType='DL'";
            //        DataTable DL = (dt.DefaultView).ToTable();
            //        DL.Columns.Remove("TestType");
            //        DL.Columns.Remove("ActualNetMode");
            //        DL.Columns.Remove("ActualBand");
            //        DL.Columns.Remove("ActualCarrier");
            //        // var DL = rec.Where(m => m.TestType == "DL").ToList().ToDataTable();
            //        var Sheet3 = package.Workbook.Worksheets.Add("DL");


            //        for (int i = 0; i < DL.Columns.Count; i++)
            //        {
            //            Sheet3.Cells[1, i + 1].Value = DL.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < DL.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < DL.Columns.Count; j++)
            //            {
            //                Sheet3.Cells[i + 2, j + 1].Value = DL.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region UL
            //        dt.DefaultView.RowFilter = "TestType='UL'";
            //        DataTable UL = (dt.DefaultView).ToTable();
            //        UL.Columns.Remove("TestType");
            //        UL.Columns.Remove("ActualNetMode");
            //        UL.Columns.Remove("ActualBand");
            //        UL.Columns.Remove("ActualCarrier");
            //        //var UL = rec.Where(m => m.TestType == "UL").ToList().ToDataTable();
            //        var Sheet4 = package.Workbook.Worksheets.Add("UL");
            //        for (int i = 0; i < UL.Columns.Count; i++)
            //        {
            //            Sheet4.Cells[1, i + 1].Value = UL.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < UL.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < UL.Columns.Count; j++)
            //            {
            //                Sheet4.Cells[i + 2, j + 1].Value = UL.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion

            //        #region UL
            //        dt.DefaultView.RowFilter = "TestType='Ping'";
            //        DataTable Ping = (dt.DefaultView).ToTable();
            //        Ping.Columns.Remove("TestType");
            //        Ping.Columns.Remove("ActualNetMode");
            //        Ping.Columns.Remove("ActualBand");
            //        Ping.Columns.Remove("ActualCarrier");
            //        // var Ping = rec.Where(m => m.TestType == "Ping").ToList().ToDataTable();
            //        var Sheet5 = package.Workbook.Worksheets.Add("Ping");

            //        for (int i = 0; i < Ping.Columns.Count; i++)
            //        {
            //            Sheet5.Cells[1, i + 1].Value = Ping.Columns[i].ColumnName;
            //        }

            //        for (int i = 0; i < Ping.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < Ping.Columns.Count; j++)
            //            {
            //                Sheet5.Cells[i + 2, j + 1].Value = Ping.Rows[i][j].ToString();
            //            }
            //        }
            //        #endregion
            //    }

            //    memStream = new MemoryStream(package.GetAsByteArray());
            //}

            //return File(memStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
        }

        #endregion



        //#region Export
        //[IsLogin(CheckPermission = false)]
        //public FileContentResult Export(Int64 ProjectId, string Where = "", string Column = "", string Group = "", Int64 UserId = 0)
        //{
        //    string FileName = DateTime.Now.ToString();

        //    PM_Report_DL pd = new PM_Report_DL();
        //    DataTable dt = pd.GetDataTable(Where, ProjectId, Where, Column, Group, UserId);

        //    byte[] xlsInBytes;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        (WriteDataIntoExcelWithFormats(dt)).SaveAs(ms);
        //        xlsInBytes = ms.ToArray();
        //    }

        //       return File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");



        //}


        //public Microsoft.Office.Interop.Excel.Workbook WriteDataIntoExcelWithFormats(System.Data.DataTable p_dtData)
        //{
        //    //Step 1 : Add reference of Microsoft.Office.Interop.l_objExcel dll into project .  
        //    Microsoft.Office.Interop.Excel.Application l_objExcel;
        //    Microsoft.Office.Interop.Excel.Workbook l_objExcelworkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet l_objExcelSheet;

        //    try
        //    {
        //        // Create the object of l_objExcel application .  
        //        l_objExcel = new Microsoft.Office.Interop.Excel.Application();

        //        // Create workbook .  
        //        l_objExcelworkBook = l_objExcel.Workbooks.Add(Type.Missing);

        //        // Get active sheet from workbook  
        //        l_objExcelSheet = l_objExcelworkBook.ActiveSheet;
        //        l_objExcelSheet.Name = "Report";

        //        // For showing alert message of overwritting of existing file .  
        //        l_objExcel.DisplayAlerts = false;

        //        // Fill the l_objExcel from p_dtData data  .  
        //        for (int rowIndex = 0; rowIndex < p_dtData.Rows.Count; rowIndex++)
        //        {
        //            for (int colIndex = 0; colIndex < p_dtData.Columns.Count; colIndex++)
        //            {
        //                // Create the columns in the Excel .  
        //                if (rowIndex == 0)
        //                {
        //                    // Write column name into Excel cell .  
        //                    l_objExcelSheet.Cells[rowIndex + 1, colIndex + 1] = p_dtData.Columns[colIndex].ColumnName;
        //                    l_objExcelSheet.Cells.Font.Color = System.Drawing.Color.Black;

        //                }


        //                // Write row value into Excel cell .  
        //                l_objExcelSheet.Cells[rowIndex + 2, colIndex + 1] = p_dtData.Rows[rowIndex][colIndex];

        //                // Formating Excel cell on the bases of column type datatable  

        //                if (p_dtData.Columns[colIndex].DataType == Type.GetType("System.Decimal"))
        //                {
        //                    // Currency Format .  
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].NumberFormat
        //                        = "$#,##0.00_);[Red]($#,##0.00)";
        //                }
        //                else if (p_dtData.Columns[colIndex].DataType == Type.GetType("System.DateTime"))
        //                {
        //                    //datetime format  
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].NumberFormat
        //                        = "mm-d-yy h:mm:ss AM/PM";

        //                }
        //                else if (p_dtData.Columns[colIndex].DataType == Type.GetType("System.String"))
        //                {
        //                    // Set Font  
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].Font.Bold = true;
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].Font.Name = "Arial Narrow";
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].Font.Size = "20";

        //                }
        //                else if (p_dtData.Columns[colIndex].DataType == Type.GetType("System.Single"))
        //                {
        //                    // Set percentage  .  
        //                    l_objExcel.Range[l_objExcel.Cells[rowIndex + 2, colIndex + 1], l_objExcel.Cells[rowIndex + 2, colIndex + 1]].NumberFormat = "0.00%";
        //                }

        //            }
        //        }


        //        // Auto fit automatically adjust the width of columns of Excel  in givien range .  
        //        l_objExcelSheet.Range[l_objExcelSheet.Cells[1, 1], l_objExcelSheet.Cells[p_dtData.Rows.Count, p_dtData.Columns.Count]].EntireColumn.AutoFit();

        //        // To set the color, font size and bold, over top row, that represent columns of data table .  
        //        l_objExcelSheet.Range[l_objExcelSheet.Cells[1, 1], l_objExcelSheet.Cells[1, p_dtData.Columns.Count]].Interior.Color =
        //        System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);

        //        l_objExcelSheet.Range[l_objExcelSheet.Cells[1, 1], l_objExcelSheet.Cells[1, p_dtData.Columns.Count]].Font.Bold = true;

        //        l_objExcelSheet.Range[l_objExcelSheet.Cells[1, 1], l_objExcelSheet.Cells[1, p_dtData.Columns.Count]].Font.Size = 15;



        //        l_objExcelworkBook.Close();
        //        l_objExcel.Quit();


        //    }

        //    catch (Exception ex)
        //    {


        //    }

        //    finally
        //    {
        //        l_objExcelSheet = null;
        //        l_objExcelworkBook = null;
        //    }

        //    return l_objExcelworkBook;
        //}
        //#endregion

  
    }
}