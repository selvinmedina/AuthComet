using System.Diagnostics.CodeAnalysis;

namespace AuthComet.Domain.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        [ExcludeFromCodeCoverage]
        public UserDto() { }

        public UserDto(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            CreationDate = DateTime.Now;
        }
    }
}
