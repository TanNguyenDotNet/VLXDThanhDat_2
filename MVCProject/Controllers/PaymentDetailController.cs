using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MVCProject.Controllers
{
    public class PaymentDetailController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: PaymentDetail
        public ActionResult index()
        {
            string idaccount = Request.QueryString["idaccount"].ToString();
            var listPaymentDetail = modelAspnet.PaymentDetails.Where(a => a.IDAccount == idaccount).ToList();
            ViewBag.IDAccount = modelAspnet.AspNetUsers.Where(a => a.Id == idaccount).FirstOrDefault().UserName;

            return View(listPaymentDetail);
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