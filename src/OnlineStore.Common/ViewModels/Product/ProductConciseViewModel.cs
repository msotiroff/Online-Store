using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Product
{
    public class ProductConciseViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Picture")]
        public string MainPictureUrl { get; set; }

        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
    }
}
