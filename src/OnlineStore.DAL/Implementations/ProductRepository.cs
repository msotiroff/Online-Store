using System.Collections.Generic;
using System.Threading.Tasks;
using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class ProductRepository : Repository<Product, string>, IProductRepository
    {
        public ProductRepository(IDbContext<Product, string> dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryId(string categoryId)
        {
            return await base.FilterAsync(nameof(Product.CategoryId), categoryId);
        }
    }
}
