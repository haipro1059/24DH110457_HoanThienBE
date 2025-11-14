// File: Controllers/CartController.cs
using _24DH110457_MyStore.Models;
using _24DH110457_MyStore.Models.ViewModel;
// ⚠️ LƯU Ý: Đảm bảo CartService và Cart nằm trong namespace _24DH110457_MyStore.Models.ViewModel
//          Xóa using MaSV_MyStore.Models.ViewModel; nếu nó không được dùng.

using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _24DH110457_MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Lấy dịch vụ giỏ hàng (Giả định CartService được định nghĩa đúng)
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // -----------------------------------------------------------
        // SỬA ACTION INDEX: Dùng CartIndexVM thay vì Cart
        // -----------------------------------------------------------
        public ActionResult Index(int? page)
        {
            var cart = GetCartService().GetCart();

            // SỬA LỖI 5 & 6: Khai báo PageSize tại đây (hoặc trong VM)
            int pageSize = 4; // Kích thước trang gợi ý

            IPagedList<Product> similarProducts = new List<Product>().ToPagedList(1, 1); // Khởi tạo rỗng

            if (cart.Items.Any())
            {
                var categoryIdsInCart = cart.Items
                    .Select(i => db.Categories.FirstOrDefault(c => c.CategoryName == i.Category)?.CategoryID)
                    .Where(id => id.HasValue)
                    .Cast<int>()
                    .ToList();

                var productIdsInCart = cart.Items.Select(i => i.ProductID).ToList();

                var query = db.Products
                    .Where(p => categoryIdsInCart.Contains(p.CategoryID) && !productIdsInCart.Contains(p.ProductID))
                    .OrderBy(p => p.ProductName)
                    .AsQueryable();

                int pageNumber = page ?? 1;

                // ÁP DỤNG PHÂN TRANG: Gán cho biến tạm
                similarProducts = query.ToPagedList(pageNumber, pageSize);
            }

            // SỬA LỖI 5 & 6: Trả về ViewModel mới
            var viewModel = new CartIndexVM
            {
                Cart = cart,
                SimilarProducts = similarProducts,
                // PageSize và GroupedItems không cần thiết ở đây, chúng được truy cập qua Cart hoặc View
            };

            return View(viewModel); // Trả về ViewModel mới
        }

        [HttpPost] // ⬅️ THUỘC TÍNH NÀY RẤT QUAN TRỌNG
        public ActionResult RemoveItem(int id)
        {
            // Logic thực hiện xóa sản phẩm
            GetCartService().GetCart().RemoveItem(id);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cartService = GetCartService();

            if (quantity < 1)
            {
                // Nếu số lượng là 0 hoặc âm, coi như yêu cầu xóa
                cartService.GetCart().RemoveItem(id);
            }
            else
            {
                // Cập nhật số lượng mới
                cartService.GetCart().UpdateQuantity(id, quantity);
            }

            // Chuyển hướng trở lại trang giỏ hàng
            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------
        // SỬA LỖI OVERLOAD (Lỗi AddItem 6 tham số)
        // -----------------------------------------------------------
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            Product product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == id);

            if (product != null)
            {
                var cartService = GetCartService();

                // SỬA LỖI 4: Chỉ gọi AddItem với 5 tham số (BỎ product.Category.CategoryName)
                cartService.GetCart().AddItem(
                    product.ProductID,
                    product.ProductName, // Tên sản phẩm
                    product.ProductImage, // Link ảnh
                    product.ProductPrice, // Đơn giá
                    quantity
                // BỎ THAM SỐ THỨ 6 (CategoryName)
                );
            }
            return RedirectToAction("Index");
        }

        // ... (Các action RemoveItem, ClearCart, UpdateQuantity giữ nguyên) ...

    }
}