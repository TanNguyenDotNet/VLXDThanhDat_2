using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Report
{
    public class InvoiceDetailParams
    {
        public string InvoiceNum { set; get; }
        public string InvoiceDate { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string CellPhone { set; get; }
        public string Contact { set; get; }
        public string Total { set; get; }
        public string Discount { set; get; }
        public string VAT { set; get; }
        public string TotalVAT { set; get; }
        public string AmountInWord { set; get; }
        public decimal? Payment { get; set; }
        public decimal Debt { get; set; }
    }
}