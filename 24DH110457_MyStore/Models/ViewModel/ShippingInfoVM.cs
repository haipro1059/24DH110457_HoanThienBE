using System.ComponentModel.DataAnnotations;

namespace _24DH110457_MyStore.Models.ViewModel
{
    // Lớp này chứa thông tin giao hàng chi tiết
    public class ShippingInfoVM
    {
        [Required(ErrorMessage = "Không được để trống tên người nhận.")]
        [Display(Name = "Tên người nhận")]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
    }
}