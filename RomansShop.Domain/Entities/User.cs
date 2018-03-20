using System;
using RomansShop.Domain.Extensibility;

namespace RomansShop.Domain.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRights Rights { get; set; }

    }
}