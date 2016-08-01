using MVCProject.Common;
using MVCProject.Models;
using MVCProject.Models.AccessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCProject.Models.ModelView;

namespace MVCProject.Controllers
{
    public class AreaMemberController : Controller
    {
        private aspnetEntities db = new aspnetEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductPrice(string page = "", string size = "", string filter = "", string order = "", string catid = "", string supplier = "")
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "28"))
                return RedirectToAction("AccessDenied", "Account");
            ListProductView product = new ListProductView();
            product.Product = AProductPriceLocationSub.Instance.GetList(page, size, filter, order, catid, supplier);
            product.Catalog = ACatalog.Instance.GetList();
            product.Locationsub = ALocationSub.Instance.GetList(true);
            return View(product);
        }
    }
}