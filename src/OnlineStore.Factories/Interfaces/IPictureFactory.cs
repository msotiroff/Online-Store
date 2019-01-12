using OnlineStore.Models;

namespace OnlineStore.Factories.Interfaces
{
    public interface IPictureFactory
    {
        Picture CreateInstance(string pictureUrl, string entityId);
    }
}
