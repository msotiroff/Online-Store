using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.Models;

namespace OnlineStore.DAL.Interfaces
{
    public interface IOrderRepository : IRepository<Order, string>
    {
    }
}
