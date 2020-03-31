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
    public class ProjectsController : ApiController
    {
        [System.Web.Http.Route("avx2/projects"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetProject(object Json)
        {
            try { 
            ProjectApiObject obj = JsonConvert.DeserializeObject<ProjectApiObject>(Json.ToString());

            PM_ProjectBL pd = new PM_ProjectBL();
            List<PM_Projects_DTO> rec = new List<PM_Projects_DTO>();
            if ( obj.toDate != null && obj.fromDate != null )
            {       
                rec = pd.ToList("ProjectByFilters", "True", obj.statusIds, obj.priorityIds, obj.clientIds, obj.toDate, obj.fromDate, obj.userId);
            }
            else if (obj.searchKey != null && obj.searchKey != "")
            {
                rec = pd.DTOToList("ProjectByKey", obj.searchKey, obj.userId);
            }
            else
            {
                 rec = pd.DTOToList("ByStatus", "True", obj.userId);
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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
        }
        [System.Web.Http.Route("avx2/projects/filters/"), System.Web.Http.HttpPost]
        public HttpResponseMessage GetProjectLookUp(object Json)
        {
          try { 
            PM_ProjectBL pd = new PM_ProjectBL();
            PM_ProjectLookup rec = new PM_ProjectLookup();
            ProjectApiObject obj = JsonConvert.DeserializeObject<ProjectApiObject>(Json.ToString());
            if (obj.toDate != null && obj.fromDate != null)
            {
                rec = pd.GetLookup("LookupByFilters", "True", obj.userId, obj.statusIds, obj.priorityIds, obj.clientIds, obj.toDate, obj.fromDate);
            }
            else if(obj.searchKey != null && obj.searchKey !="")
            {
                rec = pd.GetLookup("LookupByKey", obj.searchKey, obj.userId);
            }
            else
            {
                rec = pd.GetLookup("Lookup", "True", obj.userId);
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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                  ex.Message);
            }
}
      
    }
}