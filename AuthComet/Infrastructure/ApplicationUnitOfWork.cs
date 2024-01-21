using AuthComet.Auth.Infrastructure.AuthCometDatabase;
using EntityFramework.Infrastructure.Core.UnitOfWork;

namespace AuthComet.Auth.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork
    {
        public ApplicationUnitOfWork(AuthCometDbContext dbContext) : base(dbContext)
        {
        }
    }
}
