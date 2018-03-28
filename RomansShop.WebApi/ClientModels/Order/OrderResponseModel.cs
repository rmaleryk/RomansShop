using System;
using System.Collections.Generic;
using RomansShop.Domain.Entities;
using RomansShop.WebApi.ClientModels.Product;

namespace RomansShop.WebApi.ClientModels.Order
{
    public class OrderResponseModel
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public OrderStatus Status { get; set; }

        public IList<ProductResponseModel> Products { get; set; }
    }
}