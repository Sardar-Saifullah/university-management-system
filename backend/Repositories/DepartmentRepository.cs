

// Repositories/DepartmentRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDatabaseContext _context;

        public DepartmentRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Department> Create(Department department)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", department.Name),
                new MySqlParameter("p_code", department.Code),
                new MySqlParameter("p_created_by", department.CreatedBy),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CreateDepartment", parameters);

            department.Id = Convert.ToInt32(parameters[3].Value);
            return department;
        }

        public async Task<Department> GetById(int id)
        {
            var result = await _context.ExecuteQueryAsync("sp_GetDepartmentById",
                new MySqlParameter("p_id", id));

            if (result.Rows.Count == 0) return null;

            return MapDepartment(result.Rows[0]);
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetAllDepartments");
            var departments = new List<Department>();

            foreach (DataRow row in result.Rows)
            {
                departments.Add(MapDepartment(row));
            }

            return departments;
        }

        public async Task<Department> Update(Department department)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", department.Id),
                new MySqlParameter("p_name", department.Name),
                new MySqlParameter("p_code", department.Code),
                new MySqlParameter("p_modified_by", department.ModifiedBy),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_UpdateDepartment", parameters);

            if (!Convert.ToBoolean(parameters[4].Value))
                throw new Exception("Update failed or unauthorized");

            return department;
        }

        public async Task<bool> Delete(int id, int deletedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", id),
                new MySqlParameter("p_deleted_by", deletedBy),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_DeleteDepartment", parameters);

            return Convert.ToBoolean(parameters[2].Value);
        }

        private Department MapDepartment(DataRow row)
        {
            return new Department
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Code = row["code"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] != DBNull.Value ? Convert.ToInt32(row["created_by"]) : (int?)null,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] != DBNull.Value ? Convert.ToInt32(row["modified_by"]) : (int?)null,
                IsActive = Convert.ToBoolean(row["is_active"]),
                IsDeleted = Convert.ToBoolean(row["is_deleted"]),
            };
        }
    }
}