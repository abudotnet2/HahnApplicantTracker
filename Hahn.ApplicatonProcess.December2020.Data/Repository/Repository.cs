﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(TEntity entity)
        {

            _dataContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dataContext.Set<TEntity>().AddRange(entities);
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => _dataContext.Set<TEntity>().Where(predicate));
        }

        public async Task<TEntity> Get(int id)
        {
            return await _dataContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dataContext.Set<TEntity>().ToListAsync();
        }

        public void Remove(TEntity entity)
        {
            _dataContext.Set<TEntity>().Remove(entity);

        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dataContext.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dataContext.Set<TEntity>().Update(entity);
        }
    }
}
