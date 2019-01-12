using OnlineStore.Common.ViewModels.Product;

namespace OnlineStore.Common.ViewModels.OrderItem
{
    public class OrderItemIndexViewModel
    {
        public ProductShoppingCartViewModel Product { get; set; }

        public int Count { get; set; }
    }
}
