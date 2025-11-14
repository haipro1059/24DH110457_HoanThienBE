using System.ComponentModel.DataAnnotations;

namespace _24DH110457_MyStore.Models.ViewModel
{
    public class ProfileInfoVM
    {
        public int CustomerID { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        // Dùng để hiển thị thông báo trạng thái
        public string StatusMessage { get; set; }
    }
}