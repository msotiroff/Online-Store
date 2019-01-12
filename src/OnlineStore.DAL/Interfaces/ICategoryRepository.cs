using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.Models;

namespace OnlineStore.DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, string>
    {
    }
}
