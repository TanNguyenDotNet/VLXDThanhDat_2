using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using MVCProject.Common;

namespace MVCProject.Controllers
{
    public class OrderController : Controller
    {
        private retailEntities db = new retailEntities();
        private aspnetEntities _db = new aspnetEntities();
        //
        // GET: /Order/
        public ActionResult Index(int? page, int? size, string filter, string state,string datefrom,string dateto)
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.UserType = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), _db);

            if (!Common.Commons.CheckPermission(ViewData, _db, User.Identity.GetUserName(), "20"))
                return RedirectToAction("AccessDenied", "Account");

            var list = GetList(filter, state, datefrom, dateto);
            
            return View(list.ToList().ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 5 : (int)size)); 
        }

        public ActionResult Cancel()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            Session.Clear();
            Response.Redirect("~/Product/Home");
            return null;
        }

        public ActionResult OrderList()
        {
            if (!Request.IsAuthenticated)
                return null;
            
            Common.UserType ut = Common.Commons.GetUserType(Request, Response, User.Identity.GetUserName(), _db);

            string us = User.Identity.GetUserId();
            IEnumerable<Models.Order> list = null;
            if(ut == Common.UserType.Delivery)
                list = db.Orders.Where(c => c.DeliveryMan == us).ToList();
            else
                list = db.Orders.Where(c => c.IDAccount == us).ToList();
            return View(list);
        }

        public ActionResult Success()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("~/Account/Login");

            Session.Clear();
            return View();
        }
        
        public ActionResult Complete(string orderCode, string deliveryMan, string dateShip)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            var o = db.Orders.Single(c => c.OrderCode == orderCode);
            o.DeliveryMan = deliveryMan;
            o.DateShip = DateTime.Parse(dateShip, new System.Globalization.CultureInfo("vi-VN")).ToString("yyyyMMddHHmm");
            o.DateProcessed = DateTime.Now.ToString("yyyyMMddHHmm");
            o.State = "2";
            db.SaveChanges();
            Response.Redirect("~/Order/Index");
            return null;
        }
        private IEnumerable<Models.Order> GetList(string filter, string state, string datefrom, string dateto)
        {
            var list = from l in db.Orders select l;
            if (!string.IsNullOrEmpty(datefrom) & !string.IsNullOrEmpty(dateto))
            {
                datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                dateto = UtilDatetime.FromTime(dateto).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0 &&
                                           String.Compare(a.DateCreate, dateto) <= 0);
            }
            else if (!string.IsNullOrEmpty(datefrom))
            {
                datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0);
            }
            else if (!string.IsNullOrEmpty(datefrom))
            {
                dateto = UtilDatetime.ToTime(dateto).ToString("yyyyMMddHHmmss");
                list = list.Where(a => String.Compare(a.DateCreate, dateto) <= 0);
            }
            if (!string.IsNullOrEmpty(filter))
                list = list.Where(a => a.OrderCode.Contains(filter));
            if (!string.IsNullOrEmpty(state) && state != "3")
                list = list.Where(a => a.State == state);

            return list = list.OrderBy(a => a.DateCreate);
        }
    }
}
