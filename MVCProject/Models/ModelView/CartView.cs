using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models.ModelView
{
    public class CartView
    {
        private List<OrdersDetail> ordersdetail;
        public List<OrdersDetail> Ordersdetail
        {
            get { return ordersdetail; }
            set { ordersdetail = value; }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string userid;

        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }
        private string subid;

        public string Subid
        {
            get { return subid; }
            set { subid = value; }
        }
    }
}