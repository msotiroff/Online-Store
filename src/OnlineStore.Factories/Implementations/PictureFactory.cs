using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.Factories.Implementations
{
    public class PictureFactory : IPictureFactory
    {
        public Picture CreateInstance(string pictureUrl, string entityId)
        {
            return new Picture
            {
                Url = pictureUrl,
                EntityId = entityId
            };
        }
    }
}
