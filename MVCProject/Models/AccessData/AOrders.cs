using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Models.ModelView;
using System.Data.Entity.Validation;

namespace MVCProject.Models.AccessData
{
    public class AOrders
    {
        private volatile static AOrders _instance;
        static object locked = new object();
        public static AOrders Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new AOrders();
                }
                return _instance;
            }

        }
        private AOrders() { }
        public int CreateOrder(CartView cart)
        {
            try
            {
                using (var model = Params.ModelRetail)
                {
                    model.Orders.Add(cart.Order);
                    model.OrdersDetails.AddRange(cart.Ordersdetail);
                    return model.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                return 0;
            }
        }
        public Order GetOrderByCode(string code)
        {
            using(var model= Params.ModelRetail)
            {
                return model.Orders.Where(a => a.OrderCode == code).FirstOrDefault();
            }
        }
        public IEnumerable<Models.Order> GetList(string filter = "", string state = "", string datefrom = "", string dateto = "")
        {
            using (var model = Params.ModelRetail)
            {
                var list = from l in model.Orders select l;
                if (!string.IsNullOrEmpty(datefrom) & !string.IsNullOrEmpty(dateto))
                {
                    datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                    dateto = UtilDatetime.ToTime(dateto).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0 &&
                                               String.Compare(a.DateCreate, dateto) <= 0);
                }
                else if (!string.IsNullOrEmpty(datefrom))
                {
                    datefrom = UtilDatetime.FromTime(datefrom).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.DateCreate, datefrom) >= 0);
                }
                else if (!string.IsNullOrEmpty(dateto))
                {
                    dateto = UtilDatetime.ToTime(dateto).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.DateCreate, dateto) <= 0);
                }
                if (!string.IsNullOrEmpty(filter))
                    list = list.Where(a => a.OrderCode.Contains(filter));
                if (!string.IsNullOrEmpty(state) && state != "3")
                    list = list.Where(a => a.State == state);

                return list.OrderBy(a => a.DateCreate).ToList();
            }
        }
    }
}