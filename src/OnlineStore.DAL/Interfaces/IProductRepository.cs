using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Interfaces
{
    public interface IProductRepository : IRepository<Product, string>
    {
        Task<IEnumerable<Product>> GetByCategoryId(string categoryId);
    }
}
