using OnlineStore.Common.ViewModels.ShoppingCart;
using System.Threading.Tasks;

namespace OnlineStore.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartIndexViewModel> GetAsync(string browserId);

        Task AddProductAsync(string browserId, string productId, int count);

        Task SubtractProductAsync(string browserId, string productId, int count);

        Task RemoveProductAsync(string browserId, string productId);
        Task RemoveAsync(string browserId);
    }
}
