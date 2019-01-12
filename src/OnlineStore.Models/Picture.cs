namespace OnlineStore.Models
{
    using MSToolKit.Core.DataAccess.Abstraction;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Picture : IEntity<string>
    {
        public Picture()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string EntityId { get; set; }
    }
}