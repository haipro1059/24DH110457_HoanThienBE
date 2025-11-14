using MaSV_MyStore.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace _24DH110457_MyStore.Models.ViewModel
{
    public class Cart
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        // SỬA LỖI 4: Phương thức này chỉ nhận 5 tham số
        public void AddItem(int productID, string name, string image, decimal price, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductID == productID);
            if (existingItem == null)
            {
                Items.Add(new CartItem
                {
                    ProductID = productID,
                    ProductName = name,
                    ProductImage = image,
                    UnitPrice = price,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productID)
        {
            Items.RemoveAll(i => i.ProductID == productID);
        }

        // Cập nhật số lượng
        public void UpdateQuantity(int productID, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductID == productID);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    RemoveItem(productID);
                }
            }
        }

        // Xóa sạch giỏ hàng
        public void ClearCart()
        {
            Items.Clear();
        }

        // Tính tổng tiền
        public decimal TotalValue()
        {
            return Items.Sum(i => i.TotalPrice);
        }
    }
}