using AirView.DBLayer.AirView.Entities;
using AirView.DBLayer.Project.Model;
using SWI.Libraries.Common;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
    public class Sec_UserBL
    {
        Sec_UserDL ud = new Sec_UserDL();
        DataType dtyp = new Common.DataType();
        DataType dtp = new DataType();
        public Sec_User Single(string filter, string Value1 = null, string Value2 = null, string Value3 = null)
        {
            try
            {

                DataTable dt = ud.Get(filter, Value1, Value2, Value3);

                Sec_User User = new Sec_User();
                if (dt != null && dt.Rows.Count > 0)
                {

                    int i = 0;
                    User.Id = DataType.ToInt32(dt.Rows[i]["UserId"].ToString());
                    User.UserId = (dt.Columns.Contains("UserId")) ? !(dt.Rows[i]["UserId"].ToString().Trim().ToLower() == "null" || dt.Rows[i]["UserId"].ToString().Trim() == String.Empty) ? Int64.Parse(dt.Rows[i]["UserId"].ToString().Replace(",", "")) : 0 : 0;
                    User.CompanyId= (dt.Columns.Contains("CompanyId")) ? !(dt.Rows[i]["CompanyId"].ToString().Trim().ToLower() == "null" || dt.Rows[i]["CompanyId"].ToString().Trim() == String.Empty) ? Int64.Parse(dt.Rows[i]["CompanyId"].ToString().Replace(",", "")) : 0 : 0;
                    User.ReportToId = (dt.Columns.Contains("ReportToId")) ? !(dt.Rows[i]["ReportToId"].ToString().Trim().ToLower() == "null" || dt.Rows[i]["ReportToId"].ToString().Trim() == String.Empty) ? Int64.Parse(dt.Rows[i]["ReportToId"].ToString().Replace(",", "")) : 0 : 0;
                    // User.UserId = (dt.Columns.Contains("UserId")) ?  Convert.ToInt64(dt.Rows[i]["UserId"].ToString()) : 0;
                    // User.CompanyId = (dt.Columns.Contains("CompanyId")) ? 1 * Convert.ToInt64(dt.Rows[i]["CompanyId"].ToString()) : 0;
                    // User.ReportToId = (dt.Columns.Contains("ReportToId")) ? 1 * Convert.ToInt64(dt.Rows[i]["ReportToId"].ToString()) : 0;
                    User.FirstName = dt.Rows[i]["FirstName"].ToString();

                    User.LastName = dt.Rows[i]["LastName"].ToString();
                    User.Email = dt.Rows[i]["Email"].ToString();
                    User.Picture = dt.Rows[i]["Picture"].ToString();
                    User.UserName = dt.Rows[i]["UserName"].ToString();
                    User.Password = dt.Rows[i]["Password"].ToString();
                    User.Address = dt.Rows[i]["Address"].ToString();
                    if ((dt.Columns.Contains("MAC")))
                    {
                        User.MAC = dt.Rows[i]["MAC"].ToString();
                    }
                    if ((dt.Columns.Contains("IMEI")))
                    {
                        User.IMEI = dt.Rows[i]["IMEI"].ToString();
                    }
                       
                    User.Designation = (dt.Columns.Contains("Designation")) ? dt.Rows[i]["Designation"].ToString() : "";
                    User.Gender = (dt.Columns.Contains("Gender")) ? dt.Rows[i]["Gender"].ToString() : "";
                    User.Title = (dt.Columns.Contains("Title")) == true ? dt.Rows[i]["Title"].ToString() : "";

                    User.Contact = dt.Rows[i]["Contact"].ToString();
                    User.IsAdmin = DataType.ToBoolean(dt.Rows[i]["IsAdmin"].ToString());
                    if ((dt.Columns.Contains("IsManager")))
                    {
                        User.IsManager = DataType.ToBoolean(dt.Rows[i]["IsManager"].ToString());
                    }
                    User.homeLatitude = (dt.Columns.Contains("homeLatitude")) == true ? DataType.ToDouble(dt.Rows[i]["homeLatitude"].ToString()) : 0;
                    User.homeLongitude = (dt.Columns.Contains("homeLongitude")) == true ? DataType.ToDouble(dt.Rows[i]["homeLongitude"].ToString() ): 0;

                    User.RoleId = (dt.Columns.Contains("RoleId")) == true ? int.Parse(dt.Rows[i]["RoleId"].ToString()) : 0;

                    User.RoleName = (dt.Columns.Contains("RoleName")) == true ? dt.Rows[i]["RoleName"].ToString() : "";
                    User.DefaultUrl = (dt.Columns.Contains("DefaultUrl")) == true ? dt.Rows[i]["DefaultUrl"].ToString() : "";
                    if ((dt.Columns.Contains("DaysBack")))
                    {
                        User.DaysBack = DataType.ToInt32(dt.Rows[i]["DaysBack"].ToString());
                    }
                    if ((dt.Columns.Contains("CompanyId")))
                    {
                        User.CompanyId = DataType.ToInt32(dt.Rows[i]["CompanyId"].ToString());
                    }
                    if ((dt.Columns.Contains("DaysForward")))
                    {
                        User.DaysForward = DataType.ToInt32(dt.Rows[i]["DaysForward"].ToString());
                    }
                    return User;

                }

                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private List<Summary> SummaryToList(DataTable dt)
        {

            List<Summary> lstSummary = new List<Summary>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Summary Sumry = new Summary();
                   //Sumry.UserId = DataType.ToInt32(dt.Rows[i]["UserId"].ToString());
                    Sumry.SiteCode = dt.Rows[i]["SiteCode"].ToString();
                    Sumry.Status = dt.Rows[i]["Status"].ToString();
                    Sumry.ScheduledOn = DateTime.Parse(dt.Rows[i]["ScheduledOn"].ToString());
                    Sumry.Color = dt.Rows[i]["Color"].ToString();
                    Sumry.DefinationName = dt.Rows[i]["DefinationName"].ToString();
                    Sumry.DefinationId =int.Parse(dt.Rows[i]["DefinationId"].ToString());
                    Sumry.PDefinationId= int.Parse(dt.Rows[i]["PDefinationId"].ToString());
                    Sumry.PDefinationName= dt.Rows[i]["PDefinationName"].ToString();

                    lstSummary.Add(Sumry);
                }
            }
            return lstSummary;
        }
        private List<Sec_User> DataTableToList(DataTable dt) {

            List<Sec_User> lstUsers = new List<Sec_User>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Sec_User User = new Sec_User();
                    User.UserId = DataType.ToInt64(dt.Rows[i]["UserId"].ToString());
                   // User.CompanyId = DataType.ToInt64(dt.Rows[i]["CompanyId"].ToString());
                    User.CompanyId = (dt.Columns.Contains("CompanyId")) ? DataType.ToInt64(dt.Rows[i]["CompanyId"].ToString()) : 0;
                    User.Id = DataType.ToInt32(dt.Rows[i]["UserId"].ToString());
                    User.FirstName = dt.Rows[i]["FirstName"].ToString();
                    User.LastName = dt.Rows[i]["LastName"].ToString();
                    if (dt.Columns.Contains("ModifyDate") && !string.IsNullOrEmpty(dt.Rows[i]["ModifyDate"].ToString()))
                    {
                        User.Update_at = DateTime.Parse(dt.Rows[i]["ModifyDate"].ToString());

                    }
                    User.Picture = dt.Columns.Contains("Picture") ? dt.Rows[i]["Picture"].ToString() : null;
                    User.UserName = dt.Columns.Contains("UserName") ? dt.Rows[i]["UserName"].ToString() : null;
                    User.Email = dt.Columns.Contains("Email") ? dt.Rows[i]["Email"].ToString() : null;
                    User.Address = dt.Columns.Contains("Address") ? dt.Rows[i]["Address"].ToString() : null;
                    User.Contact = dt.Columns.Contains("Contact") ? dt.Rows[i]["Contact"].ToString() : null;
                    User.ActiveStatus = dt.Columns.Contains("IsActive") ? bool.Parse(dt.Rows[i]["IsActive"].ToString()) : false;
                    User.RoleId = (dt.Columns.Contains("RoleId")) ? Convert.ToInt32(dt.Rows[i]["RoleId"].ToString()) : 0;
                    User.RoleName = (dt.Columns.Contains("RoleName")) ? dt.Rows[i]["RoleName"].ToString() : "";
                    User.ClientName = (dt.Columns.Contains("ClientName")) ? dt.Rows[i]["ClientName"].ToString() : "";
                    User.ReportTo = (dt.Columns.Contains("ReportTo")) ? dt.Rows[i]["ReportTo"].ToString() : "";
                    User.Designation = (dt.Columns.Contains("Designation")) ? dt.Rows[i]["Designation"].ToString() : "";
                    lstUsers.Add(User);
                }
            }
            return lstUsers;
        }

        private List<OrgChart> DataTableToList(DataTable dt,string name)
        {

            List<OrgChart> lstUsers = new List<OrgChart>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OrgChart User = new OrgChart();
                    User.UserId = DataType.ToInt64(dt.Rows[i]["UserId"].ToString());
                    User.CompanyId = DataType.ToInt64(dt.Rows[i]["CompanyId"].ToString());
                    User.ReportToId = DataType.ToInt64(dt.Rows[i]["ReportToId"].ToString());
                    User.Id = DataType.ToInt32(dt.Rows[i]["UserId"].ToString());
                    User.FirstName = dt.Rows[i]["FirstName"].ToString();
                    User.LastName = dt.Rows[i]["LastName"].ToString();
                    if (dt.Columns.Contains("ModifyDate") && !string.IsNullOrEmpty(dt.Rows[i]["ModifyDate"].ToString()))
                    {
                        User.Update_at = DateTime.Parse(dt.Rows[i]["ModifyDate"].ToString());

                    }
                    User.Picture = dt.Columns.Contains("Picture") ? dt.Rows[i]["Picture"].ToString() : null;
                    User.UserName = dt.Columns.Contains("UserName") ? dt.Rows[i]["UserName"].ToString() : null;
                    User.Email = dt.Columns.Contains("Email") ? dt.Rows[i]["Email"].ToString() : null;
                    User.Address = dt.Columns.Contains("Address") ? dt.Rows[i]["Address"].ToString() : null;
                    User.Contact = dt.Columns.Contains("Contact") ? dt.Rows[i]["Contact"].ToString() : null;
                    User.ActiveStatus = dt.Columns.Contains("IsActive") ? bool.Parse(dt.Rows[i]["IsActive"].ToString()) : false;
                    User.RoleId = (dt.Columns.Contains("RoleId")) ? Convert.ToInt32(dt.Rows[i]["RoleId"].ToString()) : 0;
                    User.RoleName = (dt.Columns.Contains("RoleName")) ? dt.Rows[i]["RoleName"].ToString() : "";
                    User.ClientName = (dt.Columns.Contains("ClientName")) ? dt.Rows[i]["ClientName"].ToString() : "";
                    User.ReportTo = (dt.Columns.Contains("ReportTo")) ? dt.Rows[i]["ReportTo"].ToString() : "";
                    User.Designation = (dt.Columns.Contains("Designation")) ? dt.Rows[i]["Designation"].ToString() : "";
                    lstUsers.Add(User);
                }
            }
            return lstUsers;
        }
        

        public List<Sec_User> ToList(string filter, string Value = null, string Value2 = null, string Value3 = null)
        {
            DataTable dt = ud.Get(filter, Value, Value2, Value3);
            return DataTableToList(dt);
        }
        public List<Summary> SummaryList(string filter, string Value = null, string Value2 = null, string Value3 = null)
        {
            DataTable dt = ud.Get(filter, Value, Value2, Value3);
            return SummaryToList(dt);
        }
        public List<PM_CompanyHierarchy> ToListNew(string filter, string Value = null, string Value2 = null, string Value3 = null)
        {
            DataTable dt = ud.Get(filter, Value, Value2, Value3);
            return myDataTable.ToList<PM_CompanyHierarchy>(dt);
        }

        public List<OrgChart> hierarchy(string filter, string Value = null, string Value2 = null, string Value3 = null)
        {
            DataTable dt = ud.Get(filter, Value, Value2, Value3);
            return DataTableToList(dt,"hierarchy");
        }

        public List<Sec_User> Paging(int Skip, int Take, string Search, ref int TotalCount,string Id=null)
        {
            try
            {

                DataSet ds = ud.GetDataSet("Paging", Skip.ToString(), Take.ToString(), Search,Id);
                if (ds.Tables.Count > 0)
                {
                    DataTable Count = ds.Tables[1];
                    if (Count != null && Count.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(Count.Rows[0]["TotalRecord"].ToString());
                    }

                    return DataTableToList(ds.Tables[0]);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<SelectedList> SelectedList(string filter, string Value = null, string Message = null)
        {
            SelectedList sl = new SelectedList();
            var rec= ToList(filter, Value).OrderBy(m => m.FirstName).Select(m => new SelectedList { Text = m.FirstName + " " + m.LastName, Value = m.UserId.ToString() }).ToList();
            if (!string.IsNullOrEmpty(Message))
            {
                sl.Text = Message;
                sl.Value = "0";
                
                rec.Add(sl);
                rec = rec.OrderBy(m => m.Value).ToList();
            }
            return rec;
        }


        public void UserAssinged_Testers_Devices(int UserId, ref List<Sec_User> Users, ref List<Sec_UserDevices> UserDevices)
        {
            try
            {
                DataSet ds = ud.GetDataSet("UserAssinged_Testers_Devices", UserId.ToString());
                List<Sec_User> lstUsers = new List<Sec_User>();
                Users = ds.Tables[0].ToList<Sec_User>();
                UserDevices = ds.Tables[1].ToList<Sec_UserDevices>();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Test()
        {
            DataTable dtbl = new DataTable();
            dtbl = ud.Get("byRoleId","1");
            List<Sec_User> newStudents = dtbl.ToList<Sec_User>();

        }
      

    }
}
