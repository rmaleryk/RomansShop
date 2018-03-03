using System.ComponentModel.DataAnnotations;

namespace RomansShop.Domain
{
    /// <summary>
    ///     DTO for product creation request
    /// </summary>
    public class CategoryRequest
    {
        [Required(ErrorMessage = "The field 'Name' is required.")]
        [MaxLength(30, ErrorMessage = "The product name must be less than 30 characters.")]
        public string Name { get; set; }
    }
}