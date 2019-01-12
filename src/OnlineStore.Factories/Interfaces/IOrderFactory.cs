using System.Collections.Generic;
using OnlineStore.Models;

namespace OnlineStore.Factories.Interfaces
{
    public interface IOrderFactory
    {
        Order CreateInstance(
            string browserId, 
            string customerName, 
            string customerPhoneNumber, 
            string deliveryAddress, 
            string userId, 
            decimal totalAmount, 
            IEnumerable<OrderItem> orderItems);
    }
}
