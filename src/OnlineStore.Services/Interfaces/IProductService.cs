using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateAsync(ProductCreateViewModel model);

        Task<ProductDetailsViewModel> GetAsync(string id);

        Task<ProductUpdateViewModel> GetForUpdateAsync(string id);

        Task UpdateAsync(ProductUpdateViewModel model);

        Task RemoveAsync(string id);

        Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId);

        Task<IEnumerable<ProductConciseViewModel>> FilterAsync(
            string searchTerm, int pageIndex, int itemsPerPage);
    }
}
