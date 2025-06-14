﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using static APIFaunaEnriquillo.Core.DomainLayer.Utils.Result;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface ICommonService<TInsert, TUpdate, TResponse>
    {
        Task<Result.ResultT<PageResult<TResponse>>> GetPageResult(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> GetById(Guid Id, CancellationToken cancellationToken);
        Task<ResultT<IEnumerable<TResponse>>> GetRecentAsync(CancellationToken cancellationToken);
        Task<ResultT<TResponse>> CreateAsync(TInsert EntityInsertDto, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> UpdateAsync(Guid Id, TUpdate Entity, CancellationToken cancellationToken);
        Task<ResultT<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken);
        Task<ResultT<TResponse>> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken);



    }
}
