using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
   public class Sec_UserDevicesBL
    {
        Sec_UserDevicesDL udDL = new Sec_UserDevicesDL();
        public bool Manage(string filter, Sec_UserDevices dev) {
            try
            {
                return udDL.Manage(filter, dev.DeviceId, dev.UserId, dev.IMEI, dev.MAC, dev.Manufacturer, dev.Model,dev.IsActive, dev.Password,dev.TranferToId);

            }
            catch{throw;}
        }

        public Sec_UserDevices Single(string filter, string Value = null)
        {

            DataTable dt = udDL.Get(filter, Value);
            Sec_UserDevices ud = new Sec_UserDevices();

            if (dt.Rows.Count > 0)
            {
                ud.DeviceId = int.Parse(dt.Rows[0]["DeviceId"].ToString());
                ud.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                ud.IMEI = (dt.Columns.Contains("IMEI"))? dt.Rows[0]["IMEI"].ToString():null;
                ud.IMEI = (dt.Columns.Contains("SerialNo"))? dt.Rows[0]["SerialNo"].ToString():null;
                ud.MAC = dt.Rows[0]["MAC"].ToString();
                ud.Manufacturer = dt.Rows[0]["Manufacturer"].ToString();
                ud.Model = dt.Rows[0]["Model"].ToString();
                ud.UserFullName = (dt.Columns.Contains("UserFullName")) ? dt.Rows[0]["UserFullName"].ToString() : "";
               
            }

            return ud;
        }
        public List<Sec_UserDevices> ToList(string filter, string Value = null)
        {

            DataTable dt =  udDL.Get(filter, Value);
            List<Sec_UserDevices> udl = new List<Sec_UserDevices>();
            if (dt.Rows.Count > 0)
            {               

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Sec_UserDevices ud = new Sec_UserDevices();

                    ud.DeviceId = (dt.Columns.Contains("DeviceId"))? int.Parse(dt.Rows[i]["DeviceId"].ToString()):0;
                    if (ud.DeviceId==0)
                    {
                        if (dt.Rows[i]["UEId"].ToString() != "")
                        {
                            ud.DeviceId = (dt.Columns.Contains("UEId")) ? int.Parse(dt.Rows[i]["UEId"].ToString()) : 0;
                        }
                        else
                        {
                            ud.DeviceId = 0;
                        }
                        
                    }
                    ud.UserId = (dt.Columns.Contains("UserId")) ? int.Parse(dt.Rows[i]["UserId"].ToString()):0;
                    ud.IMEI = (dt.Columns.Contains("IMEI")) ? dt.Rows[i]["IMEI"].ToString():null;
                    if (ud.IMEI==null)
                    {
                        ud.IMEI = (dt.Columns.Contains("SerialNo")) ? dt.Rows[i]["SerialNo"].ToString() : null;

                    }
                    ud.MAC = dt.Rows[i]["MAC"].ToString();
                    ud.Manufacturer = dt.Rows[i]["Manufacturer"].ToString();
                    ud.Model = dt.Rows[i]["Model"].ToString();
                    ud.IsActive = (dt.Columns.Contains("isActive")) ? bool.Parse( dt.Rows[i]["isActive"].ToString()):false;
                    ud.UserFullName = (dt.Columns.Contains("UserFullName")) ? dt.Rows[i]["UserFullName"].ToString() : "";
                    udl.Add(ud);
                }          
            }

            return udl;
        }


     

        public List<SelectedList> SelectedList(string filter, string value = null, string Message = null,string Return=null)
        {
            SelectedList sl = new SelectedList();
            List<SelectedList> rec = new List<Common.SelectedList>();
            if (Return=="Devices")
            {
                rec = ToList(filter, value).Select(m => new SelectedList { Text = m.Model+" "+m.IMEI, Value = m.DeviceId.ToString() }).ToList();
            }
            else
            {
                var result = ToList(filter, value).GroupBy(test => test.UserFullName)
                .Select(grp => grp.First())
                .ToList();

                rec = result.Select(m => new SelectedList { Text = m.UserFullName, Value = m.UserId.ToString() }).ToList();
            }
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }

            return rec;
        }
    }
}
