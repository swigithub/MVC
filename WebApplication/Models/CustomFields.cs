using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{

    public class MainClass
    {
       // public List<CustomFields> CustomNameList { get; set; }
        public List<FieldsClass> FieldsModelList { get; set; }
    }








    public class CustomFields
    {

        public string FieldName { get; set; }
        public string CustomName { get; set; }
        public string Key { get; set; }

    }
}