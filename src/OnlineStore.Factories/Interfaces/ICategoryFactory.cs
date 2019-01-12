using OnlineStore.Models;

namespace OnlineStore.Factories.Interfaces
{
    public interface ICategoryFactory
    {
        Category CreateInstance(string name);
    }
}
