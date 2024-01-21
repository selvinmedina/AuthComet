using AuthComet.Mails.Common.Dtos;
using AuthComet.Mails.Features.Emails;
using Microsoft.AspNetCore.Mvc;

namespace AuthComet.Mails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail([FromBody] EmailDto emailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _emailService.SendEmailAsync(emailDto.To, emailDto.Subject, emailDto.Body, emailDto.IsBodyHtml);
                return Ok(new { message = "Email has been sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
