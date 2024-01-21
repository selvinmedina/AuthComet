using EntityFramework.Infrastructure.Core.UnitOfWork;

namespace AuthComet.Auth.Features.Users
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
