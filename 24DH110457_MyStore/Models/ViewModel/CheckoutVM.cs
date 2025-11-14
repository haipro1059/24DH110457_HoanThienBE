using _24DH110457_MyStore.Models;
using MaSV_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace _24DH110457_MyStore.Models.ViewModel
{
    // ⚠️ LƯU Ý: Nếu lớp này nằm trong namespace MaSV_MyStore.Models.ViewModel, bạn cần sửa lại tên namespace ở đây.
    public class CheckoutVM
    {
        // -------------------------------------------------------------------
        // THUỘC TÍNH CƠ SỞ (TÍNH TOÁN VÀ LƯU TRỮ)
        // -------------------------------------------------------------------
        public int CustomerID { get; set; }
        public string Username { get; set; }

        // BỔ SUNG LỖI: Cần TotalAmount để lưu vào DB (Dùng trong POST)
        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        // BỔ SUNG LỖI: Cần OrderDate để lưu vào DB
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // BỔ SUNG LỖI: Cần PaymentStatus
        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        // BỔ SUNG LỖI: Cần CartItems để hiển thị View khi lỗi POST
        public List<CartItem> CartItems { get; set; }

        // CÁC THUỘC TÍNH ĐỂ NHẬN DỮ LIỆU TỪ FORM (Đã có trong code gốc của bạn)
        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán.")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        [Required(ErrorMessage = "Vui lòng chọn phương thức giao hàng.")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ giao hàng.")]
        public string ShippingAddress { get; set; }

        // BỔ SUNG CÁC TRƯỜNG CẦN THIẾT KHÁC (RecipientName, ShippingPhone)
        [Display(Name = "Tên người nhận")]
        public string RecipientName { get; set; }
        public string ShippingPhone { get; set; }

        // -------------------------------------------------------------------
        // THUỘC TÍNH TÍNH TOÁN (Dùng trong GET Action)
        // -------------------------------------------------------------------
        public decimal TotalBeforeShipping { get; set; } // Controller đang gán giá trị
        public decimal ShippingFee { get; set; } // Controller đang gán giá trị
        public decimal GrandTotal { get; set; } // Controller đang gán giá trị

        // Cần có thuộc tính Cart để Controller có thể lấy dữ liệu sản phẩm 
        public Cart Cart { get; set; }
    }
}