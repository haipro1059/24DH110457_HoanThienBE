using System.Linq;
using System.Web.Mvc;
using _24DH110457_MyStore.Models;
using System; // Cần thiết cho Exception

namespace _24DH110457_MyStore.Controllers
{
    public class CustomerController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // ... (Actions Register GET, Login GET/POST, Details, Edit GET) ...

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // LƯU Ý: Nếu bạn dùng Hash, phải Hash mật khẩu ở đây
                    // model.Password = HashPassword(model.Password); 

                    db.Customers.Add(model);
                    db.SaveChanges(); // <-- LỖI XẢY RA TẠI ĐÂY

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    // BẮT VÀ HIỂN THỊ LỖI DB CHI TIẾT
                    string errorMessage = "Đăng ký thất bại. Lỗi hệ thống: ";
                    if (ex.InnerException?.InnerException?.Message != null)
                    {
                        errorMessage += ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        errorMessage += ex.Message;
                    }

                    // Tên đăng nhập đã tồn tại?
                    if (errorMessage.Contains("unique index") || errorMessage.Contains("duplicate"))
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc Email đã tồn tại. Vui lòng chọn tên khác.");
                    }
                    else if (errorMessage.Contains("cannot insert the value NULL into column"))
                    {
                        ModelState.AddModelError("", "Thiếu thông tin bắt buộc (ví dụ: UserID, Password, hoặc trường NOT NULL khác).");
                    }
                    else
                    {
                        ModelState.AddModelError("", errorMessage); // Lỗi chi tiết khác
                    }

                    return View(model);
                }
            }
            return View(model);
        }

        // ... (Actions Edit POST, Logout) ...
    }
}