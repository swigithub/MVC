using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SWI.Security.Filters;
using SWI.Libraries.Security.DAL;
using SWI.Libraries.Security.BLL;
using SWI.Libraries.Security.Entities;
using SWI.Libraries.AirView.BLL;
using SWI.Libraries.AirView.DAL;
using SWI.Libraries.AD.BLL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using AirView.DBLayer.Security.DAL;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/
    [IsLogin, ErrorHandling]
    public class PermissionController : Controller
    {
        // GET: Permission
        Permission p = new Permission();

        public ActionResult New() {
            Sec_PermissionBL pl = new Sec_PermissionBL();
            ViewBag.LastId = pl.GetLastId() + 1;
            NewData();
            ViewBag.Filter = "Insert";

            return View();
        }

        private void NewData() {

            AD_DefinationBL db = new AD_DefinationBL();
            ViewBag.Modules = db.SelectedList("byDefinationType", "Module", "-Module-");
        }

        public ActionResult Edit(int id)
        {
            Sec_PermissionBL pl = new Sec_PermissionBL();
            ViewBag.LastId = id;
            NewData();
            var rec = pl.Single("ById", id.ToString());

            ViewBag.ModuleId = (rec != null) ? rec.ModuleId : 0;
            ViewBag.Filter = "Update";

            return View("New", rec);
        }



        [HttpPost]

        public ActionResult New(Sec_Permission p,string Filter)
        {
            try
            {
                Sec_PermissionBL pb = new Sec_PermissionBL();
                bool res = pb.Manage(Filter,p);
                if (res)
                {
                    TempData["msg_success"] = "save successfully";
                }
                return RedirectToAction("all");
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
                return View();
            }
        }

        public ActionResult All()
        {
            Sec_PermissionBL pl = new Sec_PermissionBL();
            return View(pl.ToList("All"));
        }


        [HttpPost,IsLogin(CheckPermission =false)]
        public ActionResult Paging(int current, int rowCount, string searchPhrase)
        {
            Sec_PermissionBL pb = new Sec_PermissionBL();

            current = (current == 0) ? 1 : current;
            rowCount = (rowCount == 0) ? 5 : rowCount;

            int offset = (current - 1) * rowCount;
            int TotalRecord = 0;
            var rec = pb.Paging(offset, rowCount, searchPhrase, ref TotalRecord);

            return Json(new { current = current, total = TotalRecord, rows = rec, rowCount = rowCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost,IsLogin(CheckPermission = false)]
        public bool ManageStatus(int Id,bool status,string type)
        {
            try
            {
                Sec_PermissionBL pb = new Sec_PermissionBL();
                Sec_Permission p = new Sec_Permission();
                p.Id = Id;
                if (type == "set_IsMenuItem")
                {
                    p.IsMenuItem = status;
                }else if (type == "set_IsUsed")
                {
                    p.IsUsed = status;
                }

                bool res = pb.Manage(type, p);
                return res;
            }
            catch (Exception)
            {

                return false;
            }
            
        }


        public ActionResult Delete(int Id)
        {
            Sec_PermissionBL pb = new Sec_PermissionBL();
            Sec_Permission p = new Sec_Permission();
            p.Id = Id;
            pb.Manage("DeleteById", p);
            
            return RedirectToAction("all");
        }
        //public ActionResult Install()
        //{
        //    //InsertPermissions("AirView", "SWI.AirView.Controllers");
        //    //InsertPermissions("Security", "SWI.Security.Controllers");
        //    ViewBag.Actions = new Func<string, List<string>>(ActionNames);
        //    ViewBag.GetControllers = new Func<string, List<string>>(GetControllers);
        //    List<string> Controllers = new List<string>();

        //    Controllers.Add("AirView");
        //    Controllers.Add("Security");

        //  // Controllers.AddRange(GetControllers("SWI.AirView.Controllers"));
        //  //Controllers.AddRange(GetControllers("SWI.Security.Controllers"));
        //    ViewBag.Controllers = Controllers;
        //    return View();
        //}


        [HttpPost]
        public ActionResult Install(int[] Id,int[] ParentId,string[] Title,string[] URL,string[] Code,string[] Icon,string[] IsMenuItem,string[] IsUsed)
        {
            try
            {
                DataTable Permissions = new DataTable();
                Permissions.Columns.AddRange(new DataColumn[8]
                {
                                    new DataColumn("Id", typeof (int)),
                                    new DataColumn("ParentId", typeof (int)),
                                    new DataColumn("Title", typeof (string)),
                                    new DataColumn("URL", typeof (string)),
                                    new DataColumn("Code", typeof (string)),
                                    new DataColumn("Icon", typeof (string)),
                                    new DataColumn("IsMenuItem", typeof (bool)),
                                    new DataColumn("IsUsed", typeof (bool)),

                });

                for (int i = 0; i < Id.Length; i++)
                {
                    DataRow row;
                    row = Permissions.NewRow();
                    row["Id"] = Id[i];
                    row["ParentId"] = ParentId[i];
                    row["Title"] = Title[i];
                    row["URL"] = URL[i];
                    row["Code"] = Code[i];
                    row["Icon"] = Icon[i];
                    row["IsMenuItem"] = (IsMenuItem.Contains("mnu-" + Id[i])) == true ? true : false;
                    if (IsUsed!=null)
                    {
                        row["IsUsed"] = (IsUsed.Contains("no-" + Id[i])) == true ? false : true;

                    }

                    Permissions.Rows.Add(row);
                }

                Sec_PermissionDL pd = new Sec_PermissionDL();
                pd.Save(Permissions);
                TempData["msg_success"]="Save successfully";
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
            }
            


            return RedirectToAction("all");
        }

        private bool InsertPermissions(string Module,string Namespace) {
            List<string> Controllers = new List<string>();
            Controllers = GetControllers(Namespace);


            DataTable Permissions = new DataTable();
            Permissions.Columns.AddRange(new DataColumn[6]
            {
                                    new DataColumn("Id", typeof (int)),
                                    new DataColumn("ParentId", typeof (int)),
                                    new DataColumn("Title", typeof (string)),
                                    new DataColumn("URL", typeof (string)),
                                    new DataColumn("Code", typeof (string)),
                                    new DataColumn("In_menu", typeof (bool)),

            });
            Sec_PermissionBL pl = new Sec_PermissionBL();
            int Id = pl.GetLastId() + 1;
            int tempParentId = 0, ModuleId = 1;
            ModuleId = Id;


            PermissionDataRow(Permissions, Id, 0, Module, "#", null);


            foreach (string Cont in Controllers)
            {
                var Actions = ActionNames(Cont);
                bool Menu = true;
                if (p.InMenuExist(Cont))
                {
                    Menu = false;
                }

                if (Menu)
                {
                    Id++; tempParentId = Id;

                    if (Actions.Count > 1)
                    {
                        PermissionDataRow(Permissions, Id, ModuleId, Cont, "#", Cont.Substring(0, 3), Menu);
                        foreach (var Action in Actions)
                        {
                            Menu = true;
                            if (p.InMenuExist(Action))
                            {
                                Menu = false;
                            }
                            if (!p.DropActionExist(Action))
                            {
                                Id++;
                                string code = Cont.Substring(0, 3)+ Action.Substring(0, 3);
                                PermissionDataRow(Permissions, Id, tempParentId, Cont + " " + Action, "/" + Cont + "/" + Action, code, Menu);
                            }

                        }

                    }
                    else
                    {
                        string code = Cont.Substring(0, 3)+ Actions[0].Substring(0, 3);
                        PermissionDataRow(Permissions, Id, ModuleId, Cont, "/" + Cont + "/" + Actions[0], code, Menu);
                    }
                }

                
            }


            Sec_PermissionDL pd = new Sec_PermissionDL();
            return pd.Save(Permissions);

        }

        private DataRow PermissionDataRow(DataTable dt, int Id,int ParentId,string Title,string URL,string code, bool InMunu=true) {
            DataRow row;
            row = dt.NewRow();
            row["Id"] = Id;
            row["ParentId"] = ParentId;
            row["Title"] = Title;
            row["URL"] = URL;
            row["Code"] = code;
            row["In_menu"] = InMunu;
            dt.Rows.Add(row);
            return row;
        }

        private List<string> GetControllers(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                {
                    if (type.Name.Contains("Controller"))
                    {
                        namespacelist.Add(type.Name.Replace("Controller", string.Empty));

                    }

                }
            }

            foreach (string classname in namespacelist)
            {
                if (!p.DropControllerExist(classname))
                {
                    classlist.Add(classname);
                }

            }

            return classlist;
        }

        private List<string> ActionNames(string controllerName)
        {



            var types =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where typeof(IController).IsAssignableFrom(t) &&
                        string.Equals(controllerName + "Controller", t.Name, StringComparison.OrdinalIgnoreCase)
                select t;

            var controllerType = types.FirstOrDefault();

            if (controllerType == null)
            {
                return Enumerable.Empty<string>().ToList();
            }


            return new ReflectedControllerDescriptor(controllerType)
                .GetCanonicalActions().Select(x => x.ActionName).Distinct().ToList();
        }



        //public ActionResult Test() {
        //    Sec_PermissionBL pl = new Sec_PermissionBL();
        //    return View(pl.ToList("All", ""));
        //}

        [IsLogin(CheckPermission = false, Return = "")]
        public ActionResult Menu(string type)
        {
            try
            {
                var per = ViewBag.Permissions as List<Sec_Permission>;
                if (per != null)
                {
                    var p2 = per.Where(m => m.ParentId == 0).ToList();
                    if (p2.Count <= 1)
                    {
                        var p3 = per.Where(m => m.ParentId == p2[0].Id).ToList();
                        if (type == "AdminLte")
                        {
                            return PartialView("~/views/shared/_menu.cshtml", p3);

                        } else if (type== "Scale") {
                            return PartialView("~/Areas/Survey/Views/Shared/_menu.cshtml", p3);
                        }
                        else if (type == "SmartAdmin")
                        {
                            return PartialView("~/views/shared/_SmartAdminmenu.cshtml", p3);
                        }

                    }
                }
                if (type == "AdminLte")
                {
                    return PartialView("~/views/shared/_menu.cshtml", per);
                }
                else if (type == "Scale")
                {
                    return PartialView("~/Areas/Survey/Views/Shared/_menu.cshtml", per);
                }
                else if (type == "SmartAdmin")
                {
                    return PartialView("~/views/shared/_SmartAdminmenu.cshtml", per);
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        [IsLogin(CheckPermission = false)]
        public ActionResult ByRole(int id = 0)
        {
            ViewBag.RoleId = id;
            Sec_PermissionBL pl = new Sec_PermissionBL();
          
            var r = pl.ToList("byRoleId", id.ToString());
            string PIds = null;
            foreach (var item in r)
            {
                PIds += item.Id + ",";
            }
            ViewBag.PIds = PIds;
            if (id == 0)
            {
                TempData["msg_error"] = "Please Select Role";
            }
            var rec = pl.ToList("ByStatus", "1");
            return View(rec);


        }

        [HttpPost ,IsLogin(CheckPermission =false)]
        public bool ByRole(int RoleId, string[] PermissionIds)
        {

            try
            {
                DataTable Data = new DataTable();
                Data.Columns.AddRange(new DataColumn[2]
                {
                                    new DataColumn("Id", typeof (int)),
                                    new DataColumn("value", typeof (string))


                });


                foreach (var item in PermissionIds)
                {
                    DataRow row;
                    row = Data.NewRow();
                    row["Id"] = RoleId;
                    row["value"] = item;

                    Data.Rows.Add(row);
                }

                RolePermissionDL rpd = new RolePermissionDL();
                if (rpd.Insert(RoleId, Data))
                {
                    TempData["msg_success"] = "Save Successfully.";
                }
                return true;
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
                return false;
            }
        }
        [ IsLogin(CheckPermission = false)]
        public ActionResult ByUser(int id = 0)
        {

            Sec_User user = new Sec_User();
            Sec_UserBL ubl = new Sec_UserBL();
            user = ubl.Single("ById", id.ToString());
            if (user!=null)
            {
                Sec_UserSettingsDL udl = new Sec_UserSettingsDL();
                Sec_PermissionBL pl = new Sec_PermissionBL();
                Sec_UserDefinationTypeBL ud = new Sec_UserDefinationTypeBL();
                ///Projects and User projects
                DataTable Table = udl.GetDataTable("All_Projects",null, null, null);
                ViewBag.Projects = Table.ToList<PM_Projects>();
                DataTable Table1 = udl.GetDataTable("UserProjects", id.ToString(), null, null);
                ViewBag.UserProjects = Table1.ToList<PM_Projects>();
                var r = pl.ToList("byUserId", id.ToString());
                var d = ud.ToList("GetByUserId", id.ToString());
                string UDSelected = null;
                foreach (var item in d)
                {
                    UDSelected += item.DefinationTypeId + ",";
                }
                ViewBag.DIds = UDSelected;
                string Selected = null;
                foreach (var item in r)
                {
                    Selected += item.Id + ",";
                }
                ViewBag.PIds = Selected;
                ViewBag.UId = id;


                // User Clients
                ClientsBL cb = new ClientsBL();
                ViewBag.Clients=    cb.ToList("byStatus", "True");

                // get selected user Clients
                UserClientsBL uchb = new UserClientsBL();
                ViewBag.UserClients = uchb.ToList("byUserId",id.ToString());


                // get selected user Cities
                UserCityBL ucb = new UserCityBL();
              
                ViewBag.UserCities = ucb.ToList("byUserId", id.ToString());

                // get Regions
                AD_DefinationBL db = new AD_DefinationBL();
                ViewBag.Region = db.RegionsToList();

                ViewBag.Cities = db.ToList("AllCities");

                ViewBag.Scopes= db.ToList("Scopes");
                ViewBag.UserScopes = db.ToList("UserScopes", id.ToString());
               // ViewBag.UserScopes = string.Join(",", Scopes.Select(n => n.DefinationId.ToString()).ToArray());

                var Permissions = pl.ToList("byRoleId", user.RoleId.ToString());
                return View(Permissions);


            }
            else
            {
                TempData["msg_error"] = "User not Found.";
            }

            return View();
           


        }

        [HttpPost, IsLogin(CheckPermission = false)]
        public bool ByUser(int UserId, string[] PermissionIds,string[] Clients,string[] cities,int[] Scopes, int[] Projects, int[] DefinationType)
        {

            try
            {
                DataTable Data = new DataTable();
                Data.Columns.AddRange(new DataColumn[2]
                {
                                    new DataColumn("Id", typeof (int)),
                                    new DataColumn("value", typeof (string))


                });


               if(PermissionIds != null)
                {
                    foreach (var item in PermissionIds)
                    {
                        DataRow row;
                        row = Data.NewRow();
                        row["Id"] = UserId;
                        row["value"] = item;

                        Data.Rows.Add(row);
                    }
                }

                UserPermissionDL rpd = new UserPermissionDL();
                if (rpd.Insert(UserId, Data))
                {
                    // for save user Clints
                    Data.Rows.Clear();

                    if (Clients!=null && Clients.Length>0)
                    {
                        foreach (var item in Clients)
                        {
                            DataRow row;
                            row = Data.NewRow();
                            row["Id"] = UserId;
                            row["value"] = item;

                            Data.Rows.Add(row);
                        }

                        UserClientsDL ucd = new UserClientsDL();
                        ucd.Insert(UserId, Data);
                    }


                    // for save user Cities

                    Data.Rows.Clear();

                    if (cities != null && cities.Length > 0)
                    {
                        var CityIds = cities.Where(m => m.Contains("r-")).ToArray();
                        foreach (var item in cities)
                        {
                          
                            DataRow row;
                            row = Data.NewRow();
                            row["Id"] = UserId;
                            if (item.Contains("r-"))
                            {
                                row["value"] = item.Remove(0, 2);
                            }
                            else
                            {
                                row["value"] = item.Remove(0, 4);
                            }
                            

                            Data.Rows.Add(row);
                        }

                        UserCityDL uctd = new UserCityDL();
                        uctd.Insert("UserCities",UserId, Data);
                    }


                    

                    if (Scopes != null && Scopes.Length > 0)
                    {
                        Data.Rows.Clear();
                        foreach (var item in Scopes)
                        {
                            DataRow row;
                            row = Data.NewRow();
                            row["Id"] = UserId;
                            row["value"] = item;

                            Data.Rows.Add(row);
                        }

                        Sec_UserScopeDL usd = new Sec_UserScopeDL();
                        usd.Manage(UserId, Data);
                    }


                    if (DefinationType != null && DefinationType.Length > 0)
                    {
                        Data.Rows.Clear();
                        foreach (var item in DefinationType)
                        {
                            DataRow row;
                            row = Data.NewRow();
                            row["Id"] = UserId;
                            row["value"] = item;

                            Data.Rows.Add(row);
                        }

                        Sec_UserDL usd = new Sec_UserDL();
                        usd.ManageUserDefinationTypes(UserId, Data);
                    }
                    //Projects
                    if (Projects != null && Projects.Length > 0)
                    {
                        Data.Rows.Clear();
                        foreach (var item in Projects)
                        {
                            DataRow row;
                            row = Data.NewRow();
                            row["Id"] = UserId;
                            row["value"] = item;

                            Data.Rows.Add(row);
                        }

                        Sec_UserProjectsDL usd = new Sec_UserProjectsDL();
                        usd.Manage(ViewBag.UserId, Data,UserId,"Insert");
                    }
                    Permission.ChangeUserId(UserId);
                    TempData["msg_success"] = "Save Successfully.";
                }
                return true;
            }
            catch (Exception ex)
            {

                TempData["msg_error"] = ex.Message;
                return false;
            }





        }
    }
}