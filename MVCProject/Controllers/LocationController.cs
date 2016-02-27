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
    public class LocationController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: /Location/
        public ActionResult Index(int? page, int? size, string filter)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");

            IEnumerable<Models.Location> list = GetList(filter);
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 10 : (int)size));
        }

        // GET: /Location/Details/5
        public ActionResult Details(int? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location location = db.Locations.Find(id);
            //if (location == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(location);
            return RedirectToAction("AccessDenied", "Account");
        }

        // GET: /Location/Create
        public ActionResult Create()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "23"))
                return RedirectToAction("AccessDenied", "Account");
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,LocationName,LocationCode,Order")] Location location)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "23"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                location.Order = UpdateOrder(location.ID, location.Order);
                db.Locations.Add(location);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: /Location/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "24"))
                return RedirectToAction("AccessDenied", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: /Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,LocationName,LocationCode,Order")] Location location)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "24"))
                return RedirectToAction("AccessDenied", "Account");

            

            if (ModelState.IsValid)
            {
                UpdateOrder(location.ID, location.Order);
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: /Location/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Location location = db.Locations.Find(id);
            //if (location == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(location);
            return RedirectToAction("AccessDenied", "Account");
        }

        // POST: /Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;
            //Location location = db.Locations.Find(id);
            //db.Locations.Remove(location);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("AccessDenied", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private IEnumerable<Location> GetList(string filter)
        {
            IEnumerable<Models.Location> list = null;
            if (filter != null && filter != "")
                list = db.Locations.Where(c => c.LocationName.Contains(filter));
            else
                list = db.Locations;
            return list.OrderBy(o => o.Order).ToList();
        }

        private int UpdateOrder(int? id, int? beginFrom)
        {
            int m = 0;
            var max = db.Locations.Max(c => c.Order);
            m = (int)max;
            Models.Location l = null;
            if (id != null && id > 0)
            {
                l = db.Locations.Single(c => c.ID == id);
            }

            if ((l == null || l.Order != beginFrom) && (beginFrom != null && beginFrom > 0))
            {
                
                if (l != null && l.Order > beginFrom)
                {
                    var list = db.Locations.Where(c => c.Order < l.Order && c.Order >= beginFrom).ToList().OrderBy(o => o.Order);
                    foreach (var u in list)
                        u.Order = u.Order + 1;
                }
                else if (l != null && l.Order < beginFrom)
                {
                    var list = db.Locations.Where(c => c.Order > l.Order && c.Order <= beginFrom).ToList().OrderBy(o => o.Order);
                    foreach (var u in list)
                        u.Order = u.Order - 1;
                }
                else if(beginFrom < m)
                {
                    var list1 = db.Locations.Where(c => c.Order >= beginFrom).ToList().OrderBy(o => o.Order);
                    int i = (int)beginFrom + 1;
                    foreach (var u in list1)
                    {
                        u.Order = i;
                        i++;
                    }
                }
                m = (int)beginFrom;
                if (l != null)
                    l.Order = m;
            }
                        
            db.SaveChanges();
            return m;
        }
    }
}
