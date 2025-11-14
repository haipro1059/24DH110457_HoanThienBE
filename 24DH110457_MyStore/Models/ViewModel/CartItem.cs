// File: Models/ViewModel/CartItem.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MaSV_MyStore.Models.ViewModel
{
    public class CartItem
    {


        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImage { get; set; }

        // Tên danh mục của sản phẩm 
        public string Category { get; set; } // [cite: 263]

        // Tổng giá cho mỗi sản phẩm (Read-only property using expression body)
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}