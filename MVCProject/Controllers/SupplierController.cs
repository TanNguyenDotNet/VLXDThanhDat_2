using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace MVCProject.Controllers
{
    public class SupplierController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: /Supplier/
        public ActionResult Index(string page="",string size="",string filter="")
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");

            var list = from a in db.Suppliers select a;
            if (!string.IsNullOrEmpty(filter))
                list = list.Where(a => a.Name.Contains(filter));
            list = list.Where(a => a.IsDel == false);
            return View(list.OrderBy(a=>a.Name).ToPagedList(page == "" ? 1 : int.Parse(page), size == "" ? 20 : int.Parse(size)));
        }

        // GET: /Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "3"))
                return RedirectToAction("AccessDenied", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: /Supplier/Create
        public ActionResult Create()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            var item = new Supplier();
            item.IsShow = true;
            return View();
        }

        // POST: /Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Address,Phone,TaxCode,AccountBank,NameBank,IsDel,IsShow")] Supplier supplier)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            if (ModelState.IsValid)
            {
                supplier.Name = supplier.Name == "" ? "Chưa nhập Tên" : supplier.Name;
                supplier.IsDel = false;
                supplier.IsShow = true;
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: /Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: /Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Phone,TaxCode,AccountBank,NameBank,IsDel,IsShow")] Supplier supplier)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: /Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: /Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "4"))
                return RedirectToAction("AccessDenied", "Account");
            Supplier supplier = db.Suppliers.Find(id);
            supplier.IsDel = true;
            db.Entry(supplier).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
