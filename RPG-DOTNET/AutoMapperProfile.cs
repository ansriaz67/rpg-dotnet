using AutoMapper;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Dtos.SkillDto;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Models;

namespace RPG_DOTNET
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetAllCharactersDto>()
                .ForMember(dto => dto.SkillDtos, c => c.MapFrom(c=> c.CharacterSkills.Select(cs => cs.Skill)));
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}
