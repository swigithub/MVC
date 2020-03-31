using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirView_Parser;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Dynamic;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using MoreLinq;
using System.Collections;
using WebApplication.Models;
using System.Xml.Serialization;
using AirView.DBLayer.AirView.BLL;
using System.Text;
using SWI.Libraries.Security.Entities;
using SWI.Security.Filters;
using SWI.Libraries.Common;

namespace WebApplication.Controllers
{
    [IsLogin(CheckPermission = false)]
    public class ParserController : Controller
    {
        Dictionary<string, FieldsClass> dictionary = new Dictionary<string, FieldsClass>();
        Dictionary<string, int> Fieldscount = new Dictionary<string, int>();

        DataTable dt = new DataTable();
        bool OtherDataAvail = false;
        int childcount = 0;
        int inloopcount = 0;
        int innerloopcount = 0;
        AV_ParserBL bl = new AV_ParserBL();
        // GET: Parser
        //        Parser Model = new Parser();
        //List<AirView_Parser.Model.ParsedFile> ParsedModel=new List<AirView_Parser.Model.ParsedFile>();
        public ActionResult Parse()
        {
            Records._list = null;
            Records._listcolumns = null;
            dt = new DataTable();

            return View();
        }
        public PartialViewResult ParserView()
        {
            return PartialView();
        }
        public ActionResult Upload()
        {

            return View();
        }
        public void getparsedObj(object obj)
        {

            try
            {

                var objtype = obj.GetType().Name;

                if (objtype == "MsgException")
                {
                    AirView_Parser.Common.MsgException excmodel = (AirView_Parser.Common.MsgException)obj;
                    if (dictionary.ContainsKey(excmodel.classname))
                    {
                        DataRow ExcRow;
                        ExcRow = dt.NewRow();
                        ExcRow["Time"] = excmodel.Time;
                        ExcRow["Msg_Tag"] = excmodel.classname + "  " + "Parser Error";
                        dt.Rows.Add(ExcRow);
                    }
                }
                else
                {

                    if (dictionary.ContainsKey(objtype))
                    {


                        var FieldsValues = dictionary[objtype];
                        var ObjFieldsList = FieldsValues as FieldsClass;

                        DataRow workRow;
                        workRow = dt.NewRow();
                        if (objtype == "GPS")
                        {
                            FieldsClass fieldmodeltime = new FieldsClass();
                            fieldmodeltime.Field = "Time";
                            fieldmodeltime.Key = "1";
                            ObjFieldsList.Children.Add(fieldmodeltime);
                            traverseObj(obj, ObjFieldsList, ref workRow);
                            OtherDataAvail = true;
                        }
                        else
                        {
                            if (!ObjFieldsList.Children.Any(a => a.Field == "header"))
                            {
                                FieldsClass fieldmodel = new FieldsClass();
                                fieldmodel.Field = "header";
                                fieldmodel.Key = "1";
                                fieldmodel.Children = new List<FieldsClass>();
                                FieldsClass childtimemodel = new FieldsClass();
                                childtimemodel.Field = "Time";
                                childtimemodel.Key = "1";
                                fieldmodel.Children.Add(childtimemodel);

                                FieldsClass msgmodel = new FieldsClass();
                                msgmodel.Field = "Msg_Tag";
                                msgmodel.Key = "2";
                                fieldmodel.Children.Add(msgmodel);

                                if (ObjFieldsList.Children.Where(a => a.Children == null) != null && ObjFieldsList.Children.Where(a => a.Children == null).Count() > 0)
                                {

                                    foreach (var item in ObjFieldsList.Children.Where(a => a.Children == null))
                                    {
                                        if (item.Field == "Msg_Ver" || item.Field == "Instance_ID_count" || item.Field == "Network_Layer")
                                        {
                                            FieldsClass childs = new FieldsClass();
                                            childs.Field = item.Field;
                                            childs.Key = item.Key;
                                            childs.CustomName = item.CustomName;
                                            fieldmodel.Children.Add(childs);
                                        }

                                    }

                                }
                                ObjFieldsList.Children.RemoveAll(a => a.Field == "Msg_Ver" || a.Field == "Instance_ID_count" || a.Field == "Network_Layer");
                                ObjFieldsList.Children.Add(fieldmodel);
                            }

                            traverseObj(obj, ObjFieldsList, ref workRow);
                        }
                        if (OtherDataAvail)
                        {
                            workRow["Msg_Tag"] = objtype;
                            dt.Rows.Add(workRow);
                            Fieldscount.Clear();

                        }
                        OtherDataAvail = false;

                    }
                }
            }
            catch (Exception e)
            {


            }


        }
        //public void getparsedObj(object obj)
        //{
        //    try
        //    {

