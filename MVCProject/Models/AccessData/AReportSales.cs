using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using MVCProject.Models.ModelView;

namespace MVCProject.Models.AccessData
{
    public class AReportSales
    {
        private static IEnumerable<TotalCostInvoice> ListTotalCost(List<string> listOrderCode)
        {
            var listOrderDetail = AOrdersDetail.Instance.GetListProductCode(listOrderCode.Cast<string>().ToList());
            var listCostZero = listOrderDetail.Where(a => a.Cost == 0).ToList().GroupBy(b => b.OrderCode).Select(c => new { ordercode = c.Key });

            foreach (var item in listOrderDetail.Where(a => listCostZero.Select(b => b.ordercode).Contains(a.OrderCode)))
            {
                item.Cost = 0;
            }

            listOrderDetail.ForEach(a => a.Cost = a.Cost * decimal.Parse(a.Amount));

            var listTotalCost = from codes in listOrderDetail
                                group codes by new { codes.OrderCode }
                                    into g
                                    select new TotalCostInvoice { OrderCode = g.Key.OrderCode, TotalCost = (decimal)g.Sum(s => s.Cost) };
            return listTotalCost;
        }
        public static IEnumerable<Models.RevenueInvoice> GetRevenueInvoice(string filter = "", string state = "", string datefrom = "", string dateto = "", string name = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var listOrder = AOrders.Instance.GetList(filter, state, datefrom, dateto);
                var listTotalCost = ListTotalCost(listOrder.Select(a => a.OrderCode).ToList());

                var listuser = from u in model.AppNetUserTypes select u;
                if (!string.IsNullOrEmpty(name))
                {
                    listuser = listuser.Where(a => a.UserOfName.Contains(name) || a.DisplayName.Contains(name));
                }
                var listuserid = from u in model.AspNetUsers.ToList() join l in listuser.ToList() on u.UserName equals l.UserOfName select u;
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in listuserid on od.IDAccount equals u.Id
                                  join
                                    a in listuser.ToList() on u.UserName equals a.UserOfName
                                  join
                                    c in listTotalCost.ToList() on od.OrderCode equals c.OrderCode
                                  select new RevenueInvoice()
                                  {
                                      AccountName = a.DisplayName,
                                      DateCreate = DateTime.ParseExact(od.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                                      OrderCode = od.OrderCode,
                                      Total = od.Total,
                                      TotalProfit = od.Total - c.TotalCost
                                  };
                    return listRpt.ToList();
                }
                return new List<RevenueInvoice>();
            }
        }
        public static IEnumerable<Models.RevenueInvoice> GetRevenueInvoiceByIDAccount(string name = "", string state = "", string datefrom = "", string dateto = "", List<string> idaccounts = null)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var listOrder = AOrders.Instance.GetListByIDAcount(idaccounts, state, datefrom, dateto);
                var listTotalCost = ListTotalCost(listOrder.Select(a => a.OrderCode).ToList());

                var listuser = from u in model.AppNetUserTypes select u;
                if (!string.IsNullOrEmpty(name))
                    listuser = listuser.Where(a => a.DisplayName.Contains(name) || a.UserOfName.Contains(name));

