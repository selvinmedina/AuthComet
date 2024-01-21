using Microsoft.EntityFrameworkCore;

namespace AuthComet.Auth.Infrastructure.AuthCometDatabase
{
    public class AuthCometDbContext : DbContext
    {
        public AuthCometDbContext(DbContextOptions<AuthCometDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthCometDbContext).Assembly);
        }
    }
}
