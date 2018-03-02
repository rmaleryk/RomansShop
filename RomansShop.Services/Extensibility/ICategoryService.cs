using System;

namespace RomansShop.Services.Extensibility
{
    public interface ICategoryService
    {
        bool IsExist(Guid id);

        bool IsExist(string name);

        bool IsEmpty(Guid id);
    }
}