using SWI.Libraries.AirView.Entities;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

namespace SWI.Libraries.Security.Entities
{
    /*----MoB!----*/
    public class Sec_User
    {
        public Sec_User()
        {
            Permissions = new List<Sec_Permission>();
        }

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
        public List<Sec_Permission> Permissions { get; set; }
        public List<AD_DefinationTypes> DefinationTypes { get; set; }
        public string Gender { get; set; }
        public long? ReportToId { get; set; }

        public string ReportTo { get; set; }
        public bool IsActive { get; set; }
    }




}