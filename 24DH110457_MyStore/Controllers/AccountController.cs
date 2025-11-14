// File: Controllers/AccountController.cs (Đã sửa hoàn thiện)
using _24DH110457_MyStore.Models; // Namespace cho các Entity
using _24DH110457_MyStore.Models.ViewModel; // Namespace chính cho ViewModels
using MaSV_MyStore.Models.ViewModel; // Giữ lại cho các VM cũ/khác
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity; // Cần thiết cho Include/DBContext
using System; // Cần thiết cho DateTime

namespace _24DH110457_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private string HashPassword(string password)
        {
            return password;
        }

        // ======================= ĐĂNG KÝ (REGISTER) =======================
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                    UserRole = "C" // Vai trò Customer
                };
                db.Users.Add(user);

                var customer = new Customer
                {
                    Username = model.Username,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress
                };
                db.Customers.Add(customer);

                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // ======================= ĐĂNG NHẬP (LOGIN) =======================

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) // 🎯 FIX: Đảm bảo nhận returnUrl
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl) // 🎯 FIX: Đảm bảo nhận returnUrl
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.Password);

                var user = db.Users.SingleOrDefault(u =>
                    u.Username == model.Username &&
                    u.Password == hashedPassword
                );

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);

                    // 🎯 KIỂM TRA VAI TRÒ: Nếu vai trò không phải là Khách hàng (C), chuyển hướng đến Admin Area
                    if (user.UserRole != "C")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    // 2. CHUYỂN HƯỚNG KHÁCH HÀNG (Checkout/returnUrl)
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && !returnUrl.Contains("/Account/LogOff"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // ======================= ĐĂNG XUẤT (LOGOUT) =======================

        // POST: Account/Logout (Sử dụng LogOff vì BeginForm trong Layout dùng LogOff)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ======================= XEM/SỬA THÔNG TIN (PROFILE) =======================

        // GET: Account/ProfileInfo 
        [Authorize]
        public ActionResult ProfileInfo()
        {
            string username = User.Identity.Name;
            var customer = db.Customers.SingleOrDefault(c => c.Username == username);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Map Entity sang ViewModel
            var model = new ProfileInfoVM
            {
                Username = username,
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                CustomerPhone = customer.CustomerPhone,
                CustomerAddress = customer.CustomerAddress
            };

            return View(model);
        }

        // POST: Account/ProfileInfo 
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileInfo(ProfileInfoVM model)
        {
            if (ModelState.IsValid)
            {
                var customerToUpdate = db.Customers.SingleOrDefault(c => c.CustomerID == model.CustomerID);

                if (customerToUpdate != null)
                {
                    customerToUpdate.CustomerName = model.CustomerName;
                    customerToUpdate.CustomerEmail = model.CustomerEmail;
                    customerToUpdate.CustomerPhone = model.CustomerPhone;
                    customerToUpdate.CustomerAddress = model.CustomerAddress;

                    db.SaveChanges();
                    model.StatusMessage = "Cập nhật thông tin thành công!";
                    return View(model);
                }
            }

            model.StatusMessage = "Cập nhật thất bại. Vui lòng kiểm tra lại.";
            return View(model);
        }

        // ======================= ĐỔI MẬT KHẨU (CHANGE PASSWORD) =======================

        // GET: Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordVM());
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string username = User.Identity.Name;
            var user = db.Users.SingleOrDefault(u => u.Username == username);

            // Kiểm tra mật khẩu cũ
            if (user != null && user.Password == HashPassword(model.OldPassword))
            {
                user.Password = HashPassword(model.NewPassword);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("ProfileInfo");
            }

            ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
            return View(model);
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