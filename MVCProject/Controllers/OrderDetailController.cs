using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCProject.Common;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class OrderDetailController : Controller
    {
        private retailEntities db = new retailEntities();
        private aspnetEntities _db = new aspnetEntities();

        // GET: /OrderDetail/
        public ActionResult Index(long? id)
        {
            if (!Request.IsAuthenticated)
                return null;

            Common.UserType ut = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), _db);
            if (ut == UserType.Delivery)
                Response.Redirect("~/Order/OrderList");
            if (!AddToCart(id))
                return null;

            ViewData["CartRequestDetail"] = Session["CartRequestDetail"];
            return View(GenCartDetails());
        }

        [HttpPost]
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            ViewData["CartRequestDetail"] = Session["CartRequestDetail"];
            return View(GenCartDetails());
        }

        public ActionResult Addreq()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            AddRequest();
            return null;
        }

        // GET: /OrderDetail/Details/5
        public ActionResult Details()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            string enu = Security.EncryptString("User:" + User.Identity.GetUserName() + "~FrontendUser", false, EncryptType.TripleDES);
            var u = _db.AppNetUserTypes.Find(enu);
            var l = _db.Locations.Find(u.LocationID);

            ViewData["Location"] = l;
            ViewData["AccountInfo"] = u;
            ViewData["CartRequestDetail"] = Session["CartRequestDetail"];

            return View(GenCartDetails());
        }

        public ActionResult AdmView()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Common.Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "15"))
                return RedirectToAction("AccessDenied", "Account");
            ViewBag.DeliveryList = Common.Commons.GetDeliveryList(_db);
            return View(AdminViewOrder(1));
        }

        public ActionResult UserView()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            return View(AdminViewOrder(0));
        }

        // GET: /OrderDetail/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            GetCart();
            return null;
        }

        public ActionResult AddOrderDetail(string code, string subid)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");
            var listProduct = db.OrdersDetails.Where(a => a.OrderCode == code).Select(b => b.IDProduct).ToList();
            ViewBag.ProductList = Common.Commons.GetProductList(_db).Where(a => listProduct.Any(b => b.ToString() == a.Value) == false);//Han che ket noi xuong database
            ViewBag.PriceList = GetListProductPrices(subid).ToDictionary(a => a.ID, b => (decimal)b.Price);
            ViewBag.OrderCode = code;
            return View(new OrdersDetail { OrderCode = code, Tax = "0" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrderDetail([Bind(Include = "ID,IDProduct,Price,Amount,ReturnGood,DateOfOrder,Tax,Total,Description," +
            "ProductCode,RequestByUser,OrderCode,Discount")] OrdersDetail od)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                od.DateOfOrder = DateTime.Now;
                od.ProductCode = _db.Products.Where(a => a.ID == od.IDProduct).Select(b => b.ItemCode).FirstOrDefault();
                od.RequestByUser = false;
                db.OrdersDetails.Add(od);
                db.SaveChanges();
                Order _order = RefeshOrder(db.Orders.Where(a => a.OrderCode == od.OrderCode).FirstOrDefault());
                db.Entry(_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdmView", new { code = _order.OrderCode });
            }

            return View(od);
        }

        // GET: /OrderDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.OrdersDetail od = db.OrdersDetails.Find(id);
            if (od == null)
            {
                return HttpNotFound();
            }
            ViewBag.Price = od.Price.ToString("n0");
            ViewBag.Total = od.Total.ToString("n0");
            return View(od);
        }

        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDProduct,Price,Amount,ReturnGood,DateOfOrder,Tax,Total,Description," +
            "ProductCode,RequestByUser,OrderCode,Discount")] OrdersDetail od)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "2"))
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(od).State = EntityState.Modified;
                Order _order = RefeshOrder(db.Orders.Where(a => a.OrderCode == od.OrderCode).FirstOrDefault());
                if (_order != null)
                    db.Entry(_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdmView", new { code = od.OrderCode });
            }

            return View(db);
        }

        // GET: /OrderDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            return null;
        }

        // POST: /OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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

        #region Functions
        private IEnumerable<Models.OrdersDetail> AdminViewOrder(int viewBy)
        {
            string code = Request.QueryString["code"];
            if (code == null) return null;
            var o = db.Orders.Single(c => c.OrderCode == code);
            if (o.State == "0" && viewBy == 1)
            {
                o.State = "1";
                db.SaveChanges();
            }
            var us = _db.AspNetUsers.Single(c => c.Id == o.IDAccount);
            string enu = Security.EncryptString("User:" + us.UserName + "~FrontendUser", false, EncryptType.TripleDES);
            var u = _db.AppNetUserTypes.Find(enu);
            var l = _db.Locations.Find(u.LocationID);

            ViewData["Location"] = l;
            ViewData["AccountInfo"] = u;
            ViewData["OrderCode"] = code;
            ViewData["State"] = o.State;

            var list = db.OrdersDetails.Where(c => c.OrderCode == code).ToList();
            List<Models.Product> lp = new List<Product>();
            double total = 0;
            foreach (var od in list)
            {
                if (od.IDProduct > 0)
                {
                    total += (double)od.Total;
                    var p = _db.Products.Single(c => c.ID == od.IDProduct);
                    lp.Add(p);
                }
            }
            ViewData["Total"] = total.ToString("#,###");
            ViewData["ProductList"] = lp;
            return list;
        }
        private void GetCart()
        {
            List<Models.OrdersDetail> li = (List<Models.OrdersDetail>)GenCartDetails();
            if (li == null) li = new List<OrdersDetail>();

            List<string> list = new List<string>();
            if (Session["CartRequestDetail"] != null)
                list = (List<string>)Session["CartRequestDetail"];

            int ucat = 0;
            string code = Common.Commons.GenItemCode(_db, out ucat, "OC");

            if (list.Count > 0)
            {
                foreach (string s in list)
                {
                    string[] parts = s.Split('|');
                    Models.OrdersDetail od = new OrdersDetail
                    {
                        Amount = parts[1],
                        DateOfOrder = DateTime.Now,
                        Description = parts[0] + "|" + parts[2],
                        IDProduct = 0,
                        OrderCode = code,
                        Price = 0,
                        ProductCode = "",
                        RequestByUser = true,
                        ReturnGood = false,
                        Tax = "0",
                        Total = 0
                    };
                    li.Add(od);
                }
            }

            if (li.Count > 0)
            {
                decimal dicount = 0, tax = 0, taxid = 0, total = 0, totalouttax = 0;

                foreach (Models.OrdersDetail od in li)
                {
                    if (!od.RequestByUser)
                    {
                        dicount += (decimal)od.Discount;
                        totalouttax += (decimal)(decimal.Parse(od.Amount) * od.Price);
                        total += od.Total;
                    }

                    if (od.Description == null) od.Description = "";

                    od.DateOfOrder = DateTime.Now;
                    od.OrderCode = code;

                    if (od.IDProduct < 0)//(od.IDProduct > 0)
                        //var p = _db.Products.Single(c => c.ID == od.IDProduct);
                        //od.ProductCode = p.ItemCode;
                    {
                        od.ProductCode = "";
                        od.Discount = 0;
                    }

                    db.OrdersDetails.Add(od);
                    db.SaveChanges();
                }

                try
                {
                    db.Orders.Add(new Order
                    {
                        DateCreate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        DateProcessed = "",
                        DateShip = "",
                        DeliveryMan = "",
                        Description = "",
                        Discount = dicount,
                        IDAccount = User.Identity.GetUserId(),
                        OrderCode = code,
                        State = "0",
                        Tax = (total - totalouttax).ToString(),
                        TaxID = taxid.ToString(),
                        Total = total,
                        TotalWithoutTax = totalouttax
                    });

                    db.SaveChanges();
                    Response.Redirect("~/Order/Success/?code=" + code);
                }
                catch
                {
                    db.OrdersDetails.RemoveRange(li);
                }
            }
        }

        private bool AddToCart(long? id)
        {
            if (id != null)
            {
                var p = _db.Products.Find(id);
                string code = Session["Cart"].ToString();
                if (code.IndexOf("," + p.ItemCode) != -1)
                    code = code.Replace("," + p.ItemCode, "");
                else if (code.IndexOf(p.ItemCode + ",") != -1)
                    code = code.Replace(p.ItemCode + ",", "");
                else
                    code = code.Replace(p.ItemCode, "");

                Session["Cart"] = code;
                Response.Redirect("~/OrderDetail/Index");
                return false;
            }

            return true;
        }

        private void AddRequest()
        {
            List<string> list = new List<string>();
            if (Session["CartRequestDetail"] != null)
                list = (List<string>)Session["CartRequestDetail"];

            if (list.Count < 10)
            {
                string name = Request.QueryString["ProductName"];
                string desc = Request.QueryString["Description"];
                string quan = Request.QueryString["Quantity"];
                int iquan = 0;
                Int32.TryParse(quan, out iquan);
                if (name != null && name != "" && iquan > 0)
                {
                    list.Add(string.Format("{0}|{1}|{2}", name, quan, desc));
                }
            }

            Session["CartRequestDetail"] = list;
            Response.Redirect("~/OrderDetail/Index");
        }

        private IEnumerable<Models.OrdersDetail> SetDetailForm(List<Product> li, string[,] cd, AppNetUserType u)
        {
            if (cd.Length == 0)
                Response.Redirect("~/Account/Login");

            double Total = 0;
            List<Models.OrdersDetail> listOd = new List<OrdersDetail>();

            for (int i = 0; i < cd.GetLength(0); i++)
            {
                Models.OrdersDetail od = new OrdersDetail();
                od.IDProduct = long.Parse(cd[i, 0]);
                od.Price = decimal.Parse(cd[i, 1]);
                od.Amount = cd[i, 2];
                od.Tax = (cd[i, 3] != null && cd[i, 3] != "" ? float.Parse(cd[i, 3]) : 0).ToString();
                od.RequestByUser = false;

                decimal discount = 0;
                var p = (from prom in _db.Promotions
                         join promtype in _db.PromotionTypes
                         on prom.PromotionTypeID equals promtype.ID
                         where prom.ProductID == od.IDProduct && prom.Active == true
                             && prom.StartDate >= DateTime.Now && prom.EndDate <= DateTime.Now
                             && (prom.LocationID == 0 || prom.LocationID == u.LocationID)
                             && !promtype.AddType.Contains("P")
                         select new
                         {
                             prom.PromotionValue,
                             promtype.AddType,
                             promtype.ExRate
                         });

                if (p != null && p.Count() > 0)
                {
                    foreach (var item in p)
                    {
                        if (item.ExRate > 0)
                        {
                            discount += (decimal)(od.Price * (item.PromotionValue / item.ExRate));
                        }
                        else
                            discount += item.PromotionValue;
                    }
                }

                od.Discount = (discount * decimal.Parse(od.Amount));
                double thue = (double)od.Price * (double)(double.Parse(od.Tax) / 100);
                od.Total = (decimal)(double.Parse(od.Amount) * ((double)od.Price + thue));
                od.Total = od.Total - (discount * decimal.Parse(od.Amount));
                od.ProductCode = li.Where(a => a.ID == od.IDProduct).Select(b => b.ItemCode).FirstOrDefault();
                Total += (double)od.Total;
                listOd.Add(od);
            }

            ViewData["ProductList"] = li;
            ViewData["Total"] = Total.ToString("#,###");

            Session["CartDetails"] = cd;

            if ((listOd == null || listOd.Count == 0) && (cd == null || cd.GetLength(0) == 0))
            {
                Response.Redirect("~/Product/Home");
                return null;
            }

            return listOd;
        }
        private List<Models.Product> ListPrice(int? subid, string[] parts, string subprice)
        {
            var li = _db.Products.Where(c => parts.Contains(c.ItemCode)).ToList();
            decimal priceSub = subprice == null ? 0 : decimal.Parse(subprice) / 100;
            li.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));

            var listProductPriceSub = _db.ProductPrices.Where(a => a.LocationID == subid).ToList();// lay danh sach gia co' trong gio hang
            listProductPriceSub = listProductPriceSub.Where(a => li.Select(b => b.ID).Contains(a.ProductID)).ToList();
            if (listProductPriceSub.Count != 0)
            {
                foreach (var item in li)
                {
                    var _price = listProductPriceSub.Where(a => a.ProductID == item.ID).FirstOrDefault();
                    if (_price != null)
                        item.Price = _price.Price;
                }
            }

            return li;
        }
        private IEnumerable<Models.OrdersDetail> GenCartDetails()
        {
            if (Session["Cart"] == null || Session["Cart"].ToString() == "")
                return null;
            string[] parts = Session["Cart"].ToString().Split(',');
            string[,] cd = new string[parts.Length, 4];

            string[,] cdSession = null;
            if (Session["CartDetails"] != null)
                cdSession = (string[,])Session["CartDetails"];

            string enu = Security.EncryptString("User:" + User.Identity.GetUserName() + "~FrontendUser", false, EncryptType.TripleDES);
            var u = _db.AppNetUserTypes.Find(enu);
            var subprice = _db.LocationSubs.Where(a => a.ID == u.LocationSubID).FirstOrDefault();
            var li = ListPrice(u.LocationSubID, parts, subprice.LocationPrice);

            int index = 0;
            foreach (var i in li)
            {
                // Find quantity in session
                string quanSession = "1";
                if (cdSession != null && cdSession.GetLength(0) > 0)
                {
                    for (int j = 0; j < cdSession.GetLength(0); j++)
                    {
                        if (cdSession[j, 0] == i.ID.ToString())
                        {
                            quanSession = cdSession[j, 2];
                        }
                    }
                }

                cd[index, 1] = i.Price.ToString();

                cd[index, 0] = i.ID.ToString();
                string quan = Request.QueryString["quan_" + i.ID];
                cd[index, 2] = Request.QueryString["quan_" + i.ID] != null &&
                    Request.QueryString["quan_" + i.ID] != "" ? Request.QueryString["quan_" + i.ID] : quanSession;
                var t = _db.Taxes.Where(c => c.ID == i.TaxID).FirstOrDefault();//Ko co row
                if (t != null)//&& t.TaxRate > 0) Thue 0%
                    cd[index, 3] = t.TaxRate.ToString();
                index++;
            }

            return SetDetailForm(li, cd, u);
        }
        private Order RefeshOrder(Order o)
        {
            if (o == null)
                return o;
            var od = db.OrdersDetails.Where(a => a.OrderCode == o.OrderCode);
            if (od.Count() < 1)
                return o;
            o.TotalWithoutTax = 0;
            o.Total = 0;
            foreach (var item in od)
            {
                o.TotalWithoutTax += (decimal)(decimal.Parse(item.Amount) * item.Price);
                o.Total += item.Total;
            }
            o.Tax = (o.Total - o.TotalWithoutTax).ToString();
            return o;
        }
        private List<Product> GetListProductPrices(string subid)
        {
            var list = from l in _db.Products select l;
            int _subid = int.Parse(subid);
            decimal priceSub = decimal.Parse((_db.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;
            list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
            var listProductPriceSub = _db.ProductPrices.Where(a => a.LocationID == _subid).ToList();
            if (listProductPriceSub.Count != 0)
            {
                var listpp = (from p in _db.Products.ToList()
                              join pp in listProductPriceSub on p.ID equals pp.ProductID
                              select p).ToList();

                foreach (var item in listpp)
                {
                    item.Price = listProductPriceSub.Where(a => a.ProductID == item.ID).FirstOrDefault().Price;
                }

                list.ToList().AddRange(listpp.ToList());//Tai sao addrange chi? thay doi gia tri cua object?
            }
            return list.ToList();
        }
        #endregion
    }
}
