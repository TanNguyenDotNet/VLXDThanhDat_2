using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class AReportSales
    {
        public static IEnumerable<Models.RevenueInvoice> GetRevenueInvoice(string filter = "", string state = "", string datefrom = "", string dateto = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var listOrder = AOrders.GetList(filter, state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in model.AppNetUserTypes on u.UserName equals a.UserOfName
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
        public static IEnumerable<Models.RevenueOfMonth> GetRevenueOfMonth(string state = "", string month = "", string year = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInMonth(month, year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in model.AppNetUserTypes.ToList() on u.UserName equals a.UserOfName
                                  join
                                  l in model.LocationSubs.ToList() on a.LocationSubID equals l.ID
                                  group od by new
                                  {
                                      od.IDAccount,
                                      a.DisplayName,
                                      l.Name
                                  } into g
                                  select new RevenueOfMonth()
                                  {
                                      AccountName = g.Key.DisplayName,
                                      Total = g.Sum(s => s.Total),
                                      LocationSub = g.Key.Name
                                  };

                    return listRpt.ToList();
                }
                return new List<RevenueOfMonth>();
            }
        }
        public static IEnumerable<Models.RevenueOfYear> GetRevenueOfYear(string state = "", string year = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInYear(year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in model.AppNetUserTypes.ToList() on u.UserName equals a.UserOfName
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
        public static IEnumerable<Models.RevenueOfQuater> RevenueOfQuarter(string state = "", string quarter = "", string year = "")
        {
            using (var model = Params.ModelaspnetEntities)
            {
                List<DateTime> lstTime = UtilDatetime.ListGetBeginDayAndEndDayInQuarter(quarter, year);
                string datefrom = lstTime[0].ToString(new System.Globalization.CultureInfo("vi-VN")),
                    dateto = lstTime[1].ToString(new System.Globalization.CultureInfo("vi-VN"));
                var listOrder = AOrders.GetList("", state, datefrom, dateto);
                if (listOrder.Count() > 0)
                {
                    var listRpt = from od in listOrder.ToList()
                                  join
                                      u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                                  join
                                  a in model.AppNetUserTypes.ToList() on u.UserName equals a.UserOfName
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
    }
}