using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Extensibility;

namespace RomansShop.DataAccess.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class, IEntity
    {
        internal ShopDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ShopDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual TEntity GetById(Guid id)
        {
            return dbSet.AsNoTracking().FirstOrDefault(entity => entity.Id == id);
        }

        public virtual TEntity Update(TEntity entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();

            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }
    }
}