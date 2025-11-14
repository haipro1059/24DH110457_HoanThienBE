// File: Models/ViewModel/PersonalMenuVM.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaSV_MyStore.Models.ViewModel
{
    public class PersonalMenuVM
    {
        // Kiểm tra trạng thái đăng nhập
        public bool IsLoggedIn { get; set; }

        // Tên người dùng đang đăng nhập (để hiển thị lời chào)
        public string Username { get; set; }

        // Số lượng mặt hàng trong giỏ hàng
        public int CartCount { get; set; }
    }
}