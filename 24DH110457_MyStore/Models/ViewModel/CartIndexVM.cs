using PagedList;
using System.Collections.Generic;
using _24DH110457_MyStore.Models; // Cần cho Product

namespace _24DH110457_MyStore.Models.ViewModel
{
    public class CartIndexVM
    {
        public Cart Cart { get; set; } // Chứa Items, TotalValue

        // Chứa danh sách sản phẩm gợi ý
        public IPagedList<Product> SimilarProducts { get; set; }

        // Không cần PageSize hay GroupedItems ở đây, chúng sẽ được truy cập thông qua Cart.
    }
}