﻿using MVCProject.Common;
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
        [HttpGet]
        public ActionResult index(int? page, int? size, string filter, string order, string dateFrom, string dateTo, string _idaccount)
        {
            if (string.IsNullOrEmpty(_idaccount))
            {
                _idaccount = Request.QueryString["idaccount"].ToString();
            }
            List<string> listUsers = null;
            var lstUsers = modelAspnet.AspNetUsers.ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                listUsers = lstUsers.Where(a => a.UserName.Contains(filter)).Select(a => a.Id).ToList();
            }
            var listPaymentDetail = GetList(dateFrom, dateTo, _idaccount, listUsers);
        
            ViewData["Total"] = listPaymentDetail.ToList().Sum(a => a.Pay);
            ViewData["ListUsers"] = lstUsers.Where(a => listPaymentDetail.Select(p => p.IDAccountInput).Contains(a.Id)).ToList();//modelAspnet.AspNetUsers.Where(a => listPaymentDetail.Select(b => b.IDAccountInput).ToList().Contains(a.Id)).ToList();
            ViewBag.UserName = lstUsers.Where(a => a.Id == _idaccount).FirstOrDefault().UserName;
            ViewData["idaccount"] = _idaccount;
            ViewBag.Order = order == null ? "" : order;
            ViewBag.Filter = filter == null ? "" : filter;
            return View(listPaymentDetail.ToList().ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 1 : (int)size));
        }
        public ActionResult Home(int? page, int? size, string filter, string order, string dateFrom, string dateTo, string idaccount)
        {
            idaccount = User.Identity.GetUserId();

            var listPaymentDetail = GetList(dateFrom, dateTo, idaccount, null);
            ViewData["Total"] = listPaymentDetail.ToList().Sum(a => a.Pay);
            ViewBag.UserName = modelAspnet.AspNetUsers.Where(a => a.Id == idaccount).FirstOrDefault().UserName;

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
        private IEnumerable<Models.PaymentDetail> GetList(string dateFrom, string dateTo, string _idaccount,List<string> listUsers)
        {
            var listPaymentDetail = from l in modelAspnet.PaymentDetails where l.IDAccount == _idaccount select l;
            if (listUsers != null)
                listPaymentDetail = listPaymentDetail.Where(a => listUsers.Contains(a.IDAccountInput));
            if (!string.IsNullOrEmpty(dateFrom) & !string.IsNullOrEmpty(dateTo))
            {
                dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                dateTo = UtilDatetime.ToTime(dateTo).ToString("yyyyMMddHHmmss");
                listPaymentDetail = listPaymentDetail.Where(a => String.Compare(a.PayDate, dateFrom) >= 0 &&
                                                                String.Compare(a.PayDate, dateTo) <= 0);
            }
            else if (!string.IsNullOrEmpty(dateFrom))
            {
                dateFrom = UtilDatetime.FromTime(dateFrom).ToString("yyyyMMddHHmmss");
                listPaymentDetail = listPaymentDetail.Where(a => String.Compare(a.PayDate, dateFrom) >= 0);
            }
            else if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = UtilDatetime.FromTime(dateTo).ToString("yyyyMMddHHmmss");
                listPaymentDetail = listPaymentDetail.Where(a => String.Compare(a.PayDate, dateTo) <= 0);
            }
            return listPaymentDetail;
        }
    }
}