using MSToolKit.Core.DataAccess.Abstraction;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Interfaces
{
    public interface IPictureRepository : IRepository<Picture, string>
    {
        Task<IEnumerable<Picture>> GetByProductId(string productId);
    }
}
