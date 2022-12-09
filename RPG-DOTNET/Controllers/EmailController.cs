using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using RPG_DOTNET.Dtos.EmailDto;
using RPG_DOTNET.Models;
using RPG_DOTNET.Services.EmailServices;

namespace RPG_DOTNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        [HttpPost]
        public IActionResult SendEmail(Email requestEmail)
        {
            _emailService.SendEmail(requestEmail);
            return Ok();
        }
    }
}
