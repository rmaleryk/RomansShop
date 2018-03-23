using System;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;

namespace RomansShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public OrderService(IOrderRepository orderRepository, ILoggerFactory loggerFactory)
        {
            _orderRepository = orderRepository;
            _loggerFactory = loggerFactory;

            _logger = _loggerFactory.CreateLogger(GetType());
        }

        public ValidationResponse<Order> GetById(Guid id)
        {
            Order order = _orderRepository.GetById(id);

            if (order == null)
            {
                string message = $"Order with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Order>(ValidationStatus.NotFound, message);
            }

            return new ValidationResponse<Order>(order, ValidationStatus.Ok);
        }

        public ValidationResponse<Order> Add(Order order)
        {
            Order addedOrder = _orderRepository.Add(order);

            return new ValidationResponse<Order>(addedOrder, ValidationStatus.Ok);
        }

        public ValidationResponse<Order> Update(Order order)
        {
            Order orderTmp = _orderRepository.GetById(order.Id);

            if (orderTmp == null)
            {
                string message = $"Order with id {order.Id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Order>(ValidationStatus.NotFound, message);
            }

            Order updatedOrder = _orderRepository.Update(order);

            return new ValidationResponse<Order>(updatedOrder, ValidationStatus.Ok);
        }

        public ValidationResponse<Order> Delete(Guid id)
        {
            Order order = _orderRepository.GetById(id);

            if (order == null)
            {
                string message = $"Order with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Order>(ValidationStatus.NotFound, message);
            }

            _orderRepository.Delete(order);

            return new ValidationResponse<Order>(order, ValidationStatus.Ok,
                $"Order with id {id} was deleted.");
        }
    }
}