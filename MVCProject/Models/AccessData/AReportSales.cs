﻿using MVCProject.Common;
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
                var listRpt = from od in listOrder.ToList()
                              join
                                  u in model.AspNetUsers.ToList() on od.IDAccount equals u.Id
                              join
                              a in model.AppNetUserTypes on u.UserName equals a.UserOfName
                              select new RevenueInvoice()
                              {
                                  AccountName = a.DisplayName,
                                  DateCreate = od.DateCreate,
                                  OrderCode = od.OrderCode,
                                  Total = od.Total
                              };
                return listRpt.ToList();
            }
        }
    }
}