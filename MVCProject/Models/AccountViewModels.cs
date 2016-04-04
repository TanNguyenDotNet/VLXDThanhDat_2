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
        [Required]
        [Display(Name = "Tên đăng nhập")]
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
}
