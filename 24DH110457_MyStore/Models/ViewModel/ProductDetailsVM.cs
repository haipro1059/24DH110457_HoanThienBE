// File: Models/ViewModel/ProductDetailsVM.cs

using _24DH110457_MyStore.Models;
using MaSV_MyStore.Models; // Ensure this points to your Product entity
using PagedList;
using System.Collections.Generic;

namespace MaSV_MyStore.Models.ViewModel
{
    public class ProductDetailsVM
    {
        // Chính xác là model.Product
        public Product Product { get; set; }

        // Dùng để chọn mua (quantity)
        public int Quantity { get; set; } = 1;

        // Tính toán giá trị tạm thời
        public decimal EstimatedValue { get; set; }

        // Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 3; // 3 sản phẩm mỗi trang

        // Danh sách 8 sản phẩm cùng danh mục
        public List<Product> RelatedProducts { get; set; } // Sửa thành List<Product> vì ToList() được dùng trước Take(8)

        // Danh sách 8 sản phẩm bán chạy nhất cùng danh mục đã phân trang
        public IPagedList<Product> TopProducts { get; set; }
    }
}