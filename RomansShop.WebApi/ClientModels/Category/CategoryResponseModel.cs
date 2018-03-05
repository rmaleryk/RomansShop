using System;

namespace RomansShop.WebApi.ClientModels.Category
{
    /// <summary>
    ///     DTO for product response
    /// </summary>
    public class CategoryResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}