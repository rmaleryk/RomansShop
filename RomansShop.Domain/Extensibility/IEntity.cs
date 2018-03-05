using System;

namespace RomansShop.Domain.Extensibility
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}