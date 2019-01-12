using Amazon.DynamoDBv2.DataModel;
using MSToolKit.Core.DataAccess.Abstraction;
using Newtonsoft.Json;
using OnlineStore.Models.Common;
using OnlineStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Order : IEntity<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.State = OrderState.Pending;
            this.DateTime = DateTime.UtcNow;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [MinLength(ModelConstants.UserNameMinLength)]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhoneNumber { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public OrderState State { get; set; }

        public string UserId { get; set; }

        [Required]
        public string BrowserId { get; set; }

        /// <summary>
        /// Gets or sets the json serialized order items.
        /// </summary>
        [Required]
        public string JsonSerializedOrderItems { get; set; }

        /// <summary>
        /// Every order item in the current order.
        /// </summary>
        [DynamoDBIgnore]
        public IEnumerable<OrderItem> Items
            => JsonConvert.DeserializeObject<IEnumerable<OrderItem>>(this.JsonSerializedOrderItems);
    }
}