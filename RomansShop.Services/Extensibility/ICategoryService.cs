using System;

namespace RomansShop.Services.Extensibility
{
    public interface ICategoryService
    {
        bool IsExist(Guid id);

        bool isEmpty(Guid id);
    }
}