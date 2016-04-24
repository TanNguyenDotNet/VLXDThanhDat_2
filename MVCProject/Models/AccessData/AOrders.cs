using MVCProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class AOrders
    {
        public static IEnumerable<Models.Order> GetList(string filter = "", string state = "", string datefrom = "", string dateto = "")
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