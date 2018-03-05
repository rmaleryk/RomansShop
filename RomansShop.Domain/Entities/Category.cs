using System;
using RomansShop.Domain.Extensibility;

namespace RomansShop.Domain.Entities
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}