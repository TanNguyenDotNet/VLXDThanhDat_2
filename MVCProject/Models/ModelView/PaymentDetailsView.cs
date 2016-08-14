using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject.Models.ModelView
{
    public class PaymentDetailsView
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string locationname;

        public string Locationname
        {
            get { return locationname; }
            set { locationname = value; }
        }
        private decimal debt;

        public decimal Debt
        {
            get { return debt; }
            set { debt = value; }
        }
        private decimal payment;

        public decimal Payment
        {
            get { return payment; }
            set { payment = value; }
        }
        private decimal totaldebt;

        public decimal Totaldebt
        {
            get { return totaldebt; }
            set { totaldebt = value; }
        }
    }
}