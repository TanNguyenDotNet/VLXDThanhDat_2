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
    
    public partial class Product
    {
        public long ID { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public long CatID { get; set; }
        public string SKU { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string ImageLink { get; set; }
        public string Adwords { get; set; }
        public bool Show { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string Color { get; set; }
        public string Dimension { get; set; }
        public string Unit { get; set; }
        public string Warranty { get; set; }
        public bool IsDel { get; set; }
        public Nullable<byte> IsState { get; set; }
        public string UserID { get; set; }
        public string ProductName { get; set; }
        public Nullable<byte> TaxID { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
}
