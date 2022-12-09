using Microsoft.AspNetCore.Mvc;
using RPG_DOTNET.Dtos.UserDto;
using RPG_DOTNET.Models;
using RPG_DOTNET.Repository;
using RPG_DOTNET.Services.EmailServices;
using RPG_DOTNET.Services.UserAuthenticationServices;
using System.Net;

namespace RPG_DOTNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserAuthService _userAuthService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserAuthService userAuthService, ILogger<UserController> logger)
        {
            _userAuthService = userAuthService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDtos registerUserDtos)
        {
            ServiceResponce<int> response= await _userAuthService.Register(new User { Name = registerUserDtos.UserName, Email = registerUserDtos.Email },registerUserDtos.Password);
            if (!response.Success)
            { 
                return BadRequest(response);
            }
            
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            ServiceResponce<string> response = await _userAuthService.Login(loginUserDto.Email, loginUserDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("verify/{token}")]
        public async Task<IActionResult> Verify(string token)
        {
            ServiceResponce<string> response = await _userAuthService.Verify(token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            /* return Ok(response);*/
            return Redirect("https://localhost:44358/");
        }

        /*private IActionResult OK(RedirectResult redirectResult, ServiceResponce<string> response)
        {
            throw new NotImplementedException();
        }*/

        /*[HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
           return (await AuthenticationManager.SignOut());
        }*/
    }
}
