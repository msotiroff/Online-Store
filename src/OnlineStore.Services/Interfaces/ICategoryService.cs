using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryIndexViewModel>> GetAsync();

        Task<ActionExecutionResult> AddAsync(string name);

        Task<CategoryDetailsViewModel> GetAsync(string id, int pageIndex, int productsPerPage);

        Task<CategoryUpdateViewModel> GetForUpdate(string id);

        Task UpdateAsync(CategoryUpdateViewModel model);
        Task RemoveAsync(string id);
        Task<CategoryDeleteViewModel> GetForDeleteAsync(string id);
    }
}
