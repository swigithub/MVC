using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class AV_ScopeTestsBL
    {
        AV_ScopeTestsDL std = new AV_ScopeTestsDL();
        public AV_ScopeTests ToSingle(string filter, Int64 ClientId, Int64 CityId, Int64 NetworkModeId, Int64 ScopeId)
        {
            try
            {
                DataTable dt = std.Get(filter, ClientId, CityId, NetworkModeId, ScopeId);
                var ue = dt.ToList<AV_ScopeTests>();
                if (ue.Count > 0)
                {
                    return ue.FirstOrDefault();
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private AV_ScopeTests DataTableToObject(DataTable dt,int Row) {
            try
            {
                var obj = new AV_ScopeTests();

                obj.Test = (dt.Columns.Contains("DefinationName")) ? dt.Rows[Row]["DefinationName"].ToString() : null;
                obj.KeyCode = (dt.Columns.Contains("KeyCode")) ? dt.Rows[Row]["KeyCode"].ToString() : null;
                obj.DisplayText = (dt.Columns.Contains("DisplayText")) ? dt.Rows[Row]["DisplayText"].ToString() : null;

                return obj;
            }
            catch 
            {

                return null;
            }
            
        }


        public List<AV_ScopeTests> ToCustomList(string filter, Int64 ClientId, Int64 CityId, Int64 NetworkModeId, Int64 ScopeId)
        {
            try
            {

                DataTable dt = std.Get(filter, ClientId, CityId, NetworkModeId, ScopeId);
                List<AV_ScopeTests> lst = new List<AV_ScopeTests>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lst.Add(DataTableToObject(dt,i));

                    }
                }
                return lst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AV_ScopeTests> ToList(string filter, Int64 ClientId, Int64 CityId, Int64 NetworkModeId, Int64 ScopeId)
        {
            try
            {
                DataTable dt = std.Get(filter, ClientId, CityId, NetworkModeId, ScopeId);
                List<AV_ScopeTests> log = dt.ToList<AV_ScopeTests>();
                return log;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public bool Manage(string filter, AV_UEPbx ue)
        //{
        //    return std.Manage(filter, ue.UEId, ue.UEName, ue.IMEI, ue.IsIdle, ue.DeviceToken);
        //}
    }
}
