using System;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
    }
    public class RevenueInvoice
    {
        public string AccountName { get; set; }
        public string DateCreate { get; set; }
        public decimal Total { get; set; }
        public string OrderCode { get; set; }

    }
    public class RevenueOfMonth
    {
        public string AccountName { get; set; }
        public string LocationSub { get; set; }
        public decimal Total { get; set; }
    }
    public class ProductViewModel
    {
        public long ID { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        [Required(ErrorMessage = "Chọn danh mục")]
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
        public string UnitName { get; set; }
        public string Warranty { get; set; }
        public bool IsDel { get; set; }
        public Nullable<byte> IsState { get; set; }
        public string UserID { get; set; }
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Chọn thuế suất")]
        public Nullable<byte> TaxID { get; set; }
        [Required(ErrorMessage = "Nhập giá")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Nhập số")]
        public Nullable<decimal> Price
        {
            get;
            set;
        }
    }
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất {0} phải có  {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và nhập lại không giống nhau.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage="Nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(100, ErrorMessage = "Tên đăng nhập từ 6 kí tự", MinimumLength = 6)]
        public string UserName { get; set; }
        [Display(Name = "Tên cá nhân/đại lý")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Loại người dùng (Quyền)")]
        public string UserType { get; set; }

        [Required]
        [Display(Name = "Tỉnh/Thành phố")]
        public string LocationID { get; set; }
        
        [Required]
        [Display(Name = "Khu vực")]
        public string LocationSubID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất {0} phải có  {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và nhập lại không giống nhau.")]
        public string ConfirmPassword { get; set; }
        
    }
    public class LocationSubViewModel
    {
        [Required]
        [Display(Name = "Tỉnh/Thành phố")]
        public int IDLocation { get; set; }
        [Required]
        [Display(Name="Nhập tên")]
        [StringLength(50,MinimumLength=3,ErrorMessage="Nhập tên")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Nhập %")]
        [Range(0,99,ErrorMessage="Nhập số")]
        public string LocationPrice { get; set; }
    }
    public class ProductPriceViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string desc { get; set; }
        public DateTime? date { get; set; }
        public string locationsub { set; get; }
        public int idlocationsub { set; get; }
    }
}
