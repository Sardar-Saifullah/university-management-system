using BCrypt.Net;

namespace backend.Utilities
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }

    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;

        public PasswordHasher(int workFactor = 12)
        {
            _workFactor = workFactor;
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                return false;
                
            if (string.IsNullOrWhiteSpace(providedPassword))
                return false;

            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}