using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RomansShop.Domain.Entities;
using RomansShop.WebApi.ClientModels.Product;

namespace RomansShop.WebApi.ClientModels.Order
{
    public class OrderRequestModel
    {
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "The field 'CustomerEmail' is required.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "The field 'CustomerName' is required.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "The field 'Address' is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field 'Price' is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The field 'Status' is required.")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "The field 'Products' is required.")]
        public IList<ProductResponseModel> Products { get; set; }

    }
}