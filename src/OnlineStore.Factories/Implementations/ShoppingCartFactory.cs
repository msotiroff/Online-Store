using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using System;

namespace OnlineStore.Factories.Implementations
{
    public class ShoppingCartFactory : IShoppingCartFactory
    {
        public ShoppingCart CreateInstance(string browserId)
        {
            return new ShoppingCart
            {
                Id = browserId,
                LastUpdated = DateTime.UtcNow,
                TotalAmount = 0,
                JsonSerializedOrderItems = "[]"
            };
        }
    }
}
