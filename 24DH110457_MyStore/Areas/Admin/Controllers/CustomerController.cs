using _24DH110457_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _24DH110457_MyStore.Areas.Admin.Controllers
{
    public class CustomersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index()
        {
            ViewBag.Customers = db.Customers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                TempData["msg"] = "Thêm khách hàng thành công!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSelected(string[] selectedIDs)
        {
            if (selectedIDs == null || selectedIDs.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một khách hàng để xóa.";
                return RedirectToAction("Index");
            }

            try
            {
                foreach (var idString in selectedIDs)
                {
                    // 🎯 FIX: Chuyển đổi chuỗi ID sang số nguyên an toàn
                    if (int.TryParse(idString, out int customerId))
                    {
                        var customer = db.Customers.Find(customerId); // <-- Find() bây giờ nhận INT

                        // ⚠️ BỔ SUNG: Kiểm tra ràng buộc Khóa ngoại (để tránh lỗi nếu khách hàng có đơn hàng)
                        if (db.Orders.Any(o => o.CustomerID == customerId))
                        {
                            TempData["Error"] = "Không thể xóa khách hàng ID " + customerId + " vì họ có đơn hàng liên quan.";
                            // Nếu không muốn xóa, bạn cần chuyển hướng ra ngoài vòng lặp hoặc bỏ qua
                            continue;
                        }

                        if (customer != null)
                        {
                            db.Customers.Remove(customer);
                        }
                    }
                    // Nếu không thể phân tích cú pháp (ID không phải số), bỏ qua
                }

                db.SaveChanges();
                TempData["Success"] = $"Đã xóa thành công {selectedIDs.Length} khách hàng được chọn.";
            }
            // Bắt lỗi khóa ngoại nếu kiểm tra thủ công bị thiếu hoặc xảy ra lỗi chung
            catch (System.Data.Entity.Infrastructure.DbUpdateException )
            {
                TempData["Error"] = "Lỗi: Không thể xóa một hoặc nhiều khách hàng vì họ có dữ liệu liên quan.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi không xác định: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
