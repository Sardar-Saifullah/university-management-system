// ProgramRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class AcademicProgramRepository : IAcademicProgramRepository
    {
        private readonly IDatabaseContext _context;

        public AcademicProgramRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<AcademicProgram> CreateAsync(AcademicProgram program)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_dep_id", program.DepartmentId),
                new MySqlParameter("p_name", program.Name),
                new MySqlParameter("p_code", program.Code),
                new MySqlParameter("p_duration_semesters", program.DurationSemesters),
                new MySqlParameter("p_credit_hours_required", program.CreditHoursRequired),
                new MySqlParameter("p_description", program.Description ?? (object)DBNull.Value),
                new MySqlParameter("p_created_by", program.CreatedBy),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CreateProgram", parameters);

            var result = Convert.ToInt32(parameters[7].Value);
            if (result == -1) throw new UnauthorizedAccessException("User is not authorized to create programs");

            program.Id = result;
            return program;
        }

        public async Task<AcademicProgram> GetByIdAsync(int id)
        {
            var dataTable = await _context.ExecuteQueryAsync("sp_GetProgramById",
                new MySqlParameter("p_id", id));

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new AcademicProgram
            {
                Id = Convert.ToInt32(row["id"]),
                DepartmentId = Convert.ToInt32(row["dep_id"]),
                Name = row["name"].ToString(),
                Code = row["code"].ToString(),
                DurationSemesters = Convert.ToInt32(row["duration_semesters"]),
                CreditHoursRequired = Convert.ToInt32(row["credit_hours_required"]),
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                Department = new Department
                {
                    Name = row["department_name"].ToString(),
                    Code = row["department_code"].ToString()
                }
            };
        }

        public async Task<IEnumerable<AcademicProgram>> GetAllAsync()
        {
            var dataTable = await _context.ExecuteQueryAsync("sp_GetAllPrograms");
            var programs = new List<AcademicProgram>();

            foreach (DataRow row in dataTable.Rows)
            {
                programs.Add(new AcademicProgram
                {
                    Id = Convert.ToInt32(row["id"]),
                    DepartmentId = Convert.ToInt32(row["dep_id"]),
                    Name = row["name"].ToString(),
                    Code = row["code"].ToString(),
                    DurationSemesters = Convert.ToInt32(row["duration_semesters"]),
                    CreditHoursRequired = Convert.ToInt32(row["credit_hours_required"]),
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                    Department = new Department
                    {
                        Name = row["department_name"].ToString(),
                        Code = row["department_code"].ToString()
                    }
                });
            }

            return programs;
        }

        public async Task<bool> UpdateAsync(AcademicProgram program)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", program.Id),
                new MySqlParameter("p_dep_id", program.DepartmentId),
                new MySqlParameter("p_name", program.Name),
                new MySqlParameter("p_code", program.Code),
                new MySqlParameter("p_duration_semesters", program.DurationSemesters),
                new MySqlParameter("p_credit_hours_required", program.CreditHoursRequired),
                new MySqlParameter("p_description", program.Description ?? (object)DBNull.Value),
                new MySqlParameter("p_modified_by", program.ModifiedBy),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_UpdateProgram", parameters);
            return Convert.ToBoolean(parameters[8].Value);
        }

        public async Task<bool> DeleteAsync(int id, int deletedBy)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", id),
                new MySqlParameter("p_deleted_by", deletedBy),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_DeleteProgram", parameters);
            return Convert.ToBoolean(parameters[2].Value);
        }
    }
}