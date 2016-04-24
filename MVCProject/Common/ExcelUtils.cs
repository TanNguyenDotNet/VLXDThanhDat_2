using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCProject.Common
{
    public static class ExcelUtils
    {
        private static Stream CreateExcelFile<T>(List<T> list,string[] header,Stream stream = null)
        {
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Hanker";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "EPP test background";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "This is my fucking generated Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet.Cells[1, 1].LoadFromCollection(list, true, TableStyles.Dark9);
                BindingFormatForExcel(workSheet, list, header);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }
        private static void BindingFormatForExcel<T>(ExcelWorksheet worksheet, List<T> listItems,string[] header)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 15;
            // Tự động xuống hàng khi text quá dài
            //worksheet.Cells.Style.WrapText = true;
            // Tạo header
            for (int i = 1; i <= header.Count(); i++)
            {
                worksheet.Cells[1, i].Value = header[i].ToString();
            }

            // Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            //using (var range = worksheet.Cells["A1:D1"])
            //{
            //    // Set PatternType
            //    range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
            //    // Set Màu cho Background
            //    range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
            //    // Canh giữa cho các text
            //    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //    // Set Font cho text  trong Range hiện tại
            //    range.Style.Font.SetFromFont(new Font("Arial", 10));
            //    // Set Border
            //    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            //    // Set màu ch Border
            //    range.Style.Border.Bottom.Color.SetColor(Color.Blue);
            //}

            //// Đỗ dữ liệu từ list vào 
            //for (int i = 0; i < listItems.Count; i++)
            //{
            //    var item = listItems[i];
            //    worksheet.Cells[i + 2, 1].Value = item.Id + 1;
            //    worksheet.Cells[i + 2, 2].Value = item.FullName;
            //    worksheet.Cells[i + 2, 3].Value = item.Address;
            //    worksheet.Cells[i + 2, 4].Value = item.Money;
            //    // Format lại color nếu như thỏa điều kiện
            //    if (item.Money > 20050)
            //    {
            //        // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            //        using (var range = worksheet.Cells[i + 2, 1, i + 2, 4])
            //        {
            //            // Format text đỏ và đậm
            //            range.Style.Font.Color.SetColor(Color.Red);
            //            range.Style.Font.Bold = true;
            //        }
            //    }

            //}
            //// Format lại định dạng xuất ra ở cột Money
            //worksheet.Cells[2, 4, listItems.Count + 4, 4].Style.Numberformat.Format = "$#,##.00";
            //// fix lại width của column với minimum width là 15
            //worksheet.Cells[1, 1, listItems.Count + 5, 4].AutoFitColumns(15);

            //// Thực hiện tính theo formula trong excel
            //// Hàm Sum 
            //worksheet.Cells[listItems.Count + 3, 3].Value = "Total is :";
            //worksheet.Cells[listItems.Count + 3, 4].Formula = "SUM(D2:D" + (listItems.Count + 1) + ")";
            //// Hàm SumIf
            //worksheet.Cells[listItems.Count + 4, 3].Value = "Greater than 20050 :";
            //worksheet.Cells[listItems.Count + 4, 4].Formula = "SUMIF(D2:D" + (listItems.Count + 1) + ",\">20050\")";
            //// Tinh theo %
            //worksheet.Cells[listItems.Count + 5, 3].Value = "Percentatge: ";
            //worksheet.Cells[listItems.Count + 5, 4].Style.Numberformat.Format = "0.00%";
            //// Dòng này có nghĩa là ở column hiện tại lấy với địa chỉ (Row hiện tại - 1)/ (Row hiện tại - 2) Cùng một colum
            //worksheet.Cells[listItems.Count + 5, 4].FormulaR1C1 = "(R[-1]C/R[-2]C)";
        }
        public static byte[] ExportByteExcel<T>(List<T> list, params object[] args)
        {
            string[] header = (string[])args;
            var stream = CreateExcelFile(list,header);

            var buffer = stream as MemoryStream;
            return buffer.ToArray();
        }
    }
}