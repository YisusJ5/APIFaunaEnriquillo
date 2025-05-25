using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.Core.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository
{
    public interface ICommonRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid Id, CancellationToken cancellationToken);

        Task InsertAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task<PageResult<TEntity>> GetPageResultAsync(int pageNumber, int  pageSize, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task DeleteChangesAsync(TEntity entity, CancellationToken cancellationToken);

        Task<bool> ValidateAsync(Expression<Func<TEntity, bool>> predicate);
       

    }
}
