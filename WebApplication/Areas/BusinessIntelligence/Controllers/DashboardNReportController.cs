using AirView.DBLayer.Common;

using AirView.DBLayer.Project.Model;
using GleamTech.DocumentUltimate.AspNet.UI;
using Library.SWI.Project.BLL;
using Newtonsoft.Json;
using Swi.Airview.Xcelerate.BusinessLogicLayer;
using Swi.Airview.Xcelerate.BusinessLogicLayer.Modules.Common;
using Swi.Airview.Xcelerate.CoreConfiguration;
using Swi.Airview.Xcelerate.DataTransferObject.Modules.DashboradNReport;
using SWI.AirView.Common;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Areas.BusinessIntelligence.Models;

namespace WebApplication.Areas.BusinessIntelligence.Controllers
{
    public class DashboardNReportController : Controller
    {
        private string ApiMapKey()
        {

            WebConfig wc = new WebConfig();
            string MapKey = wc.AppSettings("ApiMapKey").ToString();
            return MapKey;
        }
        [HttpPost]
        public async Task<JsonResult> PostTemplate(TemplateDto GetTemplate, string datasetsNameForRemove)
        {
            DbCommand UpdateDefaultTemplate = null;
            try
            {
                if (GetTemplate.IsActive)
                {
                    UpdateDefaultTemplate = BusinessLogic.instance.HandleDashboardNReportBl.InActiveAllTemplateAgainstByUserID(GetTemplate.userID);
                    UpdateDefaultTemplate = DataAccess.Instance.HandleTemplatesAction.EndTrans(UpdateDefaultTemplate);
                }
                await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.SaveTemplateCheckStatus(GetTemplate));
                return Json(new { status = true, msg = "Template has been saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                if (UpdateDefaultTemplate != null)
                    UpdateDefaultTemplate = DataAccess.Instance.HandleTemplatesAction.CanTrans(UpdateDefaultTemplate);
                return Json(new { status = false, msg = exe.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (UpdateDefaultTemplate != null)
                    DataAccess.Instance.HandleTemplatesAction.ClsCon(UpdateDefaultTemplate);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PostTemplateForETL(TemplateDto GetTemplate)
        {

            try
            {


                await Task.Run(() => { BusinessLogic.instance.HandleDashboardNReportBl.SaveTemplateCheckStatusWithETL(GetTemplate); });
                return Json(new { status = true, msg = "Template has been saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {

                return Json(new { status = false, msg = exe.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> CheckTemplateNameIsExistOrNot(int userID, string templateName)
        {
            var id = Session["user"];
            var _userId = (LoginInformation)Session["user"];
            var _userID = _userId.UserId;
            userID = Convert.ToInt32(_userID);
            try
            {
                bool statua = await BusinessLogic.instance.HandleDashboardNReportBl.checkDataExistGeneralFunc(userID, templateName, "isTemplateTitleAlreadyInUsed", "TMP_GetTemplates");

                return Json(new { status = true, temaplateStatus = statua, msg = templateName + " " + "already exist".ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { statua = false, msg = "There was an error" }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public async Task<JsonResult> DeleteDataSet(int NodeId)
        {

            try
            {

                var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.CloneOfDataset(Convert.ToInt32(NodeId), "EtlIsDeleted", ""));
                if (dt.Rows.Count > 0)
                {
                    var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                    return new JsonResult()
                    {
                        Data = new { data = obj, msg = "Deleted successfully", status = true },
                        ContentType = "application/json; charset=utf-8",
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = "Deleted failed", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult EditDataSet(int userID, string templateName)
        {
            try
            {
                bool statua = true;

                return Json(new { status = true, temaplateStatus = statua, msg = templateName + " " + "Edit successfully".ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { statua = false, msg = "There was an error" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetTemplate(int? userID, string filter)
        {
            try
            {
                if (userID != null)
                {
                    var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.GetTemplateByUserID(Convert.ToInt32(userID), filter));
                    if (dt.Rows.Count > 0)
                    {
                        var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                        return new JsonResult()
                        {
                            Data = new { data = obj, msg = "Got data", status = true },
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = "Data not found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
        [HttpPost]
        public async Task<JsonResult> GetTemplateByModuleTypeModuleTypeId(int? userID, string filter,string ModuleType,int ModuleTypeId)
        {
            try
            {
                if (userID != null)
                {
                    var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.GetTemplateByModuleTypeModuleTypeId(Convert.ToInt32(userID),  filter,  ModuleType,  ModuleTypeId));
                    if (dt.Rows.Count > 0)
                    {
                        var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                        return new JsonResult()
                        {
                            Data = new { data = obj, msg = "Got data", status = true },
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = "Data not found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public async Task<JsonResult> GetTemplateByModuleType(int? userID, string filter, string ModuleType)
        {
            try
            {
                if (userID != null)
                {
                    var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.GetTemplateByModuleType(Convert.ToInt32(userID), filter, ModuleType));
                    if (dt.Rows.Count > 0)
                    {
                        var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                        return new JsonResult()
                        {
                            Data = new { data = obj, msg = "Got data", status = true },
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = "Data not found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        
        [HttpPost]
        public async Task<JsonResult> CloneOfDatasets(int? userID, string createTempTableQuery, string query)
        {
            string responseMsg = "Data has not been found";
            try
            {
                if (userID != null && createTempTableQuery != "")
                {
                    try
                    {

                        var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.CloneOfDataset(Convert.ToInt32(userID), "cloneOfDatasets", createTempTableQuery));
                        var dt2 = BusinessLogic.instance.HandleQueryBuilderBl.GetQueryResult(query);
                        if (dt2.Rows.Count > 0)
                        {
                            var obj = JsonConvert.SerializeObject(dt2, Newtonsoft.Json.Formatting.None);

                            return new JsonResult()
                            {
                                Data = new { data = obj, msg = "Got data", status = true },
                                ContentType = "application/json; charset=utf-8",
                                MaxJsonLength = Int32.MaxValue,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }

                    }
                    catch (Exception e)
                    {
                        responseMsg = "Table already exists ";
                        // responseMsg = e.Message;
                    }


                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            if (createTempTableQuery == "")
            {
                responseMsg = "";
            }
            return new JsonResult()
            {
                Data = new { msg = responseMsg, status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public async Task<JsonResult> GetTempNodes(string draw, int start, int length)

        {
            string search = Request.QueryString["search[value]"];
            //start = start * length;
            // int offset = (start - 1) * length;
            int totalCount = 0;
            string responseMsg = "Data has not been found";
            //this below line comment by sohail
            // var id = Session["user"];
            var userId = (LoginInformation)Session["user"];
            var userID = userId.UserId;
            //these below two lines comment by sohail
            //List<TempTable> tempList = new List<TempTable>();
           // TempTable tempName = new TempTable();
            try
            {
                if (userID != null)
                {
                    try
                    {
                        var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.GetAllEtlRecord(Convert.ToInt32(userID), "TMP_GetNodes", "", start, length, search));
                        if (dt.Rows.Count >= 0)
                        {
                           
                            var list = dt.ToList<TemplateNodesForETL>();
                            if (list.Count() > 0)
                            {
                                totalCount = list[0].TotalCount;
                            }
                            //var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);
                            return Json(new
                            {
                                recordsTotal = totalCount,
                                recordsFiltered = totalCount,
                                draw = draw,
                                data = list
                            }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    catch (Exception e)
                    {
                        responseMsg = "Data has not been found ";
                        responseMsg = e.Message;
                    }


                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = responseMsg, status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public async Task<JsonResult> GetNodeByNodeId(int NodeId)

        {

            try
            {
                var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.CloneOfDataset(Convert.ToInt32(NodeId), "GetNodeById", ""));
                if (dt.Rows.Count > 0)
                {

                    var list = dt.ToList<TemplateNodesForETL>();


                    return Json(new { data = list }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception e)
            {
                return new JsonResult()
                {
                    Data = new { msg = e.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }



            return new JsonResult()
            {
                Data = new { msg = "Data has not been found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }
        [HttpGet]
        public async Task<JsonResult> GetNodeByNodeId1(int NodeId)

        {

            try
            {
                var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.CloneOfDataset(Convert.ToInt32(NodeId), "GetNodeById", ""));
                if (dt.Rows.Count > 0)
                {

                    var list = dt.ToList<TemplateNodesForETL>();


                    return Json(new { data = list }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception e)
            {
                return new JsonResult()
                {
                    Data = new { msg = e.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }



            return new JsonResult()
            {
                Data = new { msg = "Data has not been found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }
        [HttpPost]
        public async Task<JsonResult> GetTemplateById(int? TemplateID)
        {
            try
            {
                var getTemp = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.GetTemplateByID(TemplateID));
                return Json(new { data = getTemp, status = true, msg = "Template has been saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                return Json(new { status = false, msg = exe.InnerException.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetDocumentsFromDB()
        {
            string responseMsg = "Data has not been found";
            var id = Session["user"];
            var userId = (LoginInformation)Session["user"];
            var userID = userId.UserId;
            try
            {
                if (userID != null)
                {
                    var dt = await Task.Run(() => BusinessLogic.instance.HandleDashboardNReportBl.CloneOfDataset(Convert.ToInt32(userID), "GetDocumentsWithPath", ""));
                    if (dt.Rows.Count > 0)
                    {
                        var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                        return new JsonResult()
                        {
                            Data = new { data = obj, msg = "Got data", status = true },
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new { msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new { msg = "Data not found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public PartialViewResult DocumentViewer(string documentPath)
        {

            ViewBag.documentViewer = new GleamTech.DocumentUltimate.AspNet.UI.DocumentViewer
            {
                Width = 1190,
                Height = 950,
                //  Document = "~/Content/SamplePPT.ppt",
                Document = documentPath,
                //  Document = "~/Content/DocumentNew.pdf",
                DownloadAsPdfEnabled = false,
                DownloadEnabled = false,
                PrintEnabled = false,
                //SidePaneVisible = false,
                //HighlightedKeywords = new[] { "how" },
                HighQualityEnabled = true,
                LoadingMessage = "Procssing Document Please Wait.",
                //ToolbarVisible = false,
                FitMode = FitMode.FitWidth,
                LayoutMode = LayoutMode.Continuous
            };
            return PartialView("~/Areas/BusinessIntelligence/Views/Home/DocumentsViewer.cshtml");

        }
        [HttpGet]
        [IsLogin(CheckPermission = false)]
        public PartialViewResult getPartialPage(string dirName, string FileName)
        {


            return PartialView("~/Areas/BusinessIntelligence/Views/" + dirName + "/" + FileName);
        }
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public PartialViewResult GetPartialForEntity(string FilePath, int EntityId)
        {
            string mapapikey = ApiMapKey();
            ViewBag.ApiMapKey = mapapikey;
            return PartialView(FilePath, EntityId);
        }
        [HttpGet]
        [IsLogin(CheckPermission = false)]
        public PartialViewResult GetPartialForEntity1(string FilePath, int EntityId)
        {
            string mapapikey = ApiMapKey();
            ViewBag.ApiMapKey = mapapikey;
            return PartialView(FilePath, EntityId);
        }
   
    }
}