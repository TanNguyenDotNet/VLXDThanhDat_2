using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCProject.Report
{
    public class ShowReport
    {
        //Parameters check allow null
        public static ReportViewer ProcessShowReport(string ReportPath, string DataSetName, DataTable _ReportDataSource, List<ReportParameter> rptParams,out byte[] _byte)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.ShowPrintButton = true;
            reportViewer.LocalReport.ReportPath = ReportPath;
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(DataSetName, _ReportDataSource));
            reportViewer.LocalReport.SetParameters(rptParams);
            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;
            //_byte = reportViewer.LocalReport.Render("pdf", null, out mimeType, out encoding, 
            //    out extension, 
            //   out streamids, out warnings);
            _byte = reportViewer.LocalReport.Render("pdf");
            return reportViewer;
        }
    }
}