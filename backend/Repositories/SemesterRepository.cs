

// Repositories/Implementations/SemesterRepository.cs
using backend.Data;
using backend.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace backend.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly IDatabaseContext _context;

        public SemesterRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Semester> Create(Semester semester, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_name", semester.Name),
                new MySqlParameter("p_start_date", semester.StartDate),
                new MySqlParameter("p_end_date", semester.EndDate),
                new MySqlParameter("p_is_current", semester.IsCurrent),
                new MySqlParameter("p_registration_start", semester.RegistrationStart ?? (object)DBNull.Value),
                new MySqlParameter("p_registration_end", semester.RegistrationEnd ?? (object)DBNull.Value),
                new MySqlParameter("p_academic_year", semester.AcademicYear),
                new MySqlParameter("p_created_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Int32) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_CreateSemester", parameters);

            var result = Convert.ToInt32(parameters.Last().Value);
            if (result == -1)
            {
                throw new UnauthorizedAccessException("User is not authorized to create semesters");
            }

            semester.Id = result;
            return semester;
        }

        public async Task<Semester?> GetById(int id)
        {
            var parameters = new[] { new MySqlParameter("p_id", id) };
            var result = await _context.ExecuteQueryAsync("sp_GetSemesterById", parameters);

            if (result.Rows.Count == 0) return null;

            return MapToSemester(result.Rows[0]);
        }

        public async Task<IEnumerable<Semester>> GetAll()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetAllSemesters");
            var semesters = new List<Semester>();

            foreach (DataRow row in result.Rows)
            {
                semesters.Add(MapToSemester(row));
            }

            return semesters;
        }

        public async Task<Semester?> GetCurrent()
        {
            var result = await _context.ExecuteQueryAsync("sp_GetCurrentSemester");
            return result.Rows.Count > 0 ? MapToSemester(result.Rows[0]) : null;
        }

        public async Task<Semester> Update(Semester semester, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", semester.Id),
                new MySqlParameter("p_name", semester.Name),
                new MySqlParameter("p_start_date", semester.StartDate),
                new MySqlParameter("p_end_date", semester.EndDate),
                new MySqlParameter("p_is_current", semester.IsCurrent),
                new MySqlParameter("p_registration_start", semester.RegistrationStart ?? (object)DBNull.Value),
                new MySqlParameter("p_registration_end", semester.RegistrationEnd ?? (object)DBNull.Value),
                new MySqlParameter("p_academic_year", semester.AcademicYear),
                new MySqlParameter("p_modified_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_UpdateSemester", parameters);

            var success = Convert.ToBoolean(parameters.Last().Value);
            if (!success)
            {
                throw new UnauthorizedAccessException("User is not authorized to update semesters");
            }

            return semester;
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var parameters = new[]
            {
                new MySqlParameter("p_id", id),
                new MySqlParameter("p_deleted_by", userId),
                new MySqlParameter("p_result", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            };

            await _context.ExecuteNonQueryAsync("sp_DeleteSemester", parameters);

            return Convert.ToBoolean(parameters.Last().Value);
        }

        private Semester MapToSemester(DataRow row)
        {
            return new Semester
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                StartDate = Convert.ToDateTime(row["start_date"]),
                EndDate = Convert.ToDateTime(row["end_date"]),
                IsCurrent = Convert.ToBoolean(row["is_current"]),
                RegistrationStart = row["registration_start"] != DBNull.Value ? Convert.ToDateTime(row["registration_start"]) : null,
                RegistrationEnd = row["registration_end"] != DBNull.Value ? Convert.ToDateTime(row["registration_end"]) : null,
                AcademicYear = row["academic_year"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                CreatedBy = row["created_by"] != DBNull.Value ? Convert.ToInt32(row["created_by"]) : null,
                ModifiedAt = Convert.ToDateTime(row["modified_at"]),
                ModifiedBy = row["modified_by"] != DBNull.Value ? Convert.ToInt32(row["modified_by"]) : null,
                IsActive = Convert.ToBoolean(row["is_active"]),
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }
    }
}