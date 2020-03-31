using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using Library.SWI.Project.BLL;
using Library.SWI.Survey.BLL;
using SWI.AirView.Common;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [IsLogin, ErrorHandling]
    public class ProjectSitesController : Controller
    {
        PM_ProjectSitesBL ub = new PM_ProjectSitesBL();

        #region New

        [IsLogin(CheckPermission = true)]
        public ActionResult New(Int64 Id = 0)
        {

            AD_DefinationBL db = new AD_DefinationBL();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

            ViewBag.FormType = (TempData["FormType"] != null) ? "Edit" : "New";

            ViewBag.Bands = db.ToList("Bands");

            ViewBag.Id = Id;

            ViewBag.Carriers = db.ToList("Carriers");

            ViewBag.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            if (Id == 0)
            {
                ViewBag.Projects = null;
            }
            else
            {
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

                ViewBag.Projects = Table.ToList<PM_Projects>().Where(x => x.ProjectId == Id).ToList();
            }
            ViewBag.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            ViewBag.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            ViewBag.NetworkModesm = hjh;
            ViewBag.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-"); //sl.States();
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            ViewBag.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString()); ;
            ViewBag.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            ViewBag.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            ViewBag.SiteTypes = sl.Definations("SiteTypes");
            ViewBag.SiteClasses = sl.Definations("SiteClasses");
            ViewBag.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            ViewBag.Sectors = sl.Sectors();
            return View();

           
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult New(List<ProjectSites> wo)
        {
            Response res = new Response();
            try
            {
                var duplicateValues = wo?.Select(x => x.SiteCode).GroupBy(x => x).Where(group => group.Count() > 1).Select(g => g.Key).ToList();
                string FACODE = "";
                bool IsExist = false;
                if (wo?.Select(x => x.SiteCode).Distinct()?.Count() != wo?.Count)
                {
                    res.Status = "exist";
                    FACODE = string.Join(",", duplicateValues);
                    IsExist = true;
                }

                List<string> FACODEList = new List<string>();
                int IsSiteCodeExist = 0;
                foreach (var item in wo)
                {
                    IsSiteCodeExist = ub.IsSiteCodeExistInProject("IsSiteCodeExistInProjectForNew", 0, item.ProjectId , item.SiteCode);
                    if (IsSiteCodeExist > 0)
                    {
                        FACODEList.Add(item.SiteCode);
                    }
                }
                if (FACODEList.Any() || IsExist == true)
                {
                    FACODE = FACODE + "," + string.Join(",", FACODEList);
                    var Filter = FACODE?.Split(',')?.GroupBy(x => x)?.Select(x => x.Key);
                    FACODE = string.Join(",", Filter);
                    res.Status = "exist";
                    res.Message = "Sorry following Site Id/FACode should be unique in project: " + FACODE;
                    return Json(new { response = res }, JsonRequestBehavior.AllowGet);
                }

                if (IsSiteCodeExist == -1)
                {
                    res.Status = "exception";
                    return Json(new { response = res }, JsonRequestBehavior.AllowGet);
                }

                if (wo.Count > 0)
                {
                    ub.Insert("Insert", wo, Convert.ToInt64(ViewBag.UserId), 0, wo[0].SiteName);
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

        #region GET
        [IsLogin(CheckPermission = true)]
        public ActionResult All(string Id = "0")
        {
            try
            {
                if(Id == "0" || string.IsNullOrEmpty(Id))
                {
                    return HttpNotFound();
                }
                else
                {
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
                ViewBag.Id = Id;
                TempData["ProjectId"] = Id;
                //ViewBag.Projects = ub.ToList("Get_All");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Id = Id;
                TempData["ProjectId"] = Id;
                return View();
                throw ex;
            }
        }

        #endregion

        #region edit

        #region Edit Region
        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(int Id=0,int ProjectSiteId = 0)
        {
            if (Id > 0)
            {
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
            AD_DefinationBL db = new AD_DefinationBL();
            Sec_UserSettingsDL udl = new Sec_UserSettingsDL();

            ViewBag.FormType = (TempData["FormType"] != null) ? "Edit" : "New";

            ViewBag.Id = Id;

            ViewBag.Bands = db.ToList("Bands");

            ViewBag.ProjectSiteId = ProjectSiteId;
            ViewBag.ProjectSite = ub.ToList("Get_By_Id", ProjectSiteId.ToString()).FirstOrDefault();
            if (ViewBag.ProjectSite == null)
            {
                TempData["msg_error"] = "Record Not Exist or You Are Not Authorize to access !";
                return RedirectToAction("All");
            }
            ViewBag.Carriers = db.ToList("Carriers");

            ViewBag.Regions = db.ToList("UserRegions", Convert.ToString(ViewBag.UserId));
            DataTable Table = udl.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
            ViewBag.Projects = Table.ToList<PM_Projects>().Where(x => x.ProjectId == Id).ToList();
            ViewBag.Cities = db.ToList("UserCities", Convert.ToString(ViewBag.UserId));
            ViewBag.SubCheckList = db.ToList("byDefinationType", "Sub CheckList");

            SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
            ViewBag.NetworkModes = db.SelectedList("NetworkModes", null, "-NetworkMode-");
            var hjh = db.MultiSelecet("NetworkModes", null, "-NetworkMode-");
            ViewBag.NetworkModesm = hjh;
            ViewBag.States = sl.Definations("UserStates", Convert.ToString(ViewBag.UserId), "-State-"); //sl.States();
            TSS_SurveyDocumentBL sdb = new TSS_SurveyDocumentBL();
            ViewBag.Surveys = sdb.ToList("GetAll_byIsActive", true.ToString()); ;
            ViewBag.Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));// sl.Scopes();
            ViewBag.Clients = sl.Clients("UserClients", Convert.ToString(ViewBag.UserId));
            ViewBag.SiteTypes = sl.Definations("SiteTypes");
            ViewBag.SiteClasses = sl.Definations("SiteClasses");
            ViewBag.CheckList = sl.Definations("byDefinationType", "CheckList", "-CheckList-");
            ViewBag.Sectors = sl.Sectors();
           return View();
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult Edit(ProjectSites wo, bool? ControlledIntro, bool? VMME, bool? SuperBowl)
        {
            try
            {
                ub.Update("Update", wo, Convert.ToInt64(ViewBag.UserId));
            }
            catch (Exception ex)
            {
                TempData["message"] = "error";
                return View();
            }
            string ProjectId = TempData["ProjectId"] as string;
            TempData.Keep("ProjectId");
            TempData["message"] = "success";
            //  TempData["msg_success"] = "success";
            //return RedirectToAction("All", new { Id = ProjectId });
            return RedirectToAction("Edit", new { Id = wo.ProjectId, ProjectSiteId = wo.ProjectSiteId });
        }
        #endregion

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult IsSiteCodeExistInProject(int ProjectSiteId, int ProjectId, string SiteCode)
        {
            int IsSiteIdExist = ub.IsSiteCodeExistInProject("IsSiteCodeExistInProject", ProjectSiteId, ProjectId, SiteCode);
            if(IsSiteIdExist > 0)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        #endregion

        #region isactive

        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult IsActive(Int64 ProjectSiteId, bool IsActive, int StatusId)
        {
            Response res = new Response();
            try
            {
                bool result = ub.ActiveDeactive("IsActive", ProjectSiteId, IsActive);
                string Status = IsActive == true ? "Activated" : "Deactivated";
                res.Status = "success";
                res.Message = "Site is " + Status;
                res.Value = result;
            }
            catch (Exception ex)
            {

                res.Status = "danger";
                res.Message = "There is an error. Please try again!";
                // ExceptionHandling.ErrorLogs(ViewBag.RequestUrl, ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Paging

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult Paging(int current, int rowCount, string searchPhrase, bool status, string PId)
        {
            //string ProjectId = TempData["ProjectId"] as string;
            string ProjectId = PId;
            if (ProjectId == null)
                ProjectId = "0";
            TempData.Keep("ProjectId");
            current = (current == 0) ? 1 : current;
            rowCount = (rowCount == 0) ? 5 : rowCount;

            int offset = (current - 1) * rowCount;
            int TotalRecord = 0;
            var rec = ub.Paging(offset, rowCount, searchPhrase, ref TotalRecord, ProjectId, status);
            // var rec = cb.ToList("AllRecords");
            return Json(new { current = current, total = TotalRecord, rows = rec, rowCount = rowCount }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        

        [HttpPost, IsLogin(CheckPermission = false)]
        public ActionResult UploadSites(string id)
        {
            var oob = Permission.AllowProject(Convert.ToInt64(id));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
                
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            Response response = new Response();
            response.Status = "Unsuccessfull";
            response.Message = "Site(s) not Imported. Please try again!";
            try
            {
                AD_DefinationBL defination = new AD_DefinationBL();
                Sec_UserSettingsDL usrSetting = new Sec_UserSettingsDL();
                DataTable Table = usrSetting.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
               var ProjectsList =  Table.ToList<PM_Projects>().Where(x => x.ProjectId == long.Parse(id)).ToList();
                bool isProjectNotEExist = false;
                List<string> ErrorFile = new List<string>();
                PM_ProjectBL projectBL = new PM_ProjectBL();
                var ProjectFACodeList = projectBL.ProjectFACode("ProjectFACodeList", Convert.ToInt64(id));
                SWI.AirView.Common.SelectedList sl = new SWI.AirView.Common.SelectedList();
                string FileNamePrefix = DateTime.Now.ToString("Error_File_ddMMhhmmss_");
                string FileName = null;
                bool IsFileExist = false;
                string[] DataInTable = null;
                string path = null;
                Int64 ProjectIdOut = 0;
                if (isProjectNotEExist)
                {
                    Table = usrSetting.GetDataTable("UserProjects", Convert.ToString(ViewBag.UserId), null, null);
                    ProjectsList = Table.ToList<PM_Projects>().Where(x => x.ProjectId == long.Parse(id)).ToList();
                }
                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                {
                    
                    response.Status = "InvalidProjectID";
                    response.Message = "Project ID is Empty. Please close the tab and reopen it.";
                    return Json(response);
                }
                else
                {
                    if (!Int64.TryParse(id, out ProjectIdOut))
                    {
                        response.Status = "InvalidProjectID";
                        response.Message = "Project ID is Invalid. Please close the tab and reopen it.";
                        return Json(response);
                    }
                }
                if (ProjectsList?.Count <= 0)
                {

                    response.Status = "ProjectPermissionIssue";
                    response.Message = "Sorry, You don't have permission for this Project.";
                    return Json(response);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    FileName = FileNamePrefix + fileName;
                    path = Path.Combine(Server.MapPath("~/files/Raw"), FileName);
                    if (Path.GetExtension(path) != ".csv")
                    {
                        response.Status = "Unsuccessfull";
                        response.Message = "Invalid file format, Only CSV file supported.";
                        return Json(response);
                    }
                    file.SaveAs(path);
                    IsFileExist = true;
                }
                if (IsFileExist)
                {
                    List<SelectListItem> Scopes = sl.Definations("UserScopes", Convert.ToString(ViewBag.UserId));
                    var SiteTypes = sl.Definations("SiteTypes");
                    List<SWI.Libraries.AD.Entities.AD_Defination> Cities = defination.ToList("UserCities", Convert.ToString(ViewBag.UserId));
                    DataInTable = System.IO.File.ReadAllLines(path);
                    List<ProjectSites> Sites = new List<ProjectSites>();
                    string FileHeader = string.Empty;
                    if(DataInTable?.Count() == 1)
                    {
                        response.Status = "EmptyFile";
                        response.Message = "File is empty!";
                        return Json(response);
                    }
                    if(DataInTable?.Count() > 0)
                    {
                        FileHeader = string.Join(",",DataInTable[0].Split(',').Take(20).ToArray()) + ",Error(s)";
                    }
                    ErrorFile.Add(FileHeader);
                    string InValidInput = string.Empty;
                    string RequiredFileds = string.Empty;
                    string InvalidLength = string.Empty;
                    string InvalidDataType = string.Empty;
                    string FACodeAlreadyExist = string.Empty;
                    List<string> FACodeInFile = new List<string>();
                    foreach(var item in DataInTable.Skip(1))
                    {
                        var cols = item.Split(',');
                        if(cols?.Count() > 0)
                        {
                            FACodeInFile.Add(cols[3]);
                        }
                    }
                    foreach (var row in DataInTable.Skip(1))
                    {
                        InValidInput = string.Empty;
                        RequiredFileds = string.Empty;
                        InvalidLength = string.Empty;
                        InvalidDataType = string.Empty;
                        FACodeAlreadyExist = string.Empty;
                        ProjectSites site = new ProjectSites();
                        site.ProjectId = Convert.ToInt64(id);
                        var Cols = row.Split(',');

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 0)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 0)))
                        {
                            var Market = Cities?.FirstOrDefault(x => x.DefinationName.ToLower() == Cols[0].ToLower());
                            if (Market != null)
                            {
                                 site.Market = Convert.ToString(Market.DefinationId);
                            }
                            else
                            {
                                InValidInput += "[Market]";
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Market]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 1)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 1)))
                        {
                            var Scope = Scopes?.FirstOrDefault(x => x.Text.ToLower() == Cols[1].ToLower());
                            if (Scope != null)
                            {
                                site.Scope = Convert.ToString(Scope.Value);
                            }
                            else
                            {
                                InValidInput += "[Scope]";
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 2)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 2)))
                        {
                            if(Cols[2].Length > 50)
                            {
                                InvalidLength += "[Cluster Code > 50]";
                            }
                            else
                            {
                                site.ClusterCode = Cols[2];
                                site.clusterId = Cols[2];
                            }
                                
                        }
                        else
                        {
                            RequiredFileds += "[Cluster Code]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 3)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 3)))
                        {
                            if (Cols[3].Length > 25)
                            {
                                InvalidLength += "[Site Code > 25]";
                            }
                            else
                            {
                                var IsFACodeExist = ProjectFACodeList?.Where(x => x.FACode.Trim().ToLower() == Cols[3].Trim().ToLower())?.Count() > 0 ? true : false;
                                bool IsFACodeExistInFile = FACodeInFile?.Where(x => x.Trim().ToLower() == Cols[3].Trim().ToLower())?.Count() > 1 ? true : false;
                                if (IsFACodeExist == false && IsFACodeExistInFile == false)
                                {
                                    site.SiteCode = Cols[3];
                                    site.FACode = Cols[3];
                                }
                                else
                                {
                                    FACodeAlreadyExist += "[Site Code]";
                                }
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Site Code]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 4)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 4)))
                        {
                            if (Cols[4].Length > 500)
                            {
                                InvalidLength += "[Site Name > 500]";
                            }
                            else
                            {
                                site.SiteName = Cols[4];
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Site Name]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 5)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 5)))
                        {
                            double Out = 0;
                            if (!double.TryParse(Cols[5], out Out))
                                InvalidDataType += "[Latitude]";
                            if (Cols[5].Length > 500)
                            {
                                InvalidLength += "[Latitude > 500]";
                            }
                            else
                            {
                                site.siteLatitude = Cols[5];
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Latitude]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 6)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 6)))
                        {
                            double Out = 0;
                            if (!double.TryParse(Cols[6], out Out))
                                InvalidDataType += "[Longitude]";
                            if (Cols[6].Length > 500)
                            {
                                InvalidLength += "[Longitude > 500]";
                            }
                            else
                            {
                                site.siteLongitude = Cols[6];
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Longitude]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 7)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 7)))
                        {
                            var siteTypes = SiteTypes?.FirstOrDefault(x => x.Text.ToLower() == Cols[7].ToLower());
                            if (siteTypes != null)
                            {
                                site.SiteTypeId = Convert.ToString(siteTypes.Value);
                            }
                            else
                            {
                                InValidInput += "[Site Type]";
                            }
                        }
                        else
                        {
                            RequiredFileds += "[Site Type]";
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 8)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 8)))
                        {
                            if (Cols[8].Length > 7)
                            {
                                InvalidLength += "[Color > 7]";
                            }
                            else
                            {
                                site.Color = Cols[8];
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 9)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 9)))
                        {
                            if (Cols[9].Length > 500)
                            {
                                InvalidLength += "[Description > 500]";
                            }
                            else
                            {
                                site.Description = Cols[9];
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 10)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 10)))
                        {
                            if (Cols[10].Length > 25)
                            {
                                InvalidLength += "[USID > 25]";
                            }
                            else
                            {
                                site.USID = Cols[10];
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 11)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 11)))
                        {
                            if (Cols[11].Length > 150)
                            {
                                InvalidLength += "[Sub Market > 150]";
                            }
                            else
                            {
                                site.SubMarket = Cols[11];
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 12)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 12)))
                        {
                            bool Out = false;
                            if(!bool.TryParse(Cols[12], out Out))
                            {
                                InvalidDataType += "[vMME]";
                            }
                            else
                            {
                                site.vMME = Convert.ToBoolean(Cols[12]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 13)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 13)))
                        {
                            bool Out = false;
                            if (!bool.TryParse(Cols[13], out Out))
                            {
                                InvalidDataType += "[Controlled Intro]";
                            }
                            else
                            {
                                site.ControlledIntro = Convert.ToBoolean(Cols[13]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 14)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 14)))
                        {
                            bool Out = false;
                            if (!bool.TryParse(Cols[14], out Out))
                            {
                                InvalidDataType += "[Super Bowl]";
                            }
                            else
                            {
                                site.SuperBowl = Convert.ToBoolean(Cols[14]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 15)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 15)))
                        {
                            if (Cols[15].Length > 50)
                            {
                                InvalidLength += "[isDASInBuild > 50]";
                            }
                            else
                            {
                                site.isDASInBuild = Convert.ToString(Cols[15]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 16)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 16)))
                        {
                            if (Cols[16].Length > 50)
                            {
                                InvalidLength += "[FirstNetRAN > 50]";
                            }
                            else
                            {
                                site.FirstNetRAN = Convert.ToString(Cols[16]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 17)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 17)))
                        {
                            if (Cols[17].Length > 50)
                            {
                                InvalidLength += "[IPlan Job > 50]";
                            }
                            else
                            {
                                site.IPlanJob = Convert.ToString(Cols[17]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 18)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 18)))
                        {
                            if (Cols[18].Length > 50)
                            {
                                InvalidLength += "[PaceNo > 50]";
                            }
                            else
                            {
                                site.PaceNo = Convert.ToString(Cols[18]);
                            }
                        }

                        if (!string.IsNullOrEmpty(Utilities.ElementAtOrDefault(Cols, 19)) && !string.IsNullOrWhiteSpace(Utilities.ElementAtOrDefault(Cols, 19)))
                        {
                            DateTime Out = DateTime.Now;
                            if(!DateTime.TryParse(Cols[19], out Out))
                            {
                                InvalidDataType += "[Invalid IPlanIssueDate]";
                            }

                            if (Cols[19].Length > 10)
                            {
                                InvalidLength += "[IPlanIssueDate > 10]";
                            }
                            else
                            {
                                site.IPlanIssueDate = Convert.ToString(Cols[19]);
                            }
                        }
                       
                        if(string.IsNullOrEmpty(RequiredFileds) && string.IsNullOrEmpty(InValidInput) && string.IsNullOrEmpty(InvalidDataType) && string.IsNullOrEmpty(InvalidLength) && string.IsNullOrEmpty(FACodeAlreadyExist))
                        {
                            Sites.Add(site);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(RequiredFileds))
                            {
                                RequiredFileds += (RequiredFileds?.TrimEnd(',')?.Split(',')?.Count() >= 2) ? " are required." : " is required.";
                            }

                            if (!string.IsNullOrEmpty(InvalidLength))
                            {
                                InvalidLength += " has Invalid Length.";
                            }

                            if (!string.IsNullOrEmpty(InvalidDataType))
                            {
                                InvalidDataType += " have invalid data type.";
                            }

                            if (!string.IsNullOrEmpty(InValidInput))
                            {
                                InValidInput += " have invalid input.";
                            }

                            if (!string.IsNullOrEmpty(FACodeAlreadyExist))
                            {
                                FACodeAlreadyExist += " already exist.";
                            }

                            string NewRowWithErrors = $"{string.Join(",", Cols.Take(20).ToArray()).TrimEnd(',')},{RequiredFileds} {FACodeAlreadyExist} {InValidInput} {InvalidDataType} {InvalidLength} ";
                            ErrorFile.Add(NewRowWithErrors);
                        }
                    }
                 
                    if(Sites?.Count > 0 && ErrorFile.Count <= 1)
                    {
                        var IsInserted = ub.Insert("Insert", Sites, Convert.ToInt64(ViewBag.UserId), 0, Sites[0].SiteName);
                        if (IsInserted)
                        {
                            response.Status = "Ok";
                            response.Message = "Site(s) imported successfully!";
                        }
                        else
                        {
                            response.Status = "Unsuccessfull";
                            response.Message = "There is an unexpected error. Please try again!";
                        }
                    }
                    else if(ErrorFile.Count > 1)
                    {
                        System.IO.File.WriteAllLines(Path.Combine(Server.MapPath("~/files/Raw"), FileName), ErrorFile);
                        response.Status = "OkWithErrorFile";
                        response.Value = $"/files/Raw/{FileName}";
                        response.Message = "File could not be uploaded due to errors in file.";
                    }
                    else
                    {
                        response.Status = "Unsuccessfull";
                        response.Message = "There is an unexpected error. Please try again!";
                    }
                }
            }
            catch (Exception e)
            {
                return Json(response);
            }
            return Json(response);
        }
    }
}