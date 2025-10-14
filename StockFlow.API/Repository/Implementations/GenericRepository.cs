using StockFlow.repository.Extensions;
using StockFlow.Repository.Contexts;
using StockFlow.Repository.Criteria;
using StockFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StockFlow.Repository.Implementations;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    #region Properties and private fields

    protected readonly AppDbContext _dbContext;

    private readonly DbSet<T> _dbSet;

    #endregion Properties and private fields

    #region Constructor

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    #endregion Constructor

    #region Interface Methods

    #region Add

    public virtual async Task AddAsync(T model,
        CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(model, cancellationToken);

    public virtual async Task AddRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(models, cancellationToken);

    #endregion

    #region Get

    public virtual async Task<T?> GetFirstOrDefaultAsync(Filters<T> criteria,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = GetAll().EvaluateQuery(criteria);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(Filters<T> criteria, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() =>
        {
            return GetAll().EvaluateQuery(criteria);
        }, cancellationToken);
    }

    public virtual async Task<(long count, IEnumerable<T> data)> GetAllAsyncWithCount(Filters<T> criteria,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = GetAll();

        (long count, IEnumerable<T> data) result =
            await query.EvaluatePageQuery(criteria, cancellationToken);

        return result;
    }

    #endregion

    #region Update

    public virtual async Task UpdateAsync(T model,
        CancellationToken cancellationToken = default)
        => await Task.Run(() => _dbSet.Update(model), cancellationToken);

    public virtual async Task UpdateRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default)
        => await Task.Run(() => _dbSet.UpdateRange(models), cancellationToken);

    public virtual async Task<int> UpdateRangeAsync(Filters<T> filters,
    CancellationToken cancellationToken = default)
    {
        if (filters.UpdateExpression == null)
            return 0;

        return await _dbSet.EvaluateQuery(filters)
                .ExecuteUpdateAsync(filters.UpdateExpression, cancellationToken);
    }

    #endregion

    #region Remove

    public virtual async Task RemoveAsync(T model,
        CancellationToken cancellationToken = default)
        => await Task.Run(() => _dbSet.Remove(model), cancellationToken);

    public virtual async Task<int> RemoveRangeAsync(Filters<T> criteria,
        CancellationToken cancellationToken = default)
        => await _dbSet.EvaluateQuery(criteria).ExecuteDeleteAsync(cancellationToken);

    public virtual async Task RemoveRangeAsync(IEnumerable<T> models,
        CancellationToken cancellationToken = default)
        => await Task.Run(() => { _dbSet.RemoveRange(models); }, cancellationToken);

    #endregion

    #region Save

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);

    #endregion

    #region Count

    public virtual async Task<long> GetCountAsync(Filters<T> criteria,
    CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = GetAll();
        query = query.IncludeExpressions(criteria.IncludeExpressions);
        query = query.FilterQuery(criteria);
        return await query.CountAsync(cancellationToken);
    }

    #endregion

    #region Any

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(filter, cancellationToken);

    #endregion

    #endregion Interface Methods

    #region Stored Procedure

    public async Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters)
        where TSP : class
    {
        IList<TSP> result = [];
        if (parameters != null && parameters.Length > 0)
        {
            parameters = parameters.Where(x => x != DBNull.Value).ToArray();
            result = await _dbContext.Database
            .SqlQueryRaw<TSP>(spQuery, parameters)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        }
        else
        {
            result = await _dbContext.Database
            .SqlQueryRaw<TSP>(spQuery)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        }

        return result;
    }

    public async Task<IEnumerable<TSP>> GetAllFromSP<TSP>(string spQuery, object? entity,
        CancellationToken cancellationToken = default) where TSP : class
        => await GetAllFromSP<TSP>(AppendParameters(spQuery, entity), cancellationToken);

    public virtual async Task<TSP?> GetFromSP<TSP>(string spQuery, CancellationToken cancellationToken = default,
    params object[] parameters) where TSP : class
        => (await GetAllFromSP<TSP>(spQuery, cancellationToken, parameters)).FirstOrDefault();

    public async Task<TSP?> GetFromSP<TSP>(string spQuery, object? entity, CancellationToken cancellationToken = default) where TSP : class
        => (await GetAllFromSP<TSP>(spQuery, entity, cancellationToken)).FirstOrDefault();

    public async Task ExecStoredProc(string spQuery, CancellationToken cancellationToken = default,
        params object[] parameters)
        => await _dbContext.Database.ExecuteSqlRawAsync(spQuery, parameters, cancellationToken);

    public async Task<IEnumerable<T>> GetAllFromFunction<T>(string functionName, object parametersDto, CancellationToken cancellationToken = default) where T : class
    {
        List<NpgsqlParameter> parameterList = [];
        List<string> sqlParams = [];

        foreach (PropertyInfo prop in parametersDto.GetType().GetProperties())
        {
            object? value = prop.GetValue(parametersDto, null);
            if (value != null)
            {
                string paramName = $"p_{prop.Name.ToLower()}";
                NpgsqlParameter sqlParam = CreateNpgsqlParameter(paramName, value);
                parameterList.Add(sqlParam);
                sqlParams.Add($"{paramName} => @{paramName}");
            }
        }

        string sql = $"SELECT * FROM public.\"{functionName}\"({string.Join(", ", sqlParams)});";

        return await _dbContext.Database
            .SqlQueryRaw<T>(sql, parameterList.ToArray())
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region Helper Methods

    private IQueryable<T> GetAll()
        => _dbSet.AsNoTracking().AsQueryable();

    private static bool IsComplexType(Type type) => !type.IsPrimitive && type != typeof(string) && !type.IsValueType;

    private static void AddEntityParameters(object entity, IList<NpgsqlParameter> parameters)
    {
        PropertyInfo[] infos = entity.GetType().GetProperties();

        foreach (PropertyInfo info in infos)
        {
            object? value = info.GetValue(entity, null);
            if (IsComplexType(info.PropertyType) && value != null)
            {
                AddEntityParameters(value, parameters);
            }
            else
            {
                if (value != null)
                {
                    if (info.PropertyType == typeof(string))
                    {
                        if (!string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            value = value?.ToString()?.Trim();
                            parameters.Add(new NpgsqlParameter()
                            {
                                ParameterName = "@" + info.Name,
                                Value = value ?? DBNull.Value,
                            });
                        }
                    }
                    else
                    {
                        parameters.Add(new NpgsqlParameter()
                        {
                            ParameterName = "@" + info.Name,
                            Value = value ?? DBNull.Value,
                        });
                    }
                }
            }
        }
    }

    private static string AppendParameters(string spQuery, object? entity)
    {
        if (entity == null) return spQuery;
        IList<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
        StringBuilder stringBuilder = new(spQuery);

        AddEntityParameters(entity, parameters);

        parameters = parameters.Where(x => x.Value != DBNull.Value).ToList();

        string parameterName = string.Empty;
        object parameterValue;

        stringBuilder.Append(' ');
        for (int iParameters = 0; iParameters < parameters.Count; iParameters++)
        {
            parameterName = parameters[iParameters].ParameterName;
            parameterValue = parameters[iParameters].Value;

            if (parameters[iParameters].DbType.ToString() == "String")
                stringBuilder.Append(parameterName + "='" + Convert.ToString(parameterValue) + (parameters.Count > iParameters + 1 ? "', " : "'"));
            else if (parameters[iParameters].DbType.ToString() == "DateTime" || parameters[iParameters].DbType.ToString() == "Date")
            {
                parameterValue = Convert.ToDateTime(parameterValue).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                stringBuilder.Append(parameterName + "='" + parameterValue + (parameters.Count > iParameters + 1 ? "', " : "'"));
            }
            else
                stringBuilder.Append(parameterName + "=" + Convert.ToString(parameterValue) + (parameters.Count > iParameters + 1 ? ", " : ""));
        }
        return stringBuilder.ToString();
    }

    // Helper method to create properly typed NpgsqlParameter
    private NpgsqlParameter CreateNpgsqlParameter(string paramName, object value)
    {
        if (value is string st)
        {
            st = st.Trim();
            value = string.IsNullOrWhiteSpace(st) ? DBNull.Value : st;
        }

        NpgsqlParameter parameter = new(paramName, value ?? DBNull.Value);

        // Handle specific types that might need explicit mapping
        switch (value)
        {
            case DateTime dt:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Timestamp;
                break;
            case DateOnly dateOnly:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date;
                parameter.Value = dateOnly.ToDateTime(TimeOnly.MinValue);
                break;
            case TimeOnly timeOnly:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Time;
                parameter.Value = timeOnly.ToTimeSpan();
                break;
            case Guid guid:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
                break;
            case decimal dec:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Numeric;
                break;
            case bool boolean:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean;
                break;
            case string str when str.Length > 4000:
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text;
                break;
        }

        return parameter;
    }

    #endregion Helper Methods
}