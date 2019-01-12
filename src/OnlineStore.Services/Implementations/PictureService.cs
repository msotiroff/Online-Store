using MSToolKit.Core.Validation;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Services.Implementations
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository pictureRepository;
        private readonly IPictureFactory pictureFactory;

        public PictureService(
            IPictureRepository pictureRepository,
            IPictureFactory pictureFactory)
        {
            this.pictureRepository = pictureRepository;
            this.pictureFactory = pictureFactory;
        }

        public async Task CreateAsync(string pictureUrl, string entityId)
        {
            var picture = this.pictureFactory.CreateInstance(pictureUrl, entityId);

            await this.pictureRepository.AddAsync(picture);
        }

        public async Task<IEnumerable<Picture>> GetByProductIdAsync(string productId)
        {
            return await this.pictureRepository.GetByProductId(productId);
        }

        public async Task RemoveAsync(string id)
        {
            var picture = await this.pictureRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(picture);

            await this.pictureRepository.RemoveAsync(picture);
        }
    }
}
