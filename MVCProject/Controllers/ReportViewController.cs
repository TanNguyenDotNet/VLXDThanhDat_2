﻿using MVCProject.Common;
using MVCProject.Models;
using MVCProject.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MVCProject.Controllers
{
    public class ReportViewController : ControllerBase
    {
        // GET: ReportView
        private retailEntities db = new retailEntities();
        private aspnetEntities _db = new aspnetEntities();
        private InvoiceDetailParams InvoiceParams;
        private byte[] byte_pdf = null;  
        public ActionResult ViewPDF()
        {
            return File((byte[])TempData["bytePDF"], "application/pdf");
        }
        public ActionResult InvoiceDetails()
        {
            dynamic model = new ExpandoObject();

            using (var _modelAsp = Params.ModelaspnetEntities)
            {
                using (var _model = Params.ModelRetail)
                {
                    string code = Request.QueryString["code"];
                    var _OrdersDetails = _model.OrdersDetails.Where(c => c.OrderCode == code).ToList();
                    var _Orders = _model.Orders.Where(c => c.OrderCode == code).FirstOrDefault();

                    var us = _modelAsp.AspNetUsers.Single(c => c.Id == _Orders.IDAccount);
                    string enu = Security.EncryptString("User:" + us.UserName + "~FrontendUser", false, EncryptType.TripleDES);
                    var u = _modelAsp.AppNetUserTypes.Find(enu);
                    var l = _modelAsp.Locations.Find(u.LocationID);
                    var query = (from a in _OrdersDetails
                                 join b in _modelAsp.Products.ToList() on a.IDProduct equals b.ID
                                 select new
                                 {
                                     _ProductCode = a.IDProduct.ToString("000000"),
                                     _Amount = a.Amount,
                                     _Price = a.Price,
                                     _Tax = a.Tax,
                                     _Total = a.Total,
                                     _ProductName = b.ProductName,
                                     _Unit = b.UnitName,
                                     _Discount = a.Discount == null ? "0": a.Discount.Value.ToString("n0")
                                 }).ToList();

                    InvoiceParams = new InvoiceDetailParams();
                    InvoiceParams.Address = u.Address + ", Q." + u.District;
                    InvoiceParams.CellPhone = u.Phone;
                    InvoiceParams.Contact = "_";
                    InvoiceParams.Discount = "_";
                    InvoiceParams.Name = u.DisplayName;
                    InvoiceParams.Total = Math.Round(_Orders.TotalWithoutTax).ToString("n0");
                    InvoiceParams.TotalVAT = Math.Round(_Orders.Total).ToString("n0");
                    InvoiceParams.VAT = decimal.Parse(_Orders.Tax).ToString("n0");
                    InvoiceParams.InvoiceNum = _Orders.OrderCode;
                    InvoiceParams.InvoiceDate = DateTime.ParseExact(_Orders.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    InvoiceParams.Payment = Math.Round((decimal)TotalPay(us.Id));
                    InvoiceParams.Debt = (decimal)InvoiceParams.Payment - _Orders.Total;
                    InvoiceParams.AmountInWord = Common.ConvertNumToWord.So_chu(Int64.Parse(InvoiceParams.Payment.ToString().Replace(".00", "").Replace("-", "")));

                    ViewData["InvoiceRptParams"] = InvoiceParams;
                    model.InvoiceDetail = UtilEntities.modelDynamic(query);
                    return View(model);
                }
            }

        }
        #region function     
        private decimal? TotalPay(string id)
        {
            var listPaymentDetail = (from l in _db.PaymentDetails where l.IDAccount == id select l).ToList();
            var listOrder = (from o in db.Orders
                             where o.IDAccount == id
                             select o).ToList();
            decimal? total = listOrder.Sum(a => a.Total) - listPaymentDetail.Sum(a => a.Pay);
            return total == null ? 0 : total;
        }
        #endregion
    }
}