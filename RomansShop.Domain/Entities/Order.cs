using System;
using System.Collections.Generic;
using RomansShop.Domain.Extensibility;

namespace RomansShop.Domain.Entities
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; }

        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}