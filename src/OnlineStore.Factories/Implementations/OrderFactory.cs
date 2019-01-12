using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Models.Enums;

namespace OnlineStore.Factories.Implementations
{
    public class OrderFactory : IOrderFactory
    {
        public Order CreateInstance(
            string browserId, 
            string customerName, 
            string customerPhoneNumber, 
            string deliveryAddress, 
            string userId, 
            decimal totalAmount, 
            IEnumerable<OrderItem> orderItems)
        {
            var order = new Order
            {
                BrowserId = browserId,
                CustomerName = customerName,
                CustomerPhoneNumber = customerPhoneNumber,
                DateTime = DateTime.UtcNow,
                DeliveryAddress = deliveryAddress,
                State = OrderState.Pending,
                UserId = userId,
                TotalAmount = totalAmount,
                JsonSerializedOrderItems = JsonConvert.SerializeObject(orderItems)
            };

            return order;
        }
    }
}
