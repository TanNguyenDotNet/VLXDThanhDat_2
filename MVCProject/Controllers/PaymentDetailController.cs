using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;

namespace MVCProject.Controllers
{
    public class PaymentDetailController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: PaymentDetail
        public ActionResult index(int? page, int? size, string filter, string order, string dateFrom, string dateTo, string _idaccount)
        {
            string idaccount=_idaccount;
            if (string.IsNullOrEmpty(_idaccount))
                idaccount = Request.QueryString["idaccount"].ToString();
            
            var listPaymentDetail = from l in modelAspnet.PaymentDetails where l.IDAccount == idaccount select l;
            if (dateFrom != null)
            {
                dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmm");
                listPaymentDetail = modelAspnet.PaymentDetails.Where(a => DateTime.Parse(a.PayDate) >= DateTime.Parse(dateFrom));
            }
            if (dateTo != null)
                dateTo = UtilDatetime.FromTime(dateTo).ToString("yyyyMMddHHmm");
            
            var lststring = modelAspnet.AspNetUsers.ToList();
            ViewData["Total"] = listPaymentDetail.Sum(a => a.Pay);
            ViewData["ListUsers"] = lststring.Where(a => listPaymentDetail.Select(p=>p.IDAccountInput).Contains(a.Id)).ToList();//modelAspnet.AspNetUsers.Where(a => listPaymentDetail.Select(b => b.IDAccountInput).ToList().Contains(a.Id)).ToList();
            ViewBag.IDAccount = modelAspnet.AspNetUsers.Where(a => a.Id == idaccount).FirstOrDefault().UserName;

            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            return View(listPaymentDetail.ToList().ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 20 : (int)size));
        }
        public ActionResult Payment()
        {
            TempData["idaccount"] = Request.QueryString["PayID"].ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment([Bind(Include = "ID,IDAccount,IDAccountInput,IDLocationSub,Pay,PayDate,PayDateSystem,PayBefore,PayAfter,Description")] PaymentDetail Payment)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
           

            if (ModelState.IsValid)
            {
                Payment.ID = Guid.NewGuid();
                Payment.IDAccount = TempData["idaccount"].ToString();
                Payment.PayDate = DateTime.Parse(Payment.PayDate, new System.Globalization.CultureInfo("vi-VN")).ToString("yyyyMMddHHmm");
                Payment.Pay = Payment.Pay;
                Payment.PayDateSystem = DateTime.Now.ToString("yyyyMMddHHmm");
                Payment.IDAccountInput = User.Identity.GetUserId();
                modelAspnet.PaymentDetails.Add(Payment);
                modelAspnet.SaveChanges();
                Response.Redirect("~/PaymentDetail/index?idaccount=" + Payment.IDAccount);
                return null;
            }

            return View(Payment);
        }
    }
}