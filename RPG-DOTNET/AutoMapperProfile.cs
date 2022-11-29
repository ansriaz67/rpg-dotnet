using AutoMapper;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Models;

namespace RPG_DOTNET
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetAllCharactersDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}
