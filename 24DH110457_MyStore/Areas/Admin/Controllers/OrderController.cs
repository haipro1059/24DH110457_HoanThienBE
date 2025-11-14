// File: Areas/Admin/Controllers/OrderController.cs
using System.Linq;
using System.Web.Mvc;
using _24DH110457_MyStore.Models;
using System.Data.Entity;
using System.Net;
using PagedList;
using System;
using System.Collections.Generic;

namespace _24DH110457_MyStore.Areas.Admin.Controllers
{
    [Authorize(Users = "Admin")]
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        private const int PageSize = 15; // Phân trang 15 đơn hàng/trang

        // GET: Admin/Order/Index (Danh sách Đơn hàng)
        public ActionResult Index(string searchName, string searchStatus, DateTime? searchDate, int? page)
        {
            ViewBag.CurrentName = searchName;
            ViewBag.CurrentStatus = searchStatus;

            var orders = db.Orders.Include(o => o.Customer).AsQueryable();

            // 1. Lọc theo Tên khách hàng (Đã xóa thẻ citation)
            if (!string.IsNullOrEmpty(searchName))
            {
                orders = orders.Where(o => o.Customer.CustomerName.Contains(searchName));
            }

            // 2. Lọc theo Trạng thái thanh toán (Đã xóa thẻ citation)
            if (!string.IsNullOrEmpty(searchStatus) && searchStatus != "All")
            {
                orders = orders.Where(o => o.PaymentStatus == searchStatus);
            }

            // 3. Lọc theo Ngày đặt hàng (Đã xóa thẻ citation)
            if (searchDate.HasValue)
            {
                // So sánh chỉ phần ngày (không tính giờ)
                orders = orders.Where(o => DbFunctions.TruncateTime(o.OrderDate) == DbFunctions.TruncateTime(searchDate.Value));
            }

            // 4. Sắp xếp (mới nhất lên trước) và Phân trang
            orders = orders.OrderByDescending(o => o.OrderDate);
            int pageNumber = (page ?? 1);

            // 5. Khởi tạo danh sách trạng thái thanh toán cho dropdown
            ViewBag.PaymentStatusList = new SelectList(
                new List<string> { "All", "Chưa thanh toán", "Thanh toán tiền mặt", "Pending", "Paid" },
                searchStatus // Chọn giá trị hiện tại
            );

            // Kiểm tra kết quả tìm kiếm (Đã xóa thẻ citation)
            if (!orders.Any())
            {
                // Nếu không có đơn hàng nào, thông báo sẽ được xử lý trong View
            }

            return View(orders.ToPagedList(pageNumber, PageSize));
        }

        // GET: Admin/Order/Details/{id} (Chi tiết đơn hàng)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy đơn hàng, bao gồm thông tin Khách hàng và chi tiết sản phẩm (OrderDetails)
            var order = db.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderDetails.Select(od => od.Product))
                            .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Tạo danh sách các trạng thái để truyền qua View
            ViewBag.StatusOptions = new SelectList(
                new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" },
                order.Status // Giá trị hiện tại được chọn
            );

            return View(order);
        }

        // POST: Admin/Order/UpdateStatus (Cập nhật trạng thái)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int OrderID, string Status)
        {
            // Danh sách trạng thái hợp lệ
            string[] validStatuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

            if (!validStatuses.Contains(Status))
            {
                TempData["Error"] = "Trạng thái không hợp lệ.";
                return RedirectToAction("Index");
            }

            // Lấy order từ DB
            var order = db.Orders.Find(OrderID);

            if (order == null)
            {
                TempData["Error"] = $"Không tìm thấy đơn hàng ID: {OrderID}.";
                return RedirectToAction("Index");
            }

            // Gán giá trị vào thuộc tính Status (giả định đã được thêm vào Order.cs)
            order.Status = Status;

            // Đánh dấu đối tượng là đã thay đổi và lưu vào DB
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Success"] = $"Đã cập nhật trạng thái đơn hàng #{OrderID} thành: {Status}";

            // Chuyển hướng về trang chi tiết
            return RedirectToAction("Details", new { id = OrderID });
        }


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