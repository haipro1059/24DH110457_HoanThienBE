using System.Collections.Generic;
using _24DH110457_MyStore.Models;

namespace _24DH110457_MyStore.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        public List<Product> Products { get; set; }
    }
}
