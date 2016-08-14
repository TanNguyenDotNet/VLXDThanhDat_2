using MVCProject.Common;
using MVCProject.Models.ModelView;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.AccessData
{
    public class APaymentDetails
    {
        private volatile static APaymentDetails _instance;
        static object locked = new object();
        public static APaymentDetails Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locked)
                        _instance = new APaymentDetails();
                }
                return _instance;
            }

        }
        private APaymentDetails()
        { }
        public List<PaymentDetail> GetList()
        {
            using (var model = Params.ModelaspnetEntities)
            {
                var list = model.PaymentDetails.ToList();
                return list;
            }
        }
        public IPagedList<PaymentDetailsView> GetListPaging(string page, string size, string name, string locationsub, UsersView listusers)
        {
            using (var model = Params.ModelaspnetEntities)
            {
                using (var modeldetails = Params.ModelRetail)
                {
                    if (!string.IsNullOrEmpty(name))
                        listusers.ListAppUsers = listusers.ListAppUsers.Where(a => a.DisplayName.Contains(name) || a.UserOfName.Contains(name)).ToList();
                    if (locationsub != "0")
                    { int _location = int.Parse(locationsub); listusers.ListAppUsers = listusers.ListAppUsers.Where(a => a.LocationSubID == _location).ToList(); }

                    listusers.ListUsers = (from u in model.AspNetUsers.ToList() join a in listusers.ListAppUsers on u.UserName equals a.UserOfName select u).ToList();
                    var listpayment = from u in listusers.ListUsers
                                      join
                                      p in model.PaymentDetails.ToList() on u.Id equals p.IDAccount into pay
                                      from pays in pay.DefaultIfEmpty()
                                      select new
                                      {
                                          idaccount = u.Id,
                                          payment = (pays == null ? 0 : (decimal)pays.Pay)
                                      };
                    var grouppayment = from pays in listpayment group pays by pays.idaccount into g select new { g.Key, payment = g.Sum(a => a.payment) };

                    var orders = from u in listusers.ListUsers
                                 join od in modeldetails.Orders.ToList() on u.Id equals od.IDAccount into odg
                                 from ods in odg.DefaultIfEmpty()
                                 select new
                                 {
                                     idaccount = u.Id,
                                     total = (ods == null ? 0 : (decimal)ods.Total)

                                 };

                    var grouporders = from od in orders group od by od.idaccount into g select new { g.Key, total = g.Sum(a => a.total) };

                    var paydetails = (from u in listusers.ListAppUsers
                                      join app in listusers.ListUsers on u.UserOfName equals app.UserName
                                      join gpay in grouppayment on app.Id equals gpay.Key
                                      join gorder in grouporders on app.Id equals gorder.Key
                                      join l in model.LocationSubs.ToList() on u.LocationSubID equals l.ID
                                      select new PaymentDetailsView
                                      {
                                          Username = u.UserOfName,
                                          Locationname = l.Name,
                                          Debt = gorder.total,
                                          Payment = gpay.payment,
                                          Totaldebt = gorder.total - gpay.payment
                                      }).ToList();

                    int _page = page == "" ? 1 : int.Parse(page);
                    int _size = size == "" ? 50 : int.Parse(size);
                    return paydetails.OrderBy(a => a.Locationname).ThenBy(a => a.Username).ToPagedList(_page, _size);
                }
            }
        }
    }
}