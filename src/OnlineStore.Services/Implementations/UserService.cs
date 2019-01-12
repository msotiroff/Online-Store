using MSToolKit.Core.Authentication;
using MSToolKit.Core.Authentication.Abstraction;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.User;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserManager<User> userManager;
        private readonly IUserRepository userRepository;
        private readonly ICustomMapper customMapper;

        public UserService(
            IUserManager<User> userManager,
            IUserRepository userRepository,
            ICustomMapper customMapper)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.customMapper = customMapper;
        }

        public async Task DeleteAsync(User user)
        {
            await this.userRepository.RemoveAsync(user);
        }

        public async Task<IQueryable<UserAdminViewModel>> GetAsync()
        {
            var userModels = (await this.userRepository.GetAsync())
                .Where(u => !u.IsDeleted)
                .Select(user => this.customMapper.Map<User, UserAdminViewModel>(user));

            return userModels;
        }

        public async Task<UserIndexViewModel> GetByIdAsync(string id)
        {
            CoreValidator.ThrowIfNullOrWhitespace(id);

            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return this.customMapper.Map<User, UserIndexViewModel>(user);
        }

        public async Task<UserDeleteViewModel> GetForDeleteAsync(string userId)
        {
            CoreValidator.ThrowIfNullOrWhitespace(userId);

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return this.customMapper.Map<User, UserDeleteViewModel>(user);
        }

        public async Task<AuthenticationResult> UpdateAsync(UserIndexViewModel model)
        {
            CoreValidator.ThrowIfAnyNull(model, model?.Id);

            var user = await this.userManager.FindByIdAsync(model.Id);
            var emailUpdated = user.Email != model.Email;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmailConfirmed = emailUpdated ? false : user.EmailConfirmed;
            user.Email = model.Email;
            user.Username = model.Username;

            var result = await this.userManager.UpdateAsync(user);

            return result;
        }
    }
}
