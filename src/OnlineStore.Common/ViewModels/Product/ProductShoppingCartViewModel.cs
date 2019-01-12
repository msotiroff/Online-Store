namespace OnlineStore.Common.ViewModels.Product
{
    public class ProductShoppingCartViewModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string ShortDescription { get; set; }
        
        public decimal Price { get; set; }

        public int Availability { get; set; }

        public string MainPictureUrl { get; set; }
    }
}
