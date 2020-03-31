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
    public class ProjectTasksController : ApiController
    {

        [System.Web.Http.Route("avx2/projects/tasks/{projectid}"), System.Web.Http.HttpGet]
        public HttpResponseMessage GetProjectTaskByProjectId(int projectid)
        {
            try
            {
                PM_TaskBL tb = new PM_TaskBL();
                List<PM_Task> rec = new List<PM_Task>();
                if (projectid != 0 )
                {

                    rec = tb.ToList("ByProjectId", string.Empty, string.Empty, projectid, 0);
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