using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Generics
    {
        public DataColumnCollection ColumnsList { get; set; }

        public List<DataRow> RowsList { get; set; } = new List<DataRow>();


    }
}