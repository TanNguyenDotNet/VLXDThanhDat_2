﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using MVCProject.Models.AccessData;
using PagedList;
using MVCProject.Common;
using Microsoft.AspNet.Identity;

namespace MVCProject.Controllers
{
    public class ReportsSalesController : Controller
    {
        // GET: ReportsSales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RevenueInvoice(int? page, int? size, string filter, string dateFrom, string dateTo)
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            //if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), null))
            //    return RedirectToAction("AccessDenied", "Account");
            
            var list = AReportSales.GetRevenueInvoice(filter, "2", dateFrom, dateTo);

            TempData["ExportExcel"] = list;
            TempData["header"] = new string[] { "Tên đại lý", "Ngày lập đơn hàng", "Mã đơn hàng", "Tổng tiền" };
            TempData["action"] = "RevenueInvoice";
            ViewBag.Filter = filter;

            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 50 : (int)size));
        }
        public ActionResult RevenueOfMonth(int? page, int? size, string month = "1", string year = "2016")
        {
            if (!Common.Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            //if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), null))
            //    return RedirectToAction("AccessDenied", "Account");
            var list = AReportSales.GetRevenueOfMonth("2", month, year);
            TempData["ExportExcel"] = list;
            TempData["header"] = new string[] { "Tên đại lý", "Khu vực", "Tổng tiền" };
            TempData["action"] = "RevenueOfMonth";
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 50 : (int)size));
        }
        public ActionResult ExportExcel()
        {
            byte[] buffer = null;
            switch (TempData["action"].ToString())
            {
                case "RevenueInvoice":
                    buffer = Common.ExcelUtils.ExportByteExcel((List<RevenueInvoice>)TempData["ExportExcel"], (string[])TempData["header"]);
                    break;
                case "RevenueOfMonth":
                    buffer = Common.ExcelUtils.ExportByteExcel((List<RevenueOfMonth>)TempData["ExportExcel"], (string[])TempData["header"]);
                    break;
            }

            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer);
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return RedirectToAction(TempData["action"].ToString());
        }
    }
}