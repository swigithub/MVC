using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
 public   class OrgChart
    {
        public text text { get; set; }
        public string image { get; set; }

        public Int64 Reporting { get; set; }
        public List<OrgChart> children { get; set; }




        //////////////

        public decimal RoleId { get; set; }
        public int Id { get; set; }

        public bool IsManager { get; set; }
        public Int64 UserId { get; set; }
        public DateTime HiringDate { get; set; }
        //  [Required, Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        // [Required,Display(Name ="User Name")]
        public string UserName { get; set; }
        //  [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public Int64? CompanyId { get; set; }
        // [Required, DataType(DataType.Password),Compare("Password", ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }

        public string Color { get; set; }

        public string ClientName { get; set; }
        public string Designation { get; set; }

        //  [Required,EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public bool ActiveStatus { get; set; }
        public bool IsAdmin { get; set; }
        public string Picture { get; set; }
        public string Remember_token { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Update_at { get; set; }
        public string IMEI { get; set; }
        public string MAC { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public bool Message { get; set; }
        public string DefaultUrl { get; set; }
        public int DaysForward { get; set; }
        public int DaysBack { get; set; }
        public double homeLatitude { get; set; }
        public double homeLongitude { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public Int64? ReportToId { get; set; }

        public string ReportTo { get; set; }
        public bool IsActive { get; set; }
    }
    public class Chart
    {
        public text text { get; set; }
        public string image { get; set; }

        public List<Chart> children { get; set; }


    }
    public  class text
    {
        public string name { get; set; }
        public string title { get; set; }

        public string contact { get; set; }
    }
}
