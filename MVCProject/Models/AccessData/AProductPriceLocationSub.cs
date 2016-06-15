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
        public IPagedList<Product> GetList(string page = "", string size = "", string filter = "", string order = "", string catid = "", string subid = "", string supplier = "", List<long> ExceptIdProduct = null)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = from l in model.Products where l.IsDel == false && l.Show == true select l;
                if (ExceptIdProduct != null)
                    list = list.Where(a => !ExceptIdProduct.Contains(a.ID));
                int _subid = string.IsNullOrEmpty(subid) ? 0 : int.Parse(subid);
                var _list = new List<Product>();
                string[] _filter;
                if (!string.IsNullOrEmpty(filter))
                {
                    _filter = filter.Split('+');
                    if (_filter.Length > 1)
                    {
                        filter = _filter[0].Trim();
                        list = list.Where(a => a.ProductName.Contains(filter));
                        _list = model.Products.SqlQuery(string.Format("Select * from dbo.Product where id like '%{0}%' ", _filter[1].ToString().Trim())).ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(filter))
                        {
                            filter = filter.Trim();
                            list = list.Where(a => a.ProductName.Contains(filter));
                            _list = model.Products.SqlQuery(string.Format("Select * from dbo.Product where id like '%{0}%' ", filter)).ToList();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(supplier))
                { int sup = int.Parse(supplier); list = list.Where(a => a.SupplierID == sup); }
                if (!string.IsNullOrEmpty(catid) && catid != "0")
                { int _catid = int.Parse(catid); list = list.Where(b => b.CatID == _catid); }

                decimal priceSub = 0;
                if (_subid > 0)
                    priceSub = decimal.Parse((model.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;

                var listpp = (from p in model.Products
                              join pp in model.ProductPrices.Where(a => a.LocationID == _subid) on p.ID equals pp.ProductID
                              select new { p, pp.Price, pp.Created }).ToList();

                list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
                if (listpp.Count > 0)
                {
                    listpp.ForEach(a => { a.p.Price = a.Price; a.p.DateUpdate = a.Created; });
                    list.ToList().AddRange(listpp.Select(a => a.p).ToList());
                }

                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);
                var listMain = list.ToList();
                if (_list.Count > 0)
                { listMain.AddRange(_list); }
                return listMain.Distinct().OrderBy(a => a.ProductName).ToPagedList(_page, _size);
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