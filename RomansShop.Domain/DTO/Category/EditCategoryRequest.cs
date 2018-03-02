﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RomansShop.Domain
{
    /// <summary>
    ///     DTO for product change request
    /// </summary>
    public class EditCategoryRequest
    {
        [Required(ErrorMessage = "The field 'Id' is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The field 'Name' is required.")]
        [MaxLength(30, ErrorMessage = "The product name must be less than 30 characters.")]
        public string Name { get; set; }
    }
}