using AirView.DBLayer.Security.Entities;
using SWI.Libraries.AD.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication.Services
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkGroupController : ApiController
    {

        [Route("swi/Groups/list"), HttpGet]
        public List<Sec_Workgroup> GetGroupsList()
        {

            try
            {
                AD_DefinationBL dbl = new AD_DefinationBL();
                return dbl.List_Work_Group("GetGroup").Select(m => new Sec_Workgroup { WorkgroupName = m.WorkgroupName, WorkgroupId = Convert.ToInt32(m.WorkgroupId) }).ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
