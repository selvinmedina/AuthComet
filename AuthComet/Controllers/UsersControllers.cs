using AuthComet.Auth.Features.Users;
using AuthComet.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthComet.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserDto user)
        {
            var response = await _usersService.CreateAsync(user);
            if (response.Ok)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Message);
        }
    }
}
