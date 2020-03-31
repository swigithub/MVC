using SWI.Libraries.AirView.BLL;
using SWI.Security.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SWI.Common.Controllers
{
    [IsLogin(CheckPermission = false,Return ="")]
    public class CascadeDropdownController : Controller
    {
        // GET: CascadeDropdown
        [HttpPost]
        public ActionResult UserCities(int Id)
        {
            if (!Permission.IsLogin()) return null;

            Permission p = new Permission();
            List<SelectListItem> items = new List<SelectListItem>();
            UserCityBL ucb = new UserCityBL();
            var lst = ucb.ToList("byUserId", Id.ToString());
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet); ;
        }

        public List<SelectListItem> Clients(string filter)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ClientsBL cb = new ClientsBL();
            var lst = cb.ToList(filter);
            items.Add(new SelectListItem { Text = "Select Client", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.ClientName, Value = item.ClientId.ToString() });
            }
            return items;
        }

        [HttpPost]
        public ActionResult Clients(int CountryId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ClientsBL cb = new ClientsBL();
            var lst = cb.ToList("ByCountryId", CountryId.ToString());
          //  items.Add(new SelectListItem { Text = "Select Client", Value = "0" });
            foreach (var item in lst)
            {
                items.Add(new SelectListItem { Text = item.ClientName, Value = item.ClientId.ToString() });
            }
            return Json(new SelectList(items, "Value", "Text"), JsonRequestBehavior.AllowGet); ;
        }
    }
}