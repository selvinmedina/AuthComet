using AuthComet.Auth.Features.Auth;
using AuthComet.Auth.Features.Queues;
using AuthComet.Auth.Features.Users;
using AuthComet.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthComet.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly AuthService _authService;
        private readonly RabbitMQProducer _rabbitMQProducer;

        public UsersController(UsersService usersService,
            AuthService authService,
            RabbitMQProducer rabbitMQProducer)
        {
            _usersService = usersService;
            _authService = authService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserDto user)
        {
            var response = await _usersService.CreateAsync(user);
            if (response.Ok)
            {
                _rabbitMQProducer.SendMessage(new()
                {
                    UserId = response.Data!.Id,
                    Email = response.Data.Email,
                    Username = response.Data.Username
                });

                return Ok(response.Data);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var response = await _usersService.LoginAsync(loginDto);
            if (response.Ok)
            {
                var token = _authService.GenerateToken(response.Data!.Id, response.Data.Email);
                return Ok(new { token });
            }

            return BadRequest(response.Message);
        }

        [Authorize]
        [HttpGet("GetUserData")]
        public IActionResult GetUserData()
        {
            var userId = User.FindFirst("userId")?.Value;

            return Ok(new { UserId = userId });
        }
    }
}
