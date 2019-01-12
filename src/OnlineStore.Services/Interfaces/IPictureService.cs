using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IPictureService
    {
        Task CreateAsync(string pictureUrl, string entityId);

        Task RemoveAsync(string id);

        Task<IEnumerable<Picture>> GetByProductIdAsync(string productId);
    }
}
