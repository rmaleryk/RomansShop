using System;
using System.Collections.Generic;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;

namespace RomansShop.Services.Extensibility
{
    public interface IUserService
    {
        ValidationResponse<User> GetById(Guid id);

        ValidationResponse<IEnumerable<User>> GetByRights(UserRights rights);

        ValidationResponse<User> Add(User category);

        ValidationResponse<User> Update(User category);

        ValidationResponse<User> Delete(Guid id);

        ValidationResponse<User> Authenticate(string email, string password);
    }
}