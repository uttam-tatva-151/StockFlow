using StockFlow.Repository.Criteria;
using System.Linq.Expressions;

namespace StockFlow.Repository.Interfaces;
public interface IGenericRepository<T> where T : class
{
    #region Add

    Task AddAsync(T model,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default);

    #endregion

    #region Get

    Task<T?> GetFirstOrDefaultAsync(Filters<T> criteria,
        CancellationToken cancellationToken = default);

    Task<(long count, IEnumerable<T> data)> GetAllAsyncWithCount(Filters<T> criteria,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync(Filters<T> criteria,
    CancellationToken cancellationToken = default);

    #endregion

    #region Update

    Task UpdateRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default);

    Task<int> UpdateRangeAsync(Filters<T> filters,
    CancellationToken cancellationToken = default);

    Task UpdateAsync(T model,
        CancellationToken cancellationToken = default);

    #endregion

    #region Remove

    Task RemoveAsync(T model,
        CancellationToken cancellationToken = default);

    Task<int> RemoveRangeAsync(Filters<T> criteria,
        CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default);

    #endregion

    #region Save

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Count

    Task<long> GetCountAsync(Filters<T> criteria,
    CancellationToken cancellationToken = default);

    #endregion

    #region Any

    Task<bool> AnyAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default);

    #endregion

    #region Stored Procedure

    Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters) where TSP : class;

    Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, object? entity,
        CancellationToken cancellationToken = default) where TSP : class;

    Task<TSP?> GetFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters) where TSP : class;

    Task<TSP?> GetFromSP<TSP>(string spQuery, object? entity,
        CancellationToken cancellationToken = default) where TSP : class;

    Task ExecStoredProc(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters);

    #endregion
}