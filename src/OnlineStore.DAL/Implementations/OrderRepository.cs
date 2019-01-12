using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class OrderRepository : Repository<Order, string>, IOrderRepository
    {
        public OrderRepository(IDbContext<Order, string> dbContext) 
            : base(dbContext)
        {
        }
    }
}
