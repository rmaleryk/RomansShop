﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RomansShop.Domain
{
    /// <summary>
    ///     Product Entity
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

    }
}
