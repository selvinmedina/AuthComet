namespace AuthComet.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        private User() { }

        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            CreationDate = DateTime.Now;
        }
    }
}
