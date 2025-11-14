// File: Models/Metadata.cs (ĐÃ SỬA LỖI CÚ PHÁP VÀ THỪA THUỘC TÍNH)

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _24DH110457_MyStore.Models
{
    // Lớp Metadata cho Entity Category
    public class CategoryMetadata
    {
        // 1. Khóa chính: CategoryID (Khắc phục lỗi InvalidOperationException)
        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        // 2. Tên danh mục: CategoryName (Áp dụng xác thực)
        [Required(ErrorMessage = "Tên danh mục không được để trống.")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự.")]
        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }

        // CÁC THUỘC TÍNH KHÁC CỦA CATEGORY (Nếu cần)
        // Ví dụ:
        // public string Description { get; set; }
    }

    // Đảm bảo không còn bất kỳ dòng code hoặc dấu chấm phẩy thừa nào bên ngoài lớp Metadata
}