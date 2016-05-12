using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Common;
using Microsoft.AspNet.Identity;
using MVCProject.Models.ModelView;
using MVCProject.Models;
using MVCProject.Models.AccessData;

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
            using (var model = Params.ModelaspnetEntities)
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
                cartview.Ordersdetail = new List<OrdersDetail>();
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

            if (cartview.Ordersdetail != null & cartview.Ordersdetail.Where(a => a.IDProduct == int.Parse(id)).Count() > 1)
            {
                int amount = int.Parse(quantity) + int.Parse(cartview.Ordersdetail.Where(a => a.IDProduct == int.Parse(id)).FirstOrDefault().Amount);
                cartview.Ordersdetail.Where(a => a.IDProduct == int.Parse(id)).FirstOrDefault().Amount = amount.ToString();
            }
            cartview.Ordersdetail.Add(new OrdersDetail() { IDProduct = int.Parse(id), Amount = quantity == "" ? "1" : quantity });
            return RedirectToAction("Order", "Product");
        }

        public ActionResult CartView()
        {
            if (Session[CommonsConst.SessionCart] == null)
                return RedirectToAction("Index", "Account");
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            var cartview = (CartView)Session[CommonsConst.SessionCart];

            if(cartview.Ordersdetail!=null)
            {
                
            }
            return RedirectToAction("Order", "Product");
        }

        private void ProcessCartView(CartView cartView)
        {
            var list = AProductPriceLocationSub.Instance.GetList(cartView.Ordersdetail.Select(a => a.IDProduct).ToList(), cartView.Subid);
            var listTax = ATax.Instance.GetList();
            foreach (var item in cartView.Ordersdetail)
            {
                item.Price = list.Where(a => a.ID == item.IDProduct).Select(b => b.Price).FirstOrDefault().Value;
                item.Tax = listTax.Where(a => a.ID == (byte)list.Where(b => b.ID == item.IDProduct).Select(b => b.TaxID).FirstOrDefault().Value).Select(c => c.TaxRate).FirstOrDefault().Value.ToString();
                item.ProductCode = list.Where(a => a.ID == item.IDProduct).Select(b => b.ItemCode).FirstOrDefault().ToString();
                item.ReturnGood = false;
                item.RequestByUser = false;
                item.DateOfOrder = DateTime.Now;
                item.Discount = 0;
                item.Sale = 0;
                item.Total = 0;
                
            }
        }
    }
}