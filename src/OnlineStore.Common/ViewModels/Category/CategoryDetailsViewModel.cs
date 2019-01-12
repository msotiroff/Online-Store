using MSToolKit.Core.Collections;
using OnlineStore.Common.ViewModels.Product;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Category
{
    public class CategoryDetailsViewModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string PictureUrl { get; set; }

        [Display(Name = "Products")]
        public PaginatedList<ProductConciseViewModel> Products { get; set; }
    }
}
