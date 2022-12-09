using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG_DOTNET.Data;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Services.WeaponServices;

namespace RPG_DOTNET.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        private readonly ILogger<WeaponController> _logger;

        public WeaponController(IWeaponService weaponService, ILogger<WeaponController> logger)
        {
          _weaponService = weaponService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddWeapon(WeaponDto weaponDto)
        {
            return Ok(await _weaponService.AddWeapon(weaponDto));
        }
    }
}
