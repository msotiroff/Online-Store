using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class ShoppingCartRepository : Repository<ShoppingCart, string>, IShoppingCartRepository
    {
        public ShoppingCartRepository(IDbContext<ShoppingCart, string> dbContext) 
            : base(dbContext)
        {
        }
    }
}
