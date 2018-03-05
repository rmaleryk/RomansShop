using System;

namespace RomansShop.WebApi.ClientModels.Product
{
    /// <summary>
    ///     DTO for product response
    /// </summary>
    public class ProductResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

    }
}