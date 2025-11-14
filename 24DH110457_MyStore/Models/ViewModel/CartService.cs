// File: Models/ViewModel/CartService.cs

using System.Web.SessionState; // Hoặc System.Web.HttpSessionStateBase
using System.Web; // Nếu bạn dùng HttpContext.Current.Session

namespace _24DH110457_MyStore.Models.ViewModel
{
    // Giả định bạn đang sử dụng HttpSessionStateBase
    public class CartService
    {
        private const string SessionKey = "Cart";
        private readonly HttpSessionStateBase _session;

        // SỬA LỖI: Nhận HttpSessionStateBase
        public CartService(HttpSessionStateBase session)
        {
            _session = session;
        }

        public Cart GetCart()
        {
            // ⚠️ ĐÂY LÀ PHẦN QUAN TRỌNG NHẤT: Kiểm tra và Khởi tạo
            Cart cart = _session[SessionKey] as Cart;

            if (cart == null)
            {
                // Nếu chưa có, tạo mới và lưu vào Session
                cart = new Cart();
                _session[SessionKey] = cart;
            }
            return cart;
        }

        // ... (Các hàm khác) ...
    }
}