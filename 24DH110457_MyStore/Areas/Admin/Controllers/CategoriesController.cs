using System.Net;
using System.Web.Mvc;
using _24DH110457_MyStore.Models;
using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace _24DH110457_MyStore.Areas.Admin.Controllers
{
    
    public class CategoriesController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Sử dụng Include() để tải sâu danh sách Products 
            var category = db.Categories
                               .Include(c => c.Products)
                               .SingleOrDefault(c => c.CategoryID == id);

            if (category == null) return HttpNotFound();

            

            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description")] Category category)
        {
           
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Nếu không hợp lệ, trả về View để hiển thị lỗi
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            var category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();

            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description")] Category category)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // Tải Product liên quan để hiển thị cảnh báo 
            var category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryID == id);
            if (category == null) return HttpNotFound();

            return View(category);
        }

        // POST: Admin/Categories/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.Find(id);

           
            try
            {
               
                if (db.Products.Any(p => p.CategoryID == id))
                {
                    TempData["Error"] = "Không thể xóa danh mục này vì nó đang chứa các sản phẩm liên quan.";
                    return RedirectToAction("Delete", new { id = id });
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
               
                TempData["Error"] = "Lỗi ràng buộc dữ liệu: Không thể xóa danh mục này.";
               
                TempData["DetailedError"] = ex.Message;
                return RedirectToAction("Delete", new { id = id });
            }
            catch (Exception ex) 
            {
                
                TempData["Error"] = "Lỗi không xác định: " + ex.Message;
                return RedirectToAction("Delete", new { id = id });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}