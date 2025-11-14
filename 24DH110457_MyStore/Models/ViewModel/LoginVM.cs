// File: Models/ViewModel/LoginVM.cs
using System.ComponentModel.DataAnnotations;

namespace MaSV_MyStore.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        // ⚠️ SỬA LỖI CS1061: BỔ SUNG THUỘC TÍNH REMEMBERME BỊ THIẾU
        [Display(Name = "Ghi nhớ tôi?")]
        public bool RememberMe { get; set; }
    }
}