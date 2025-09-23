using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Data
{
    public interface IDatabaseContext
    {
        MySqlConnection CreateConnection();
        Task<DataTable> ExecuteQueryAsync(string procedureName, params MySqlParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters);
        Task<T> ExecuteScalarAsync<T>(string procedureName, params MySqlParameter[] parameters);
        Task<MySqlTransaction> BeginTransactionAsync();
        Task ExecuteInTransaction(Func<Task> operation);
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public MySqlConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task<DataTable> ExecuteQueryAsync(string query, params MySqlParameter[] parameters)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand(query, connection);

            // Determine if it's a stored procedure or raw SQL
            if (!query.Contains(" ") && !query.Contains(";")) // Simple heuristic for stored procedure names
            {
                command.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                command.CommandType = CommandType.Text;
            }

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
            return result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<MySqlTransaction> BeginTransactionAsync()
        {
            var connection = CreateConnection();
            await connection.OpenAsync();
            return await connection.BeginTransactionAsync();
        }

        public async Task ExecuteInTransaction(Func<Task> operation)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await operation();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}