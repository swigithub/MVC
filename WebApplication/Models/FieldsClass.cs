using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FieldsClass
    {
        //public string MsgTag { get; set; }

        public string Field { get; set; }
        //public string Parent { get; set; }
        public string Key { get; set; }
        public string CustomName { get; set; }
        public string Parent { get; set; }

        public List<FieldsClass> Children { get; set; }


    }
}