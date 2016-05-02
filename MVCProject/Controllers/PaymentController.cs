using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;

namespace MVCProject.Controllers
{
    public class PaymentController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: Payment
        public ActionResult Index(int? page, int? size, string filter)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), "21"))
                return RedirectToAction("AccessDenied", "Account");
            string userEnctypt = "";
            var _listUser = from l in modelAspnet.AspNetUsers select l;
            if (!string.IsNullOrEmpty(filter))
            { _listUser = _listUser.Where(a => a.UserName.Contains(filter));}
            if (_listUser.Count()<1)
                return View(_listUser.ToList().ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
             var listUser = _listUser.ToList();
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
            ViewData["LocationSub"] = modelAspnet.LocationSubs.ToList().Where(a => listUserType.Select(p => p.LocationSubID).ToList().Contains(a.ID)).ToList();
            ViewData["UsersType"] = listUserType;
            return View(listUser.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
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