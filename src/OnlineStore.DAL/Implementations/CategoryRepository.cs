using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class CategoryRepository : Repository<Category, string>, ICategoryRepository
    {
        public CategoryRepository(IDbContext<Category, string> dbContext) 
            : base(dbContext)
        {
        }
    }
}
