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
                var list = from l in model.Products where l.IsDel == false && l.IsState == 1 select l;
                int _subid = int.Parse(subid.DefaultIfEmpty('0').ToString());
                if (!string.IsNullOrEmpty(filter))
                    list = list.Where(a => a.ProductName.Contains(filter));
                if (!string.IsNullOrEmpty(catid))
                    list = list.Where(b => b.CatID == _subid);

                decimal priceSub = 0;
                if (!string.IsNullOrEmpty(subid))
                    priceSub = decimal.Parse((model.LocationSubs.Where(a => a.ID == _subid).FirstOrDefault().LocationPrice)) / 100;

                list.ToList().ForEach(a => a.Price = a.Price + (a.Price * priceSub));
                
                var listProductPriceSub = model.ProductPrices.Where(a => a.LocationID == _subid).ToList();

                if (listProductPriceSub.Count != 0)
                {
                    var listpp = (from p in list
                                  join pp in listProductPriceSub on p.ID equals pp.ProductID
                                  select p).ToList();

                    foreach (var item in listpp)
                    {
                        item.Price = listProductPriceSub.Where(a => a.ProductID == item.ID).FirstOrDefault().Price;
                    }

                    list.ToList().AddRange(listpp.ToList());//Tai sao addrange chi? thay doi gia tri cua object?
                }

                int _page = page == "" ? 1 : int.Parse(page);
                int _size = size == "" ? 20 : int.Parse(size);
                return list.ToPagedList(_page, _size);
            }
        }
    }
}