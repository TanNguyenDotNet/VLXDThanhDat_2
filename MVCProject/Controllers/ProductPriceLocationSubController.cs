using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCProject.Common;
using MVCProject.Models;
using PagedList;

namespace MVCProject.Controllers
{
    public class ProductPriceLocationSubController : Controller
    {
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: ProductPriceLocationSub
        public ActionResult index(int? page, int? size, string filter, string order, string catid, string subid = "1")
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), "25"))
                return RedirectToAction("AccessDenied", "Account");
            if (modelAspnet.LocationSubs.Count() < 1)
                return View();
            var list = GetList(filter, order, catid == null || catid == "" ? "0" : catid);
            int _subid = int.Parse(subid);
            decimal priceSub = decimal.Parse((modelAspnet.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;
            list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
            var listProductPriceSub = modelAspnet.ProductPrices.Where(a => a.LocationID == _subid).ToList();
            if (listProductPriceSub.Count != 0)
            {
                var listpp = (from p in modelAspnet.Products.ToList()
                              join pp in listProductPriceSub on p.ID equals pp.ProductID
                              select p).ToList();

                foreach (var item in listpp)
                {
                    item.Price = listProductPriceSub.Where(a => a.ProductID == item.ID).FirstOrDefault().Price;
                }

                list.ToList().AddRange(listpp.ToList());//Tai sao addrange chi? thay doi gia tri cua object?
            }
            initList();
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }
        IEnumerable<Models.Product> GetList(string filter, string order, string cid)
        {
            var list = from p in modelAspnet.Products

                       //join pp in modelAspnet.ProductPrices on p.ID equals pp.ProductID into pp_join
                       //from pp in pp_join.DefaultIfEmpty()
                       //where
                       //  pp.ID == null && pp.LocationID != _subid //Left join
                       select p;//Lay danh sach khong bao gom gia chiet khau cua vung
            if (!string.IsNullOrEmpty(filter))
                list = list.Where(a => a.ProductName.Contains(filter));
            if (cid != "0")
            { long cat = long.Parse(cid); list = list.Where(a => a.CatID == cat); }
            list = list.Where(a => a.IsDel == false);
            list = OrderList(list, order);

            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            return list.ToList();
        }
        IQueryable<Models.Product> OrderList(IQueryable<Models.Product> list, string order)
        {
            switch (order)
            {
                case "price":
                    list = list.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.Price);
                    break;
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    list = list.OrderBy(s => s.ProductName);
                    break;
            }

            return list;
        }
        void initList()
        {
            ViewData["CatList"] = modelAspnet.Catalogs.ToList();
            ViewData["SubList"] = Params.listLocationSub;
        }
    }
}