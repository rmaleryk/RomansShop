using System;
using System.ComponentModel.DataAnnotations;

namespace RomansShop.Domain
{
    /// <summary>
    ///     DTO for product creation request
    /// </summary>
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "The field 'Name' is required.")]
        [MaxLength(30, ErrorMessage = "The product name must be less than 30 characters.")]
        public string Name { get; set; }

        [MaxLength(300, ErrorMessage = "The product description must be less than 300 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field 'Price' is required.")]
        public decimal? Price { get; set; }

        public long? Quantity { get; set; }

        public Guid CategoryId { get; set; }
    }
}