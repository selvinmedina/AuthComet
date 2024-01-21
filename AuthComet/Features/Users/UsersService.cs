using AuthComet.Auth.Common;
using AuthComet.Auth.Features.Notification;
using AuthComet.Domain.Dtos;
using AuthComet.Domain.Entities;
using AuthComet.Domain.Response;
using AuthComet.Domain.Validations;
using EntityFramework.Infrastructure.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthComet.Auth.Features.Users
{
    public class UsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserDomain _userDomain;
        private readonly ILogger<UsersService> _logger;
        private readonly INotificationService _notificationService;

        public UsersService(
            ILogger<UsersService> logger,
            IUnitOfWork unitOfWork,
            UserDomain userDomain,
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _userDomain = userDomain;
            _logger = logger;
            _notificationService = notificationService;
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

        public async Task<Response<UserDto>> LoginAsync(LoginDto loginDto)
        {

            var isLoginDtoValid = _userDomain.CheckLoginDto(loginDto);
            if (!isLoginDtoValid.Ok)
            {
                return Response<UserDto>.Fail(isLoginDtoValid.Message);
            }

            try
            {
                var user = await _unitOfWork.Repository<User>().AsQueryable()
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null)
                {
                    return Response<UserDto>.Fail(Messages.InvalidUserOrPassword);
                }

                if (!_userDomain.VerifyPassword(loginDto.Password, user.Password))
                {
                    return Response<UserDto>.Fail(Messages.InvalidUserOrPassword);
                }

                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    CreationDate = user.CreationDate
                };

                await _notificationService.SendUserWhoLoggedIn(userDto.Username, userDto.Email);

                return Response<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user");
                return Response<UserDto>.Fail(ex.Message);
            }
        }
    }
}