        //        var objtype = obj.GetType().Name;
        //        if (objtype == "MsgException")
        //        {
        //            AirView_Parser.Common.MsgException excmodel = (AirView_Parser.Common.MsgException)obj;
        //            if (dictionary.ContainsKey(excmodel.classname))
        //            {
        //                DataRow ExcRow;
        //                ExcRow = dt.NewRow();
        //                ExcRow["Time"] = excmodel.Time;
        //                ExcRow["Msg_Tag"] = excmodel.classname + "  " + "Parser Error";
        //                dt.Rows.Add(ExcRow);
        //            }
        //        }
        //        else
        //        {

        //            if (dictionary.ContainsKey(objtype))
        //            {



        //                var FieldsValues = dictionary[objtype];
        //                var ObjFieldsList = FieldsValues as FieldsClass;
        //                DataRow workRow;
        //                workRow = dt.NewRow();
        //                if (objtype == "GPS")
        //                {
        //                    FieldsClass fieldmodeltime = new FieldsClass();
        //                    fieldmodeltime.Field = "Time";
        //                    fieldmodeltime.Key = "1";
        //                    ObjFieldsList.Children.Add(fieldmodeltime);
        //                    traverseObj(obj, ObjFieldsList, ref workRow);
        //                    OtherDataAvail = true;
        //                }
        //                else
        //                {
        //                    if (ObjFieldsList.Children.Where(a => a.Children == null) != null 
        //                        && ObjFieldsList.Children.Where(a => a.Children == null).Count() > 0)
        //                    {
        //                        FieldsClass fieldmodel = new FieldsClass();
        //                        fieldmodel.Field = "header";
        //                        fieldmodel.Key = "1";
        //                        fieldmodel.Children = new List<FieldsClass>();
        //                        FieldsClass childtimemodel = new FieldsClass();
        //                        childtimemodel.Field = "Time";
        //                        childtimemodel.Key = "1";
        //                        fieldmodel.Children.Add(childtimemodel);

        //                        FieldsClass msgmodel = new FieldsClass();
        //                        msgmodel.Field = "Msg_Tag";
        //                        msgmodel.Key = "2";
        //                        fieldmodel.Children.Add(msgmodel);
        //                        foreach (var item in ObjFieldsList.Children.Where(a => a.Children == null))
        //                        {
        //                            if (item.Field == "Msg_Ver" || item.Field == "Instance_ID_count" || item.Field == "Network_Layer")
        //                            {
        //                                FieldsClass childs = new FieldsClass();
        //                                childs.Field = item.Field;
        //                                childs.Key = item.Key;
        //                                childs.CustomName = item.CustomName;
        //                                fieldmodel.Children.Add(childs);
        //                            }

