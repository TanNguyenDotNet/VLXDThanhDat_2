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
    
    public partial class PaymentDetail
    {
        public System.Guid ID { get; set; }
        public string IDAccount { get; set; }
        public string IDAccountInput { get; set; }
        public Nullable<int> IDLocationSub { get; set; }
        public Nullable<decimal> Pay { get; set; }
        public string PayDate { get; set; }
        public string PayDateSystem { get; set; }
        public Nullable<decimal> DebtBefore { get; set; }
        public Nullable<decimal> DebtAfter { get; set; }
        public string Description { get; set; }
    }
}
