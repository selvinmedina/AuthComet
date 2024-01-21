using AuthComet.Domain.Dtos;
using AuthComet.Domain.Entities;
using AuthComet.Domain.Response;
using AuthComet.Domain.Validations;
using EntityFramework.Infrastructure.Core.UnitOfWork;

namespace AuthComet.Auth.Features.Users
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserDomain _userDomain;
        private readonly ILogger<UsersService> _logger;

        public UsersService(
            ILogger<UsersService> logger,
            IUnitOfWork unitOfWork, 
            UserDomain userDomain)
        {
            _unitOfWork = unitOfWork;
            _userDomain = userDomain;
            _logger = logger;
        }

        public async Task<Response<UserDto>> CreateAsync(UserDto user)
        {
            var validationResult = _userDomain.CreateUser(user);
            if (!validationResult.Ok)
            {
                return Response<UserDto>.Fail(validationResult.Message);
            }

            try
            {
                _unitOfWork.Repository<User>().Add(validationResult.Data!);
                await _unitOfWork.SaveAsync();

                user.Id = validationResult.Data!.Id;
                user.CreationDate = validationResult.Data!.CreationDate;

                return Response<UserDto>.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return Response<UserDto>.Fail(ex.Message);
            }
        }
    }
}