        //                        }
        //                        ObjFieldsList.Children.RemoveAll(a => a.Field == "Msg_Ver" || a.Field == "Instance_ID_count" || a.Field == "Network_Layer");
        //                        ObjFieldsList.Children.Add(fieldmodel);
        //                    }
        //                    traverseObj(obj, ObjFieldsList, ref workRow);
        //                }
        //                if (OtherDataAvail)
        //                {
        //                    workRow["Msg_Tag"] = objtype;
        //                    dt.Rows.Add(workRow);
        //                }
        //                OtherDataAvail = false;

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {


        //    }


        //}

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    string _FileName = "";
        //    var path = "";

        //    try
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            path = Path.Combine(Server.MapPath("~/Files/"), DateTime.Now.Ticks + fileName);
        //            file.SaveAs(path);
        //            ViewBag.Message = "File Uploaded Successfully";

        //        }
        //        Session["FilePath"] = path;
        //    }
        //    catch (Exception e)
        //    {
        //        ViewBag.Message = e.Message;
        //    }
        //    return View("Parse");

        //}
        [HttpPost]
        public string GetFileFromWorkorder(FileGet model)
        {
            try
            {
                Response.Cookies["FilePathC"].Value = model.FilePath;
                Session["FilePath"] = model.FilePath;
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public ActionResult FilterFields(MainClass Model)
        {
            try
            {
               //Session["FilePath"] = @"C:\Users\ali.farhan\Downloads\rdoMobility_rdoCCW_BI-SSV-Test_Alpha_503_2020-03-26_21-08-45.avx";
                if (Session["FilePath"] == null)
                {
                    Session["FilePath"] =Request.Cookies["FilePathC"].Value.ToString();
                }
                string filepath = Session["FilePath"] != null ? Session["FilePath"].ToString() : "";
                dt = getData(Model, filepath);
                Parser P = new Parser(filepath, getparsedObj);
                P.Parse();
                //delete empty rows
                //int count = dt.Columns.Count;

                //for (int i = dt.Rows.Count - 1; i >= 0; i--)
                //{
                //    bool allNull = true;
                //    for (int j = 0; j < count; j++)
                //    {
                //        if (dt.Rows[i][j] != DBNull.Value)
                //        {
                //            allNull = false;
                //        }
                //    }
                //    if (allNull)
                //    {
                //        dt.Rows[i].Delete();
                //    }
                //}
                //dt.AcceptChanges();
                //string JSONString = string.Empty;
                //JSONString = JsonConvert.SerializeObject(dt);

                ////write string to file
                //System.IO.File.WriteAllText(@"D:\path" + DateTime.Now.Second + ".txt", JSONString);

            }
            catch (Exception e)
            {


            }
            Records._list = dt.AsEnumerable().ToList<DataRow>();
            Records._listcolumns = dt.Columns;
            Generics Mo = new Generics();
            Mo.ColumnsList = dt.Columns;
            Mo.RowsList = dt.AsEnumerable().Take(50).ToList();

            try
            {
                return PartialView(@"~\Views\Parser\_TableDate.cshtml", Mo);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public DataTable getData(MainClass Model, string filePath="")
        {
            dt = new DataTable();
            dictionary=new Dictionary<string, FieldsClass>();
                string colname = "Time";
                DataColumn column = new DataColumn();
                column.ColumnName = colname;
                column.DataType = typeof(object);
                dt.Columns.Add(column);
                string msgcolname = "Msg_Tag";
                DataColumn msgcolumn = new DataColumn();
                msgcolumn.ColumnName = msgcolname;
                msgcolumn.DataType = typeof(object);
                dt.Columns.Add(msgcolumn);
                ColumnCreate(Model.FieldsModelList, ref dt);
                foreach (var item in Model.FieldsModelList.DistinctBy(a => a.Field))
                {
                    dictionary.Add(item.Field, Model.FieldsModelList.Where(a => a.Field == item.Field).FirstOrDefault());

                }
                return dt;
        }

        public DataTable GetParserDataFromFile(MainClass Model,string FilePath)
        {
            getData(Model,FilePath);

            Parser P = new Parser(FilePath, getparsedObj);
            P.Parse();
            return dt;

        }
        public ActionResult GetFilteredFieldsData(MainClass Model,string FileList )
        {
            getData(Model);
            if (FileList != "")
            {
                var files = FileList.Split(',');
                foreach (var file in files)
                {
                    var path = Server.MapPath("~/Content" + file);
                    Parser P = new Parser(path, getparsedObj);
                    P.Parse();
                }
            }
            // For Export Data
            Records._list = dt.AsEnumerable().ToList<DataRow>();
            Records._listcolumns = dt.Columns;
            Generics Mo = new Generics();
            // For List
            Mo.ColumnsList = dt.Columns;
            Mo.RowsList = dt.AsEnumerable().Take(50).ToList();

            try
            {
                return PartialView(@"~\Views\Parser\_TableDate.cshtml", Mo);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public ActionResult SaveTemplate(MainClass Model, string tempname, List<string> keys)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(Model);
            var user = Session["user"] as LoginInformation;
            string keystring = string.Join(",", keys.ToArray());
            var IsInserted = bl.Insert("Insert", jsonData, tempname, keystring, (int)user.UserId);
            return Json(IsInserted, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetTemplates(int TemplateId = 0)
        {
            var user = Session["user"] as LoginInformation;
            var Filter = TemplateId == 0 ? "GetTemplates" : "GetTemplateById";
            var res = bl.Get(Filter, TemplateId, (int)user.UserId);
            dynamic result;
            if (TemplateId == 0)
            {
                result = res.ToList<Templates>();
            }
            else
            {
                if (res.Rows.Count > 0)
                {
                    result = (res.Rows[0]["Keys"].ToString()).Split(',');
                }
                else
                {
                    result = "";
                }
            }
            return Json(result);
        }
        [HttpPost]
        public ActionResult DeleteTemplate(int TemplateId)
        {
            var Filter = "DeleteTemplateById";
            var res = bl.Get(Filter, TemplateId);
            return GetTemplates();
        }
        //public List<string> GetKeys(dynamic result,List<string> keys)
        //{
        //    foreach (var item in result)
        //    {
        //        keys.Add(item.Key);
        //        if (item.Children!=null && item.Children.Count > 0)
        //        {
        //            GetKeys(item.Children, keys);
        //        }
        //    }
        //    return keys;
        //}


        public ActionResult ExportToExcel()
        {

            string attachment = "attachment; filename=ParserExport " + DateTime.Now.Ticks + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in Records._listcolumns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in Records._list)
            {
                tab = "";
                for (i = 0; i < Records._listcolumns.Count; i++)
                {

                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return null;

        }

        public ActionResult ExportToCsv()
        {
            string fileName = "ParserExport " + DateTime.Now.Ticks + ".csv";
            string delimiter = ",";
            //prepare the output stream
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AppendHeader("Content-Disposition",
                string.Format("attachment; filename={0}", fileName));

            string value = "";
            StringBuilder builder = new StringBuilder();

            //write the csv column headers
            for (int i = 0; i < Records._listcolumns.Count; i++)
            {

                value = Records._listcolumns[i].ColumnName;
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                {
                    builder.Append(value);

                }

                Response.Write(value);
                Response.Write((i < Records._listcolumns.Count - 1) ? delimiter : Environment.NewLine);
                builder.Clear();
            }

            //write the data
            foreach (DataRow row in Records._list)
            {
                for (int i = 0; i < Records._listcolumns.Count; i++)
                {
                    value = row[i].ToString();
                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes

                    if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                    {
                        builder.Append(value);

                    }

                    Response.Write(builder.ToString());
                    Response.Write((i < Records._listcolumns.Count - 1) ? delimiter : Environment.NewLine);
                    builder.Clear();
                }
            }

            Response.End();
            return null;
        }
        public ActionResult GetNextRecords(int pageindex)
        {
            Generics Mo = new Generics();
            Mo.RowsList = Records._list.Skip(pageindex * 50).Take(50).ToList();
            Mo.ColumnsList = Records._listcolumns;
            string html = renderPartialViewtostring(@"~\Views\Parser\RecordsList.cshtml", Mo);

            return Json(html, JsonRequestBehavior.AllowGet);

        }
        protected string renderPartialViewtostring(string Viewname, object model)
        {
            if (string.IsNullOrEmpty(Viewname))

                Viewname = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewresult = ViewEngines.Engines.FindPartialView(ControllerContext, Viewname);
                ViewContext viewcontext = new ViewContext(ControllerContext, viewresult.View, ViewData, TempData, sw);
                viewresult.View.Render(viewcontext, sw);
                return sw.GetStringBuilder().ToString();
            }

        }

        private void ColumnCreate(List<FieldsClass> List, ref DataTable dt)
        {
            try
            {
                List<FieldsClass> Parentlist = new List<FieldsClass>();
                string colname = "";

                foreach (var item in List)
                {

                    var checkparentresult = CheckParent(item, ref dt);
                    if (!checkparentresult)
                    {

                        Parentlist.Add(item);
                    }



                }
                //foreach (var pr in Parentlist)
                //{
                //    //pr.Children.ForEach(a => { a.Children.ForEach(s => { s.Field = a.Field + "." + s.Field; }); });
                //    ColumnCreate(pr.Children.OrderBy(a => a.Key).ToList(), ref dt);
                //}
            }

            catch (Exception e)
            {


            }
        }

        public void GetPrefixTag()
        {
            FieldsClass Field = new FieldsClass();
            string prefix = "";
            while (Field.Children != null)
            {
                prefix = prefix + Field.Field;


            }


        }

        public bool CheckParent(FieldsClass Model, ref DataTable dt)
        {
            if (Model.Children != null && Model.Children.Count() > 0)
            {
                ColumnCreate(Model.Children, ref dt);
                return false;
            }
            else
            {
                string customname = "";//Model.CustomName;
                string fieldname = Model.Field;

                string colname = string.IsNullOrEmpty(customname) ? fieldname : customname;
                DataColumnCollection columns = dt.Columns;
                if (!columns.Contains(colname))
                {
                    DataColumn dtcolumn = new DataColumn();
                    dtcolumn.ColumnName = colname;
                    dtcolumn.DataType = typeof(object);
                    dt.Columns.Add(dtcolumn);
                }
                return true;
            }

        }

        public void traverseObj(object Parseobj, FieldsClass list, ref DataRow row)
        {
            try
            {
                var fieldarr = list.Children.OrderBy(o => o.Key).Select(a => a.Field).ToArray();
                var FilterProps = Projection(Parseobj, fieldarr);
                var KeyValProps = ((IDictionary<String, Object>)FilterProps);
                // var Childs = ObjFieldsArr.Where(a => a.Children != null).Select(a => a.Field).ToArray();
                int count = 0;

                foreach (var kv in KeyValProps)
                {

                    string Propname = kv.Key;
                    var Propvalue = kv.Value;
                    if (Propvalue != null)
                    {
                        if (Propvalue is IList && !(Propvalue is IList<string>))
                        {
                            var list_type_val = listTraverse(Propvalue, list.Children.Where(a => a.Field == kv.Key).FirstOrDefault(), ref row,0);
                        }
                        else if (Propvalue is object && !(Propvalue is string) && !(Propvalue is IList<string>))
                        {
                            //var objarrs = list.Where(a => a.Children != null && a.Children.Parent == Propname).Select(a => a.Children.Field).ToArray();
                            //var objfieldarr = list.Where(a => a.Children != null && a.Parent == Propname).ToArray();
                            if (Propname != "header")
                            {
                                OtherDataAvail = true;
                            }
                            traverseObj(kv.Value, list.Children.Where(a => a.Field == kv.Key).FirstOrDefault(), ref row);
                        }
                        else if (Propvalue is IList<string>)
                        {
                            var stringlist = Propvalue as List<string>;
                            bool firstentry = true;
                            int indcount = 1;



                            foreach (var item in stringlist)
                            {
                                //innerloopcount = stringlist.Count;
                                if (firstentry)
                                {
                                    row[kv.Key] = item;
                                    firstentry = false;
                                }
                                else
                                {
                                    if (Fieldscount.ContainsKey(kv.Key))
                                    {
                                        int previousval = Fieldscount[kv.Key];
                                        Fieldscount[kv.Key] = previousval + 1;


                                    }
                                    else
                                    {
                                        Fieldscount.Add(kv.Key, indcount);
                                    }
                                    indcount = Fieldscount[kv.Key];

                                    CheckAndCreateColumn(kv.Key, indcount, ref row, stringlist.Count, "0");

                                    row[kv.Key + "." + indcount] = item;
                                    ++indcount;

                                }



                            }

                            //var stringlist = Propvalue as List<string>;
                            //bool firstentry = true;
                            //int indcount = 1;
                            //foreach (var item in stringlist)
                            //{
                            //    innerloopcount = stringlist.Count;
                            //    if (firstentry)
                            //    {
                            //        row[kv.Key] = item;
                            //        firstentry = false;
                            //    }
                            //    else
                            //    {
                            //        CheckAndCreateColumn(kv.Key, indcount, ref row, stringlist.Count,"1");

                            //        row[kv.Key + "." + indcount] = item;
                            //        ++indcount;

                            //    }



                            //}


                        }
                        else
                        {

                            var customname = ""; //list.Children.Where(a => a.Field == kv.Key).FirstOrDefault().CustomName != null ? list.Children.Where(a => a.Field == kv.Key).FirstOrDefault().CustomName : "";
                            string colname = string.IsNullOrEmpty(customname) ? kv.Key : customname;
                            if (colname != "Msg_Tag")
                            {
                                row[colname] = kv.Value;
                            }
                        }
                    }
                    if(Propname!="Time" && Propname!= "Msg_Ver" && Propname != "Instance_ID_count" && Propname != "Instance_IDs" && Propname != "Network_Layer")
                    {
                        OtherDataAvail = true;

                    }
                        



                }
            }
            catch (Exception e)
            {


            }

        }
        public dynamic listTraverse(dynamic li_type_obj, FieldsClass list, ref DataRow row, int colcount = 0, bool simplelist = true)
        {
            try
            {
                bool modifycolname = false;

                foreach (var obj in li_type_obj as IList)
                {
                    if (colcount > 0)
                    {
                        modifycolname = true;

                    }
                    List<string> fields = new List<string>();
                    if (list.Children != null && list.Children.Count > 0)
                    {

                        fields = list.Children.OrderBy(k => k.Key).Select(a => a.Field).ToList();

                    }
                    else
                    {
                        fields.Add(list.Field);

                    }
                    var li_val_props = Projection(obj, fields);
                    var KeyValProps = ((IDictionary<String, Object>)li_val_props);
                    foreach (var pro in KeyValProps)
                    {
                        if (pro.Value != null)
                        {
                            if (pro.Value is IList && !(pro.Value is IList<string>))
                            {
                                listTraverse(pro.Value, list.Children.Where(a => a.Field == pro.Key).FirstOrDefault(), ref row,colcount,false);
                                simplelist = false;
                            }
                            else if (pro.Value is object && !(pro.Value is string) && !(pro.Value is IList<string>))
                            {

                                traverseObj(pro.Value, list.Children.Where(a => a.Field == pro.Key).FirstOrDefault(), ref row);
                            }
                            else
                            {
                                var customname = ""; /*list.Children.Where(a => a.Field == pro.Key).FirstOrDefault().CustomName != null ? list.Children.Where(a => a.Field == pro.Key).FirstOrDefault().CustomName : "";*/
                                string colname = string.IsNullOrEmpty(customname) ? pro.Key : customname;
                                var value = pro.Value;
             
                                if (pro.Value is IList<string>)
                                {
                                    var stringlist = pro.Value as List<string>;
                                    bool firstentry = true;
                                    int indcount = 1;
                                    simplelist = false;
                                  
                                         
                                    
                                    foreach (var item in stringlist)
                                    {
                                        //innerloopcount = stringlist.Count;
                                        if (firstentry && colcount==0)
                                        {
                                            row[colname] = item;
                                            firstentry = false;
                                        }
                                        else
                                        {
                                            if (Fieldscount.ContainsKey(colname))
                                            {
                                                int previousval = Fieldscount[colname];
                                                Fieldscount[colname] = previousval+1;
                                                

                                            }
                                            else
                                            {
                                                Fieldscount.Add(colname, indcount);
                                            }
                                            indcount = Fieldscount[colname];
                                            string poscheck = "0";
                                            if (!simplelist)
                                            {
                                                poscheck = "1";
                                            }
                                            CheckAndCreateColumn(colname, indcount, ref row, stringlist.Count, poscheck);

                                            row[colname + "." + indcount] = item;
                                            ++indcount;

                                        }



                                    }
                                    //innerloopcount = stringlist.Count + innerloopcount;
                                    //value = string.Join(",", stringlist).ToString();

                                }
                                else
                                {

                                    var checkchild = childrencount(list, ref childcount, ref inloopcount);



                                    childcount = 0;
                                        inloopcount = 0;

                                        
                                    if (colcount>0)
                                    {
                                        string poscheck = "0";
                                        if(!simplelist)
                                        {
                                            poscheck = "1";
                                        }
                                        var created = CheckAndCreateColumn(colname, colcount, ref row, list.Children.Count() + checkchild, poscheck/*+innerloopcount-1*/);
                                       
                                            row[colname + "." + colcount] = value;
                                        
                                    }
                                    else
                                    {


                                        row[colname] = value;
                                    }
                                    

                                }

                            }
                        }

                    }

                    //dt.Rows.Add(row);
                    //DataRow newrow = dt.NewRow();
                    //row = newrow;
                    ++colcount;
                }
            }
            catch (Exception e)
            {


            }

            return null;

        }


        //for traversing the list type properties
        //for traversing the list type properties
        //public dynamic listTraverse(dynamic li_type_obj, FieldsClass list, ref DataRow row)
        //{
        //    try
        //    {
        //        int colcount = 0;
        //        bool modifycolname = false;

        //        foreach (var obj in li_type_obj as IList)
        //        {
        //            if (colcount > 0)
        //            {
        //                modifycolname = true;

        //            }
        //            List<string> fields = new List<string>();
        //            if (list.Children != null && list.Children.Count > 0)
        //            {

        //                fields = list.Children.OrderBy(k => k.Key).Select(a => a.Field).ToList();

        //            }
        //            else
        //            {
        //                fields.Add(list.Field);

        //            }
        //            var li_val_props = Projection(obj, fields);
        //            var KeyValProps = ((IDictionary<String, Object>)li_val_props);
        //            foreach (var pro in KeyValProps)
        //            {
        //                if (pro.Value != null)
        //                {
        //                    if (pro.Value is IList && !(pro.Value is IList<string>))
        //                    {
        //                        listTraverse(pro.Value, list.Children.Where(a => a.Field == pro.Key).FirstOrDefault(), ref row);

        //                    }
        //                    else if (pro.Value is object && !(pro.Value is string) && !(pro.Value is IList<string>))
        //                    {

        //                        traverseObj(pro.Value, list.Children.Where(a => a.Field == pro.Key).FirstOrDefault(), ref row);
        //                    }
        //                    else
        //                    {
        //                        var customname = ""; /*list.Children.Where(a => a.Field == pro.Key).FirstOrDefault().CustomName != null ? list.Children.Where(a => a.Field == pro.Key).FirstOrDefault().CustomName : "";*/
        //                        string colname = string.IsNullOrEmpty(customname) ? pro.Key : customname;
        //                        var value = pro.Value;
        //                        if (pro.Value is IList<string>)
        //                        {
        //                            var stringlist = pro.Value as List<string>;
        //                            bool firstentry = true;
        //                            int indcount = 1;
        //                            foreach (var item in stringlist)
        //                            {
        //                                innerloopcount = stringlist.Count;
        //                                if (firstentry)
        //                                {
        //                                    row[colname] = item;
        //                                    firstentry = false;
        //                                }
        //                                else
        //                                {
        //                                    CheckAndCreateColumn(colname, indcount, ref row, stringlist.Count);

        //                                    row[colname + "." + indcount] = item;
        //                                    ++indcount;

        //                                }



        //                            }
        //                            value = string.Join(",", stringlist).ToString();

        //                        }
        //                        else
        //                        {
        //                            if (modifycolname)
        //                            {

        //                                var checkchild = childrencount(list, ref childcount, ref inloopcount);
        //                                childcount = 0;
        //                                inloopcount = 0;

        //                                CheckAndCreateColumn(colname, colcount, ref row, list.Children.Count() + checkchild/*+innerloopcount-1*/);
        //                                innerloopcount = 0;
        //                                row[colname + "." + colcount] = value;
        //                            }
        //                            else
        //                            {
        //                                row[colname] = value;
        //                            }

        //                        }

        //                    }
        //                }

        //            }

        //            //dt.Rows.Add(row);
        //            //DataRow newrow = dt.NewRow();
        //            //row = newrow;
        //            ++colcount;
        //        }
        //    }
        //    catch (Exception e)
        //    {


        //    }

        //    return null;

        //}

        private static dynamic Projection(object a, IEnumerable<string> props)
        {
            if (a == null)
            {
                return null;
            }
            IDictionary<string, object> res = new ExpandoObject();
            var type = a.GetType();
            try
            {
                foreach (var pair in props.Select(n => new
                {
                    Name = n,
                    Property = type.GetProperty(n)
                }))
                {
                    res[pair.Name] = pair.Property.GetValue(a, new object[0]);
                }
            }

            catch(Exception e)
            {


            }

            return res;

        }
        public bool CheckAndCreateColumn(string colname, int count, ref DataRow row, int addincrement, string type)
        {


            string newcolname = colname + "." + count;
            if (count > 1)
            {
                colname = colname + "." + (count - 1);

            }
            DataColumnCollection columns = dt.Columns;
            if (!columns.Contains(newcolname))
            {
                try
                {
                    DataColumn dtcolumn = new DataColumn();
                    dtcolumn.ColumnName = newcolname;
                    dtcolumn.DataType = typeof(object);

                    dt.Columns.Add(dtcolumn);
                    int colposition = GetColumnPosition(colname, ref row) + 1;
                    if (colposition < columns.Count)
                    {
                        if (type != "1")
                        {
                            dtcolumn.SetOrdinal(colposition);
                        }
                    }
                }
                catch (Exception e)
                {

                }
                return true;
            }
            else
            {
                return false;
            }



        }
        //public bool CheckAndCreateColumn(string colname, int count, ref DataRow row, int addincrement)
        //{
        //    string newcolname = colname + "." + count;
        //    if (count > 1)
        //    {
        //        colname = colname + "." + (count - 1);

        //    }
        //    DataColumnCollection columns = dt.Columns;
        //    if (!columns.Contains(newcolname))
        //    {
        //        try
        //        {
        //            DataColumn dtcolumn = new DataColumn();
        //            dtcolumn.ColumnName = newcolname;
        //            dtcolumn.DataType = typeof(object);

        //            dt.Columns.Add(dtcolumn);
        //            int colposition = GetColumnPosition(colname, ref row) + addincrement;
        //            if (colposition < columns.Count)
        //            {
        //                dtcolumn.SetOrdinal(colposition);
        //            }
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }



        //}
        public int GetColumnPosition(string colname, ref DataRow row)
        {

            int colindex = row.Table.Columns[colname].Ordinal;
            return colindex;


        }

        public int childrencount(FieldsClass Model, ref int childcount, ref int inloopcount)
        {
            if (Model.Children != null && Model.Children.Count > 0)
            {
                foreach (var item in Model.Children)
                {
                    if (item.Children != null && item.Children.Count > 0)
                    {
                        ++inloopcount;
                        childcount = childcount + item.Children.Count;

                        childrencount(item, ref childcount, ref inloopcount);

                    }
                }
            }
            return childcount - inloopcount;
        }


    }
    public class FileGet
    {
        public string FilePath { get; set; }
    }

    public class Templates
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
    }


}