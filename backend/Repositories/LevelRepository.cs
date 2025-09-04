// Repositories/Implementations/LevelRepository.cs
using backend.Data;
using backend.Models;
using backend.Repositories;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly IDatabaseContext _context;

        public LevelRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Level> Create(Level level, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", level.Name),
                new MySqlParameter("p_description", level.Description),
                new MySqlParameter("p_created_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CreateLevel", parameters);

            var result = Convert.ToInt32(parameters[3].Value);
            if (result == -1) throw new UnauthorizedAccessException("User is not authorized to create levels");

            level.Id = result;
            return level;
        }

        public async Task<Level?> GetById(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_GetLevelById", parameters);

            if (result.Rows.Count == 0) return null;

            var row = result.Rows[0];
            return new Level
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Description = row["description"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                IsActive = Convert.ToBoolean(row["is_active"])
            };
        }

        public async Task<IEnumerable<Level>> GetAll()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetAllLevels");
            var levels = new List<Level>();

            foreach (DataRow row in result.Rows)
            {
                levels.Add(new Level
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Description = row["description"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    IsActive = Convert.ToBoolean(row["is_active"])
                });
            }

            return levels;
        }

        public async Task<Level?> Update(Level level, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", level.Id),
                new MySqlParameter("p_name", level.Name),
                new MySqlParameter("p_description", level.Description),
                new MySqlParameter("p_modified_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_UpdateLevel", parameters);

            var success = Convert.ToBoolean(parameters[4].Value);
            return success ? level : null;
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", id),
                new MySqlParameter("p_deleted_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_DeleteLevel", parameters);
            return Convert.ToBoolean(parameters[2].Value);
        }
    }
}