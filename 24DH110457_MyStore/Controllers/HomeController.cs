// File: Controllers/HomeController.cs
using _24DH110457_MyStore.Models;
using _24DH110457_MyStore.Models.ViewModel; // Keep this one if the namespace is _24DH110457_MyStore
using MaSV_MyStore.Models.ViewModel;

// using MaSV_MyStore.Models.ViewModel; // DELETE THIS LINE (It causes duplication/confusion)
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _24DH110457_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        public ActionResult ProductList(int? categoryId, string searchTerm, int? page)
        {
            // Lấy tất cả sản phẩm, bao gồm Category
            IQueryable<Product> products = db.Products.Include(p => p.Category).AsQueryable();

            // Lọc theo Danh mục (nếu ID được cung cấp)
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
                // Lưu CategoryID vào ViewBag để dùng trong View (ví dụ: hiển thị tên danh mục)
                ViewBag.CurrentCategory = db.Categories.Find(categoryId.Value)?.CategoryName;
            }
            else
            {
                ViewBag.CurrentCategory = "Tất cả sản phẩm";
            }

            // --- Xử lý Tìm kiếm (nếu có) ---
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // ... (Logic tìm kiếm nếu cần) ...
            }

            // --- PHÂN TRANG (Sử dụng PagedList) ---
            int pageSize = 9; // 9 sản phẩm mỗi trang (Ví dụ)
            int pageNumber = page ?? 1;

            // Lấy danh sách phân trang (sử dụng Model List<Product> hoặc IEnumerable<Product>)
            var pagedProducts = products.ToList().ToPagedList(pageNumber, pageSize);

            // View này sẽ sử dụng @model PagedList.IPagedList<Product>
            return View(pagedProducts);
        }

        // GET: Home/Index (Hiển thị Trang chủ)
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            IQueryable<Product> products = db.Products.AsQueryable();

            // Tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    p.ProductDescription.Contains(searchTerm) ||
                    p.Category.CategoryName.Contains(searchTerm));
            }

            // Lấy 10 sản phẩm bán chạy nhất
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10)
                .ToList();

            // Lấy 20 sản phẩm mới và phân trang
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize; // Mặc định 6 sản phẩm / trang

            model.NewProducts = products
                .OrderBy(p => p.OrderDetails.Count()) // Ít người mua nhất
                .Take(20) // Lấy 20 sản phẩm
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Home/ProductDetail/5 (Hiển thị chi tiết sản phẩm)
        public ActionResult ProductDetail(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            ProductDetailsVM model = new ProductDetailsVM();
            model.Product = pro;

            // Lấy các sản phẩm cùng danh mục (Loại trừ sản phẩm hiện tại)
            var relatedProductsQuery = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

            // Sản phẩm tương tự (Related Products)
            model.RelatedProducts = relatedProductsQuery.Take(8).ToList(); // 8 sản phẩm cùng danh mục

            // Sản phẩm bán chạy nhất cùng danh mục (Top Products)
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize; // Mặc định 3 sản phẩm/trang

            model.TopProducts = relatedProductsQuery
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(8) // Lấy 8 sản phẩm bán chạy nhất
                .ToPagedList(pageNumber, pageSize);

            // Tính toán giá trị tạm thời (estimatedValue)
            if (quantity.HasValue)
            {
                model.Quantity = quantity.Value;
                model.EstimatedValue = model.Quantity * model.Product.ProductPrice;
            }

            return View(model);
        }

        // POST: Home/ProductDetail
        [HttpPost]
        public ActionResult ProductDetail(ProductDetailsVM model)
        {
            // Logic xử lý nút Thêm vào Giỏ hàng tại đây (sẽ gọi CartController.AddItem)
            // ...

            return RedirectToAction("ProductDetail", new { id = model.Product.ProductID, quantity = model.Quantity });
        }
    }
}