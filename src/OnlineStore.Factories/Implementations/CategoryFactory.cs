using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.Factories.Implementations
{
    public class CategoryFactory : ICategoryFactory
    {
        public Category CreateInstance(string name)
        {
            return new Category
            {
                Name = name
            };
        }
    }
}
