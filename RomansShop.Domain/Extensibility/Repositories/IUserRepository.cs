using System.Collections.Generic;
using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetByRights(UserRights rights);

        User GetByEmail(string email);
    }
}