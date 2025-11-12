using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var TypeEntity = typeof(TEntity);

            if (_repositories.TryGetValue(TypeEntity, out object? Repository))
                return (IGenericRepository<TEntity,TKey>)Repository;

            var newRepo = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories[TypeEntity] = newRepo;
            return newRepo;
        }
         

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
