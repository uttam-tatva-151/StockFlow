using StockFlow.Common.Exceptions;
using StockFlow.Repository.Criteria;
using StockFlow.Repository.DTOs.Common;
using StockFlow.Repository.Interfaces;
using StockFlow.Service.Interfaces;
using StockFlow.Service.Record;
using Microsoft.EntityFrameworkCore;
using Service.Common.Mappings;
using System.Linq.Expressions;

namespace StockFlow.Service.Implementations;

public class GenericService<TEntity, TDto>(IGenericRepository<TEntity> repository
    )
    : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    #region Properties and private fields

    private readonly IGenericRepository<TEntity> _repository = repository;

    #endregion Properties and private fields

    #region Interface Methods

    #region Add

    public virtual async Task AddAsync(TDto dto,
        bool scopeTransaction = false,
        CancellationToken cancellationToken = default)
    {
        TEntity model = MapperlyMapper.Map<TDto, TEntity>(dto);
        await _repository.AddAsync(model, cancellationToken);

        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity,
       bool scopeTransaction = false,
       CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(entity, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> models,
        bool scopeTransaction = false,
        CancellationToken cancellationToken = default)
    {
        await _repository.AddRangeAsync(models, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default)
        => await _repository.GetFirstOrDefaultAsync(criteria, cancellationToken);

    public virtual async Task<TDto> GetFirstOrDefaultDtoAsync<TId>(TId id,
        CancellationToken cancellationToken = default)
    {
        TEntity? model = await GetFirstOrDefaultAsync(GetFiltersWithId(id), cancellationToken);
        return MapperlyMapper.Map<TEntity, TDto>(model);
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync<TId>(TId id,
       CancellationToken cancellationToken = default)
        => await GetFirstOrDefaultAsync(GetFiltersWithId(id), cancellationToken);

    public Task<IEnumerable<TEntity>> GetAllAsync(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(criteria, cancellationToken);

    public virtual async Task<PageResponseRecord<TDto>> GetAllAsync(PageRequestDTO? pageRequest,
        CancellationToken cancellationToken = default)
    {
        pageRequest ??= new PageRequestDTO();

        Filters<TEntity> criteria = new()
        {
            PageNumber = pageRequest.PageNumber,
            PageSize = pageRequest.PageSize,
        };

        (long count, IEnumerable<TEntity> data) = await _repository.GetAllAsyncWithCount(criteria, cancellationToken);

        return new PageResponseRecord<TDto>(
            count,
            MapperlyMapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(data));
    }

    public virtual async Task<(long count, IEnumerable<TEntity> data)> GetAllAsyncWithCount(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default)
        => await _repository.GetAllAsyncWithCount(criteria, cancellationToken);

    #endregion

    #region Update

    public virtual async Task UpdateAsync(TEntity model,
        bool scopeTransaction = false,
        CancellationToken cancellationToken = default)
    {
        await _repository.UpdateAsync(model, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync<TId>(TId id, TDto dto,
    bool scopeTransaction = false, CancellationToken cancellationToken = default)
    {
        TEntity model = await GetFirstOrDefaultAsync(GetFiltersWithId(id), cancellationToken)
                ?? throw new ResourceNotFoundException(nameof(TEntity));
        //_mapper.UpdateCallPadHistoryLogDtoFromCallPadHistoryLog(dto, model);
        model = MapperlyMapper.Map<TDto, TEntity>(dto);
        await UpdateAsync(model, scopeTransaction, cancellationToken);
    }

    public virtual async Task<int> UpdateAsync(Filters<TEntity> filters,
        CancellationToken cancellationToken = default)
    {
        //TODO: check
        GetFirstOrDefaultFromRange(filters);
        return await _repository.UpdateRangeAsync(filters, cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> models,
        bool scopeTransaction = false, CancellationToken cancellationToken = default)
    {
        await _repository.UpdateRangeAsync(models, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> UpdateRangeAsync(Filters<TEntity> filters,
     CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateRangeAsync(filters, cancellationToken);
    }

    #endregion

    #region Remove

    public virtual async Task RemoveAsync(TEntity model, bool scopeTransaction = false,
      CancellationToken cancellationToken = default)
    {
        await _repository.RemoveAsync(model, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> RemoveAsync<TId>(TId id,
        CancellationToken cancellationToken = default)
        => await _repository.RemoveRangeAsync(GetFiltersWithId(id), cancellationToken);

    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> models, bool scopeTransaction = false,
      CancellationToken cancellationToken = default)
    {
        await _repository.RemoveRangeAsync(models, cancellationToken);
        if (!scopeTransaction) await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> RemoveRangeAsync(Filters<TEntity> filters,
    CancellationToken cancellationToken = default)
        => await _repository.RemoveRangeAsync(filters, cancellationToken);

    #endregion

    #region Save

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _repository.SaveChangesAsync(cancellationToken);

    #endregion

    #region Count

    public virtual async Task<long> GetCountAsync(Filters<TEntity> criteria,
        CancellationToken cancellationToken = default)
        => await _repository.GetCountAsync(criteria, cancellationToken);

    #endregion

    #region Any
    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default)
        => await _repository.AnyAsync(filter, cancellationToken);

    #endregion

    #endregion Interface Methods

    #region Helper Methods

    private static void GetFirstOrDefaultFromRange<T>(Filters<T> model)
    {
        model.IsPageRequest = true;
        model.PageSize = 1;
        model.PageNumber = 1;
    }

    private static Filters<TEntity> GetFiltersWithId<TId>(TId id)
    {
        return new Filters<TEntity>
        {
            Filter = entity => EF.Property<TId>(entity, "Id")!.Equals(id),
            IsPageRequest = true,
            PageSize = 1,
            PageNumber = 1
        };
    }

    #endregion Helper Methods

    #region Stored Procedure

    public async Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters) where TSP : class
        => await _repository.GetAllFromSP<TSP>(spQuery, cancellationToken, parameters);

    public async Task<IEnumerable<TDto>> GetAllFromSPWithDto<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters) where TSP : class
    {
        IEnumerable<TSP> result = await _repository.GetAllFromSP<TSP>(spQuery, parameters, cancellationToken);
        return MapperlyMapper.Map<IEnumerable<TSP>, IEnumerable<TDto>>(result);
    }

    public async Task ExecStoredProc(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters)
    {
        await _repository.ExecStoredProc(spQuery, cancellationToken, parameters);
    }

    public async Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, object? entity, CancellationToken cancellationToken = default) where TSP : class
    => await _repository.GetAllFromSP<TSP>(spQuery, entity, cancellationToken);

    public async Task<IEnumerable<TDto>> GetAllFromSP(string spQuery, object? entity, CancellationToken cancellationToken = default)
    => await _repository.GetAllFromSP<TDto>(spQuery, entity, cancellationToken);

    public async Task<TSP?> GetFromSP<TSP>(string spQuery, object? entity, CancellationToken cancellationToken = default) where TSP : class
    => await _repository.GetFromSP<TSP>(spQuery, entity, cancellationToken);

    public async Task<TSP?> GetFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default, params object[] parameters) where TSP : class
    => await _repository.GetFromSP<TSP>(spQuery, cancellationToken, parameters);

    #endregion
}