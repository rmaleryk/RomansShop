using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByName(string categoryName);
    }
}