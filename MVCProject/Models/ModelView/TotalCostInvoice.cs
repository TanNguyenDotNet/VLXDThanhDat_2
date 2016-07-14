using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.ModelView
{
    public class TotalCostInvoice
    {
        private string _OrderCode;

        public string OrderCode
        {
            get { return _OrderCode; }
            set { _OrderCode = value; }
        }
        private decimal _TotalCost;

        public decimal TotalCost
        {
            get { return _TotalCost; }
            set { _TotalCost = value; }
        }

    }
}