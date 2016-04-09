using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class PaymentController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: Payment
        public ActionResult Index()
        {
            string userEnctypt = "";
            var listUser = modelAspnet.AspNetUsers.ToList();
            var listUserType = modelAspnet.AppNetUserTypes.ToList();
            List<string> lstType = new List<string>();
            List<string> lstUser = new List<string>();
            for (int i = 0; i < listUser.Count; i++)
            {
                userEnctypt = MVCProject.Common.Security.EncryptString("User:" + listUser[i].UserName + "~FrontendUser",
                            false, MVCProject.Common.EncryptType.TripleDES);
                if(listUserType.Where(a=>a.Username==userEnctypt).FirstOrDefault()!=null)
                {
                    lstType.Add(listUserType.Where(a => a.Username == userEnctypt).FirstOrDefault().Username);
                    lstUser.Add(listUser[i].Id);
                }
            }
            listUserType = listUserType.Where(a => lstType.Contains(a.Username)).ToList();
            listUser = listUser.Where(a => lstUser.Contains(a.Id)).ToList();
            ViewData["SumOfOrders"] = SumOfOrders(lstUser);
            ViewData["SumOfPaymentDetail"] = SumOfPaymentDetail(lstUser);
            ViewData["LocationSub"] = Params.listLocationSub.Where(a => listUserType.Select(p => p.LocationSubID).ToList().Contains(a.ID)).ToList();
            ViewData["UsersType"] = listUserType;
            return View(listUser);
        }
        #region function
        private Dictionary<string, decimal> SumOfOrders(List<string> lstUser)
        {
            var listOrder = from o in modelRetail.Orders
                            where lstUser.Contains(o.IDAccount)
                            group o by new
                            {
                                o.IDAccount
                            } into g
                            select new
                            {
                                idaccount = g.Key.IDAccount,
                                total = g.Sum(x => x.Total)
                            };
            return listOrder.ToDictionary(a => a.idaccount, a => a.total);
        }
        private Dictionary<string, decimal?> SumOfPaymentDetail(List<string> lstUser)
        {
            var listPaymentDetail = from o in modelAspnet.PaymentDetails
                                    where lstUser.Contains(o.IDAccount)
                                    group o by new
                                    {
                                        o.IDAccount
                                    } into g
                                    select new
                                    {
                                        idaccount = g.Key.IDAccount,
                                        totalpay = g.Sum(x => x.Pay)
                                    };
            return listPaymentDetail.ToDictionary(a => a.idaccount, a => a.totalpay);
        }
        #endregion
    }
}