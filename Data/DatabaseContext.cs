using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace backend.Data
{
    public interface IDatabaseContext
    {
        MySqlConnection CreateConnection();
        Task<DataTable> ExecuteQueryAsync(string procedureName, params MySqlParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters);

        Task<T> ExecuteScalarAsync<T>(string procedureName, params MySqlParameter[] parameters);
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public MySqlConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task<DataTable> ExecuteQueryAsync(string procedureName, params MySqlParameter[] parameters)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(parameters);

            var dataTable = new DataTable();
            using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);

            return dataTable;
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(parameters);

            return await command.ExecuteNonQueryAsync();
        }
        public async Task<T> ExecuteScalarAsync<T>(string procedureName, params MySqlParameter[] parameters)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(parameters);

            var result = await command.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}