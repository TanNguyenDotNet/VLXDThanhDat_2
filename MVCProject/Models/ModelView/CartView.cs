﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models.ModelView
{
    public class CartView
    {
        public CartView()
        {
            this.Ordersdetail = null;
            this.Order = null;
            this.Product = null;
            this.Catalogid = string.Empty;
            this.Filter = string.Empty;
            this.OrderAsc = string.Empty;
            this.Page = string.Empty;
            this.Size = string.Empty;
            this.Subid = string.Empty;
            this.Userid = string.Empty;
            this.Username = string.Empty;
        }
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
        
        private Order order = null;
        public Order Order
        {
            get { return order; }
            set { order = value; }
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
        private string size;

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        private string catalogid;
        public string Catalogid
        {
            get { return catalogid; }
            set { catalogid = value; }
        }
        private string supplier;

        public string Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }
        private string _order;
        public string OrderAsc
        {
            get { return _order; }
            set { _order = value; }
        }

        private string filter;

        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        private string page;

        public string Page
        {
            get { return page; }
            set { page = value; }
        }
    }
}