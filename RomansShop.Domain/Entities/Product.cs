using System;
using System.Collections.Generic;
using RomansShop.Domain.Extensibility;

namespace RomansShop.Domain.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public long Quantity { get; set; }

        public Guid? CategoryId { get; set; }

        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}