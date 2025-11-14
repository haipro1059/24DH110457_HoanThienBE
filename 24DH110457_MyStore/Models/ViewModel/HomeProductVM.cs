// File: Models/ViewModel/HomeProductVM.cs
using PagedList;
using System.Collections.Generic;
using _24DH110457_MyStore.Models; // Namespace Entity Model của bạn
using System.Linq; // Added for completeness, though not strictly needed here

namespace MaSV_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        // Tiêu chí tìm kiếm
        public string SearchTerm { get; set; }

        // Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6; // Số sản phẩm mỗi trang

        // Danh sách 10 sản phẩm nổi bật (bán chạy nhất)
        public List<Product> FeaturedProducts { get; set; }

        // Danh sách 20 sản phẩm mới đã phân trang
        public IPagedList<Product> NewProducts { get; set; }
    }
}