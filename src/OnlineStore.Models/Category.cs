using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Category : IEntity<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateCreated = DateTime.UtcNow;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MinLength(ModelConstants.CategoryNameMinLength)]
        public string Name { get; set; }

        public string PictureUrl { get; set; }
    }
}
