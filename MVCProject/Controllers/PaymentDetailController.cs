using MVCProject.Common;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class PaymentDetailController : Controller
    {
        private retailEntities modelRetail = Params.ModelRetail;
        private aspnetEntities modelAspnet = Params.ModelaspnetEntities;
        // GET: PaymentDetail
        public ActionResult index()
        {
            string idaccount = Request.QueryString["idaccount"];
            var listPaymentDetail = modelAspnet.PaymentDetails.Where(a => a.IDAccount == idaccount).ToList();
            ViewBag.IDAccount = modelAspnet.AspNetUsers.Where(a => a.Id == idaccount).FirstOrDefault().UserName;

            return View(listPaymentDetail);
        }
    }
}