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
using System.Dynamic;
using PagedList;

namespace MVCProject.Controllers
{
    public class ProductPriceController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: /ProductPrice/
        public ActionResult Index(int? page, int? size, string filter, string order,string subid)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");
            List<Models.ProductPriceViewModel> list = null;
            filter = filter == null ? "" : filter;
            list = (from l in db.ProductPrices.ToList()
                    join p in db.Products.Where(a=>a.ProductName.Contains(filter)).ToList() on l.ProductID equals p.ID
                    join ls in db.LocationSubs.ToList() on l.LocationID equals ls.ID
                    select new ProductPriceViewModel()
                                                                                    {
                                                                                        id = l.ProductID.ToString(),
                                                                                        name = p.ProductName,
                                                                                        price = l.Price,
                                                                                        desc = l.Description,
                                                                                        date = l.Created,
                                                                                        locationsub = ls.Name,
                                                                                        idlocationsub=ls.ID
                                                                                    }).ToList();
            
            //Tìm giải pháp search tiếng việt không dấu
            //if (filter != null)
            //    list = list.Where(a => a.name.Contains(char.Parse(filter))).ToList();
            if (subid != null & subid != "0")
                list = list.Where(a => a.idlocationsub.ToString() == subid).ToList();
            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            ViewData["SubList"] = Common.Params.listLocationSub;
            return View(list.ToList().ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }

        // GET: /ProductPrice/Details/5
        public ActionResult Details(long? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //ProductPrice productprice = db.ProductPrices.Find(id);
            //if (productprice == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(productprice);
            return null;
        }

        // GET: /ProductPrice/Create
        public ActionResult Create(long? id,string subid)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "7"))
                return RedirectToAction("AccessDenied", "Account");

            var pp = new Models.ProductPrice();
            if (id != null) pp.ProductID = (long)id;
            if (subid != null)
                pp.LocationID = int.Parse(subid);

            ViewBag.ProductList = Common.Commons.GetProductList(db).Where(a=>a.Value==id.Value.ToString());
            ViewBag.LocationSubList = Common.Params.listItemLocationSub.Where(a => a.Value == subid); 

            return View(pp);
        }

        // POST: /ProductPrice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,ProductID,Price,Created,Description,UserID,LocationID")] ProductPrice productprice)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "7"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                var pp = db.ProductPrices.Where(a => a.LocationID == productprice.LocationID & a.ProductID == productprice.ProductID).FirstOrDefault();
                if (pp != null)
                    db.ProductPrices.Remove(pp);
                db.SaveChanges();
                productprice.Created = DateTime.Now;
                productprice.UserID = User.Identity.GetUserId();
                db.ProductPrices.Add(productprice);
                db.SaveChanges();
                return RedirectToAction("Index", "ProductPriceLocationSub");
            }

            return View(productprice);
        }

        // GET: /ProductPrice/Edit/5
        public ActionResult Edit(long? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "8"))
                return RedirectToAction("AccessDenied", "Account");

            ViewBag.ProductList = Common.Commons.GetProductList(db);
            ViewBag.LocationList = Common.Commons.GetLocationList(db);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPrice productprice = db.ProductPrices.Find(id);
            if (productprice == null)
            {
                return HttpNotFound();
            }
            return View(productprice);
        }

        // POST: /ProductPrice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,ProductID,Price,Created,Description,UserID,LocationID")] ProductPrice productprice)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "8"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(productprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productprice);
        }

        // GET: /ProductPrice/Delete/5
        public ActionResult Delete(long? id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //ProductPrice productprice = db.ProductPrices.Find(id);
            //if (productprice == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(productprice);
            return null;
        }

        // POST: /ProductPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
            //    return null;

            //ProductPrice productprice = db.ProductPrices.Find(id);
            //db.ProductPrices.Remove(productprice);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void GetProductName()
        {
            var nameList = (from pr in db.ProductPrices
                            join p in db.Products
                            on pr.ProductID equals p.ID
                            select new
                            {
                                p.ID,
                                p.ProductName
                            });

            Dictionary<long, string> nList = null;
            if (nameList != null && nameList.Count() > 0)
            {
                nList = new Dictionary<long, string>();
                foreach (var item in nameList)
                    nList.Add(item.ID, item.ProductName);
            }
            ViewData["NameList"] = nList;
        }
    }
}
