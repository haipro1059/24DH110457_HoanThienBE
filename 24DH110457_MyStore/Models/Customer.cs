// File: Models/Customer.cs (Đã loại bỏ các thuộc tính trùng lặp)

namespace _24DH110457_MyStore.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }

        public int CustomerID { get; set; } // Khóa chính của Khách hàng

        // 🔑 KHÓA NGOẠI: Liên kết 1-1 với bảng User
        public int UserID { get; set; }

        // ❌ LOẠI BỎ THUỘC TÍNH TRÙNG LẶP GÂY LỖI:
        // public string Username { get; set; }
        // public string Password { get; set; }
        // public string CustomerImage { get; set; }

        // THÔNG TIN CÁ NHÂN
        public string Username { get; set; }
        public string CustomerImage { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual User User { get; set; } // Navigation Property
    }
}