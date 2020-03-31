using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.Project.Model
{
   public class PM_CompanyHierarchy
    {
        //public PM_CompanyHierarchy()
        //{
        //    client = new List<Client>();
        //    role = new List<UserRoles>();
        //    users = new List<Users>();
        //}

        public Int64 ClientId { get; set; }
        public string ClientName { get; set; }
        public Int64 RoleId { get; set; }
        public string RoleName { get; set; }
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }




        //public List<Client> client { get; set; }
        //public List<UserRoles> role { get; set; }
        //public List<Users> users { get; set; }

    }
    public class Client
    {
        public Client()
        {
            userRolesList = new List<UserRoles>();
        }
        public Int64 ClientId { get; set; }
        public string ClientName { get; set; }
        public List<UserRoles> userRolesList { get; set; }
    }

    public class UserRoles
    {
        public UserRoles()
        {
            userList = new List<Users>();
        }
        public Int64? RoleId { get; set; }
        public string RoleName { get; set; }

        public List<Users> userList { get; set; }

    }

    public class Users
    {
        public Int64? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

    }



    //public class Client
    //    {
    //    public Int64? ClientId { get; set; }
    //    public string ClientName { get; set; }

    //}

    //public class UserRoles
    //{
    //    public Int64? RoleId { get; set; }
    //    public string RoleName { get; set; }

    //}

    //public class Users
    //{
    //    public Int64? UserId { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string UserName { get; set; }

    //}


}
