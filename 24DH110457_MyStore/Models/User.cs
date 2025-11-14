// File: Models/User.cs (Giả định có UserID làm khóa chính)

namespace _24DH110457_MyStore.Models
{
    using System;
    using System.Collections.Generic;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Customers = new HashSet<Customer>();
        }

        // Cần Khóa chính (Primary Key) để Entity Framework hoạt động đúng
        // Nếu Username là khóa chính, giữ nguyên. Nếu không, cần UserID.
        public int UserID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}