using OnlineStore.Models;

namespace OnlineStore.Factories.Interfaces
{
    public interface IShoppingCartFactory
    {
        ShoppingCart CreateInstance(string browserId);
    }
}
