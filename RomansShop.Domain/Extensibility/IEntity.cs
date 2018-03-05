using System;
using System.Collections.Generic;
using System.Text;

namespace RomansShop.Domain.Extensibility
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}