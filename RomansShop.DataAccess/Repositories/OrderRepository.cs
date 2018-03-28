using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
        }

        public override IEnumerable<Order> GetAll()
        {
            IList<Order> orders = dbSet
                .AsNoTracking()
                .OrderByDescending(order => order.Date)
                .ToList();

            LoadProductsForOrders(ref orders);

            return orders;
        }

        public IEnumerable<Order> GetByUserId(Guid userId)
        {
            IList<Order> orders = dbSet.AsNoTracking()
                .Where(order => order.UserId == userId)
                .OrderByDescending(order => order.Date)
                .ToList();

            LoadProductsForOrders(ref orders);

            return orders;
        }

        public IEnumerable<Order> GetByStatus(OrderStatus status)
        {
            IList<Order> orders = dbSet.AsNoTracking()
                .Where(order => order.Status == status)
                .OrderByDescending(order => order.Date)
                .ToList();

            LoadProductsForOrders(ref orders);

            return orders;
        }

        public override Order Update(Order order)
        {
            // TODO: Crutch!!! to prevent duplicate products
            context.Set<OrderProduct>().RemoveRange(context.Set<OrderProduct>().Where(op => op.OrderId == order.Id));

            dbSet.Update(order);
            context.SaveChanges();

            return order;
        }

        private void LoadProductsForOrders(ref IList<Order> orders)
        {
            foreach (Order order in orders)
            {
                context.OrderProducts
                    .AsNoTracking()
                    .Where(op => op.OrderId == order.Id).ToList()
                    .ForEach(op =>
                    {
                        op.Product = context.Set<Product>().AsNoTracking().Where(product => product.Id == op.ProductId).FirstOrDefault();
                        order.OrderProducts.Add(op);
                    });
            }
        }
    }
}