using OnlineStore.Common.ViewModels.Picture;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static OnlineStore.Models.Common.ModelConstants;

namespace OnlineStore.Common.ViewModels.Product
{
    public class ProductUpdateViewModel
    {
        [Required]
        public string Id { get; set; }

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

        public IEnumerable<PictureConciseViewModel> Pictures { get; set; }
    }
}
