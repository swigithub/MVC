using AirView.DBLayer.Security.DAL;
using SWI.Libraries.Security.Entities;
using System;
using System.Data;


namespace SWI.Libraries.Security.BLL
{
    /*----MoB!----*/
    public class UserDateRightsBL
    {

        UserDateRightsDL udrdl = new UserDateRightsDL();
        public UserDateRights Single(string filter,string value)
        {

            DataTable dt = udrdl.Get(filter, value);
            UserDateRights udr = new UserDateRights();

            if (dt.Rows.Count > 0)
            {
                udr.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                udr.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                udr.DaysForward = int.Parse(dt.Rows[0]["DaysForward"].ToString());
                udr.DaysBack = int.Parse(dt.Rows[0]["DaysBack"].ToString());
                udr.AssignDate = DateTime.Parse(dt.Rows[0]["AssignDate"].ToString());
                udr.IsActive = bool.Parse(dt.Rows[0]["IsActive"].ToString());
            }

            return udr;
        }


      
    }
}
