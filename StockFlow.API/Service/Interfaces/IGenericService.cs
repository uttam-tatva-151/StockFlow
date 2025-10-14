using StockFlow.Repository.Criteria;
using StockFlow.Repository.DTOs.Common;
using StockFlow.Service.Record;
using System.Linq.Expressions;

namespace StockFlow.Service.Interfaces;

public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    #region Add
    Task AddAsync(TEntity entity, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    Task AddAsync(TDto dto, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> models, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    #endregion

    #region Get
    Task<TEntity?> GetFirstOrDefaultAsync(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default);

    Task<TDto> GetFirstOrDefaultDtoAsync<TId>(TId id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllAsync(Filters<TEntity> criteria,
    CancellationToken cancellationToken = default);

    Task<PageResponseRecord<TDto>> GetAllAsync(PageRequestDTO? pageRequest,
        CancellationToken cancellationToken = default);

    Task<(long count, IEnumerable<TEntity> data)> GetAllAsyncWithCount(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default);

    #endregion

    #region Update

    Task UpdateAsync(TEntity model, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    Task UpdateAsync<TId>(TId id, TDto dto,
        bool scopeTransaction = false, CancellationToken cancellationToken = default);

    Task<int> UpdateAsync(Filters<TEntity> filters,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> models, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    #endregion

    #region Remove

    Task RemoveAsync(TEntity model, bool scopeTransaction = false,
        CancellationToken cancellationToken = default);

    Task<int> RemoveAsync<TId>(TId id,
        CancellationToken cancellationToken = default);

    Task<int> RemoveRangeAsync(Filters<TEntity> filters,
    CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<TEntity> models, bool scopeTransaction = false,
      CancellationToken cancellationToken = default);

    #endregion

    #region Save
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    #endregion

    #region Count

    Task<long> GetCountAsync(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default);

    #endregion

    #region Any
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default);
    #endregion

    #region StoredProcedure

    Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters)
        where TSP : class;

    Task<IEnumerable<TDto>> GetAllFromSPWithDto<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters) where TSP : class;

    Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, object entity, CancellationToken cancellationToken = default) where TSP : class;

    Task<IEnumerable<TDto>> GetAllFromSP(string spQuery, object entity, CancellationToken cancellationToken = default);

    Task<TSP?> GetFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
    params object[] parameters) where TSP : class;

    Task<TSP?> GetFromSP<TSP>(string spQuery, object entity,
        CancellationToken cancellationToken = default) where TSP : class;

    Task ExecStoredProc(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters);
    #endregion
}