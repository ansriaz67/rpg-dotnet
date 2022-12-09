using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG_DOTNET.Dtos.CharacerSkillsDto;
using RPG_DOTNET.Services.CharacterSkillService;

namespace RPG_DOTNET.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterSkillController : ControllerBase
    {
        private readonly ICharacterSkillService _characterSkillService;
        private readonly ILogger<CharacterSkillController> _logger;

        public CharacterSkillController(ICharacterSkillService characterSkillService, ILogger<CharacterSkillController> logger)
        {
            _characterSkillService = characterSkillService;
            _logger = logger;
        }
        [HttpPost]   
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillsDto addCharacterSkillsDto)
        {
            return Ok(await _characterSkillService.AddCharacterSkill(addCharacterSkillsDto));
        }
    }
}
