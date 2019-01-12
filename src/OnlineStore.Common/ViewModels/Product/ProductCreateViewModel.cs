using System.ComponentModel.DataAnnotations;
using static OnlineStore.Models.Common.ModelConstants;

namespace OnlineStore.Common.ViewModels.Product
{
    public class ProductCreateViewModel
    {
        public ProductCreateViewModel(string categoryId)
        {
            this.CategoryId = categoryId;
        }

        public ProductCreateViewModel()
        {
        }

        [Required]
        [MinLength(ProductNameMinLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(ProductDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), ProductPriceMinValueAsString, ProductPriceMaxValueAsString)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Availability { get; set; }

        [Required]
        public string CategoryId { get; set; }
    }
}
