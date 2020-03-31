using Newtonsoft.Json;
using Swi.Airview.Xcelerate.BusinessLogicLayer;
using Swi.Airview.Xcelerate.DataTransferObject.Modules.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.BusinessIntelligence.Controllers
{
    public class QueryBuilderController : Controller
    {
        [HttpPost]
        public JsonResult GetViews()
        {
            try
            {
                var obj = BusinessLogic.instance.HandleQueryBuilderBl.GetViewsList();
                return Json(new { obj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }
        [HttpPost]
        public JsonResult GetRemoteDatabase(RemoteServerDto RemoteInfo)
        {
            try
            {
                System.Data.DataTable dt = BusinessLogic.instance.HandleQueryBuilderBl.GetRemoteDatabases(RemoteInfo);
                if (dt.Rows.Count > 0)
                {
                    var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                    return new JsonResult()
                    {
                        Data = new { data = obj, msg = "Got Remote Databases", status = true },
                        ContentType = "application/json; charset=utf-8",
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult()
                {
                    Data = new {msg = "Got Remote Databases", status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {

                return new JsonResult()
                {
                    Data = new {msg = ex.Message.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        [HttpPost]
        public JsonResult GetRemoteViewsNTables(RemoteServerDto RemoteInfo)
        {
            try
            {
                System.Data.DataTable dt = BusinessLogic.instance.HandleQueryBuilderBl.GetRemoteViewsNTables(RemoteInfo);
                if (dt.Rows.Count > 0)
                {
                    var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                    return new JsonResult()
                    {
                        Data = new { data = obj, msg = "Got Tables N Views", status = true },
                        ContentType = "application/json; charset=utf-8",
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult()
                {
                    Data = new { msg = "Record not found", status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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

        }
        public JsonResult GetRemoteColumnsOfTablesNViews(RemoteServerDto RemoteInfo)
        {
            try
            {
                System.Data.DataTable dt = BusinessLogic.instance.HandleQueryBuilderBl.GetRemoteColumnsOfViewsNTables(RemoteInfo);
                if (dt.Rows.Count > 0)
                {
                    var obj = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                    return new JsonResult()
                    {
                        Data = new { data = obj, msg = "Got Tables N Views", status = true },
                        ContentType = "application/json; charset=utf-8",
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

                return new JsonResult()
                {
                    Data = new { msg = "Record not found", status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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

        }

        [HttpPost]
        public JsonResult GetViewColumns(string viewname)
        {
            var obj = BusinessLogic.instance.HandleQueryBuilderBl.GetViewsColumnsList(viewname);
            var Cols = obj.Select(p =>
                                                               new
                                                               {
                                                                   ColumnName = p.ColumnName,
                                                                   ColumnDataType = p.ColumnDataType,
                                                                   IsSelectType = p.ColumnDefinationType
                                                               }).ToList();
           
            return Json(new { obj=Cols }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetQueryResult(string query)
        {
            try
            {

                var dt = BusinessLogic.instance.HandleQueryBuilderBl.GetQueryResult(query);
                if (dt.Rows.Count >0)
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
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = new {  msg = ex.InnerException.ToString(), status = false },
                    ContentType = "application/json; charset=utf-8",
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult()
            {
                Data = new {  msg = "Data not found", status = false },
                ContentType = "application/json; charset=utf-8",
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult GetFilters(string Type)
        {
            try
            {
                
                return Json(BusinessLogic.instance.HandleQueryBuilderBl.ToList("byDefinationType", Type), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetReportrecord(string reportFilter, string where, string select, string group)
        {

            try
            {

                
                var result = BusinessLogic.instance.HandleQueryBuilderBl.ToDefinitionList(reportFilter, where, select, group);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetReport(string Filter)
        {
            try
            {
               
                return Json(BusinessLogic.instance.HandleQueryBuilderBl.ToList(Filter, "Report"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}