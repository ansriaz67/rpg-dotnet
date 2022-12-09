using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Data;
using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Models;
using System.Security.Claims;

namespace RPG_DOTNET.Services.WeaponServices
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public WeaponService(IMapper mapper, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponce<GetAllCharactersDto>> AddWeapon(WeaponDto weaponDto)
        {
            ServiceResponce<GetAllCharactersDto> response= new ServiceResponce<GetAllCharactersDto>();
            try 
            {
                Character character = await _dataContext.Characters.FirstOrDefaultAsync(a => a.User.Id == GetUserId() && a.Id == weaponDto.CharacterId);
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not Found.";
                    return response;
                }
                else
                {
                    Weapon weapon = new Weapon
                    {
                        Name = weaponDto.Name,
                        Damage = weaponDto.Damage,
                        Character= character,
                    };
                    await _dataContext.AddAsync(weapon);
                    await _dataContext.SaveChangesAsync();
                    response.Data = _mapper.Map<GetAllCharactersDto>(character);
                }

            }
            catch(Exception ex)
            {   
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
