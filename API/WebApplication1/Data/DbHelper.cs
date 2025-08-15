using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1.Data
{
    public class DbHelper : IDbHelper
    {
        private readonly string _connectionString;

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Returns a scalar value (e.g., int, string, DateTime)
        public async Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var result = await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: commandType);
            return result != null ? result : default!;
        }

        // Returns a list of models mapped from a table result
        public async Task<List<T>> ExecuteTableAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure) where T : class, new()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<T>(sql, parameters, commandType: commandType);
            return result.ToList();
        }

        // Returns multiple table results mapped to multiple models
        public async Task<(List<T1>, List<T2>)> ExecuteMultipleTablesAsync<T1, T2>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
            where T1 : class, new()
            where T2 : class, new()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var multi = await connection.QueryMultipleAsync(sql, parameters, commandType: commandType);
            var list1 = (await multi.ReadAsync<T1>()).ToList();
            var list2 = (await multi.ReadAsync<T2>()).ToList();
            return (list1, list2);
        }
    }
}