using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Common;
using Microsoft.AspNet.Identity;
using MVCProject.Models.ModelView;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateCart(string id)
        {
            using(var model = Params.ModelaspnetEntities)
            {
                if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                    return null;
                if (!Commons.CheckPermission(ViewData, model, User.Identity.GetUserName(), "1"))
                    return RedirectToAction("AccessDenied", "Account");
                if (string.IsNullOrEmpty(id))
                    return RedirectToAction("Index", "Account");

                var user = (from l in model.AppNetUserTypes
                            join u in model.AspNetUsers.Where(a => a.UserName == id) on l.UserOfName equals u.UserName
                            select new { l, u.Id }).FirstOrDefault();

                var cartview = new CartView();
                cartview.Userid = user.Id;
                cartview.Username = user.l.UserOfName;
                cartview.Subid = user.l.LocationSubID.ToString();

                if (Session[CommonsConst.SessionCart] != null)
                    Session.Remove(CommonsConst.SessionCart);
                Session[CommonsConst.SessionCart] = cartview;
                return RedirectToAction("Order", "Product");
            }
        }
        public ActionResult AddItemToCart(string id, string quantity)
        {
            if (Session[CommonsConst.SessionCart] == null)
                return RedirectToAction("Index", "Account");
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            var cartview = (CartView)Session[CommonsConst.SessionCart];

            cartview.Ordersdetail.Add(new OrdersDetail() { });
        }
	}
}