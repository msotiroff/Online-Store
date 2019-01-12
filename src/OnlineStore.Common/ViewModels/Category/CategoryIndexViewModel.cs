using OnlineStore.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Category
{
    public class CategoryIndexViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(ModelConstants.CategoryNameMinLength)]
        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string PictureUrl { get; set; }
    }
}
