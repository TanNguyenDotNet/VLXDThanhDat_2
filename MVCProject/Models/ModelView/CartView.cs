using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models.ModelView
{
    public class CartView
    {
        private List<OrdersDetail> ordersdetail = null;
        public List<OrdersDetail> Ordersdetail
        {
            get { return ordersdetail; }
            set { ordersdetail = value; }
        }
        private List<Product> product = null;

        public List<Product> Product
        {
            get { return product; }
            set { product = value; }
        }
        private string username;
        private Order order = null;
        public Order Order
        {
            get { return order; }
            set { order = value; }
        }
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