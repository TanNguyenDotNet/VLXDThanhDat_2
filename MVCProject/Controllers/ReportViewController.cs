using MVCProject.Common;
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

namespace MVCProject.Controllers
{
    public class ReportViewController : Controller
    {
        // GET: ReportView
        private retailEntities db = new retailEntities();
        private aspnetEntities _db = new aspnetEntities();
        private Datasets.DataSetRetails dsDetail = new Datasets.DataSetRetails();
        private InvoiceDetailRptParams InvoiceRptParams;
        private InvoiceDetailParams InvoiceParams;
        private byte[] byte_pdf = null;
        public ActionResult Index()
        {
            string path = Request.MapPath(Request.ApplicationPath) + @"Report\ReportOrderDetail.rdlc";
            GetDetail();
            ViewBag.ReportViewer = ShowReport.ProcessShowReport(path, "DataSetOrderDetail", dsDetail.Tables["OrdersDetail"], InvoiceRptParams.rptParams,out byte_pdf);
            TempData["bytePDF"] = byte_pdf;
            return View();
        }
        public ActionResult ViewPDF()
        {
            return File((byte[])TempData["bytePDF"], "application/pdf");
        }
        public ActionResult InvoiceDetail()
        {
            dynamic model = new ExpandoObject();

            using (var _modelAsp= Params.ModelaspnetEntities)
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
                                 join b in _modelAsp.Products.ToList() on a.ProductCode equals b.ItemCode
                                 select new {
                                     _ProductCode = a.ProductCode, 
                                     _Amount = a.Amount, 
                                     _Price = a.Price, 
                                     _Tax=a.Tax,
                                     _Total=a.Total,
                                     _ProductName=b.ProductName,
                                     _Unit=b.UnitName
                                    }).ToList();
                   
                    InvoiceParams = new InvoiceDetailParams();
                    InvoiceParams.Address = u.Address + ", Q." + u.District;
                    InvoiceParams.AmountInWord = Common.ConvertNumToWord.So_chu(Int64.Parse(_Orders.Total.ToString().Replace(".00", "")));
                    InvoiceParams.CellPhone = u.Phone;
                    InvoiceParams.Contact = "_";
                    InvoiceParams.Discount = "_";
                    InvoiceParams.Name = u.DisplayName;
                    InvoiceParams.Total = _Orders.TotalWithoutTax.ToString("n0");
                    InvoiceParams.TotalVAT = _Orders.Total.ToString("n0");
                    InvoiceParams.VAT = decimal.Parse(_Orders.Tax).ToString("n0");
                    InvoiceParams.InvoiceNum = _Orders.OrderCode;
                    InvoiceParams.InvoiceDate = DateTime.ParseExact(_Orders.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    InvoiceParams.Payment = TotalPay(us.Id);
                    InvoiceParams.Debt = (decimal)InvoiceParams.Payment - _Orders.Total;

                    ViewData["InvoiceRptParams"] = InvoiceParams;
                    model.InvoiceDetail = UtilEntities.modelDynamic(query);
                    return View(model);
                }
            }
            
        }
        #region function
        List<OrdersDetail> GetDetail()
        {
            string code = Request.QueryString["code"];
            if (code == null) return null;
            var o = db.Orders.Where(c => c.OrderCode == code).FirstOrDefault();
            var list = db.OrdersDetails.Where(c => c.OrderCode == code).ToList();
            var us = _db.AspNetUsers.Single(c => c.Id == o.IDAccount);
            string enu = Security.EncryptString("User:" + us.UserName + "~FrontendUser", false, EncryptType.TripleDES);
            var u = _db.AppNetUserTypes.Find(enu);
            var l = _db.Locations.Find(u.LocationID);

            foreach (var item in list)
            {
                DataRow dr = dsDetail.OrdersDetail.NewRow();
                dr["IDProduct"] = item.IDProduct;
                dr["ProductCode"] = item.ProductCode;
                dr["Amount"] = item.Amount;
                dr["Price"] = item.Price;
                dr["Tax"] = item.Tax;
                dr["Total"] = item.Total;
                //dr["ProductName"] = ((List<Product>)HttpContext.Application["listProduct"]).Where(a => a.ItemCode == item.ProductCode).FirstOrDefault().ProductName;
                //dr["Unit"] = ((List<Product>)HttpContext.Application["listProduct"]).Where(a => a.ItemCode == item.ProductCode).FirstOrDefault().Unit;
                dsDetail.OrdersDetail.Rows.Add(dr);
            }
            InvoiceRptParams = new InvoiceDetailRptParams();
            InvoiceRptParams.Address = u.Address + ", Q." + u.District;
            InvoiceRptParams.AmountInWord = Common.ConvertNumToWord.So_chu(Int64.Parse(o.Total.ToString().Replace(".00", "")));
            InvoiceRptParams.CellPhone = u.Phone;
            InvoiceRptParams.Contact = "_";
            InvoiceRptParams.Discount = "_";
            InvoiceRptParams.Name = u.DisplayName;
            InvoiceRptParams.Total = o.TotalWithoutTax.ToString("n0");
            InvoiceRptParams.TotalVAT = o.Total.ToString("n0");
            InvoiceRptParams.VAT = o.Tax.ToString();
            InvoiceRptParams.InvoiceNum = o.OrderCode;
            InvoiceRptParams.InvoiceDate = DateTime.ParseExact(o.DateCreate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");

            return list;
        }
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