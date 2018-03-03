using System;
using System.Collections.Generic;

namespace RomansShop.Domain
{
    /// <summary>
    ///     Product Category Entity
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}