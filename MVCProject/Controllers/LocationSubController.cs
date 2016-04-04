using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Common;
using System.ComponentModel;
using MVCProject.Models;
using System.Net;
using System.Data.Entity;

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
                             select new {id=a.ID, name = a.Name, description = a.Description, locationprice=a.LocationPrice, locationName = b.LocationName }).ToList();
                model.LocationSub = UtilEntities.modelDynamic(query);
                return View(model);
            }
            
        }
        public ActionResult Create()
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "3"))
                //return RedirectToAction("AccessDenied", "Account");
            using (var model = Params.ModelaspnetEntities)
            {
                ViewBag.LocationList = Common.Commons.GetLocationList(Params.ModelaspnetEntities);
                return View();
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDLocation,LocationPrice,Name,Description,IsDel")] LocationSub _LocationSub)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,IDLocation,LocationPrice,Name,Description,IsDel")]LocationSub _LocationSub)
        {
            using (var model =Params.ModelaspnetEntities)
            {
                if (ModelState.IsValid)
                {
                    model.Entry(_LocationSub).State = EntityState.Modified;
                    model.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(_LocationSub);
            }
        }
        public ActionResult Edit(int? id)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LocationSub locationSub = model.LocationSubs.Find(id);
                if (locationSub == null)
                {
                    return HttpNotFound();
                }
                ViewBag.LocationList = Common.Commons.GetLocationList(Params.ModelaspnetEntities);
                return View(locationSub);
            }
        }
    }
}