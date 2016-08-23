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
        public ActionResult ProductPrice(string page = "", string size = "", string filter = "", string order = "", string catid = "", string subid = "", string supplier = "")
        {
            this.CheckPermission("33");

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
            this.CheckPermission("33");

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
        public ActionResult Debt(string page = "", string size = "", string name = "", string locationsub = "0")
        {
            this.CheckPermission("33");

            List<string> roles = (List<string>)ViewData["AccessList"];
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());

            if (CheckExistLocation(user, roles))
                return RedirectToAction("AccessDenied", "Account");

            var listUsers = GetListIDAccountByLocation(roles, user);
            if (listUsers.ListUsers.Count == 0)
                return RedirectToAction("AccessDenied", "Account");

            var list = APaymentDetails.Instance.GetListPaging(page, size, name, locationsub, listUsers);
            return View(list);
        }
        public ActionResult Accounts(string name = "", string locationsub = "0")
        {
            this.CheckPermission("33");

            List<string> roles = (List<string>)ViewData["AccessList"];
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());

            if (CheckExistLocation(user, roles))
                return RedirectToAction("AccessDenied", "Account");

            var listUsers = GetListIDAccountByLocation(roles, user);
            if (listUsers.ListUsers.Count == 0)
                return RedirectToAction("AccessDenied", "Account");
            if (!string.IsNullOrEmpty(name))
                listUsers.ListAppUsers = listUsers.ListAppUsers.Where(a => a.UserOfName.Contains(name) || a.DisplayName.Contains(name)).ToList();

            var locations = ALocationSub.Instance.GetList();

            var listuser = from u in listUsers.ListAppUsers
                           join l in locations on u.LocationSubID equals l.ID
                           select new UserView
                               {
                                   Displayname = u.DisplayName,
                                   Username = u.UserOfName,
                                   Locationsubname = l.Name
                               };
            ViewData["locations"] = LocationByRole(roles);
            return View(listuser.OrderBy(a => a.Locationsubname).ThenBy(a => a.Username));
        }
        public ActionResult LoginHistory(string page = "", string size = "", string name = "", string locationsub = "0", string datefrom = "", string dateto = "")
        {
            this.CheckPermission("33");
            List<string> roles = (List<string>)ViewData["AccessList"];
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());

            if (CheckExistLocation(user, roles))
                return RedirectToAction("AccessDenied", "Account");

            var listUsers = GetListIDAccountByLocation(roles, user);
            if (listUsers.ListUsers.Count == 0)
                return RedirectToAction("AccessDenied", "Account");
            if (!string.IsNullOrEmpty(name))
                listUsers.ListAppUsers = listUsers.ListAppUsers.Where(a => a.UserOfName.Contains(name) || a.DisplayName.Contains(name)).ToList();

            if (locationsub != "0")
            {
                int _locationsub = int.Parse(locationsub);
                listUsers.ListAppUsers = listUsers.ListAppUsers.Where(a => a.LocationSubID == _locationsub).ToList();
            }
            var list = ALoginHistory.Instance.GetListDetailPaging(page, size, name, listUsers, datefrom, dateto);

            ViewData["locations"] = LocationByRole(roles);

            return View(list);
        }

        private UsersView GetListIDAccountByLocation(List<string> roles, AppNetUserType user)
        {
            List<string> listIDAccount = new List<string>();
            UsersView users = new UsersView();
            if (roles.Contains("32"))
            {
                users.ListAppUsers = AAppNetUserType.Instance.GetListUserFrontendByLocation(user.LocationID.ToString());
            }
            else
            {
                users.ListAppUsers = AAppNetUserType.Instance.GetListUserFrontendByLocation("", user.LocationSubID.ToString());
            }
            users.ListAppUsers.Insert(0, new AppNetUserType() { UserOfName = "", DisplayName = "Tất cả" });
            users.ListUsers = AUsers.Instance.GetListByUsername(users.ListAppUsers.Select(a => a.UserOfName).ToList()).ToList();
            return users;
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

            product.Locationsub = LocationByRole(roles);
            return product;
        }

        private List<LocationSub> LocationByRole(List<string> roles)
        {
            var user = AAppNetUserType.Instance.GetUserById(User.Identity.GetUserId());
            if (roles.Contains("32"))
                return ALocationSub.Instance.GetList(true, "", user.LocationID.ToString());
            else
                return ALocationSub.Instance.GetList(true, user.LocationSubID.ToString());
        }
    }
}