using System;

namespace RomansShop.Domain.Entities
{
    /// <summary>
    ///     Product Category Entity
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}