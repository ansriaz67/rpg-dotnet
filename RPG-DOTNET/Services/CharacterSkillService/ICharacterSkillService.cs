using RPG_DOTNET.Dtos.CharacerSkillsDto;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Services.CharacterSkillService
{
    public interface ICharacterSkillService
    {
        Task<ServiceResponce<GetAllCharactersDto>> AddCharacterSkill(AddCharacterSkillsDto addCharacterSkillDto);
    }
}
