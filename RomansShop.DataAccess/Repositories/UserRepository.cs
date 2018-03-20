using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
        }

        public override IEnumerable<User> GetAll()
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(user => user.FullName)
                .ToList();
        }

        public User GetByEmail(string email)
        {
            return dbSet
                .AsNoTracking()
                .FirstOrDefault(user => user.Email == email);
        }

        IEnumerable<User> IUserRepository.GetByRights(UserRights rights)
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(user => user.FullName)
                .Where(user => user.Rights == rights)
                .ToList();
        }
    }
}