using AirView.DBLayer.Template.BLL;
using AirView.DBLayer.Template.DAL;
using AirView.DBLayer.Template.Model;
using Library.SWI.Template.Model;
using Newtonsoft.Json;
using PagedList;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.AD.Entities;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication.Areas.Project.View_Models;
using SWI.AirView.Common;
using AirView.DBLayer.Common;
using AirView.DBLayer.Schema.BAL;
using System.Text.RegularExpressions;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.Model;
using AirView.Filters.MobileAuth;

namespace WebApplication.Areas.Project.Controllers
{
    [IsLogin]
    public class TemplateController : Controller
    {
        #region PRIVATE PROPERTIES

        private TMP_NodeBL NodeBL = new TMP_NodeBL();
        private TMP_NodesPropertiesBL NodesPropertiesBL = new TMP_NodesPropertiesBL();
        private TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
        private TMP_NodeSettingsBL NodeSettingsBL = new TMP_NodeSettingsBL();
        private AD_DefinationBL DefinationBL = new AD_DefinationBL();
        private TMP_GetSiteReportBL GetSiteReportBL = new TMP_GetSiteReportBL();

        private DirectoryHandler dh = new DirectoryHandler();

        #endregion

        // GET: Project/Template
        /// <summary>
        /// INDEX VIEW
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public ActionResult Index(int? id)
        {
  
            if (id > 0)
            {
                //var oob = Permission.AllowProject(Convert.ToInt64(id));
                //if (oob == null)
                //{
                //    TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                //    return RedirectToAction("index", "error", new { Area = "Project" });
                //}
                //else
                //{
                   
                //     TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
                //}

                ViewBag.TemplateId = id;
                var templateInfo = TemplatesBL.ToList("GetById", id.ToString());

                ViewBag.TablesName = new SelectList(new TablesBL().ViewsList(), "Name", "Name");
                if (templateInfo != null && templateInfo.Count > 0)
                {
                    return View();
                }
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }

        /// <summary>
        /// TEMPLATE LIST VIEW
        /// </summary>
        /// <returns></returns>
        [IsLogin(CheckPermission = true)]
        public ActionResult List(int? projectId = 0)
        {
            if (projectId == null || projectId == 0)
                throw new NullReferenceException();
            List<TMP_Templates> templateDataList = new List<TMP_Templates>();
            ViewBag._ProjectID = projectId;
            //ViewBag._ScopeId = scopeId;

            templateDataList = TemplatesBL.ToList("GET_ALL").Where(x => x.ProjectId == projectId).ToList();

            for (int i = 0; i < templateDataList.Count(); i++)
            {
                if (!string.IsNullOrEmpty(templateDataList[i].Parameters))
                {
                    dynamic parametersJson = JsonConvert.DeserializeObject(templateDataList[i].Parameters);
                    var urlParamList = "?";

                    foreach (var item in parametersJson)
                    {
                        urlParamList += item.ParameterName + "=&";
                    }
                    urlParamList = urlParamList.TrimEnd('&');
                    templateDataList[i].Parameters = urlParamList;
                }
            }

            return View(templateDataList);
        }

        /// <summary>
        /// REPORT VIEW
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public ActionResult Report(int? id)
        {
            ViewBag.Id = id != 0 ? id.ToString() : "0";
            return View();
        }

        /// <summary>
        /// DASHBOARD VIEW
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [IsLogin(CheckPermission = false)]
        public ActionResult Dashboard(string Id, int? ProjectId)
        {
            var templateData = TemplatesBL.ToList("GetById", Id).FirstOrDefault();
            ViewBag.PageType = templateData.PageType.ToLower();
            ViewBag.Id = Id;
            var oob = Permission.AllowProject(Convert.ToInt64(ProjectId));
            if (oob == null)
            {
                TempData["msg_error"] = "This Project is not assigned to you. Please contact administrator for project assignment.";
                return RedirectToAction("index", "error", new { Area = "Project" });
            }
            else
            {
               
                 TempData["ProjectEntity"]  = oob; TempData.Keep("ProjectEntity");
            }

            // ViewBag.ScopeId = ScopeId;
            //ViewBag.Page = page;
            //ViewBag.PageStartIndex = StartIndex;
            //ViewBag.PageEndIndex = EndIndex;
            return View();
        }


        /// <summary>
        /// GET Node JSON and settings data against specific template
        /// </summary>
        /// <param name="TemplateId">Template Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetNodeData(string TemplateId, string CurrentPageURL = "")
        {
            var templateData = TemplatesBL.ToList("GetById", TemplateId).FirstOrDefault();
            var nodeList = NodeBL.ToList("ByTemplateId", TemplateId);
            var NodesList = nodeList.Select(p => new { id = p.NodeId, x = p.x_axis, y = p.y_axis, width = p.Width, height = p.Height, title = p.NodeTitle, nodeUrl = p.NodeUrl, Data = Utilities.RenderToString(GetNodeWidgetPartial(p.NodeId.ToString(), CurrentPageURL, "false")) });
            string nodeJSON = "";
            nodeJSON = new JavaScriptSerializer().Serialize(NodesList);
            return Json(new
            {
                NodeJson = nodeJSON,
                TemplateCoreSetting = templateData
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// For Adding new Template 
        /// </summary>
        /// <param name="TemplateId">Template Id</param>
        /// <param name="TemplateTitle">Title for Template</param>
        /// <param name="BackgroundColor">Background color for template</param>
        /// <param name="PageType">Page Type like Full Paged or Containeed Page</param>
        /// <param name="ParameterJSON">Parameters JSON contains SiteId, NetworkModeId etc.</param>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="ScopeId">Scope Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult AddTemplate(string TemplateId, string TemplateTitle, string BackgroundColor, string PageType, string ParameterJSON, int? ProjectId, int? ScopeId, string TemplateType, int ModuleType)
        {
            if (!string.IsNullOrEmpty(TemplateTitle))
            {
                // Add PROJECT ID and SCOPE ID when you need
                if (!string.IsNullOrEmpty(TemplateId))
                {
                    TemplatesBL.Manage("Update", new TMP_Templates
                    {
                        TemplateId = int.Parse(TemplateId),
                        TemplateTitle = TemplateTitle,
                        BackgroundColor = BackgroundColor,
                        ProjectId = ProjectId ?? 0,
                        ScopeId = ScopeId ?? 0,
                        PageType = PageType,
                        Parameters = ParameterJSON,
                        ModuleId = ModuleType,
                        TemplateType = TemplateType
                    });
                    return Json("Ok");
                }
                else
                {
                    TemplatesBL.Manage("Insert", new TMP_Templates
                    {
                        TemplateTitle = TemplateTitle,
                        BackgroundColor = BackgroundColor,
                        PageType = PageType,
                        ProjectId = ProjectId ?? 0,
                        ScopeId = ScopeId ?? 0,
                        Parameters = ParameterJSON,
                        IsActive = true,
                        IsDefault = false,
                        ModuleId = ModuleType,
                        TemplateType = TemplateType
                    });
                    return Json("Ok");
                }
            }
            return Json("Error");
        }

        [IsLogin(CheckPermission = false)]
        public JsonResult GetModuleTypes(string templateType)
        {
            if (templateType == "dashboard")
            {
                return Json(new TMP_ModuleTypeBL().ModuleTypesList("dashboard"), JsonRequestBehavior.AllowGet);
            }
            else if (templateType == "report")
            {
                return Json(new TMP_ModuleTypeBL().ModuleTypesList("report"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        /// <summary>
        /// For Updating and Deleting Template
        /// </summary>
        /// <param name="Filter">Filter</param>
        /// <param name="TemplateId">Template Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult ManageTemplate(string Filter, string TemplateId)
        {
            if (Filter == "GetById" && !string.IsNullOrEmpty(TemplateId))
            {
                TMP_Templates Templates = TemplatesBL.ToList("GetById", TemplateId).FirstOrDefault();
                return Json(Templates);
            }
            if (Filter == "Delete" && !string.IsNullOrEmpty(TemplateId))
            {
                TemplatesBL.Manage("Disable", new TMP_Templates
                {
                    TemplateId = int.Parse(TemplateId) 
                });
                return Json("OK");
            }
            if (Filter == "Active" && !string.IsNullOrEmpty(TemplateId))
            {
                TemplatesBL.Manage("Active", new TMP_Templates
                {
                    TemplateId = int.Parse(TemplateId)
                });
                return Json("OK");
            }
            return Json("Error");
        }

        /// <summary>
        /// Take Node JSON and Save
        /// </summary>
        /// <param name="GridInfoJSON">Grid Information in JSON format</param>
        /// <param name="TemplateId">Template Id</param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult SaveNodeGrid(string GridInfoJSON, string TemplateId)
        {
            dynamic GridInfoObj = JsonConvert.DeserializeObject(GridInfoJSON);
            var gridItemList = NodeBL.ToList("ByTemplateId", TemplateId);
            string tentativeId = null;

            List<int> totalItemLists = gridItemList.Select(x => x.NodeId).ToList();

            foreach (var item in GridInfoObj)
            {
                tentativeId = item.id.ToString();
                if (tentativeId.Contains("-"))
                {
                    var pageTypeId = tentativeId.Split('-').First();
                    var SplitData = tentativeId.Split('-');
                    string NodePageUrl = TemplateUtiliy.NodeUrlByNodeType(SplitData?[1].Trim());

                    if (!string.IsNullOrEmpty(pageTypeId))
                    {
                        NodeBL.Manage("Insert", new TMP_Node
                        {
                            PageTyppeId = int.Parse(pageTypeId),
                            TemplateId = int.Parse(TemplateId),
                            Height = item.height,
                            Width = item.width,
                            x_axis = item.x,
                            y_axis = item.y,
                            NodeUrl = NodePageUrl,
                            IsActive = true
                        });
                    }
                }
                else
                {
                    NodeBL.Manage("Update", new TMP_Node
                    {
                        NodeId = item.id,
                        TemplateId = int.Parse(TemplateId),
                        Height = item.height,
                        Width = item.width,
                        x_axis = item.x,
                        y_axis = item.y,
                        IsActive = true
                    });

                    totalItemLists.Remove(int.Parse(item.id.ToString()));
                }
            }

            if (totalItemLists != null && totalItemLists.Count > 0)
            {
                foreach (var item in totalItemLists)
                {
                    NodeBL.Manage("SoftDelete", new TMP_Node
                    {
                        NodeId = int.Parse(item.ToString())
                    });
                }
            }
            return Json("Ok");
        }


        /// <summary>
        /// Add Node Relavent Data
        /// Node Type and Node Query
        /// </summary>
        /// <param name="NodeId">Node Id</param>
        /// <param name="NodeType">Node Type</param>
        /// <param name="NodeQuery">Node Query</param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult AddNodeRelaventData(string NodeId, string NodeType, string NodeQuery)
        {
            if (!string.IsNullOrEmpty(NodeId) && !string.IsNullOrEmpty(NodeType))
            {
                string partialView = "";
                if (NodeType == "PieChart")
                {
                    partialView = "~/Views/Home/_pieChart.cshtml";
                }
                else if (NodeType == "DataTable")
                {
                    partialView = "~/Views/Home/_latestOrders.cshtml";
                }
                else
                {
                    partialView = "";
                }
                NodeBL.Manage("UpdateRelaventData", new TMP_Node
                {
                    NodeId = int.Parse(NodeId),
                    NodeUrl = partialView,
                    NodeSQL = NodeQuery
                });
                return Json("Ok");
            }
            return Json("Error");
        }

        /// <summary>
        /// REMOVE Node from database by updating IsActive = false
        /// </summary>
        /// <param name="nodeId">Node Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult RemoveNodeData(string nodeId)
        {
            NodeBL.Manage("SoftDelete", new TMP_Node
            {
                NodeId = int.Parse(!string.IsNullOrEmpty(nodeId) ? nodeId : "0")
            });
            return Json("Ok");
        }



        [IsLogin(CheckPermission = false)]

        public PartialViewResult GetNodeWidgetPartial(string NodeId, string CurrentPageURL, string WithPartialView = null, int page = 1, int StartIndex = 1, int EndIndex = 10)
        {
            string Icon = string.Empty;
            AD_Defination pageType = null;
            if (!string.IsNullOrEmpty(NodeId))
            {
                var nodeData = NodeBL.ToList("ByNodeId", NodeId.ToString()).Where(x => x.NodeId == int.Parse(NodeId)).FirstOrDefault();
                pageType = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.DefinationId == nodeData.PageTyppeId).FirstOrDefault();

                TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
                string TemplateType = TemplatesBL.ToList("GetTemplateTypeByNodeId", NodeId.ToString()).FirstOrDefault()?.TemplateType;

                TemplateRDViewModel TemplateRDVM = new TemplateRDViewModel();
                TemplateRDVM.Page = page;
                TemplateRDVM.PageStartIndex = StartIndex;
                TemplateRDVM.PageEndIndex = EndIndex;
                TemplateRDVM.TemplateType = TemplateType;

                if (!string.IsNullOrEmpty(CurrentPageURL))
                {
                    var queryString = HttpUtility.ParseQueryString(new Uri(CurrentPageURL).Query);

                    // Get Data From QueryString
                    TemplateRDVM.ProjectId = int.Parse(!string.IsNullOrEmpty(queryString.Get("ProjectId")) ? queryString.Get("ProjectId") : "0");
                    TemplateRDVM.ScopeId = int.Parse(!string.IsNullOrEmpty(queryString.Get("ScopeId")) ? queryString.Get("ScopeId") : "0");
                    TemplateRDVM.SiteId = int.Parse(!string.IsNullOrEmpty(queryString.Get("SiteId")) ? queryString.Get("SiteId") : "0");
                    TemplateRDVM.NetworkModeId = int.Parse(!string.IsNullOrEmpty(queryString.Get("NetworkModeId")) ? queryString.Get("NetworkModeId") : "0");
                    TemplateRDVM.BandId = int.Parse(!string.IsNullOrEmpty(queryString.Get("BandId")) ? queryString.Get("BandId") : "0");
                    TemplateRDVM.CarrierId = int.Parse(!string.IsNullOrEmpty(queryString.Get("CarrierId")) ? queryString.Get("CarrierId") : "0");
                    TemplateRDVM.NodeId = int.Parse(NodeId);
                    TemplateRDVM.PageType = pageType != null ? pageType.DefinationName : null;
                }

                if (!string.IsNullOrEmpty(WithPartialView))
                {
                    TemplateRDVM.WithPartialView = WithPartialView.ToLower() == "true" ? true : false;
                }

                if (nodeData != null && !string.IsNullOrEmpty(nodeData.NodeUrl))
                {
                    string partialView = "~/Areas/Project/Views/PartialTemplate/" + nodeData.NodeUrl;
                    return PartialView(partialView, TemplateRDVM);
                }
            }

            switch (pageType?.DefinationName)
            {
                case "CHART":
                    Icon = "<div class='body-icon'><i class='fa fa-bar-chart default-cell-icon'></i></div>";
                    break;
                case "MAP":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-globe default-cell-icon'></i></div>";
                    break;
                case "TABLE":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-table default-cell-icon'></i></div>";
                    break;
                case "PAGE":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-clipboard default-cell-icon'></i></div>";
                    break;
                case "IMAGES":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-image default-cell-icon'></i></div>";
                    break;
                case "TABLE_WITH_MAP":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-globe default-cell-icon'></i></div>";
                    break;
            }
            return PartialView();
            //return PartialView(Icon, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get Node Widget
        /// </summary>
        /// <param name="NodeId">Node Id</param>
        /// <param name="CurrentPageURL">Current Page URL</param>
        /// <param name="WithPartialView">With Partial View</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]

        public ActionResult GetNodeWidget(string NodeId, string CurrentPageURL, string WithPartialView = null, int page = 1, int StartIndex = 1, int EndIndex = 10)
        {
            string Icon = string.Empty;
            AD_Defination pageType = null;
            if (!string.IsNullOrEmpty(NodeId))
            {
                var nodeData = NodeBL.ToList("ByNodeId", NodeId.ToString()).Where(x => x.NodeId == int.Parse(NodeId)).FirstOrDefault();
                pageType = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.DefinationId == nodeData.PageTyppeId).FirstOrDefault();

                TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
                string TemplateType = TemplatesBL.ToList("GetTemplateTypeByNodeId", NodeId.ToString()).FirstOrDefault()?.TemplateType;

                TemplateRDViewModel TemplateRDVM = new TemplateRDViewModel();
                TemplateRDVM.Page = page;
                TemplateRDVM.PageStartIndex = StartIndex;
                TemplateRDVM.PageEndIndex = EndIndex;
                TemplateRDVM.TemplateType = TemplateType;

                if (!string.IsNullOrEmpty(CurrentPageURL))
                {
                    var queryString = HttpUtility.ParseQueryString(new Uri(CurrentPageURL).Query);

                    // Get Data From QueryString
                    TemplateRDVM.ProjectId = int.Parse(!string.IsNullOrEmpty(queryString.Get("ProjectId")) ? queryString.Get("ProjectId") : "0");
                    TemplateRDVM.ScopeId = int.Parse(!string.IsNullOrEmpty(queryString.Get("ScopeId")) ? queryString.Get("ScopeId") : "0");
                    TemplateRDVM.SiteId = int.Parse(!string.IsNullOrEmpty(queryString.Get("SiteId")) ? queryString.Get("SiteId") : "0");
                    TemplateRDVM.NetworkModeId = int.Parse(!string.IsNullOrEmpty(queryString.Get("NetworkModeId")) ? queryString.Get("NetworkModeId") : "0");
                    TemplateRDVM.BandId = int.Parse(!string.IsNullOrEmpty(queryString.Get("BandId")) ? queryString.Get("BandId") : "0");
                    TemplateRDVM.CarrierId = int.Parse(!string.IsNullOrEmpty(queryString.Get("CarrierId")) ? queryString.Get("CarrierId") : "0");
                    TemplateRDVM.NodeId = int.Parse(NodeId);
                    TemplateRDVM.PageType = pageType != null ? pageType.DefinationName : null;
                }

                if (!string.IsNullOrEmpty(WithPartialView))
                {
                    TemplateRDVM.WithPartialView = WithPartialView.ToLower() == "true" ? true : false;
                }

                if (nodeData != null && !string.IsNullOrEmpty(nodeData.NodeUrl))
                {
                    string partialView = "~/Areas/Project/Views/PartialTemplate/" + nodeData.NodeUrl;
                    return PartialView(partialView, TemplateRDVM);
                }
            }

            switch (pageType?.DefinationName)
            {
                case "CHART":  
                    Icon = "<div class='body-icon'><i class='fa fa-bar-chart default-cell-icon'></i></div>";
                    break;
                case "MAP":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-globe default-cell-icon'></i></div>";
                    break;
                case "TABLE":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-table default-cell-icon'></i></div>";
                    break;
                case "PAGE":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-clipboard default-cell-icon'></i></div>";
                    break;
                case "IMAGES":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-image default-cell-icon'></i></div>";
                    break;
                case "TABLE_WITH_MAP":  // 
                    Icon = "<div class='body-icon'><i class='fa fa-bar-globe default-cell-icon'></i></div>";
                    break;
            }

            return Json(Icon, JsonRequestBehavior.AllowGet);
        }

        [IsLogin(CheckPermission = false)]
        [HttpPost]
        public ActionResult GetDataTableProperties(string NodeId)
        {
            TMP_NodeSettingsBL nodeSetting = new TMP_NodeSettingsBL();
            var Data = nodeSetting.ToList("GET_BY_NODEID", NodeId);
            string IsPagingEnable = "false";
            string PageSize = "10";
            string dataTableColumns = null;
            if (Data.Count > 0)
            {
                dataTableColumns = Data?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.Settings;
                PageSize = Data?.FirstOrDefault(x => x.MappedId == "PAGE_SIZE")?.Value;
                IsPagingEnable = Data?.FirstOrDefault(x => x.MappedId == "IS_PAGING_ENABLE")?.Value?.ToLower();
                if (IsPagingEnable == "enable")
                { IsPagingEnable = "true"; }
                else
                { IsPagingEnable = "false"; }
            }
            return Json(new { dataTableColumns = dataTableColumns, IsPagingEnable = IsPagingEnable, PageSize = PageSize  });

        }

        /// <summary>
        /// Get Node Widget Data
        /// </summary>
        /// <param916 name="NodeId">Node Id</param>
        /// <param name="BandId">Band Id</param>
        /// <param name="CareerId">Career Id</param>
        /// <param name="NetworkModeId">Network Mode Id</param>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="ScopeId">Scope Id</param>
        /// <param name="SiteId">Site Id</param>
        /// <param name="PlotId">Plot Id</param>
        /// <param name="UserId">User Id</param>
        /// <param name="ControlType">Control Type</param>
        /// <param name="ControlDetailType">Control Detail Type</param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult GetNodeWidgetData(int NodeId, int BandId, int CareerId, int NetworkModeId, int ProjectId, int SiteId, int PlotId, int UserId, string ControlType, string ControlDetailType, int page = 1, int StartIndex = 1, int EndIndex = 10, int ScopeId = -1, string RPTType = null)
        {
            try
            {
                var draw = Request.Form.GetValues("draw")?.FirstOrDefault();
                var start = Request.Form.GetValues("start")?.FirstOrDefault();
                var length = Request.Form.GetValues("length")?.FirstOrDefault();
                //Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();

                TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
                string TemplateType = TemplatesBL.ToList("GetTemplateTypeByNodeId", NodeId.ToString()).FirstOrDefault()?.TemplateType;

                TMP_NodeSettingsBL nodeSetting = new TMP_NodeSettingsBL();
                var Data =  nodeSetting.ToList("GET_BY_NODEID", NodeId.ToString());
                var Seleced = Data?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.Settings;

                string WhereClause = string.Empty;
                TMP_NodeSettings QueryWhereClause = new TMP_NodeSettings();
                List<TMP_NodeSettings> NodeSetting = new List<TMP_NodeSettings>();
                TMP_GetSiteReportVM ReportSummery = new TMP_GetSiteReportVM();
                string FilterWithScopeId = "";
                string FilterWithNetworkModeId = "";
                string FilterWithBandId = "";
                string FilterWithCarrierId = "";
                string FilterWithSiteId = "";
                string FilterWithProjectId = "";


                string legendData = "";
                string LSiteIdKML = "''=''";
                string LBandIdKML = "''=''";
                string LNetworkModeIdKML = "''=''";
                string LCarrierKML = "''=''"; ;
                if (ScopeId > 0)
                {
                    FilterWithScopeId = $" ScopeId={ScopeId} AND ";
                }
                if (NetworkModeId > 0)
                {
                    LNetworkModeIdKML = $"NetworkModeId= CAST({NetworkModeId} AS NVARCHAR(15)) ";
                    FilterWithNetworkModeId = $" NetworkModeId={NetworkModeId} AND ";
                }
                if (BandId > 0)
                {
                    LBandIdKML = $"BandId= CAST({BandId} AS NVARCHAR(15)) ";
                    FilterWithBandId = $" BandId={BandId} AND ";
                }
                if (CareerId > 0)
                {
                    LCarrierKML = $"CarrierId= CAST({CareerId} AS NVARCHAR(15)) ";
                    FilterWithCarrierId = $" CarrierId={CareerId} AND ";
                }
                if (SiteId > 0)
                {
                    LSiteIdKML = $"SiteId= CAST({SiteId} AS NVARCHAR(15)) ";
                    FilterWithSiteId = $" SiteId={SiteId} AND ";
                }
                if (ProjectId > 0)
                {
                    FilterWithProjectId = $" ProjectId={ProjectId} AND ";
                }
                if (NodeId > 0)
                {
                    NodeSetting = NodeSettingsBL.ToList("GET_BY_NODEID", NodeId.ToString());
                    ReportSummery.BaseURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                    QueryWhereClause = NodeSetting.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "query").FirstOrDefault();
                    WhereClause = QueryWhereClause != null ? QueryWhereClause.Value : null;

                    if (ControlType == "PAGE" && NodeSetting.Count > 0)
                    {
                        GetSiteReportBL.LSiteIdKML = LSiteIdKML;
                        GetSiteReportBL.LBandIdKML = LBandIdKML;
                        GetSiteReportBL.LNetworkModeIdKML = LNetworkModeIdKML;
                        GetSiteReportBL.LCarrierKML = LCarrierKML;
                        ReportSummery = GetSiteReportBL.ToObject("GET_PAGE_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, null, null, WhereClause, ProjectId);
                    }

                    else if (ControlType == "TABLE" && NodeSetting.Count > 0)
                    {
                        var Query = string.Empty;
                        if (TemplateType == "dashboard")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2}", ProjectId, ScopeId, QueryWhereClause?.Value));
                            else
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) ", ProjectId, ScopeId));
                        }
                        else if (TemplateType == "report")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value));
                            else
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, Utilities.ReplaceLastOccurrence(string.Format("WHERE {0} {1} {2} {3} {4} {5} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId), "AND", " "));
                        }

                        GetSiteReportBL.ExeCustomQuery = Query;

                        ReportSummery = GetSiteReportBL.ToObject("GET_TABLE_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, null, null, WhereClause, ProjectId, page, StartIndex, EndIndex, draw, start, length, sortColumn, sortColumnDir, Query);

                        var data = new { draw = draw, recordsFiltered = ReportSummery.TotalNoOfRecords, recordsTotal = ReportSummery.TotalNoOfRecords, data = ReportSummery };
                        var result = new JsonResult()
                        {
                            Data = data,
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        return result;
                    }

                    else if (ControlType == "MAP" && NodeSetting.Count > 0)
                    {
                        GetSiteReportBL.LSiteIdKML = LSiteIdKML;
                        GetSiteReportBL.LBandIdKML = LBandIdKML;
                        GetSiteReportBL.LNetworkModeIdKML = LNetworkModeIdKML;
                        GetSiteReportBL.LCarrierKML = LCarrierKML;
                        var Query = string.Empty;
                        Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(QueryWhereClause?.Settings);
                        string TableName = DeserializeQuerySetting?.QuerySetting?.Table;
                        DateTime TodayDate = DateTime.UtcNow;
                        DateTime MonthStart = new DateTime(TodayDate.Year,TodayDate.Month, 1);
                        bool IsDateFilterReq = false;
                        bool IsDS_SiteTestSummary = false;
                        if (TableName.ToLower() == "DS_SiteTestSummary".ToLower() || TableName.ToLower() == "DS_SiteTestLogs".ToLower())
                        {
                            IsDateFilterReq = true;
                        }
                        if (TableName.ToLower() == "DS_SiteTestSummary".ToLower())
                            IsDS_SiteTestSummary = true;
                        if (TemplateType == "dashboard")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2} AND [Site Schedule Date] >= '{3}' AND  [Site Schedule Date] <= '{4}' ", ProjectId, ScopeId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2} AND [TimeStamp] >= '{3}' AND  [TimeStamp] <= '{4}' ", ProjectId, ScopeId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                }
                                else
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2}", ProjectId, ScopeId, QueryWhereClause?.Value));
                            }
                            else
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND  [Site Schedule Date] >= '{2}' AND  [Site Schedule Date] <= '{3}' ", ProjectId, ScopeId, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                      
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND [TimeStamp] >= '{2}' AND  [TimeStamp] <= '{3}' ", ProjectId, ScopeId, MonthStart, TodayDate));
                                    }
                                    
                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) ", ProjectId, ScopeId));
                                }
                                
                            }
                        }
                        else if (TemplateType == "report")
                        {

                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                            {
                                GetSiteReportBL.FilterClause = QueryWhereClause?.Value;
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} AND [Site Schedule Date] >= '{7}' AND  [Site Schedule Date] <= '{8}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} AND [TimeStamp] >= '{7}' AND  [TimeStamp] <= '{8}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                   
                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value));
                                }
                            }
                            else
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} [Site Schedule Date] >= '{6}' AND  [Site Schedule Date] <= '{7}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId,FilterWithProjectId, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} [TimeStamp] >= '{6}' AND  [TimeStamp] <= '{7}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, MonthStart, TodayDate));
                                    }
                                 
                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, Utilities.ReplaceLastOccurrence(string.Format(" WHERE {0} {1} {2} {3} {4} {5} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId), "AND", " "));
                                }
                            }   
                        }
                        Query = Query.Replace("FROM", " ,Latitude as latiCordinateValue, Longitude as longCordinateValue From ");
                        if(Query.ToLower().Contains("group by"))
                        {
                            Query = Query + ", Latitude, Longitude ";
                        }
                        GetSiteReportBL.ExeCustomQuery = Query;
                        
                        GetSiteReportBL.TableName = DeserializeQuerySetting.QuerySetting.Table;
                        string mapType = NodeSetting.Where(x => x.KeyName.ToLower().Contains("map plot")).FirstOrDefault().Value;

                        ReportSummery = GetSiteReportBL.ToObject("GET_MAP_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, mapType, null, WhereClause, ProjectId);

                        CreateKML("/Content/AirViewLogs/SampleKML/", ReportSummery.MapType, ReportSummery.MapPlotKML);
                        ReportSummery.KMLFilePath = ReportSummery.BaseURL + "/Content/AirViewLogs/SampleKML/" + ReportSummery.MapType + ".kml";

                        ReportSummery.AzmuthDataJSON = !string.IsNullOrEmpty(ReportSummery.AzmuthData) ? GetAzimuth(ReportSummery.AzmuthData) : null;
                    }
                    else if (ControlType == "IMAGES" && NodeSetting.Count > 0)
                    {
                        ReportSummery = GetSiteReportBL.ToObject("GET_OOKLA_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, null, null, WhereClause, ProjectId);
                    }
                    else if (ControlType == "GET_DASHBOARD_MAP")
                    {
                        ReportSummery = GetSiteReportBL.ToObject("GET_DASHBOARD_MAP", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, null, null, WhereClause, ProjectId);
                        return new JsonResult()
                        {
                            Data = ReportSummery,
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }

                    else if (ControlType == "TABLE_WITH_MAP" && NodeSetting.Count > 0)
                    {
                        var Query = string.Empty;
                        Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(QueryWhereClause?.Settings);
                        string TableName = DeserializeQuerySetting?.QuerySetting?.Table;
                        DateTime TodayDate = DateTime.UtcNow;
                        DateTime MonthStart = new DateTime(TodayDate.Year, TodayDate.Month, 1);
                        bool IsDateFilterReq = false;
                        bool IsDS_SiteTestSummary = false;
                        if (TableName.ToLower() == "DS_SiteTestSummary".ToLower() || TableName.ToLower() == "DS_SiteTestLogs".ToLower())
                        {
                            IsDateFilterReq = true;
                        }
                        if (TableName.ToLower() == "DS_SiteTestSummary".ToLower())
                            IsDS_SiteTestSummary = true;
                        if (TemplateType == "dashboard")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2} AND [Site Schedule Date] >= '{3}' AND  [Site Schedule Date] <= '{4}' ", ProjectId, ScopeId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2} AND [Site Schedule Date] >= '{3}' AND  [Site Schedule Date] <= '{4}' ", ProjectId, ScopeId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                }
                                else
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2}", ProjectId, ScopeId, QueryWhereClause?.Value));
                            }
                            else
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND  [Site Schedule Date] >= '{2}' AND  [Site Schedule Date] <= '{3}' ", ProjectId, ScopeId, MonthStart, TodayDate));
                                    }
                                    else
                                    {

                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND [TimeStamp] >= '{2}' AND  [TimeStamp] <= '{3}' ", ProjectId, ScopeId, MonthStart, TodayDate));
                                    }

                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) ", ProjectId, ScopeId));
                                }

                            }
                        }
                        else if (TemplateType == "report")
                        {

                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                            {
                                GetSiteReportBL.FilterClause = QueryWhereClause?.Value;
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} AND [Site Schedule Date] >= '{7}' AND  [Site Schedule Date] <= '{8}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} AND [TimeStamp] >= '{7}' AND  [TimeStamp] <= '{8}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value, MonthStart, TodayDate));
                                    }

                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, QueryWhereClause?.Value));
                                }
                            }
                            else
                            {
                                if (IsDateFilterReq)
                                {
                                    if (IsDS_SiteTestSummary)
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} [Site Schedule Date] >= '{6}' AND  [Site Schedule Date] <= '{7}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, MonthStart, TodayDate));
                                    }
                                    else
                                    {
                                        Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} [TimeStamp] >= '{6}' AND  [TimeStamp] <= '{7}' ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId, MonthStart, TodayDate));
                                    }

                                }
                                else
                                {
                                    Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, Utilities.ReplaceLastOccurrence(string.Format(" WHERE {0} {1} {2} {3} {4} {5} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId), "AND", " "));
                                }
                            }
                        }
                        Query = Query.Replace("FROM", " ,Latitude as latiCordinateValue, Longitude as longCordinateValue From ");
                        if (Query.ToLower().Contains("group by"))
                        {
                            Query = Query + ", Latitude, Longitude ";
                        }
                        GetSiteReportBL.ExeCustomQuery = Query;
                        GetSiteReportBL.TableName = DeserializeQuerySetting.QuerySetting.Table;
                        ReportSummery = GetSiteReportBL.ToObject("GET_TABLE_WITH_MAP_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, null, null, WhereClause, ProjectId, page, StartIndex, EndIndex, draw, start, length, sortColumn, sortColumnDir, Query);
                        ReportSummery.AzmuthDataJSON = !string.IsNullOrEmpty(ReportSummery.AzmuthData) ? GetAzimuth(ReportSummery.AzmuthData) : null;

                        var data = new { draw = draw, recordsFiltered = ReportSummery.TotalNoOfRecords, recordsTotal = ReportSummery.TotalNoOfRecords, data = ReportSummery };
                        var result = new JsonResult()
                        {
                            Data = data,
                            ContentType = "application/json; charset=utf-8",
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        return result;
                    }

                    else if (ControlType == "CHART" && NodeSetting.Count > 0)
                    {
                        Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(QueryWhereClause.Settings);
                        var chartX_AxisCol = Data.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "x-axis").FirstOrDefault();
                        var chartY_AxisCol = Data.Where(x => x.NodeId == NodeId && x.KeyName.ToLower() == "y-axis").FirstOrDefault();
                        GetSiteReportBL.xAxis = chartX_AxisCol?.Value;
                        GetSiteReportBL.yAxis = chartY_AxisCol?.Value;
                        var chartQuery = "";
                        var Query = string.Empty;
                        if (TemplateType == "dashboard")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0}) AND {2}", ProjectId, ScopeId, QueryWhereClause?.Value));
                            else
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE ProjectId IN ({0})  ", ProjectId, ScopeId));
                        }
                        else if (TemplateType == "report")
                        {
                            if (QueryWhereClause?.Value != string.Empty && QueryWhereClause?.Value != "()")
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, string.Format(" WHERE {0} {1} {2} {3} {4} {5} {6} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId,FilterWithProjectId, QueryWhereClause?.Value));
                            else
                                Query = TemplateUtiliy.QueryBuilder(QueryWhereClause?.Settings, Utilities.ReplaceLastOccurrence(string.Format("WHERE {0} {1} {2} {3} {4} {5} ", FilterWithScopeId, FilterWithNetworkModeId, FilterWithSiteId, FilterWithBandId, FilterWithCarrierId, FilterWithProjectId), "AND", " "));
                        }

                        GetSiteReportBL.ExeCustomQuery = Query;

                        string controlDetailType = NodeSetting.Where(x => x.NodeId == NodeId && x.MappedId.Contains("CHART_DATA")).FirstOrDefault()?.Value;
                        string ChartType = NodeSetting.Where(x => x.NodeId == NodeId && x.MappedId.Contains("CHART_TYPE")).FirstOrDefault()?.Value;
                        
                        ReportSummery = GetSiteReportBL.ToObject("GET_CHART_DATA", NodeId, SiteId, BandId, CareerId, NetworkModeId, ScopeId, PlotId, UserId, controlDetailType, ChartType, WhereClause, ProjectId);
                    }

                    return new JsonResult()
                    {
                        Data = ReportSummery,
                        ContentType = "application/json; charset=utf-8",
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            catch(Exception er)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// Saved Form Information
        /// </summary>
        /// <param name="FormId">Form Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult SavedFormInfo(string FormId)

















        {
            FormId = !string.IsNullOrEmpty(FormId) ? FormId : "1";
            List<TMP_NodesProperties> FormItems = NodesPropertiesBL.ToList("ByNodeTypeId", FormId);

            string FormHTML, ControlTypeName, DataTypeName;
            FormHTML = DataTypeName = ControlTypeName = "";
            int i = 1;

            var controlTypeList = DefinationBL.ToList("byDefinationType", "ControlType");
            var dataTypeList = DefinationBL.ToList("byDefinationType", "DataType");

            var formTypeList = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.PDefinationId == 0);

            if (FormId == "-1")
            {
                return Json(new
                {
                    FormTypeList = formTypeList.ToList()
                }, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in FormItems)
            {

                var InputValueType = dataTypeList.Where(x => x.DefinationId == Convert.ToInt64(item.DataType)).FirstOrDefault().DefinationName;
                DataTypeName = "<select readonly style='min-width:100px;' class='form-control dropdown DataTypeChange' data-row='" + i + "' id='dataType" + i + "' name='dataType'>";
                foreach (var dataTypeItem in dataTypeList)
                {
                    DataTypeName += dataTypeItem.DefinationId.ToString() == item.DataType ?
                        "<option value='" + dataTypeItem.DefinationId + "' selected='selected'> " + dataTypeItem.DefinationName + " </option> " :
                        "";//"<option value='" + dataTypeItem.DefinationId + "'> " + dataTypeItem.DefinationName + " </option> ";
                }
                DataTypeName += "</select>";

                ControlTypeName = "<select readonly style='min-width:100px;' class='form-control dropdown' data-row='" + i + "' id='controlType" + i + "' name='controlType'>";
                foreach (var controlTypeItem in controlTypeList)
                {
                    ControlTypeName += controlTypeItem.DefinationId.ToString() == item.ControlType ?
                        "<option value='" + controlTypeItem.DefinationId + "' selected='selected'> " + controlTypeItem.DefinationName + " </option> " :
                        "";//"<option value='" + controlTypeItem.DefinationId + "'> " + controlTypeItem.DefinationName + " </option> ";
                }
                ControlTypeName += "</select>";






               if(InputValueType.ToLower() == "number" || InputValueType.ToLower() =="decimal")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><input class='form-control' type='number' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() =="image")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><input class='form-control' readonly type='text' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "date")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><input class='form-control'  type='date' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "colorpicker")
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><input class='form-control'  type='color' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }
                else if (InputValueType.ToLower() == "radiobutton")
                {
                    if(item.DefaultValue.ToLower() == "selected")
                    {
                        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select  class='form-control'  id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'><option selected value='selected' >Selected</option><option value='unselected' >Unselected</option></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";

                    }
                    else {
                        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input required class='form-control title' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'><option value='selected' >Selected</option><option selected value='unselected' >Unselected</option></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";

                    }
                }

                //else if (InputValueType.ToLower() == "staticdropdown")
                //{
                //    if(item.DefaultValue !="")
                //        {
                //      var SplitedValues = item.DefaultValue.Split(',');
                //        var Options = "";
                //        foreach(var value in SplitedValues)
                //        {
                //           Options += "<option  value='"+ value + "'+ >"+ value+ "</option>";
                //        }

                //        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'>"+ Options + "</select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                //    }
                //    else
                //    {

                //        FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control' type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><select class='form-control'   id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' '" + item.DefaultValue + "'></select></td> <td><input class='form-control' readonly type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                //    }
                //}
                else
                {
                    FormHTML += "<tr> <td><input type='checkbox' class='case'/></td> <td><input class='form-control title' required type='text' id='title" + i + "' name='title' value='" + item.Title + "'/></td> <td> " + ControlTypeName + " </td>   <td> " + DataTypeName + " </td> <td><input class='form-control'  type='text' id='defaultValue" + i + "' name='defaultValue' onclick='dynamicQueryPop(" + i + ")' value='" + item.DefaultValue + "'/></td> <td><input class='form-control' type='number' id='maxLength" + i + "' name='maxLength' value='" + item.MaxLength + "'/></td>";
                }



                if (item.Comments.ToLower() == "true")
                {
                    FormHTML += "<td> <input type='checkbox' class='comment' data-row='" + i + "' checked/>";
                    FormHTML += "<input type='hidden' value='" + item.Comments + "' id='comment" + i + "' name='comment' /> </td>";
                }
                else
                {
                    FormHTML += "<td> <input type='checkBox' class='comment' data-row='" + i + "' value='" + item.IsAttachment + "' />";
                    FormHTML += "<input type='hidden' value='" + item.Comments + "' id='comment" + i + "' name='comment' /> </td>";
                }

                if (item.Required.ToLower() == "true")
                {
                    FormHTML += "<td> <input type='checkbox' class='require' data-row='" + i + "' checked/>";
                    FormHTML += "<input type='hidden' value='" + item.Required + "' id='require" + i + "' name='required' /> </td> ";
                }
                else
                {
                    FormHTML += "<td> <input type='checkbox' class='require' data-row='" + i + "' value='" + item.Required + "' />";
                    FormHTML += "<input type='hidden' value='" + item.Required + "' id='require" + i + "' name='required' /> </td> ";
                }

                if (item.IsAttachment.ToLower() == "true")
                {
                    FormHTML += "<td> <input type='checkbox' class='isAttachment' data-row='" + i + "' checked/>";
                    FormHTML += "<input type='hidden' value='" + item.IsAttachment + "' id='isAttachment" + i + "' name='isAttachments' /> </td>";
                }
                else
                {
                    FormHTML += "<td> <input type='checkBox' class='isAttachment' data-row='" + i + "' value='" + item.IsAttachment + "' />";
                    FormHTML += "<input type='hidden' value='" + item.IsAttachment + "' id='isAttachment" + i + "' name='isAttachments' /> </td>";
                }

              

                FormHTML += "<td><input type='hidden' name='formid' id='formid" + i + "'  value='" + item.FormId + "'    /><input type='hidden' class='deleted' name='isdeleted' value='0' id='isdeleted" + i + "'  /><input class='form-control sortOrderAutoValue' type='number' id='sortOrderInput" + i + "' name='sortOrder' value='" + item.SortOrder + "'/></td> </tr>";
                i++;
            }

            return Json(new
            {
                Form = FormHTML,
                FormTypeList = formTypeList.ToList()
            });
        }
        [IsLogin(CheckPermission = false),HttpGet]
        public JsonResult GetTaskEntries(Int64 SiteId=0,Int64 TaskId=0)
        {
            try
            {
                List<PM_TaskEntry> SavedformDefinations = new List<PM_TaskEntry>();
                List<PM_TaskEntry> MyList = new List<PM_TaskEntry>();
                PM_TaskEntryBL te = new PM_TaskEntryBL();
                if (SiteId > 0 && TaskId  >0)
                {
                     SavedformDefinations = te.ToList("GetTaskEntries", Convert.ToInt64(TaskId), SiteId);
                }
                if (SavedformDefinations.Count > 0)
                {
                    Int64 itemMaxHeight = SavedformDefinations.Max(y => y.Revision);
                    for (int i = 1; i <= itemMaxHeight; i++)
                    {
                        PM_TaskEntry obj = new PM_TaskEntry();
                        obj.CreatedOn = SavedformDefinations.Where(x => x.Revision == i).FirstOrDefault().CreatedOn;
                        obj.Revision = i;
                        obj.CurrentRevision = SavedformDefinations.Where(x => x.Revision == i).ToList();
                        MyList.Add(obj);
                    }
                }


                return Json(new
                {
                    Data= MyList,
                    Status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                   Status = false
                }, JsonRequestBehavior.AllowGet);
            }
            
        }
       public JsonResult FormBuilderRederHtml(string FormTypeId, string NodeId, string TemplateId, Int64 SiteId = 0)
        {
            List<TMP_NodeSettings> NodeSetting = new List<TMP_NodeSettings>();
            NodeSetting = NodeSettingsBL.ToList("GET_BY_NODEID", NodeId.ToString());
            TMP_Node nodeInfo = new TMP_Node();
            List<TMP_NodesProperties> formDefinations = null;

            TMP_TemplatesBL TemplatesBL = new TMP_TemplatesBL();
            string TemplateType = TemplatesBL.ToList("GetTemplateTypeByNodeId", NodeId.ToString()).FirstOrDefault()?.TemplateType;


            bool isFormNode = false;

            if (FormTypeId.ToLower() == "milestone" || FormTypeId.ToLower() == "task" || FormTypeId.ToLower() == "node")
            {
                if (FormTypeId.ToLower() == "node")
                {
                    isFormNode = true;

                    nodeInfo = NodeBL.ToList("ByTemplateId", TemplateId).Where(x => x.NodeId == int.Parse(NodeId)).FirstOrDefault();
                    FormTypeId = nodeInfo.PageTyppeId.ToString();

                    var formTypeName = DefinationBL.ToList("byDefinationType", "FORMTYPE");
                    var formTypeDetail = formTypeName.Where(x => x.KeyCode.Contains("node".ToUpper())).FirstOrDefault();

                    if (formTypeDetail != null && NodeId != null)
                    {
                        formDefinations = NodesPropertiesBL.ToList("ByNodeTypeId", FormTypeId);
                    }
                }
                else
                {
                    var formTypeName = DefinationBL.ToList("byDefinationType", "FORMTYPE");
                    var formTypeDetail = formTypeName.Where(x => x.KeyCode.Contains(FormTypeId.ToUpper())).FirstOrDefault();

                    if (formTypeDetail != null && NodeId != null)
                    {
                        formDefinations = NodesPropertiesBL.ToList("ByNodeTypeId", NodeId);
                    }
                }
            }

            // TMP_NodesProperties
            string FormHTMLTabular = string.Empty;
            List<AD_Defination> dataTypeList = DefinationBL.ToList("byDefinationType", "DataType");
            List<AD_Defination> nodeTemplateList = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.PDefinationId == nodeInfo.PageTyppeId).ToList();
            var controlValues = NodeSettingsBL.ToList("GET_BY_NODEID", NodeId);
            // Get Mapped Values (DOM Ids)
            var mappingDomIds = controlValues.Select(x => x.MappedId);
            //   PM_TaskEntryBL te = new PM_TaskEntryBL();
            //if(SiteId > 0)
            //   {
            //       List<PM_TaskEntry> SavedformDefinations = te.ToList("GetTaskEntries", Convert.ToInt64(NodeId), SiteId); ;
            //   }
            if (formDefinations != null)
            {
                // Table header 
                if (isFormNode)
                {
                    FormHTMLTabular += "<table class='table'> <thead> <tr> <th>Label</th> <th>Mapping Id</th> <th>Value</th> <th>Settings</th> </tr> </thead> <tbody>";
                }
                else
                {
                    FormHTMLTabular += "<table class='table'> <tbody>";
                }

                int i = 0;
                foreach (var item in formDefinations)
                {
                    var type = dataTypeList.Where(x => x.DefinationId == int.Parse(item.DataType)).FirstOrDefault();
                    TMP_NodeSettings controlValue = controlValues.Where(x => x.KeyName.ToString() == item.Title.ToString()).FirstOrDefault();
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                    string dropdown, value, controlType, settingValue;
                    dropdown = value = controlType = settingValue = string.Empty;

                    string isRequired = item.Required == "true" ? "required" : string.Empty;
                    string Comments = "<td></td>";
                    string maxLenght = !string.IsNullOrEmpty(item.MaxLength) ? string.Format("maxlength='{0}'", item.MaxLength) : string.Empty;
                    
                    string defaultValue = !string.IsNullOrEmpty(item.DefaultValue) ? string.Format("value='{0}'", item.DefaultValue) : string.Empty;
                    if (item.Comments.ToLower() == "true")
                    {
                        Comments = "<td><input maxlength='200' class='form-control' data-FormId='" + item.FormId + "' placeholder=Comments type=text id=sortOrderInputComments" + item.FormId + " name=comment' /></td>";
                    }
                    if (controlValue != null)
                    {
                        value = !string.IsNullOrEmpty(controlValue.Value) ? "value='" + WebUtility.HtmlEncode(controlValue.Value) + "'" : string.Empty;
                        settingValue = !string.IsNullOrEmpty(controlValue.Settings) ? "value='" + controlValue.Settings + "'" : string.Empty;
                    }

                    FormHTMLTabular += "<tr>  <td> <label>" + textInfo.ToTitleCase(item.Title) + "</label> </td>";

                    // if form is node type add additional dropdown for dom id's for rendering dom ids to Mapping id 
                    if (isFormNode)
                    {
                        if (type.DefinationName.ToLower() != "query")
                        {
                            FormHTMLTabular += "<td>  <div class='DomIdsList'> </div></td>" ;
                        }
                        else
                        {
                            var Query = controlValue?.Value ?? "";
                            FormHTMLTabular += "<td colspan='2'>";
                            FormHTMLTabular += "<textarea id='CustomQueryTextArea' readonly class='form-control' style='width: 100%; height: 156px; resize: vertical;'>" + Query + "</textarea><div class='text-center'><span></span></div><input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='hidden' readonly='readonly' id='queryBuilderJSON" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + "  /></td> ";
                        }
                    }

                    if (type.DefinationName.ToLower() == "text")
                    {
                        var nodeid = Convert.ToDecimal(NodeId);
                        var pageTitle = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "page title").FirstOrDefault();
                        string tempValue = !string.IsNullOrEmpty(pageTitle?.Value) ? "value='" + WebUtility.HtmlEncode(pageTitle?.Value) + "'" : string.Empty;

                        controlType = "text";
                        //if(pageTitle != null)
                        //{
                        //    FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + tempValue + " /> </td>";
                        //}else
                        //{

                        //}
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " /> </td>";
                    }

                    if (type.DefinationName.ToLower() == "number" || type.DefinationName.ToLower() == "decimal")
                    {
                        controlType = "number";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " /> </td>";
                    }

                    if (type.DefinationName.ToLower() == "date")
                    {
                        controlType = "date";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + defaultValue + "  " + value + "/> </td>";
                    }
                    if (type.DefinationName.ToLower() == "radiobutton")
                    {
                        controlType = "radio";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + defaultValue + "  " + value + "/> </td>";
                    }
                    if (type.DefinationName.ToLower() == "colorpicker")
                    {
                        controlType = "color";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + defaultValue + "   " + value + "/> </td>";
                    }

                    if (type.DefinationName.ToLower() == "checkbox")
                    {
                        controlType = "checkbox";
                        FormHTMLTabular += "<td> <input type='checkbox' class='frm-dynamic' data-FormId='" + item.FormId + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + defaultValue + "/> </td>";
                    }

                    if (type.DefinationName.ToLower() == "staticdropdown")
                    {

                        var nodeid = Convert.ToDecimal(NodeId);
                        var queryJson = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "query").FirstOrDefault();
                        var MapLongitude = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "longitude").FirstOrDefault();
                        var MapLatitude = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "latitude").FirstOrDefault();
                        var xaxis = NodeSetting.Where(x => x.NodeId == nodeid && x.MappedId == "X_AXIS").FirstOrDefault();
                        var yaxis = NodeSetting.Where(x => x.NodeId == nodeid && x.MappedId == "Y_AXIS").FirstOrDefault();
                        var MapPlotColor = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "color").FirstOrDefault();
                        string dropdownList = "";
                        string longitude = "";
                        string latitude = "";

                        // MapLongitude?.DefinationId == 133324 || MapLatitude?.DefinationId == 133324
                        if (MapLongitude?.MappedId == "MAP_LONGITUDE" || MapLatitude?.MappedId == "MAP_LATITUDE")
                        {
                            Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(queryJson?.Settings);
                            if (DeserializeQuerySetting.QuerySetting.MetaData?.Count() != 0)
                            {

                                if (item.Title.ToLower() == "longitude")
                                {
                                    foreach (var col in DeserializeQuerySetting.QuerySetting.MetaData)
                                    {
                                        if (col.Alias.ToLower() == MapLongitude.Value.ToLower())
                                        {
                                            longitude += "<option value='" + col.ColumnName + "' selected>" + MapLongitude.Value + "</option>";
                                        }
                                    }
                                    dropdownList = longitude;
                                }
                                if (item.Title.ToLower() == "latitude")
                                {
                                    foreach (var col in DeserializeQuerySetting.QuerySetting.MetaData)
                                    {
                                        if (col.Alias.ToLower() == MapLatitude.Value.ToLower())
                                        {
                                            latitude += "<option value='" + col.ColumnName + "' selected>" + MapLatitude.Value + "</option>";
                                        }
                                    }
                                    dropdownList = latitude;
                                }
                            }

                        }

                        // if(queryJson != null && queryJson.DefinationId == 133322)
                        if (queryJson != null && (xaxis?.MappedId == "X_AXIS" || yaxis?.MappedId == "Y_AXIS"))
                        //if (queryJson != null && queryJson.DefinationId == 133322)
                        {
                            Schema DeserializeQuerySetting = new JavaScriptSerializer().Deserialize<Schema>(queryJson?.Settings);
                            if (DeserializeQuerySetting.QuerySetting.MetaData?.Count() != 0)
                            {
                                if (item.Title.ToLower() == "x-axis")
                                {
                                    string axisCols = "";
                                    var X_AXIS = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "x-axis").FirstOrDefault();
                                    // .Where(x => x.DataType == "nvarchar" || x.DataType == "varchar")
                                    foreach (var col in DeserializeQuerySetting.QuerySetting.MetaData)
                                    {
                                        string temp = "";
                                        temp = col.Alias;
                                        if (temp == X_AXIS?.Value)
                                            axisCols += "<option value='" + temp + "' selected>" + col.Alias + "</option>";
                                        else
                                            axisCols += "<option value='" + temp + "'>" + col.Alias + "</option>";
                                    }
                                    dropdownList = axisCols;
                                }
                                else if (item.Title.ToLower() == "y-axis")
                                {
                                    List<string> AggCols = new List<string>();
                                    string axisCols = "";
                                    var Y_AXIS = NodeSetting.Where(x => x.NodeId == nodeid && x.KeyName.ToLower() == "y-axis").FirstOrDefault();
                                    foreach (var col in DeserializeQuerySetting.QuerySetting.MetaData.Where(x => x.DataType == "float" || x.DataType == "int" || x.DataType == "rear" || x.DataType == "numeric"))
                                    {
                                        string temp = "";
                                        temp = col.Alias;
                                        if (temp == Y_AXIS?.Value)
                                            axisCols += "<option value='" + temp + "' selected>" + col.Alias + "</option>";
                                        else
                                            axisCols += "<option value='" + temp + "'>" + col.Alias + "</option>";
                                    }
                                    foreach (var col in DeserializeQuerySetting.QuerySetting.MetaData.Where(x => x.GroupBy == false && x.Funtion != "-1"))
                                    {
                                        string temp = "";
                                        temp = col.Alias;
                                        if (temp == Y_AXIS?.Value)
                                            axisCols += "<option value='" + temp + "' selected>" + col.Alias + "</option>";
                                        else
                                            axisCols += "<option value='" + temp + "'>" + col.Alias + "</option>";
                                    }
                                    dropdownList = axisCols;
                                }
                            }

                        }

                        //if(item.Title.ToLower() == "page title")
                        //{
                        //    controlType = "text";
                        //    FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='" + controlType + "' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " /> </td>";
                        //}
                        List<string> ddValues = item.DefaultValue.Split(',').ToList();

                        controlType = "dropdown";
                        FormHTMLTabular += "<td> <select name='" + item.Title + "' class='form-control frm-dynamic' data-FormId='" + item.FormId + "' " + isRequired + ">";
                        if (isFormNode)
                        {
                            FormHTMLTabular += "<option value=''>Select " + textInfo.ToTitleCase(item.Title) + "</option>";
                        }
                        else
                        {
                            FormHTMLTabular += "<option value=''> </option>";
                        }

                        if (item.Title.ToLower() == "y-axis" || item.Title.ToLower() == "x-axis")
                        {
                            FormHTMLTabular += dropdownList;
                        }
                        else if (item.Title.ToLower() == "longitude" || item.Title.ToLower() == "latitude" || item.Title.ToLower() == "color")
                        {
                            FormHTMLTabular += dropdownList;
                        }
                        else
                        {
                            foreach (var ddItem in ddValues)
                            {
                                if (controlValue != null && ddItem.Trim() == controlValue.Value)
                                {
                                    FormHTMLTabular += "<option value='" + ddItem.Trim() + "' selected>" + textInfo.ToTitleCase(ddItem.Trim()) + "</option>";
                                }
                                else
                                {
                                    FormHTMLTabular += "<option value='" + ddItem.Trim() + "'>" + textInfo.ToTitleCase(ddItem.Trim()) + "</option>";
                                }
                            }
                        }

                        FormHTMLTabular += "</select> </td>";
                    }

                    // Dynamic Drop Down
                    if (type.DefinationName.ToLower() == "dynamicdropdown")
                    {
                        // DYNAMIC DROPDOWN
                    }

                    if (type.DefinationName.ToLower() == "image")
                    {
                        controlType = "image";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' id='" + controlType + i + "' name='" + item.Title + "' type='text' readonly='readonly' style='cursor: pointer;' " + value + " onclick='fileManagerSetting(\"" + controlType + "\", \"" + item.Title + "\", \"" + controlType + i + "\" )' /> </td>";
                    }

                    // DateTime or Date 
                    //if (type.DefinationName.ToLower() == "date")
                    //{
                    //    controlType = "date";
                    //    FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' id='" + controlType + i + "' name='" + item.Title + "' type='" + controlType + "' ' " + value + " /> </td>";
                    //}

                    if (type.DefinationName.ToLower() == "table")
                    {
                        controlType = "table";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='text' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " /> </td>";
                    }

                    if (type.DefinationName.ToLower() == "imagelist")
                    {
                        controlType = "imagelist";
                        FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='text' id='sortOrderInput" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " /> </td>";
                    }

                    if (type.DefinationName.ToLower() == "query")
                    {
                        controlType = "query";
                        //FormHTMLTabular += "<td> <input class='form-control frm-dynamic' data-FormId='" + item.FormId + "' placeholder='" + textInfo.ToTitleCase(item.Title) + "' type='text' readonly='readonly' id='queryBuilderJSON" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + value + " style='cursor: pointer;'  onclick='OpenQueryBuilder(\"" + controlType + "\", \"queryBuilderJSON" + item.FormId + "\" )'  /> </td>";
                    }
                   
                    // if form is node type add additional settings input type 
                    if (isFormNode)
                    {
                        if (controlType.ToLower() == "imagelist" || controlType.ToLower() == "table" || controlType.ToLower() == "text" || controlType.ToLower() == "date" || controlType.ToLower() == "image" || controlType.ToLower() == "query")
                        {
                            if (type.DefinationName.ToLower() == "query")
                            {
                                FormHTMLTabular += "<td class='text-center'><a onclick='OpenQueryBuilder(\"" + controlType + "\", \"queryBuilderJSON" + item.FormId + "\" )'><span class='glyphicon glyphicon-cog'></span></a> <input class='form-control' name='AdvControlSetting" + i + "' id='AdvControlSetting" + i + "' type='hidden' readonly='readonly' style='cursor: pointer;' " + settingValue + " /> </td>";
                            }
                            else
                            {
                                FormHTMLTabular += "<td class='text-center'><a onclick='AdvanceControlSetting(\"" + controlType + "\", \"AdvControlSetting" + i + "\" )'><span class='glyphicon glyphicon-cog'></span></a> <input class='form-control' name='AdvControlSetting" + i + "' id='AdvControlSetting" + i + "' type='hidden' readonly='readonly' style='cursor: pointer;' " + settingValue + " /> </td>";
                            }

                        }
                        else
                        {
                            FormHTMLTabular += "<td></td>";
                        }
                    }
                    FormHTMLTabular +=Comments;
                    FormHTMLTabular += "</tr>";
                    i++;
                }
                FormHTMLTabular += " </tbody>  </table>";
            }
            else
            {
                FormHTMLTabular += "No Form Available";
            }

            // NODE TEMPLATE DROPDOWN
            string nodeTemplateListHTML = string.Empty;
            foreach (var item in nodeTemplateList)
            {

                var nodeData = NodeBL.ToList("ByNodeId", NodeId.ToString()).Where(x => x.NodeId == int.Parse(NodeId)).FirstOrDefault();
                if (nodeData?.NodeUrl == "_thankyouPage.cshtml" && item.KeyCode == "_thankyouPage.cshtml")
                {
                    nodeTemplateListHTML += "<option value='" + item.KeyCode + "' selected>" + item.DisplayText + "</option>";
                }
                else if (nodeData?.NodeUrl == "_welcomePage.cshtml" && item.KeyCode == "_welcomePage.cshtml")
                {
                    nodeTemplateListHTML += "<option value='" + item.KeyCode + "' selected>" + item.DisplayText + "</option>";
                }
                else
                {
                    nodeTemplateListHTML += "<option value='" + item.KeyCode + "'>" + item.DisplayText + "</option>";
                }

            }
            TMP_NodeSettingsBL nodeSetting = new TMP_NodeSettingsBL();
            var Data = nodeSetting.ToList("GET_BY_NODEID", NodeId);
            string UserSelectedCols = null;
            string WhereClause = null;
            string WhereClauseJson = "";
            if (Data.Count > 0)
            {
                UserSelectedCols = Data?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.Settings;
                WhereClause = Data?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.Value;
                WhereClauseJson = Data?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.QueryWhereClause;
            }

            return Json(new
            {
                FormHtml = FormHTMLTabular,
                NodeBasicInfo = nodeInfo,
                MappingIds = mappingDomIds,
                NodeTempList = nodeTemplateListHTML,
                UserDataTableCols = UserSelectedCols,
                WhereClause = WhereClause,
                WhereClauseJson = WhereClauseJson
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GET HTML OF FORM_BUILDER 
        /// </summary>
        /// <param name="FormTypeId"></param>
        /// <param name="NodeId"></param>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetFormBuilderRenderedHTML(string FormTypeId, string NodeId, string TemplateId,Int64 SiteId=0)
        {
            return FormBuilderRederHtml( FormTypeId, NodeId, TemplateId, SiteId= 0);
        }

        [IsLogin(Return = "NoCheck", CheckPermission = false)]
        [MobileAuth]
        public JsonResult GetFormBuilderRenderedHTMLMobile(string FormTypeId, string NodeId, string TemplateId, Int64 SiteId = 0, string Key = null)
        {
            var Result = FormBuilderRederHtml(FormTypeId, NodeId, TemplateId, SiteId = 0);
            return Result;
        }

        /// <summary>
        /// Get Form Builder For Query Rendered HTML
        /// </summary>
        /// <param name="FormTypeId"></param>
        /// <param name="NodeId"></param>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetFormBuilderForQueryRenderedHTML(string FormTypeId, string NodeId, string TemplateId)
        {
            TMP_Node nodeInfo = new TMP_Node();
            List<TMP_NodesProperties> formDefinations = null;

            nodeInfo = NodeBL.ToList("ByTemplateId", TemplateId).Where(x => x.NodeId == int.Parse(NodeId)).FirstOrDefault();
            FormTypeId = nodeInfo.PageTyppeId.ToString();

            var formTypeName = DefinationBL.ToList("byDefinationType", "FORMTYPE");
            var formTypeDetail = formTypeName.Where(x => x.KeyCode.Contains("node".ToUpper())).FirstOrDefault();

            if (formTypeDetail != null && NodeId != null)
            {
                formDefinations = NodesPropertiesBL.ToList("ByNodeTypeId", FormTypeId);
            }

            string FormHTMLTabular = "";
            var dataTypeList = DefinationBL.ToList("byDefinationType", "DataType");
            var nodeTemplateList = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.PDefinationId == nodeInfo.PageTyppeId);
            //var nodeTemplateList = dddefination.ToList("byDefinationType", "DataType");
            var controlValues = NodeSettingsBL.ToList("GET_BY_NODEID", NodeId);
            // Get Mapped Values (DOM Ids)
            var mappingDomIds = controlValues.Select(x => x.MappedId);

            foreach (var item in formDefinations)
            {
                var type = dataTypeList.Where(x => x.DefinationId == int.Parse(item.DataType)).FirstOrDefault();
                TMP_NodeSettings controlValue = controlValues.Where(x => x.KeyName.ToString() == item.Title.ToString()).FirstOrDefault();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                string isRequired, defaultValue, maxLenght, dropdown, value, settingValue;
                isRequired = defaultValue = maxLenght = dropdown = value = settingValue = string.Empty;

                isRequired = item.Required == "true" ? "required" : string.Empty;
                maxLenght = !string.IsNullOrEmpty(item.MaxLength) ? string.Format("maxlength='{0}'", item.MaxLength) : string.Empty;
                defaultValue = !string.IsNullOrEmpty(item.DefaultValue) ? string.Format("value='{0}'", item.DefaultValue) : string.Empty;

                if (controlValue != null)
                {
                    value = !string.IsNullOrEmpty(controlValue.Value) ? "value='" + WebUtility.HtmlEncode(controlValue.Value) + "'" : string.Empty;
                    settingValue = !string.IsNullOrEmpty(controlValue.Settings) ? "value='" + controlValue.Settings + "'" : string.Empty;

                }

                if (type.DefinationName.ToLower() == "query")
                {
                    //<label>" + textInfo.ToTitleCase(item.Title) + "</label> 
                    FormHTMLTabular += "<textarea class='form-control' placeholder='" + textInfo.ToTitleCase(item.Title) + "' readonly='readonly' id='queryBuilderJSON" + item.FormId + "' name='" + item.Title + "' " + isRequired + " " + maxLenght + " " + defaultValue + "  " + " style='cursor: pointer;'  onclick='OpenQueryBuilder(\"query\", \"queryBuilderJSON" + item.FormId + "\" )'  >"+ controlValue?.Value + "</textarea> ";
                }
            }

            // NODE TEMPLATE DROPDOWN
            string nodeTemplateListHTML = String.Empty;
            foreach (var item in nodeTemplateList)
            {
                nodeTemplateListHTML += "<option value='" + item.KeyCode + "'>" + item.DisplayText + "</option>";
            }

            string settings = "";
          if (controlValues.Count > 0)
              {
                settings = controlValues?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.Settings;
               }

            string WhereClauseJson = "";
            if (controlValues.Count > 0)
            {
                WhereClauseJson = controlValues?.FirstOrDefault(x => x.KeyName.ToLower() == "query")?.QueryWhereClause;
            }


            return Json(new
            {
                FormHtml = FormHTMLTabular,
                NodeBasicInfo = nodeInfo,
                MappingIds = mappingDomIds,
                NodeTempList = nodeTemplateListHTML,
                Settings = settings,
                WhereClauseJson = WhereClauseJson
            }, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// INSERT OR UPDATE DATA ENTERED IN RENDERED FORM BUILDER 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult PostFormBuilderValuesReport(FormCollection fc)
        {
            try
            {
                string selectedNodeId = fc["SelectedNodeId"];
                string templateId = fc["RouteTemplateId"];

                if (!string.IsNullOrEmpty(selectedNodeId))
                {
                    // advanced settings data 
                    var formItemList = NodesPropertiesBL.ToList("GET_FORM_DATA", selectedNodeId);
                    var formValues = NodeSettingsBL.ToList("GET_BY_NODEID", selectedNodeId);
                    int i = 0;
                    foreach (var item in formItemList)
                    {
                        var FormValueToUpdate = formValues.Where(x => x.KeyName == item.Title).FirstOrDefault();
                        string Setting = string.Empty;
                        string FilterClause = string.Empty;
                        if (item.Title.ToLower() == "query")
                        {
                            //if ((item.NodeTypeId == 133323 || item.NodeTypeId == 153348 || item.NodeTypeId == 133322 || item.NodeTypeId == 133324) && item.Title.ToLower() == "query")
                            if (item.Title.ToLower() == "query")
                            {
                                Setting = fc["DataSetColumnsHidden"];
                                FilterClause = fc["FilterClauseHidden"];
                            }
                            else
                            {
                                Setting = !string.IsNullOrEmpty(fc["AdvControlSetting" + i]) ? fc["AdvControlSetting" + i] : FormValueToUpdate?.Settings;
                            }
                        }
                        else
                        {
                            Setting = !string.IsNullOrEmpty(fc["AdvControlSetting" + i]) ? fc["AdvControlSetting" + i] : FormValueToUpdate?.Settings;
                        }
                        if (FormValueToUpdate != null && !string.IsNullOrEmpty(fc[item.Title]))
                        {
                            NodeSettingsBL.Manage("ReportQueryBuilderUPDATE", new TMP_NodeSettings
                            {
                                NodeSettingsId = FormValueToUpdate.NodeSettingsId,

                                Value = WebUtility.HtmlDecode(!string.IsNullOrEmpty(fc[item.Title].ToString()) ? fc[item.Title].ToString() : FormValueToUpdate.Settings),


                                QueryWhereClause = FilterClause
                            });
                        }

                        i++;
                    }
                }
                //return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception er)
            {
                return Json("error");
            }
            return Json("ok");
        }

        /// <summary>
        /// Dashboard Query Builder Value
        /// </summary>
        /// <param name="fc">Form Collection Values</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult DashboardQueryBuilderValue(FormCollection fc)
        {
            return Json("OK");
        }

        /// <summary>
        /// INSERT OR UPDATE DATA ENTERED IN RENDERED FORM BUILDER 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult PostFormBuilderValues(FormCollection fc)
        {
            try
            {
                string selectedNodeId = fc["SelectedNodeId"];
                string templateId = fc["RouteTemplateId"];

                if (!string.IsNullOrEmpty(selectedNodeId))
                {
                    // advanced settings data 
                    var formItemList = NodesPropertiesBL.ToList("GET_FORM_DATA", selectedNodeId);
                    var formValues = NodeSettingsBL.ToList("GET_BY_NODEID", selectedNodeId);
                    int i = 0;
                    foreach (var item in formItemList)
                    {
                        var FormValueToUpdate = formValues.Where(x => x.KeyName == item.Title).FirstOrDefault();
                        string Setting = string.Empty;
                        string FilterClause = string.Empty;
                        if (item.Title.ToLower() == "query")
                        {
                            //if ((item.NodeTypeId == 133323 || item.NodeTypeId == 153348 || item.NodeTypeId == 133322 || item.NodeTypeId == 133324) && item.Title.ToLower() == "query")
                            if (item.Title.ToLower() == "query")
                            {
                                Setting = fc["DataSetColumnsHidden"];
                                FilterClause = fc["FilterClauseHidden"];
                            }
                            else
                            {
                                Setting = !string.IsNullOrEmpty(fc["AdvControlSetting" + i]) ? fc["AdvControlSetting" + i] : FormValueToUpdate?.Settings;
                            }
                        }
                        else
                        {
                            Setting = !string.IsNullOrEmpty(fc["AdvControlSetting" + i]) ? fc["AdvControlSetting" + i] : FormValueToUpdate?.Settings;
                        }
                        if (FormValueToUpdate != null && !string.IsNullOrEmpty(fc[item.Title]))
                        {
                            NodeSettingsBL.Manage("UPDATE", new TMP_NodeSettings
                            {
                                NodeSettingsId = FormValueToUpdate.NodeSettingsId,
                                DefinationId = FormValueToUpdate.DefinationId,
                                NodeId = FormValueToUpdate.NodeId,
                                KeyName = !string.IsNullOrEmpty(item.Title) ? item.Title : FormValueToUpdate.KeyName,
                                MappedId = !string.IsNullOrEmpty(fc["AdvSettingMappingId" + i]) ? fc["AdvSettingMappingId" + i] : FormValueToUpdate.MappedId,
                                Value = WebUtility.HtmlDecode(!string.IsNullOrEmpty(fc[item.Title].ToString()) ? fc[item.Title].ToString() : FormValueToUpdate.Settings),
                                Settings = Setting,
                                SortOrder = i,
                                QueryWhereClause = FilterClause
                            });
                        }
                        else if (!string.IsNullOrEmpty(fc[item.Title]))
                        {
                            NodeSettingsBL.Manage("INSERT", new TMP_NodeSettings
                            {
                                NodeId = decimal.Parse(selectedNodeId),
                                DefinationId = item.NodeTypeId,
                                KeyName = item.Title,
                                MappedId = fc["AdvSettingMappingId" + i],
                                Value = fc[item.Title],
                                Settings = Setting,
                                SortOrder = i,
                                QueryWhereClause = FilterClause
                            });
                        }
                        //else if ((item.NodeTypeId == 133323 || item.NodeTypeId == 153348 || item.NodeTypeId == 133322 || item.NodeTypeId == 133324) &&  item.Title.ToLower() == "query")
                        else if (item.Title.ToLower() == "query")   // FOR ONLY TABLE & FOR TABLE WITH MAP Resp...
                        {
                            var MapperID = fc["AdvSettingMappingId" + i] ?? "-1";
                            NodeSettingsBL.Manage("INSERT", new TMP_NodeSettings
                            {
                                NodeId = decimal.Parse(selectedNodeId),
                                DefinationId = item.NodeTypeId,
                                KeyName = item.Title,
                                MappedId = MapperID,
                                Value = fc[item.Title] ?? "",
                                Settings = Setting,
                                SortOrder = i,
                                QueryWhereClause = FilterClause
                            });
                        }
                        i++;
                    }
                }
                //return Redirect(Request.UrlReferrer.ToString());
            }
            catch(Exception er)
            {
                return Json("error");
            }
            return Json("ok");
        }

        /// <summary>
        /// Base Settings Form Values
        /// Like updating or adding title url or query
        /// </summary>
        /// <param name="NodeId">Node Id</param>
        /// <param name="NodeTitle">Node Title</param>
        /// <param name="NodeURL">Node URL</param>
        /// <param name="NodeQuery">Node Query</param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult BaseSettingsFormValues(string NodeId, string NodeTitle, string NodeURL, string NodeQuery)
        {
            try{
                // base settings data 
                if (!string.IsNullOrEmpty(NodeId))
                {
                    NodeBL.Manage("UpdateRelaventData", new TMP_Node
                    {
                        NodeId = int.Parse(NodeId),
                        NodeTitle = NodeTitle,
                        NodeUrl = NodeURL,
                        NodeSQL = NodeQuery
                    });
                    return Json("ok");
                }
            }
            catch(Exception er)
            {

            }
            return Json("error");
        }

        /// <summary>
        /// Get File Folders List for FILE MANAGEMENT 
        /// </summary>
        /// <param name="BaseFolderPath">Base Folder Path</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetFileFolders(string BaseFolderPath)
        {
            string FileInfo = string.Empty;
            List<string> Directories = new List<string>();
            Dictionary<string, string> FileFolderList = new Dictionary<string, string>();

            BaseFolderPath = string.IsNullOrEmpty(BaseFolderPath) ? BaseFolderPath = "/Content/AirViewLogs/" : BaseFolderPath += "/";

            Directories.AddRange(dh.Folders(Server.MapPath(BaseFolderPath)));
            Directories.AddRange(dh.Files(Server.MapPath(BaseFolderPath)));

            string fileMgrJson = "[ ";
            if (Directories != null)
            {
                foreach (var item in Directories)
                {
                    if (item.Contains(".jpg") || item.Contains(".png"))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(BaseFolderPath + item));
                        FileInfo = img.Width + " x " + img.Height;
                    }
                    fileMgrJson += "{ \"FileName\": \"" + item + "\", \"FilePath\": \"" + BaseFolderPath + item + "\", \"FileInfo\": \"" + FileInfo + "\" },";
                }
                fileMgrJson = fileMgrJson.Remove(fileMgrJson.Length - 1);
            }
            fileMgrJson += " ]";

            return Json(fileMgrJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Upload file in Content for FILE MANAGEMENT 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult UploadFiles()
        {
            var currentPath = !string.IsNullOrEmpty(Request["FileUploadPath"]) ? Request["FileUploadPath"] : "/Content/AirViewLogs/";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fname = Path.Combine(Server.MapPath(currentPath), fname);
                        file.SaveAs(fname);
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// FORM BUILDER PARTIAL VIEW 
        /// </summary>
        /// <param name="FormTypeId">Form Type Id</param>
        /// <param name="KeyId">Key Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public ActionResult FormBuilderPartial(int FormTypeId, string KeyId)
        {
            ViewBag.DefinationId = !string.IsNullOrEmpty(KeyId) ? KeyId : "0";
            ViewBag.FormId = FormTypeId;

            return PartialView("_formBuilder");
        }

        /// <summary>
        /// Generic form builder main view 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="definationId">Defination Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public ActionResult FormBuilder(int id, string definationId)
        {
            ViewBag.DefinationId = !string.IsNullOrEmpty(definationId) ? definationId : "0";
            ViewBag.FormId = id;

            return View();
        }

        /// <summary>
        /// Pass list of form elements to save form builder core information
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="controlType">Control Type</param>
        /// <param name="dataType">Data Type</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="maxLength">Max Length</param>
        /// <param name="required">Requires</param>
        /// <param name="sortOrder">Sort Order</param>
        /// <param name="formTypeId">Form Type Id</param>
        /// <returns></returns>
        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public ActionResult FormBuilder(string[] title, string[] controlType, string[] dataType, string[] defaultValue, string[] maxLength, string[] required, string[] isAttachments, int[] sortOrder, string formTypeId, string definationTypeId,int[] formid,int[] isdeleted,string[] comment)
        {
            TMP_NodesProperties nodeProp = new TMP_NodesProperties();
            nodeProp.NodeTypeId = decimal.Parse(definationTypeId);
           // NodesPropertiesBL.Manage("Delete", nodeProp);
            if (title != null)
            {
                for (int i = 0; i < title.Length; i++)
                {
                    if (title[i] != null && controlType[i] != null && maxLength[i] != null)
                    {
                        nodeProp = new TMP_NodesProperties
                        {
                            NodeTypeId = decimal.Parse(definationTypeId),
                            Title = title[i],
                            ControlType = controlType[i],
                            DataType = dataType[i],
                            DefaultValue = defaultValue[i],
                            MaxLength = maxLength[i],
                            Required = required[i] == "true" ? "true" : "false",
                            IsAttachment = isAttachments[i] == "true" ? "true" : "false",
                            Comments = comment[i] == "true" ? "true" : "false",
                            SortOrder = sortOrder[i],
                            FormId = formid[i],
                            IsDeleted = isdeleted[i]
                        };
                        NodesPropertiesBL.Manage("Insert", nodeProp);
                    }
                }
                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }
            // return RedirectToAction("FormBuilder", "Template");
            return Json(new { Status=false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get set of Data Type, Control Type, Form Type List
        /// </summary>
        /// <param name="FormId">Form Id</param>
        /// <param name="DefinationId">Defination Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetTypeList(string FormId, string DefinationId)
        {
            // Control Type like INPUT etc 
            List<AD_Defination> controlTypeList = DefinationBL.ToList("byDefinationType", "ControlType");

            // Data Type like DROPDOWN, IMAGE, MAP, TABLE etc.
            List<AD_Defination> dataTypeList = DefinationBL.ToList("byDefinationType", "DataType");

            // Form Type like NODE, MILESTONE etc.
            AD_Defination formType = DefinationBL.ToList("byDefinationType", "FormType").Where(x => x.DefinationId == int.Parse(FormId)).FirstOrDefault();
            string formTypeName = formType != null ? formType.DefinationName : null;

            //Node Type like PAGE, MAP_TABLE etc.
            List<AD_Defination> nodeTypeList = DefinationBL.ToList("byDefinationType", "NodeType").Where(x => x.PDefinationId == 0).ToList();

            return Json(new
            {
                ControlTypeList = controlTypeList,
                DataTypeList = dataTypeList,
                FormTypeName = formTypeName,
                NodeTypeList = nodeTypeList
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Core Form List
        /// </summary>
        /// <param name="DefinationId">Defination Id</param>
        /// <returns></returns>
        [IsLogin(CheckPermission = false)]
        public JsonResult GetCoreFormList(string DefinationId)
        {
            // Form Core Structure
            List<TMP_NodesProperties> FormItems = new List<TMP_NodesProperties>();
            FormItems = DefinationId != null ? NodesPropertiesBL.ToList("ByNodeTypeId", DefinationId.ToString()).ToList() : null;

            return Json(new
            {
                FormItems = FormItems
            }, JsonRequestBehavior.AllowGet);
        }

        #region Public Functions

        /// <summary>
        /// CREATE KML file on ADDRESS (Path) 
        /// </summary>
        /// <param name="address">Path where file to be generate</param>
        /// <param name="plotName">Plot Name like PCI, RERP etc</param>
        /// <param name="KML">KML File</param>
        public void CreateKML(string address, string plotName, string KML)
        {
            DirectoryHandler dh = new DirectoryHandler();
            dh.CreateDirectory(Server.MapPath(address));
            System.IO.File.WriteAllText(Server.MapPath(address) + plotName + ".kml", KML);
            return;
        }


        public dynamic GetAzimuth(string data)
        {
            try
            {
                Azmiuth az;
                List<Azmiuth> azlst = new List<Azmiuth>();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = (Int32)900000000;
                var list = serializer.Deserialize<dynamic>(data);

                foreach (var item in list)
                {
                    double _lat = 0;
                    double _lon = 0;
                    if(double.TryParse(item["Latitude"], out _lat) && double.TryParse(item["Longitude"], out _lon))
                    {
                        az = new Azmiuth();
                        az.Latitude = _lat;
                        az.Longitude = _lon;
                        az.Site = item["Site"];
                        az.Sector = item["Sector"];
                        az.PCI = Convert.ToString(item["PCI"]);
                        az.Color = item["SectorColor"];
                        az.StartAngle = int.Parse(item["Azimuth"]) - (int.Parse(item["BeamWidth"]) / 2);
                        az.EndAngle = int.Parse(item["Azimuth"]) + (int.Parse(item["BeamWidth"]) / 2);

                        if (az.EndAngle == 360)
                        {
                            az.EndAngle = 0;
                        }
                        else if (az.EndAngle > 360)
                        {
                            double tmpEnd = az.EndAngle - 360;
                            az.EndAngle = (tmpEnd >= 0 && tmpEnd <= 1) ? 1 : Convert.ToInt32(az.EndAngle - 360);
                        }
                        azlst.Add(az);
                    }
                    
                }
                var temp = azlst.Distinct().ToList();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = (Int32)900000000;
                var itemfirst = jss.Serialize(temp);
                return itemfirst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// Function to get data table JSON and return AZMUTH list 
        /// </summary>
        /// <param name="data">Data Table JSON</param>
        /// <returns></returns>
        //public string GetAzimuth(List<Azmiuth> data)
        //{
        //    Azmiuth az;
        //    List<Azmiuth> azlst = new List<Azmiuth>();

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    // serializer.MaxJsonLength =(Int32)900000000;
        //    serializer.MaxJsonLength = int.MaxValue;
        //    var list = data;

        //    foreach (var item in list)
        //    {
        //        az = new Azmiuth();
        //        string latitude = Convert.ToString(item.Latitude) == "" ? "1" : Convert.ToString(item.Latitude);
        //        string longitude = Convert.ToString(item.Longitude)  == "" ? "1" : Convert.ToString(item.Longitude);
        //        az.Latitude = Convert.ToDouble(latitude);
        //        az.Longitude = Convert.ToDouble(longitude);
        //        az.Site = item.Site;
        //        az.Sector = item.Sector;
        //        az.PCI = item.PCI;
        //        az.Color = item.Color;
        //        az.StartAngle = int.Parse(Convert.ToString(item.StartAngle)) - (int.Parse(Convert.ToString(item.StartAngle)) / 2);
        //        az.EndAngle = int.Parse(Convert.ToString(item.EndAngle)) + (int.Parse(Convert.ToString(item.EndAngle)) / 2);

        //        if (az.EndAngle == 360)
        //        {
        //            az.EndAngle = 0;
        //        }
        //        else if (az.EndAngle > 360)
        //        {
        //            double tmpEnd = az.EndAngle - 360;
        //            az.EndAngle = (tmpEnd >= 0 && tmpEnd <= 1) ? 1 : Convert.ToInt32(az.EndAngle - 360);
        //        }
        //        azlst.Add(az);
        //    }
        //    var temp = azlst.Distinct().ToList();
        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    jss.MaxJsonLength = int.MaxValue;
        //    var result = jss.Serialize(temp);
        //    return result;
        //}

        [IsLogin(CheckPermission = false)]
        public ActionResult ChangeDefaultTemplate(int TemplateId, int ProjectId, int ScopeId, bool? IsDefault, int ModuleId)
        {
            TemplatesBL.Manage("MakeTemplateDefault", new TMP_Templates
            {
                TemplateId = TemplateId,
                IsDefault = IsDefault,
                ModuleId = ModuleId,
                ProjectId = ProjectId,
            });
            return RedirectToAction("List", new { ProjectId = ProjectId, ScopeId = ScopeId });
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult MakeTemplateUndefault(int TemplateId, int ProjectId, int ScopeId, int ModuleId)
        {
            TemplatesBL.Manage("MakeTemplateUndefault", new TMP_Templates
            {
                TemplateId = TemplateId,
                IsDefault = false,
                ModuleId = ModuleId,
                ProjectId = ProjectId,
            });
            return RedirectToAction("List", new { ProjectId = ProjectId, ScopeId = ScopeId });
        }

        [HttpPost]
        [IsLogin(CheckPermission = false)]
        public JsonResult GetDbTableColumnName(string ViewName)
        {
            var Cols = new TablesBL().ViewsColumn(ViewName).Select(p => 
                                                                new
                                                                {
                                                                    ColumnName = p.ColumnName,
                                                                    DataType = p.DataType,
                                                                    IsSelectType = p.DefinationType != null ? "select" : ""
                                                                }).ToList();
            return Json(Cols, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}