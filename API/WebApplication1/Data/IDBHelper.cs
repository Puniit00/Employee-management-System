using Dapper;
using System.Data;

namespace WebApplication1.Data
{
    public interface IDbHelper
    {
        Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> ExecuteTableAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure) where T : class, new();
        Task<(List<T1>, List<T2>)> ExecuteMultipleTablesAsync<T1, T2>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
            where T1 : class, new()
            where T2 : class, new();
    }
}