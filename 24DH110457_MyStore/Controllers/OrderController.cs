// File: Controllers/OrderController.cs (Final Code)
using _24DH110457_MyStore.Models;
using _24DH110457_MyStore.Models.ViewModel;
using MaSV_MyStore.Models.ViewModel;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace _24DH110457_MyStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private CartService GetCartService() => new CartService(Session);

        // GET: Order/Checkout (Hiển thị trang thanh toán)
        public ActionResult Checkout()
        {
            var cart = GetCartService().GetCart();

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            string username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null)
            {
                return RedirectToAction("ProfileInfo", "Account");
            }

            // Tính toán trước tổng tiền và phí
            decimal totalBeforeShipping = cart.TotalValue();
            decimal shippingFee = 30000;
            decimal grandTotal = totalBeforeShipping + shippingFee;

            var model = new CheckoutVM
            {
                Cart = cart,
                TotalBeforeShipping = totalBeforeShipping,
                ShippingFee = shippingFee,
                GrandTotal = grandTotal,
                CustomerID = customer.CustomerID,
                Username = customer.Username,

                // Ánh xạ các thuộc tính flat
                ShippingAddress = customer.CustomerAddress,
                RecipientName = customer.CustomerName,
                ShippingPhone = customer.CustomerPhone
            };

            return View(model);
        }

        // POST: Order/Checkout (Xử lý đặt hàng)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = GetCartService().GetCart();

            // Gán lại các giá trị tính toán và giỏ hàng cho model khi POST
            model.Cart = cart;
            model.TotalBeforeShipping = cart.TotalValue();
            model.ShippingFee = 30000;
            model.GrandTotal = model.TotalBeforeShipping + model.ShippingFee;

            // Lấy lại Customer để đảm bảo CustomerID/Username không bị mất
            string username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);
            model.CustomerID = customer.CustomerID;

            // Kiểm tra Validation và Giỏ hàng
            if (!ModelState.IsValid || cart == null || !cart.Items.Any())
            {
                return View(model);
            }

            // 1. Thiết lập trạng thái
            string paymentStatus = (model.PaymentMethod == "Tiền mặt") ? "Unpaid" : "Pending";
            string orderStatus = "Processing";

            // 2. Tạo đối tượng Order 
            var order = new Order
            {
                CustomerID = model.CustomerID,
                OrderDate = DateTime.Now,
                TotalAmount = model.GrandTotal,
                PaymentStatus = paymentStatus,
                Status = orderStatus,

                // ⚠️ FIX LỖI DB VALIDATION AddressDelivery:
                RecipientName = model.RecipientName ?? customer.CustomerName ?? "Khách hàng",
                ShippingPhone = model.ShippingPhone ?? customer.CustomerPhone ?? "0",
                ShippingAddress = model.ShippingAddress,
                AddressDelivery = model.ShippingAddress, // ⬅️ DÒNG FIX BẮT BUỘC ĐƯỢC THÊM VÀO
                PaymentMethod = model.PaymentMethod,
                ShippingMethod = model.ShippingMethod,

                OrderDetails = new List<OrderDetail>()
            };

            // 3. Thêm OrderDetails
            foreach (var item in cart.Items)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                });
            }

            // 4. LƯU VÀ XỬ LÝ LỖI DBENTITYVALIDATION
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();

                // 5. Thành công
                GetCartService().GetCart().ClearCart();
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.PropertyName + ": " + x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                ModelState.AddModelError("", "Lỗi lưu dữ liệu (DB Validation): " + fullErrorMessage);

                model.Cart = cart;
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống không xác định: " + ex.Message);
                model.Cart = cart;
                return View(model);
            }
        }

        // POST: Order/CancelOrder (Hành động hủy)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CancelOrder(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null || order.Status == "Shipped" || order.Status == "Delivered" || order.Status == "Cancelled")
            {
                TempData["Error"] = "Không thể hủy đơn hàng này do trạng thái hiện tại.";
                return RedirectToAction("OrderHistory");
            }

            try
            {
                order.Status = "Cancelled";
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = $"Đơn hàng #{id} đã được hủy thành công.";
                return RedirectToAction("OrderHistory");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi hệ thống khi hủy đơn hàng #{id}. Chi tiết: {ex.Message}";
                return RedirectToAction("OrderHistory");
            }
        }

        // GET: Order/OrderSuccess/5 (Xác nhận đơn hàng)
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/OrderHistory (Tra cứu Lịch sử mua hàng)
        public ActionResult OrderHistory()
        {
            string username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Load Orders cùng với OrderDetails và Product
            var orders = db.Orders
                            .Where(o => o.CustomerID == customer.CustomerID)
                            .Include(o => o.OrderDetails.Select(od => od.Product))
                            .OrderByDescending(o => o.OrderDate)
                            .ToList();

            return View(orders);
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}