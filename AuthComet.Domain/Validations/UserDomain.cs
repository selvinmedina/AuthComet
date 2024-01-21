using AuthComet.Domain.Dtos;
using AuthComet.Domain.Entities;
using AuthComet.Domain.Response;
using AuthComet.Domain.Validations.Messages;
using System.Text.RegularExpressions;

namespace AuthComet.Domain.Validations
{
    public class UserDomain
    {
        public Response<User> CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username))
                return Response<User>.Fail(UserMessages.UsernameRequired);

            if (string.IsNullOrWhiteSpace(userDto.Password))
                return Response<User>.Fail(UserMessages.PasswordRequired);

            if (string.IsNullOrWhiteSpace(userDto.Email) || !IsValidEmail(userDto.Email))
                return Response<User>.Fail(UserMessages.InvalidEmail);

            var user = new User()
            {
                Id = userDto.Id, // Assuming Id is handled externally or auto-incremented
                Username = userDto.Username,
                Password = HashPassword(userDto.Password),
                Email = userDto.Email,
                CreationDate = DateTime.Now
            };

            return Response<User>.Success(user);
        }

        public Response<bool> CheckLoginDto(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || !IsValidEmail(loginDto.Email))
                return Response<bool>.Fail(UserMessages.InvalidEmail);

            if (string.IsNullOrWhiteSpace(loginDto.Password))
                return Response<bool>.Fail(UserMessages.PasswordRequired);

            return Response<bool>.Success(true);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            return Regex.IsMatch(email, pattern);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
