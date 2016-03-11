using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Common;
using System.ComponentModel;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class LocationSubController : Controller
    {
        // GET: LocationSub
        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            using (var _model = Params.ModelaspnetEntities)
            {
                var query = (from a in _model.LocationSubs.ToList()
                             join b in _model.Locations.ToList() on a.IDLocation equals b.ID
                             select new { name = a.Name, description = a.Description, locationName = b.LocationName }).ToList();
                List<ExpandoObject> joinData = new List<ExpandoObject>();

                foreach (var item in query)
                {
                    IDictionary<string, object> itemExpando = new ExpandoObject();
                    foreach (PropertyDescriptor property
                             in
                             TypeDescriptor.GetProperties(item.GetType()))
                    {
                        itemExpando.Add(property.Name, property.GetValue(item));
                    }
                    joinData.Add(itemExpando as ExpandoObject);
                }

                model.LocationSub = joinData;
                return View(model);
            }
            
        }
        public ActionResult Create()
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "3"))
                //return RedirectToAction("AccessDenied", "Account");

            ViewBag.LocationList = Common.Commons.GetLocationList(Params.ModelaspnetEntities);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDLocation,Name,Description,IsDel")] LocationSub _LocationSub)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "3"))
                //return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                using (var model = Params.ModelaspnetEntities)
                {
                    model.LocationSubs.Add(_LocationSub);
                    model.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(_LocationSub);
        }
    }
}