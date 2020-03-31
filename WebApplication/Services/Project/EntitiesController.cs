using AirView.DBLayer.AirView.BLL;
using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.DTO;
using AirView.DBLayer.Project.Model;
using Library.SWI.Project.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Services.Project
{
    public class EntitiesController : ApiController
    {
        [Route("avx2/projects/entities"), HttpPost]
        public HttpResponseMessage GetProjectEntities(object Json)
        {
            try
            {
                PM_ProjectSitesBL ps = new PM_ProjectSitesBL();
                List<PM_ProjectEntity_DTO> rec = new List<PM_ProjectEntity_DTO>();
                ProjectApiObject obj = JsonConvert.DeserializeObject<ProjectApiObject>(Json.ToString());
                if (obj.toDate != null && obj.fromDate != null)
                {
                    rec = ps.GetEntitiesByProjectId("GetEntitiesByFilters", obj.projectId, obj.userId,obj.marketIds, obj.statusIds, obj.typeIds, obj.clientIds, obj.toDate, obj.fromDate);
                }
                else if (obj.searchKey != null && obj.searchKey != "")
                {
                    rec = ps.GetEntitiesByProjectId("GetEntitiesBySeachKey", obj.projectId, obj.userId,obj.searchKey);
                }
                else
                {
                    rec = ps.GetEntitiesByProjectId("GetAllEntities", obj.projectId, obj.userId);
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
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
        
        [Route("avx2/projects/entities/filters"), HttpPost]
        public HttpResponseMessage GetProjectEntitiyFilters(object Json)
        {
            try
            {
                PM_ProjectSitesBL ps = new PM_ProjectSitesBL();
                PM_ProjectEntityFilters_DTO rec = new PM_ProjectEntityFilters_DTO();
                ProjectApiObject obj = JsonConvert.DeserializeObject<ProjectApiObject>(Json.ToString());



                if (obj.projectId != null && obj.userId != null&& obj.projectId != 0 && obj.userId != 0)
                {
                    rec = ps.GetEntitiesFilters("EntitiesFilters", obj.projectId, obj.userId, null, obj.statusIds, obj.typeIds, obj.clientIds, obj.toDate, obj.fromDate);
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
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

    }
}
