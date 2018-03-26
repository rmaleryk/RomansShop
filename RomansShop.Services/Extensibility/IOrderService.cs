using System;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;

namespace RomansShop.Services.Extensibility
{
    public interface IOrderService
    {
        ValidationResponse<Order> GetById(Guid id);

        ValidationResponse<Order> Add(Order order);

        ValidationResponse<Order> Update(Order order);

        ValidationResponse<Order> Delete(Guid id);
    }
}