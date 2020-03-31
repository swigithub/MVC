using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SWI.Libraries.AirView.BLL
{
    /*----MoB!----*/
    public class ClientsBL
    {
        ClientsDL cd = new ClientsDL();
        
        public int Manage(string filter, AD_Clients cl)
        {
            return cd.Manage(filter, cl.ClientId, cl.ClientName, cl.IsActive,cl.Logo, cl.ClientTypeId, cl.PClientId, cl.ClientPrefix);
        }
        public AD_Clients Single(string filter, string value = null)
        {

            DataTable dt = cd.Get(filter, value);
            AD_Clients Client = new AD_Clients();

            if (dt.Rows.Count > 0)
            {
                Client.ClientId = int.Parse(dt.Rows[0]["ClientId"].ToString());
                Client.ClientName = dt.Rows[0]["ClientName"].ToString();
                Client.IsActive = bool.Parse(dt.Rows[0]["IsActive"].ToString());
            }

            return Client;
        }

        public AD_Clients Single1(string filter, string value = null)
        {
            try
            {
                DataTable dt = cd.Get(filter, value);
                AD_Clients Client = new AD_Clients();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 0;
                    Client.ClientId = int.Parse(dt.Rows[i]["ClientId"].ToString());
                    Client.ClientName = dt.Rows[i]["ClientName"].ToString();
                    Client.IsActive = bool.Parse(dt.Rows[i]["IsActive"].ToString());
                  //  Client.Logo = dt.Rows[i]["Logo"].ToString();
                    Client.ClientTypeId = (dt.Columns.Contains("ClientTypeId")) == true ? int.Parse(dt.Rows[i]["ClientTypeId"].ToString()) : 0;
                   // Client.PClientId = (dt.Columns.Contains("PClientId")) == true ? Int64.Parse(dt.Rows[i]["PClientId"].ToString()) : 0;
                    Client.ClientPrefix = dt.Rows[i]["ClientPrefix"].ToString();
                    return Client;
                }
                return null;
            }
            catch (Exception ex) {
                return null;
            }       
            
        }
        public List<AD_Clients> ToList(string filter, string value = null, string value1 = null)
        {

            DataTable dt = cd.Get(filter, value,value1);
            List<AD_Clients> Clients = dt.ToList<AD_Clients>();
            return Clients;
        }

        public List<SelectedList> SelectedList(string filter, string value = null, string Message = null)
        {
            SelectedList sl = new SelectedList();

            var rec = ToList(filter, value).Select(m => new SelectedList { Text = m.ClientName, Value = m.ClientId.ToString() }).ToList();
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }

            return rec;
        }

        public List<AD_Clients> Paging(int Skip, int Take, string Search, ref int TotalCount, string Id = null)
        {
            try
            {

                DataSet ds = cd.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search);
                DataTable Count = ds.Tables[1];
                if (Count != null && Count.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Count.Rows[0]["TotalRecord"].ToString());
                }

                return ds.Tables[0].ToList<AD_Clients>();

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ActiveDeactive(string filter, Int64 UserId = 0, string Value1 = null, string Value2 = null)
        {
            try
            {

                cd.Manage(filter, Convert.ToDecimal(Value1));
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
