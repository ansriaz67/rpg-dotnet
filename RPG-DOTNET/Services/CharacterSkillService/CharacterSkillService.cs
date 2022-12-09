using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Data;
using RPG_DOTNET.Dtos.CharacerSkillsDto;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Dtos.SkillDto;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Models;
using System.Security.Claims;

namespace RPG_DOTNET.Services.CharacterSkillService
{
    public class CharacterSkillService : ICharacterSkillService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterSkillService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponce<GetAllCharactersDto>> AddCharacterSkill(AddCharacterSkillsDto addCharacterSkillDto)
        {
            ServiceResponce<GetAllCharactersDto> response= new ServiceResponce<GetAllCharactersDto>();
            try 
            {
                Character character = await _context.Characters.Include(c => c.Weapon).Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                    .FirstOrDefaultAsync(a => a.Id == addCharacterSkillDto.CharacterId && a.User.Id == GetUserId());
                if (character == null)
                {
                    response.Success = false;
                    response.Message = "Character not Found.";
                    return response;
                }
                else
                {
                    Skill skill = await _context.Skill.FirstOrDefaultAsync(s => s.Id == addCharacterSkillDto.SkillId);
                    if (skill == null)
                    {
                        response.Success = false;
                        response.Message = "Skill not Found.";
                        return response;
                    }
                    CharacterSkills characterSkills = new CharacterSkills
                    {
                        Character = character,
                        Skill = skill
                    };
                    await _context.AddAsync(characterSkills);
                    await _context.SaveChangesAsync();
                    response.Data = _mapper.Map<GetAllCharactersDto>(character);
                }
            } 
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
