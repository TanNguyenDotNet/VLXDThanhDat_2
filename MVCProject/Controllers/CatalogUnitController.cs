using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using MVCProject.Common;

namespace MVCProject.Controllers
{
    public class CatalogUnitController : Controller
    {
        aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: CatalogUnit
        public ActionResult Index()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");

            return View(modelAspnet.CatalogUnits.ToList());
        }
        public ActionResult Create()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Show,Description")] CatalogUnit catalog)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(catalog.Name))
                    catalog.Name = "Chưa nhập đơn vị tính";
                catalog.Show = true;
                modelAspnet.CatalogUnits.Add(catalog);
                modelAspnet.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalog);
        }
        public ActionResult Edit(int id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            var catalog = modelAspnet.CatalogUnits.Where(a => a.ID == id).FirstOrDefault();

            return View(catalog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Show,Description")] CatalogUnit catalog)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(catalog.Name))
                    catalog.Name = "Chưa nhập đơn vị tính";
                modelAspnet.Entry(catalog).State = System.Data.Entity.EntityState.Modified;
                modelAspnet.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalog);
        }
    }
}