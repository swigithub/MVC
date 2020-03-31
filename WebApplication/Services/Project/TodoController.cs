using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.BLL;
using AirView.DBLayer.Project.DAL;
using AirView.DBLayer.Project.DTO;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Library.SWI.Project.Model;
using Newtonsoft.Json;
using SWI.AirView.Models;
using SWI.Libraries.AD.BLL;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication.Areas.Project.View_Models;

namespace WebApplication.Services.Project
{
    public class TodoController : ApiController
    {
        [System.Web.Http.Route("avx2/projects/{projectid}/events"), System.Web.Http.HttpGet]
        public HttpResponseMessage GetProjectEventsByProjectId(int projectid =0,int userid =0,DateTime? todate=null,DateTime? fromdate= null)
        {
            try
            {
                 PM_TodoBL pd = new PM_TodoBL();
                List<PM_Todo> rec = new List<PM_Todo>();
                string WhereClause = "";
                if (projectid != 0 && userid != 0)
                {
                   
                    rec = pd.GetTodo("Get_Todo",projectid,userid,WhereClause,todate,fromdate);
                }
                if (rec != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rec);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "No Data");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
        }
        [System.Web.Http.Route("avx2/projects/{UserId}/events"), System.Web.Http.HttpPost]
        public HttpResponseMessage AddEventInProjectByProjectId([FromUri]int UserId, PM_Todo todo)
        {
            try
            {
                PM_TaskBL pb = new PM_TaskBL();
                todo.CreatedOn = DateTime.Now;
                if (todo.TodoId == 0)
                {
                    var result = pb.SaveTodo(todo, "Insert_Todo");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                       "No Data");
                }
              
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
        }

        [System.Web.Http.Route("avx2/projects/{UserId}/events/{eventId}"), System.Web.Http.HttpPut]
        public HttpResponseMessage UpdateEventInProjectByProjectId([FromUri]int UserId, [FromUri]int eventId,PM_Todo todo)
        {
            try
            {
                PM_TaskBL pb = new PM_TaskBL();
                todo.CreatedOn = DateTime.Now;
                if (todo.TodoId != 0)
                {
                    var result = pb.SaveTodo(todo, "Edit_Todo");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                       "No Data");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
        }

        [System.Web.Http.Route("avx2/projects/{projectid}/events/filters"), System.Web.Http.HttpGet]
        public HttpResponseMessage GetProjectEventsByProjectId(int projectid = 0, int userid = 0)
        {
            try
            {
                PM_TodoBL pd = new PM_TodoBL();
                PM_ProjectEventFilters_DTO rec = new PM_ProjectEventFilters_DTO();
                string WhereClause = "";
                if (projectid != 0 && userid != 0)
                {
                  
                    rec = pd.GetTodofilters("Get_Todo_Filters", projectid, userid, WhereClause);
                }
                if (rec != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rec);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "No Data");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
        }
    }
}