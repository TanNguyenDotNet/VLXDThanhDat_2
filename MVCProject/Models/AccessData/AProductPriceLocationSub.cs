using MVCProject.Common;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class AProductPriceLocationSub
    {
        private volatile static AProductPriceLocationSub _instance;
        static object locked = new object();
        public static AProductPriceLocationSub Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AProductPriceLocationSub();
                }
                return _instance;
            }

        }
        private AProductPriceLocationSub()
        { }
        public IPagedList<Product> GetList(string page = "", string size = "", string filter = "", string order = "", string catid = "", string subid = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.Products where l.IsDel == false && l.Show == true select l;
                int _subid = string.IsNullOrEmpty(subid) ? 0 : int.Parse(subid);
                if (!string.IsNullOrEmpty(filter))
                    list = list.Where(a => a.ProductName.Contains(filter));
                if (!string.IsNullOrEmpty(catid) && catid != "0")
                { int _catid = int.Parse(catid); list = list.Where(b => b.CatID == _catid); }

                decimal priceSub = 0;
                if (_subid > 0)
                    priceSub = decimal.Parse((model.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;

                var listpp = (from p in model.Products
                              join pp in model.ProductPrices.Where(a => a.LocationID == _subid) on p.ID equals pp.ProductID
                              select new { p, pp.Price }).ToList();

                list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
                if (listpp.Count > 0)
                {
                    listpp.ForEach(a => a.p.Price = a.Price);
                    list.ToList().AddRange(listpp.Select(a => a.p).ToList());
                }

                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);

                return list.OrderBy(a => a.ProductName).ToPagedList(_page, _size);
            }
        }
        public List<Product> GetList(List<long> listId, string subid = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.Products where l.IsDel == false && l.Show == true && listId.Contains(l.ID) select l;
                int _subid = string.IsNullOrEmpty(subid) ? 0 : int.Parse(subid);
                decimal priceSub = 0;
                if (_subid > 0)
                    priceSub = decimal.Parse((model.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;

                var listpp = (from p in model.Products
                              join pp in model.ProductPrices.Where(a => a.LocationID == _subid) on p.ID equals pp.ProductID
                              select new { p, pp.Price }).ToList();

                list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
                if (listpp.Count > 0)
                {
                    listpp.ForEach(a => a.p.Price = a.Price);
                    list.ToList().AddRange(listpp.Select(a => a.p).ToList());
                }

                return list.ToList();
            }
        }
    }
}