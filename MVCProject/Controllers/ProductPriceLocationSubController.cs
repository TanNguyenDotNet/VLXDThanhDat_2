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
        public ActionResult index(int? page, int? size, string filter, string order, string catid,string subid="1")
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, modelAspnet, User.Identity.GetUserName(), null))
                return RedirectToAction("AccessDenied", "Account");
            var list = GetList(filter, order, catid == null || catid == "" ? "0" : catid);
            int _subid=int.Parse(subid);
            decimal priceSub = decimal.Parse((modelAspnet.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;
            list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
            var listProductPriceSub = modelAspnet.ProductPrices.Where(a => a.LocationID == _subid).ToList();
            if (listProductPriceSub.Count != 0)
            {
                list = from l in list.ToList()
                       join p in listProductPriceSub on l.ID equals p.ProductID into pp
                       from ps in pp
                       where ps.ProductID == null
                       select l;
            }
            int count = list.Count();
            initList();
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }
        IEnumerable<Models.Product> GetList(string filter, string order, string cid)
        {
            long lcid = 0;
            try
            {
                cid = cid == null || cid == "" ? "0" : cid;
                lcid = long.Parse(cid);
            }
            catch { lcid = 0; cid = "0"; }

            IEnumerable<Models.Product> list = null;
            if (lcid > 0 && filter != null && filter != "")
                list = modelAspnet.Products.Where(c => c.CatID == lcid
                    && c.ProductName.Contains(filter) && c.Show == true);
            else if (lcid > 0)
                list = modelAspnet.Products.Where(c => c.CatID == lcid);
            else if (filter != null && filter != "")
                list = modelAspnet.Products.Where(c => c.ProductName.Contains(filter) && c.Show == true);
            else
                list = modelAspnet.Products.Where(c => c.Show == true);

            list = OrderList(list, order);
            
            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            return list.ToList();
        }
        IEnumerable<Models.Product> OrderList(IEnumerable<Models.Product> list, string order)
        {
            if (list == null || list.Count() == 0)
                return list;

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
            ViewData["CatList"]=modelAspnet.Catalogs.ToList();
            ViewData["SubList"] = modelAspnet.LocationSubs.ToList();
        }
    }
}