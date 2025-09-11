using Microsoft.AspNetCore.Mvc;

namespace ProjectD_API.Email
{
    [ApiController]
    [Route("api/mail")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [HttpGet("send-test")]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendEmailAsync("", "", "");
            return Ok("Test email sent.");
        }
    }
}
