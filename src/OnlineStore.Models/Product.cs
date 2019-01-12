using MSToolKit.Core.DataAccess.Abstraction;
using System;
using System.ComponentModel.DataAnnotations;

using static OnlineStore.Models.Common.ModelConstants;

namespace OnlineStore.Models
{
    public class Product : IEntity<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

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

        public bool IsDeleted { get; set; }
    }
}