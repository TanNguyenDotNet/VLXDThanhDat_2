//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdersDetail
    {
        public long ID { get; set; }
        public long IDProduct { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public bool ReturnGood { get; set; }
        public Nullable<System.DateTime> DateOfOrder { get; set; }
        public string Tax { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public bool RequestByUser { get; set; }
        public string OrderCode { get; set; }
        public Nullable<decimal> Discount { get; set; }
    }
}
