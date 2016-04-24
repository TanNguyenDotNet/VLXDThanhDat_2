using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using MVCProject.Models.AccessData;
using PagedList;

namespace MVCProject.Controllers
{
    public class ReportsSalesController : Controller
    {
        // GET: ReportsSales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RevenueInvoice(int? page,int? size, string filter, string dateFrom, string dateTo)
        {
            var list = AReportSales.GetRevenueInvoice(filter, "2", dateFrom, dateTo);
            TempData["ExportExcel"] = list;
            ViewBag.Filter = filter;
            return View(list.ToPagedList(page == null ||
                page == 0 ? 1 : (int)page, size == null || size == 0 ? 50 : (int)size));
        }
        public ActionResult ExportExcel()
        {
            var test = string.Format("{2}", "te");
            var buffer = Common.ExcelUtils.ExportByteExcel((List<RevenueInvoice>)TempData["ExportExcel"],"Mã đơn hàng","Tên đại lý","Ngày lập đơn hàng","Tổng tiền");
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelDemo.xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer);
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return RedirectToAction("RevenueInvoice");
        }
    }
}