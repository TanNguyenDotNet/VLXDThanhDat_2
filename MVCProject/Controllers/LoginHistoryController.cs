using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models.AccessData;
using Microsoft.AspNet.Identity;
using MVCProject.Common;
using MVCProject.Models;
using MVCProject.Models.ModelView;

namespace MVCProject.Controllers
{
    public class LoginHistoryController : ControllerBase
    {
        // GET: LoginHistory
        public ActionResult Index(string page = "", string size = "", string acc = "", string datefrom = "", string dateto = "")
        {
            if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), ""))
                return RedirectToAction("AccessDenied", "Account");
            var list = ALoginHistory.Instance.GetList(page, size, acc, datefrom, dateto);
            TempData["ExportExcel"] = list;
            return View(list);
        }
        public ActionResult ExportExcel()
        {
            if (!Commons.CheckLogin(Request, Response, User.Identity.GetUserName()))
                return null;
            if (!Commons.CheckPermission(ViewData, Params.ModelaspnetEntities, User.Identity.GetUserName(), "24"))
                return RedirectToAction("AccessDenied", "Account");
            byte[] buffer = null;
            buffer = ExcelUtils.ExportByteExcel((List<LoginHistory>)TempData["ExportExcel"], "Code", "TaiKhoan", "TongSoLanDangNhap");


            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}{1:_dd_MM_yyyy_HH_mm_ss}.xlsx", "LichSuTruyCap", DateTime.Now));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer);
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return RedirectToAction(TempData["action"].ToString());
        }
    }
}