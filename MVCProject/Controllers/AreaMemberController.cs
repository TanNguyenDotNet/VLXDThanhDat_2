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
using PagedList;

namespace MVCProject.Controllers
{
    public class AreaMemberController : ControllerBase
    {
        private aspnetEntities db = new aspnetEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductPrice(string page = "", string size = "", string filter = "", string order = "", string catid = "", string subid = "", string supplier = "")
        {
            if (!Commons.CheckPermission(ViewData, db, User.Identity.GetUserName(), "33"))
                return RedirectToAction("AccessDenied", "Account");

            List<string> roles = (List<string>)ViewData["AccessList"];
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());
            //Kiểm tra khư vực có tồn tại
            if (CheckExistLocation(user, roles))
                return RedirectToAction("AccessDenied", "Account");

            ListProductView product = new ListProductView();
            product.Page = page; product.Size = size;
            product.Filter = filter; product.OrderAsc = order;
            product.Catalogid = catid; product.Supplier = supplier;
            product.Subid = subid;
            LoadProductPrice(product, roles);
            return View(product);
        }
        public ActionResult Orders(int? page, int? size, string dateFrom, string dateTo, string name)
        {
            if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), "33"))
                return RedirectToAction("AccessDenied", "Account");

            List<string> roles = (List<string>)ViewData["AccessList"];
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());

            if (CheckExistLocation(user, roles))
                return RedirectToAction("AccessDenied", "Account");

            if (string.IsNullOrEmpty(dateFrom) == true && string.IsNullOrEmpty(dateTo))
            {
                dateFrom = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                dateTo = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            var listUsers = GetListIDAccountByLocation(roles, user);
            if (listUsers.ListUsers.Count == 0)
                return RedirectToAction("AccessDenied", "Account");
            var list = AReportSales.GetRevenueInvoiceByIDAccount(name, "2", dateFrom, dateTo, listUsers.ListUsers.Select(a => a.Id).ToList());

            //ViewBag.Filter = name;
            ViewData["Users"] = listUsers.ListAppUsers;
            ViewData["Total"] = list.Sum(a => a.Total);
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 50 : (int)size));
        }
        private UsersView GetListIDAccountByLocation(List<string> roles, AppNetUserType user)
        {
            List<string> listIDAccount = new List<string>();
            if (roles.Contains("32"))
            {
                UsersView users = new UsersView();
                users.ListAppUsers = AAppNetUserType.Instance.GetListUserFrontendByLocation(user.LocationID.ToString());
                users.ListAppUsers.Insert(0, new AppNetUserType() { UserOfName = "", DisplayName = "Tất cả" });
                users.ListUsers = AUsers.Instance.GetListByUsername(users.ListAppUsers.Select(a => a.UserOfName).ToList()).ToList();

                return users;
            }
            else
            {
                UsersView users = new UsersView();
                users.ListAppUsers = AAppNetUserType.Instance.GetListUserFrontendByLocation("", user.LocationSubID.ToString());
                users.ListAppUsers.Insert(0, new AppNetUserType() { UserOfName = "", DisplayName = "Tất cả" });
                users.ListUsers = AUsers.Instance.GetListByUsername(users.ListAppUsers.Select(a => a.UserOfName).ToList()).ToList();
                return users;
            }
        }
        private bool CheckExistLocation(AppNetUserType user, List<string> roles)
        {

            var list = ALocationSub.Instance.GetList(true, "", user.LocationID.ToString());
            if (list.Count == 0)
                return true;
            if (!roles.Contains("32"))
            {
                list = list.Where(a => a.ID == user.LocationSubID).ToList();
                if (list.Count == 0)
                {
                    return true;
                }
            }

            return false;
        }
        private ListProductView LoadProductPrice(ListProductView product, List<string> roles)
        {
            product.Product = AProductPriceLocationSub.Instance.GetList(product.Page, product.Size,
                product.Filter, product.OrderAsc, product.Catalogid, product.Subid, product.Supplier);
            product.Catalog = ACatalog.Instance.GetList();

            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());

            if (roles.Contains("32"))
                product.Locationsub = ALocationSub.Instance.GetList(true, "", user.LocationID.ToString());
            else
                product.Locationsub = ALocationSub.Instance.GetList(true, user.LocationSubID.ToString());

            return product;
        }
    }
}