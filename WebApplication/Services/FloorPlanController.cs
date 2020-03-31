using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.AirView.Models;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Services
{
    public class FloorPlanController : ApiController
    {
        [System.Web.Http.Route("swi/Floor/GetById"), System.Web.Http.HttpPost]
        public Response GetById(Int64 SiteId)
        {
            Response res = new Response();

            try
            {
                FloorPlan_DL FloorDL = new FloorPlan_DL();
                DataTable Table = FloorDL.Get("GetById", SiteId);
                res.Value= Table.ToList<AV_FloorPlan>();
                res.Status = "success";
                res.Message = "success";
                return res;
            }
            catch(Exception ex)
            {
                res.Status = "error";
                res.Message = ex.Message;
                return res;
            }
            
        }
    }
}
