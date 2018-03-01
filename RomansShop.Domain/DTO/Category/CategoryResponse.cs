using System;

namespace RomansShop.Domain
{
    /// <summary>
    ///     DTO for product response
    /// </summary>
    public class CategoryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}