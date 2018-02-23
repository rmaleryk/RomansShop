using System;
using System.ComponentModel.DataAnnotations;

namespace RomansShop.Domain
{
    /// <summary>
    ///     Product Entity
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

    }
}
