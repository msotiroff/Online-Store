using OnlineStore.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Common.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        [Required]
        [MinLength(ModelConstants.CategoryNameMinLength)]
        public string Name { get; set; }
    }
}
