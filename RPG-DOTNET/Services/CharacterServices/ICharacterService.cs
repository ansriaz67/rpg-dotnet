using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Services
{
    public interface ICharacterService
    {
        Task<ServiceResponce<List<GetAllCharactersDto>>> GetAllCharacters();
        Task<ServiceResponce<GetAllCharactersDto>> GetCharacterById(int id);
        Task<ServiceResponce<List<GetAllCharactersDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponce<GetAllCharactersDto>> UpdateCharacter(UpdateCharacterDto updateCharacter);
        Task<ServiceResponce<List<GetAllCharactersDto>>> DeleteCharacter(int id);
    }
}