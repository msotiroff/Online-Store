using Amazon.DynamoDBv2.DataModel;
using MSToolKit.Core.DataAccess.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineStore.Models
{
    public class ShoppingCart : IEntity<string>
    {
        public ShoppingCart(string id)
        {
            this.Id = id;
        }

        public ShoppingCart()
        {
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the json serialized products
        /// </summary>
        [Required]
        public string JsonSerializedOrderItems { get; set; }

        /// <summary>
        /// Every product in the current order.
        /// </summary>
        [DynamoDBIgnore]
        public IEnumerable<OrderItem> Items
            => JsonConvert.DeserializeObject<IEnumerable<OrderItem>>(this.JsonSerializedOrderItems);

        /// <summary>
        /// Full override of the products in this order. 
        /// Also updates TotalAmount and LastUpdated properties.
        /// </summary>
        public void UpdateItems(IEnumerable<OrderItem> items)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var serializedProducts = JsonConvert.SerializeObject(items, jsonSettings);

            this.JsonSerializedOrderItems = serializedProducts;

            this.LastUpdated = DateTime.UtcNow;
            this.TotalAmount = items.Sum(i => i.Product.Price * i.Count);
        }
    }
}
