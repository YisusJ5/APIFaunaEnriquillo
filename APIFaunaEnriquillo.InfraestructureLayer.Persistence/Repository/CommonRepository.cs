using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.InfraestructureLayer.Persistence.Repository
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        protected readonly FaunaDbContext _dbContext;

        public CommonRepository(FaunaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteChangesAsync(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid Id, CancellationToken cancellationToken)=>
            await _dbContext.Set<T>().FindAsync(Id, cancellationToken);

        public async Task<PageResult<T>> GetPageResultAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
          var t = await _dbContext.Set<T>().AsNoTracking().CountAsync(cancellationToken);
          var items = await _dbContext.Set<T>().
                Skip(( pageNumber -1)* pageSize).
                Take(pageSize).
                ToListAsync(cancellationToken);
            return new PageResult<T> (items, pageNumber, t, pageSize);
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
           _dbContext.Set<T>().Add(entity);
            await SaveChangesAsync(cancellationToken);

        }


        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
             _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified; 
            await SaveChangesAsync(cancellationToken);
        }

        public Task<bool> ValidateAsync(Expression<Func<T, bool>> predicate)
        {
            var exists = _dbContext.Set<T>().AnyAsync(predicate);
            return exists;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await _dbContext.SaveChangesAsync();
    }
}
