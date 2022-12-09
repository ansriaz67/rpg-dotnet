using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Models;
using RPG_DOTNET.Services;
using System.Security.Claims;


namespace RPG_DOTNET.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(ICharacterService characterService, ILogger<CharacterController> logger)
        {
            _characterService = characterService;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get([FromQuery] PagingParameterModel pagingparametermodel)
        {
            /* int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);*/
            return Ok(await _characterService.GetAllCharacters(pagingparametermodel));
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetSearchCharacters(string name)
        {
            return Ok(await _characterService.SearchCharacters(name));
        }

        [HttpGet("sortdesc/{name}")]
        public async Task<IActionResult> GetSortingCharacters(string name)
        {
            return Ok(await _characterService.SortingCharacters(name));
        }

        [HttpGet("singleuser/{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacters(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacters(UpdateCharacterDto updateCharacter)
        {
            ServiceResponce<GetAllCharactersDto> response = await _characterService.UpdateCharacter(updateCharacter);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponce<List<GetAllCharactersDto>> response = await _characterService.DeleteCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }
}
