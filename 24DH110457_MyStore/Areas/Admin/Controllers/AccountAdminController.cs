using System.Linq;
using System.Web.Mvc;
using _24DH110457_MyStore.Models; // Cho phép truy cập DB Context

namespace _24DH110457_MyStore.Areas.Admin.Controllers
{
    // BẮT BUỘC: Yêu cầu quyền Admin để truy cập
    [Authorize(Roles = "Admin")]
    public class AccountAdminController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/AccountAdmin/Index (Hiển thị hồ sơ Admin)
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var user = db.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                // Nếu không tìm thấy, log off và quay lại đăng nhập
                System.Web.Security.FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Trả về View để hiển thị chi tiết hồ sơ
            return View(user);
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