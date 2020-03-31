//using DatabaseSchemaReader;
using Newtonsoft.Json;
using Swi.Airview.Xcelerate.BusinessLogicLayer;
using Swi.Airview.Xcelerate.CoreConfiguration;
using Swi.Airview.Xcelerate.Data.Models;
using SWI.Security.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.BusinessIntelligence.Controllers
{
    [IsLogin(CheckPermission = false)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> getTemplate(int? id)
        {
            //const string providername = "System.Data.SqlClient";
            //const string connectionString = @"Data Source=WEB-SARFRAZ;Integrated Security=true;Initial Catalog=aircod_crm_DB;user id=alizain;password=1234567";
            //var dbReader = new DatabaseReader(connectionString, providername);
            //var schema = dbReader.ReadAll().Tables;

            ////foreach (var table in schema.Tables)
            ////{
            ////    Debug.WriteLine("Table " + table.Name);

            ////    foreach (var column in table.Columns)
            ////    {
            ////        Debug.Write("\tColumn " + column.Name + "\t" + column.DataType.TypeName);
            ////        if (column.DataType.IsString) Debug.Write("(" + column.Length + ")");
            ////        if (column.IsPrimaryKey) Debug.Write("\tPrimary key");
            ////        if (column.IsForeignKey) Debug.Write("\tForeign key to " + column.ForeignKeyTable.Name);
            ////        Debug.WriteLine("");
            ////    }
            ////    //Table Products
            ////    // Column ProductID int Primary key
            ////    // Column ProductName nvarchar(40)
            ////    // Column SupplierID int Foreign key to Suppliers
            ////    // Column CategoryID int Foreign key to Categories
            ////    // Column QuantityPerUnit nvarchar(20)
            ////    // Column UnitPrice money
            ////    // Column UnitsInStock smallint
            ////    // Column UnitsOnOrder smallint
            ////    // Column ReorderLevel smallint
            ////    // Column Discontinued bit
            ////}


            ////var obj = await BusinessLogic.instance.HandleTemplate.get();
           
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);

           // return Json(new { data = schema }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> getTemplateList()
        {
            var data = await DataAccess.Instance.HandleTemplateNodesAction.GetAll();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}
