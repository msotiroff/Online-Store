using MSToolKit.Core.Authentication;
using OnlineStore.Common.ViewModels.User;
using OnlineStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserIndexViewModel> GetByIdAsync(string id);

        Task<AuthenticationResult> UpdateAsync(UserIndexViewModel model);

        Task<IQueryable<UserAdminViewModel>> GetAsync();

        Task<UserDeleteViewModel> GetForDeleteAsync(string userId);

        Task DeleteAsync(User user);
    }
}
