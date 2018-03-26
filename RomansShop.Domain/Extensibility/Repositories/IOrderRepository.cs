using System;
using System.Collections.Generic;
using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetByUserId(Guid userId);

        IEnumerable<Order> GetByStatus(OrderStatus status);
    }
}