                var listuserid = from u in model.AspNetUsers.Where(b => idaccounts.Contains(b.Id)).ToList() join l in listuser.ToList() on u.UserName equals l.UserOfName select u;
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in listuserid on od.IDAccount equals u.Id
                                  join
                                    a in listuser.ToList() on u.UserName equals a.UserOfName
                                  join
                                    c in listTotalCost.ToList() on od.OrderCode equals c.OrderCode
                                  select new RevenueInvoice()
                                  {
                                      AccountName = a.DisplayName,
                                      DateCreate = DateTime.ParseExact(od.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                                      OrderCode = od.OrderCode,
                                      Total = od.Total
                                  };
                    return listRpt.ToList();
                }
                return new List<RevenueInvoice>();
            }
        }
        public static IEnumerable<Models.RevenueInvoice> GetRevenueInvoiceOfCus(string filter = "", string state = "", string datefrom = "", string dateto = "", string name = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var listOrder = AOrders.Instance.GetList(filter, state, datefrom, dateto);
                var listuser = from u in model.AppNetUserTypes select u;
                if (!string.IsNullOrEmpty(name))
                {
                    listuser = listuser.Where(a => a.UserOfName == name);
                }
                var listuserid = from u in model.AspNetUsers.ToList() join l in listuser.ToList() on u.UserName equals l.UserOfName select u;
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in listuserid on od.IDAccount equals u.Id
                                  join
                                  a in listuser.ToList() on u.UserName equals a.UserOfName
                                  select new RevenueInvoice()
                                  {
                                      AccountName = a.DisplayName,
                                      DateCreate = DateTime.ParseExact(od.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),
                                      OrderCode = od.OrderCode,
                                      Total = od.Total
                                  };
                    return listRpt.ToList();
                }
                return new List<RevenueInvoice>();
            }
        }
        public static IEnumerable<Models.RevenueOfMonth> GetRevenueOfMonth(string state = "", string month = "", string year = "", string filterName = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInMonth(month, year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.Instance.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listTotalCost = ListTotalCost(listOrder.Select(a => a.OrderCode).ToList());
                    var listUser = from l in model.AppNetUserTypes select l;

                    if (!string.IsNullOrEmpty(filterName))
                        listUser = listUser.Where(a => a.UserOfName.Contains(filterName) || a.DisplayName.Contains(filterName));
                    var listRpt = (from od in listOrder.ToList()
                                   join
                                       u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                   join
                                   a in listUser.ToList() on u.UserName equals a.UserOfName
                                   join
                                   l in model.LocationSubs.ToList() on a.LocationSubID equals l.ID
                                   join cost in listTotalCost on od.OrderCode equals cost.OrderCode
                                   group od by new
                                   {
                                       od.IDAccount,
                                       a.DisplayName,
                                       l.Name,
                                       cost.TotalCost
                                   } into g
                                   select new RevenueOfMonth()
                                   {
                                       AccountName = g.Key.DisplayName,
                                       Total = g.Sum(s => s.Total),
                                       LocationSub = g.Key.Name,
                                       TotalProfit = g.Key.TotalCost
                                   }).ToList();

                    var listZero = listRpt.Where(a => a.TotalProfit == 0).ToList();
                    foreach (var item in listRpt.Where(a => listZero.Select(b => b.AccountName).Contains(a.AccountName)))
                    {
                        item.TotalProfit = 0;
                    }
                    listRpt = (from a in listRpt
                               group a by new
                               {
                                   a.AccountName,
                                   a.LocationSub
                               } into g
                               select new RevenueOfMonth()
                               {
                                   AccountName = g.Key.AccountName,
                                   Total = g.Sum(s => s.Total),
                                   LocationSub = g.Key.LocationSub,
                                   TotalProfit = g.Sum(s => s.TotalProfit)
                               }).ToList();
                    listRpt.ForEach(a => a.TotalProfit = a.Total - a.TotalProfit);
                    return listRpt.ToList();
                }
                return new List<RevenueOfMonth>();
            }
        }
        public static IEnumerable<Models.RevenueOfYear> GetRevenueOfYear(string state = "", string year = "", string filterName = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInYear(year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.Instance.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listUser = from l in model.AppNetUserTypes select l;
                    if (!string.IsNullOrEmpty(filterName))
                        listUser = listUser.Where(a => a.UserOfName.Contains(filterName) || a.DisplayName.Contains(filterName));
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in listUser.ToList() on u.UserName equals a.UserOfName
                                  join
                                  l in model.LocationSubs.ToList() on a.LocationSubID equals l.ID
                                  group od by new
                                  {
                                      od.IDAccount,
                                      a.DisplayName,
                                      l.Name
                                  } into g
                                  select new RevenueOfYear()
                                  {
                                      AccountName = g.Key.DisplayName,
                                      Total = g.Sum(s => s.Total),
                                      LocationSub = g.Key.Name
                                  };

                    return listRpt.ToList();
                }
                return new List<RevenueOfYear>();
            }
        }
        public static IEnumerable<Models.RevenueOfQuater> RevenueOfQuarter(string state = "", string quarter = "", string year = "", string filterName = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInQuarter(quarter, year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.Instance.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listUser = from l in model.AppNetUserTypes select l;
                    if (!string.IsNullOrEmpty(filterName))
                        listUser = listUser.Where(a => a.UserOfName.Contains(filterName) || a.DisplayName.Contains(filterName));
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in listUser.ToList() on u.UserName equals a.UserOfName
                                  join
                                  l in model.LocationSubs.ToList() on a.LocationSubID equals l.ID
                                  group od by new
                                  {
                                      od.IDAccount,
                                      a.DisplayName,
                                      l.Name
                                  } into g
                                  select new RevenueOfQuater()
                                  {
                                      AccountName = g.Key.DisplayName,
                                      Total = g.Sum(s => s.Total),
                                      LocationSub = g.Key.Name
                                  };

                    return listRpt.ToList();
                }
                return new List<RevenueOfQuater>();
            }
        }
        public static IEnumerable<PaymentOfDay> GetPaymentOfDay(string dateFrom = "", string dateTo = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {

                var list = from p in model.PaymentDetails select p;
                if (!string.IsNullOrEmpty(dateFrom) & !string.IsNullOrEmpty(dateTo))
                {
                    dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                    dateTo = UtilDatetime.ToTime(dateTo).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateFrom) >= 0 &&
                                               String.Compare(a.PayDate, dateTo) <= 0);
                }
                else if (!string.IsNullOrEmpty(dateFrom))
                {
                    dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateFrom) >= 0);
                }
                else if (!string.IsNullOrEmpty(dateTo))
                {
                    dateTo = UtilDatetime.ToTime(dateTo).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateTo) <= 0);
                }

                var listPay = from p in list.ToList()
                              select new PaymentOfDay()
                              {
                                  Date = DateTime.ParseExact(p.PayDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"),

                                  Total = (decimal)p.Pay
                              };
                //Group by ngay
                listPay = from p in listPay.ToList()
                          group p by new
                          {
                              p.Date
                          } into g
                          select new PaymentOfDay()
                          {
                              Date = g.Key.Date,
                              Total = g.Sum(a => a.Total)
                          };
                return listPay.OrderBy(a => a.Date);
            }
        }
        public static IEnumerable<PaymentOfStore> GetPaymentOfStore(string dateFrom = "", string dateTo = "", string filterName = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {

                var list = from p in model.PaymentDetails select p;
                if (!string.IsNullOrEmpty(dateFrom) & !string.IsNullOrEmpty(dateTo))
                {
                    dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                    dateTo = UtilDatetime.ToTime(dateTo).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateFrom) >= 0 &&
                                               String.Compare(a.PayDate, dateTo) <= 0);
                }
                else if (!string.IsNullOrEmpty(dateFrom))
                {
                    dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateFrom) >= 0);
                }
                else if (!string.IsNullOrEmpty(dateTo))
                {
                    dateTo = UtilDatetime.ToTime(dateTo).ToString("yyyyMMddHHmmss");
                    list = list.Where(a => String.Compare(a.PayDate, dateTo) <= 0);
                }
                var listUser = from l in model.AspNetUsers select l;
                if (!string.IsNullOrEmpty(filterName))
                    listUser = listUser.Where(a => a.UserName.Contains(filterName));
                var listPay = from p in list.ToList()
                              join a in listUser on p.IDAccount equals a.Id
                              group p by new
                              {
                                  p.IDAccount,
                                  a.UserName
                              } into g
                              select new PaymentOfStore()
                              {
                                  Account = g.Key.UserName,
                                  Total = (decimal)g.Sum(p => p.Pay)
                              };

                return listPay.ToList();
            }
        }
    }
}