using backend.Dtos;
using backend.Models;
using backend.Repositories;
using backend.Utilities;

namespace backend.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegistrationService(
            IRegistrationRepository registrationRepository,
            IPasswordHasher passwordHasher)
        {
            _registrationRepository = registrationRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<StudentRegistrationResponse> RegisterStudent(StudentRegistrationDto request, int createdBy)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Contact = request.Contact
            };

            var profile = new StudentProfile
            {
                RegistrationNo = request.RegistrationNo,
                EnrollYear = request.EnrollYear,
                DepartmentId = request.DepartmentId,
                ProgramId = request.ProgramId,
                LevelId = request.LevelId
            };

            var result = await _registrationRepository.RegisterStudent(user, profile, createdBy);
            if (result == null) return null;

            return new StudentRegistrationResponse
            {
                UserId = result.UserId,
                Name = user.Name,
                Email = user.Email,
                RegistrationNo = result.RegistrationNo,
                Department = result.DepartmentName,
                Program = result.ProgramName,
                Level = result.LevelName
            };
        }

        public async Task<TeacherRegistrationResponse> RegisterTeacher(TeacherRegistrationDto request, int createdBy)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Contact = request.Contact
            };

            var profile = new TeacherProfile
            {
                DepartmentId = request.DepartmentId,
                Designation = request.Designation,
                HireDate = request.HireDate,
                Qualification = request.Qualification,
                Specialization = request.Specialization,
                ExperienceYears = request.ExperienceYears
            };

            var result = await _registrationRepository.RegisterTeacher(user, profile, createdBy);
            if (result == null) return null;

            return new TeacherRegistrationResponse
            {
                UserId = result.UserId,
                Name = user.Name,
                Email = user.Email,
                Designation = result.Designation ?? string.Empty,
                Department = result.DepartmentName ?? string.Empty,
                HireDate = request.HireDate,
                Qualification = request.Qualification,
                Specialization = request.Specialization,
                ExperienceYears = request.ExperienceYears
            };
        }

        public async Task<AdminRegistrationResponse> RegisterAdmin(AdminRegistrationDto request, int createdBy)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Contact = request.Contact
            };

            var profile = new AdminProfile
            {
                Level = request.Level,
                HireDate = request.HireDate,
                DepartmentId = request.DepartmentId
            };

            var result = await _registrationRepository.RegisterAdmin(user, profile, createdBy);
            if (result == null) return null;

            return new AdminRegistrationResponse
            {
                UserId = result.UserId,
                Name = user.Name,
                Email = user.Email,
                Level = result.Level,
                 HireDate = request.HireDate,
                Department = result.DepartmentName,

            };
        }
    }
}