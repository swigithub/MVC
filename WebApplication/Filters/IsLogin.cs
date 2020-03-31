using SWI.Libraries.Security.Entities;
using SWI.Security.Controllers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SWI.Security.Filters
{
    /*----MoB!----*/
    public class IsLogin : ActionFilterAttribute
    {
        public  int UID { get; set; }
        public string controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string Code { get; set; }
        public string Return { get; set; }
        public bool CheckPermission = true;
        private bool IsAjax(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
   
        public override void OnActionExecuting(ActionExecutingContext filterContext)

        {
            UID = Permission.getSharedvalue();
            var Us = HttpContext.Current.Session["user"] as LoginInformation;

            var Response = filterContext.HttpContext.Response;
            var Request = filterContext.HttpContext.Request;
            string RequestUrl = Request.Path;

            //string Area=Request.RequestContext.RouteData.
            Area = (string)filterContext.RouteData.DataTokens["area"];
            controller = (string)filterContext.RouteData.Values["controller"];
            Action = (string)filterContext.RouteData.Values["action"];
            if (Return == "NoCheck")
            {
                goto Execute;
            }
            if (Us == null)
            {
                if (Return == "")
                {
                    filterContext.Result = new EmptyResult();
                }
                else if (IsAjax(filterContext))
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = "session expired",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.HttpContext.Response.Clear();
                }
                else if (Return == "login")
                {
                    filterContext.Result = new RedirectToRouteResult(
                             new RouteValueDictionary(
                            new { controller = "Login", action = "index", area = "" }));
                }
                else
                {
                    #region check If id Exist

                    //string id = string.Empty;
                    //foreach (var parameter in filterContext.ActionParameters)
                    //{
                    //    var dt = parameter.Key;
                    //    if (dt == "id" || dt=="Id" || dt=="ID")
                    //    {
                    //        id = (string)parameter.Value.ToString();
                    //    }
                    //}
                    //string QueryString = "?";
                    //foreach (var parameter in filterContext.ActionParameters)
                    //{
                    //    var dt = parameter.Key;
                    //    QueryString = QueryString + parameter.Key + "=" + parameter.Value+"&";
                    //}

                    filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary(
                      //new { controller = "Login", action = "index", tctl = controller, tact = Action  + QueryString }));
                      new { controller = "Login", action = "index", area = "" }));
                    // QueryString = QueryString.Remove(QueryString.Length - 1);
                    HttpContext.Current.Session["PrevUrl"] = Request.RawUrl;// RequestUrl+ QueryString ;

                    #endregion check If id Exist
                }
            }
            else
            {
                if (!Request.Url.ToString().StartsWith("http://122"))
                {
                    if (!Request.Url.ToString().StartsWith("http://192"))
                    {
                        if (!Request.Url.ToString().StartsWith("http://96.57.107.147"))
                        {
                            if (!Request.Url.ToString().StartsWith("http://demo.airviewx.com"))
                            {
                                if (!Request.Url.ToString().StartsWith("http://amp.airviewx.com"))
                                {
                                    if (!Request.IsSecureConnection && !Request.IsLocal) //
                                    {
                                        //Response.ClearContent();
                                        //Response.ClearHeaders();
                                        //Response.Flush();

                                        //filterContext.Result = new RedirectResult(Request.Url.ToString().Replace("http:", "http:"));
                                        //goto Result;

                                        // Response.Redirect(Request.Url.ToString().Replace("http:", "https:"));
                                    }
                                }
                            }
                        }
                    }
                }

                if (CheckPermission)
                {
                    #region Check Permission

                    //if (Permission.UnUsedPermission.Count == 0)
                    //{
                    //    Sec_PermissionBL pb = new Sec_PermissionBL();
                    //    Permission.UnUsedPermission.AddRange(pb.ToList("NoUse", ""));
                    //}

                    //string Code = string.Empty;
                    //if (Permission.UnUsedPermission.Count > 0 && Permission.UnUsedPermission != null)
                    //{
                    //    string tempCode = "/" + controller + "/" + Action;
                    //    if (Permission.UnUsedPermission.Any(m => m.URL.ToLower().Equals(tempCode.ToLower()))) goto Result;
                    //}

                    //if (Action == "Menu") { goto Result; }
                    // if (Us.IsAdmin) goto Result;

                    if (!string.IsNullOrEmpty(Code))
                    {
                        if (Permission.AllowCode(Code) == false)
                        {
                            filterContext.Controller.TempData["msg_error"] = "You do not have permission for this section";
                            filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary(
                              new { controller = "error", action = "index", area = "", tctl = controller, tact = Action }));
                        }
                    }
                    else
                    {
                        if(Area != "" && Area !=null)
                        {
                            Code = "/"+Area+ "/" + controller + "/" + Action;
                        }
                        else { 
                        Code = "/" + controller + "/" + Action;
                        }
                        //Code = controller.Substring(0, 3) + Action.Substring(0, 3);
                        if (Permission.AllowUrl(Code) == false)
                        {
                            filterContext.Controller.TempData["msg_error"] = "You do not have permission for this section";
                            filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary(
                              new { controller = "error", action = "index", area = "", tctl = controller, tact = Action }));
                        }
                    }

                    #endregion Check Permission
                }

                Result:

                string CopyrightValue = string.Empty;
                string Version = string.Empty;
                var Copyright = eSpares.Levity.ApplicationAssemblyUtility.ApplicationAssembly.CustomAttributes.Where(m => m.AttributeType.Name == "AssemblyCopyrightAttribute").FirstOrDefault();
                var VersionAttribute = eSpares.Levity.ApplicationAssemblyUtility.ApplicationAssembly.CustomAttributes.Where(m => m.AttributeType.Name == "AssemblyFileVersionAttribute").FirstOrDefault();
                if (Copyright != null)
                {
                    CopyrightValue = Copyright.ConstructorArguments.FirstOrDefault().Value.ToString().Replace("Year", DateTime.Now.Year.ToString());
                }

                if (VersionAttribute != null)
                {
                    Version = VersionAttribute.ConstructorArguments.FirstOrDefault().Value.ToString();
                }

                filterContext.Controller.ViewBag.IsLogin = "1";
                filterContext.Controller.ViewBag.RoleID = Us.RoleId;
                filterContext.Controller.ViewBag.UserId = Us.UserId;
                filterContext.Controller.ViewBag.IsAdmin = Us.IsAdmin;
                filterContext.Controller.ViewBag.RoleName = Us.RoleName;
                filterContext.Controller.ViewBag.DefaultUrl = Us.DefaultUrl;
                filterContext.Controller.ViewBag.CompId = Us.CompanyId;
                filterContext.Controller.ViewBag.DaysBack = Us.DaysBack;
                filterContext.Controller.ViewBag.DaysForward = Us.DaysForward;
                filterContext.Controller.ViewBag.IsManager = Us.IsManager;
                filterContext.Controller.ViewBag.picture = Us.Picture;
                filterContext.Controller.ViewBag.Name = Us.Name;
                filterContext.Controller.ViewBag.UserName = Us.UserName;
                // filterContext.Controller.ViewBag.Email = Us.Email;
                filterContext.Controller.ViewBag.Permissions = Us.Permissions;
                filterContext.Controller.ViewBag.UserCities = Us.UserCities;
                filterContext.Controller.ViewBag.Allow = new Func<string, bool>(Permission.AllowCode);
                filterContext.Controller.ViewBag.AllowUri = new Func<string, bool>(Permission.AllowUrl);
                filterContext.Controller.ViewBag.CopyrightValue = CopyrightValue;
                filterContext.Controller.ViewBag.Version = Version;

                string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    filterContext.Controller.ViewBag.View = "Mobile";
                }
                else
                {
                    filterContext.Controller.ViewBag.View = "Desktop";
                }
               if(UID > 0) { 
                if(UID == Us.UserId)
                {
                        Permission.UpdateSession(Us.UserName);
                        Permission.ChangeUserId(0);
                }
                }
            }

            //filterContext.Controller.ViewBag.Controller = controller;
            //filterContext.Controller.ViewBag.Action = Action;
            filterContext.Controller.ViewBag.RequestUrl = RequestUrl;

            Execute:
            Code = string.Empty;

            base.OnActionExecuting(filterContext);
        }


        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    var rd = filterContext.RequestContext.RouteData;

        //    controller = rd.GetRequiredString("controller");
        //    Action = rd.GetRequiredString("action");

        //    base.OnResultExecuting(filterContext);
        //}
    }

}