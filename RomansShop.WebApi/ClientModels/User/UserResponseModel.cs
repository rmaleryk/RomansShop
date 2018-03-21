using System;
using RomansShop.Domain.Entities;

namespace RomansShop.WebApi.ClientModels.User
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserRights Rights { get; set; }
    }
}