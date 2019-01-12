using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(IDbContext<User, string> dbContext) 
            : base(dbContext)
        {
        }
    }
}
