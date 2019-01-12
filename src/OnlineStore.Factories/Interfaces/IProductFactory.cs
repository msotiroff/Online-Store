using OnlineStore.Models;

namespace OnlineStore.Factories.Interfaces
{
    public interface IProductFactory
    {
        Product CreateInstance(
            string name, string description, decimal price, int availability, string categoryId);
    }
}
