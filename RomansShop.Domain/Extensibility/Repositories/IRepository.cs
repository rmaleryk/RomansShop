using System;
using System.Collections.Generic;

namespace RomansShop.Domain.Extensibility.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity Add(TEntity entity);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(Guid id);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);
    }
}