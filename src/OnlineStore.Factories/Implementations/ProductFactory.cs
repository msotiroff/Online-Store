using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.Factories.Implementations
{
    public class ProductFactory : IProductFactory
    {
        public Product CreateInstance(string name, string description, decimal price, int availability, string categoryId)
        {
            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Availability = availability,
                CategoryId = categoryId
            };
        }
    }
}
