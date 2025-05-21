using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using static APIFaunaEnriquillo.Core.DomainLayer.Utils.Result;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface ICommonService<TInsert, TUpdate, TResponse>
    {
        Task<ResultT<PageResult<TResponse>>> GetPageResult(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> GetById(Guid Id, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> CreateAsync(TInsert EntityInsertDto, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> UpdateAsync(Guid Id, TUpdate Entity, CancellationToken cancellationToken);
        Task<ResultT<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken);



    }
}
