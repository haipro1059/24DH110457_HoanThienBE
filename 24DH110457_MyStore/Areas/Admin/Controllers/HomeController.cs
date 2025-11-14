using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using _24DH110457_MyStore.Models;

namespace _24DH110457_MyStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // ======================= TRANG CHỦ ADMIN =======================
        public ActionResult Index()
        {
            // Lấy danh sách thống kê hàng hóa từng loại (Category)
            var query = from c in db.Categories
                        select new CategoryProductStats
                        {
                            CategoryName = c.CategoryName,
                            ProductCount = c.Products.Count(),
                            MaxPrice = c.Products.Count() > 0 ? c.Products.Max(p => p.ProductPrice) : 0,
                            MinPrice = c.Products.Count() > 0 ? c.Products.Min(p => p.ProductPrice) : 0,
                            AvgPrice = c.Products.Count() > 0 ? c.Products.Average(p => p.ProductPrice) : 0
                        };

            var stats = query.ToList();

            // Truyền dữ liệu sang View
            return View(stats);
        }

        // ======================= BIỂU ĐỒ GOOGLE CHART =======================
        public ActionResult CategoryChart()
        {
            var data = from c in db.Categories
                       select new
                       {
                           CategoryName = c.CategoryName,
                           ProductCount = c.Products.Count()
                       };

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
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
