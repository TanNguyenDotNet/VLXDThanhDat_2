using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using MVCProject.Common;
using MVCProject.Models.ModelView;
using MVCProject.Models.AccessData;

namespace MVCProject.Controllers
{
    public class OrderController : Controller
    {
        private retailEntities db = new retailEntities();
        private aspnetEntities _db = new aspnetEntities();
        //
        // GET: /Order/
        public ActionResult Index(int? page, int? size, string filter, string state, string datefrom, string dateto)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            ViewBag.UserType = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), _db);
            if (!Common.Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "20"))
                return RedirectToAction("AccessDenied", "Account");
            try
            {
                if (string.IsNullOrEmpty(datefrom) && string.IsNullOrEmpty(dateto))
                    datefrom = DateTime.Now.ToString("dd/MM/yyyy HH:ss");
                var list = GetList(filter, state, datefrom, dateto);

                return View(list.ToList().ToPagedList(page == null ||
                    page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
            }
            catch (Exception ex)
            {
                Common.UtilException.ErrorLog(HttpContext.Server.MapPath("~/LogError/"), ex);
                return View(ex);
            }
        }
        public ActionResult OrderCreateByAdmin()
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (Session[CommonsConst.SessionCart] == null)
                return RedirectToAction("Index", "Account");

            var cartview = (CartView)Session[CommonsConst.SessionCart];

            if (cartview.Ordersdetail.Count == 0 || cartview.Order.Total==0)
                return RedirectToAction("Index", "Account");
            int ucat = 0;
            string code = Commons.GenItemCode(_db, out ucat, "OC");
            cartview.Order.OrderCode = code;
            cartview.Ordersdetail.ForEach(a => a.OrderCode = code);
            if (AOrders.Instance.CreateOrder(cartview) <= 0)
                return RedirectToAction("CartView", "Cart");
            return Redirect("/Order/SuccessOrderByAdmin?code=" + code);
        }
        public ActionResult Cancel()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            Session.Clear();
            Response.Redirect("~/Product/Home");
            return null;
        }
        public ActionResult OpenAddItemToOrder()
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "28"))
                return RedirectToAction("AccessDenied", "Account");
            if (Session[CommonsConst.SessionOrder] != null)
                Session.Remove(CommonsConst.SessionOrder);

            var orderview = new OrderAddItemView();
            orderview.Order = AOrders.Instance.GetOrderByCode(Request.QueryString["code"]);
            orderview.ExceptIdProduct = AOrdersDetail.Instance.GetListByCode(Request.QueryString["code"]).Select(a => a.IDProduct).ToList();
            orderview.Subid = Request.QueryString["subid"];
            Session[CommonsConst.SessionOrder] = orderview;
            return Redirect("~/Product/ListProductOrder");
        }
        public ActionResult OrderList(int? page, int? size, string state, string datefrom, string dateto)
        {
            if (!Request.IsAuthenticated)
                return null;
            if (string.IsNullOrEmpty(datefrom))
            {
                datefrom = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            if (string.IsNullOrEmpty(dateto))
            {
                dateto = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }

            TimeSpan time = UtilDatetime.ToTime(dateto) - UtilDatetime.FromTime(datefrom);
            if (time.TotalDays > 93)
            {
                return RedirectToAction("OrderList");
            }
            Common.UserType ut = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), _db);
            string usid = User.Identity.GetUserId();
            IEnumerable<Models.Order> list = null;
            if (ut == Common.UserType.Delivery)
                list = GetList("", "", datefrom, dateto, usid).ToList();
            else
                list = GetList("", "", datefrom, dateto, usid).ToList();
            return View(list.ToList().ToPagedList(page == null ||
                    page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }

        public ActionResult Success()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            Session.Clear();
            return View();
        }
        public ActionResult SuccessOrderByAdmin()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");
            
            Session.Remove(CommonsConst.SessionCart);
            return View();
        }

        public ActionResult Complete(string orderCode, string deliveryMan, string dateShip)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            var o = db.Orders.Single(c => c.OrderCode == orderCode);
            o.DeliveryMan = deliveryMan;
            o.DateShip = DateTime.Parse(dateShip, new System.Globalization.CultureInfo("vi-VN")).ToString("yyyyMMddHHmmss");
            o.DateProcessed = DateTime.Now.ToString("yyyyMMddHHmmss");
            o.State = "2";
            db.SaveChanges();
            return Redirect("~/Order/Index");
            //return null;
        }
        private IEnumerable<Models.Order> GetList(string filter, string state, string datefrom, string dateto,string userid="")
        {
            var list = from l in db.Orders select l;
            if (!string.IsNullOrEmpty(datefrom) & !string.IsNullOrEmpty(dateto))
            {
                datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                dateto = UtilDatetime.ToTime(dateto).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0 &&
                                           String.Compare(a.DateCreate, dateto) <= 0);
            }
            else if (!string.IsNullOrEmpty(datefrom) && string.IsNullOrEmpty(filter))
            {
                datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0);
            }
            else if (!string.IsNullOrEmpty(dateto) && string.IsNullOrEmpty(filter))
            {
                dateto = UtilDatetime.ToTime(dateto).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, dateto) <= 0);
            }
            if (!string.IsNullOrEmpty(filter))
                list = list.Where(a => a.OrderCode.Contains(filter));
            if (!string.IsNullOrEmpty(state) && state != "3")
                list = list.Where(a => a.State == state);
            if (!string.IsNullOrEmpty(userid))
                list = list.Where(a => a.IDAccount == userid);
            return list = list.OrderBy(a => a.DateCreate);
        }
    }
}
