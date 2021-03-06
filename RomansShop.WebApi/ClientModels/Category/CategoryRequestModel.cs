﻿using System.ComponentModel.DataAnnotations;

namespace RomansShop.WebApi.ClientModels.Category
{
    public class CategoryRequestModel
    {
        [Required(ErrorMessage = "The field 'Name' is required.")]
        [MaxLength(30, ErrorMessage = "The product name must be less than 30 characters.")]
        public string Name { get; set; }
    }
}