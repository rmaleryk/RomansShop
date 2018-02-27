using System;

namespace RomansShop.Domain
{
    /// <summary>
    ///     Product Entity
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public long Quantity { get; set; }
    }
}