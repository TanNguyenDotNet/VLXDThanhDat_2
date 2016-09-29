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
using Microsoft.AspNet.Identity;
using MVCProject.Models.AccessData;
namespace MVCProject.Controllers
{
    public class LocationSubController : Controller
    {
        aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: LocationSub
        public ActionResult Index()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");
            dynamic model = new ExpandoObject();
            using (var _model = Params.ModelaspnetEntities)
            {
                var query = (from a in _model.LocationSubs.ToList()
                             join b in _model.Locations.ToList() on a.IDLocation equals b.ID
                             where a.IsDel != 1
                             select new { id = a.ID, name = a.Name, description = a.Description, locationprice = a.LocationPrice, locationName = b.LocationName }).ToList();
                model.LocationSub = UtilEntities.modelDynamic(query);
                return View(model);
            }

        }
        public ActionResult Create()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");
            using (var model = Params.ModelaspnetEntities)
            {
                ViewBag.LocationList = Common.Commons.GetLocationList(Params.ModelaspnetEntities);
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDLocation,LocationPrice,Name,Description,IsDel,Show")] LocationSub _LocationSub)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                using (var model = Params.ModelaspnetEntities)
                {
                    double n;
                    if (string.IsNullOrEmpty(_LocationSub.Name))
                        _LocationSub.Name = "Chưa nhập tên";
                    if (string.IsNullOrEmpty(_LocationSub.LocationPrice) || double.TryParse(_LocationSub.LocationPrice.ToString(),out n) == false)
                        _LocationSub.LocationPrice = "0";
                    model.LocationSubs.Add(_LocationSub);
                    model.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(_LocationSub);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDLocation,LocationPrice,Name,Description,IsDel,Show")]LocationSub _LocationSub)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            using (var model = Params.ModelaspnetEntities)
            {
                if (ModelState.IsValid)
                {
                    double n;
                    ALogSystem.Instance.save("LocationSub", DateTime.Now.ToString(CommonsConst.formatedatetime), User.Identity.GetUserId(), _LocationSub.ID.ToString(), _LocationSub.Name, model.LocationSubs.Where(a => a.ID == _LocationSub.ID).Select(a => a.LocationPrice).FirstOrDefault());
                    if (string.IsNullOrEmpty(_LocationSub.Name))
                        _LocationSub.Name = "Chưa nhập tên";
                    if (string.IsNullOrEmpty(_LocationSub.LocationPrice) || double.TryParse(_LocationSub.LocationPrice.ToString(), out n) == false)
                        _LocationSub.LocationPrice = "0";
                    model.Entry(_LocationSub).State = EntityState.Modified;
                    
                    model.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(_LocationSub);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
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