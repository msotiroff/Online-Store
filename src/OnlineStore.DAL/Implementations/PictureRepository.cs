using System.Collections.Generic;
using System.Threading.Tasks;
using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.DAL.Implementations
{
    public class PictureRepository : Repository<Picture, string>, IPictureRepository
    {
        public PictureRepository(IDbContext<Picture, string> dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Picture>> GetByProductId(string productId)
        {
            return await base.FilterAsync(nameof(Picture.EntityId), productId);
        }
    }
}
