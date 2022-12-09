using RPG_DOTNET.Dtos.CharacterDto;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Models;
using RPGDOTNET.Migrations;

namespace RPG_DOTNET.Services.WeaponServices
{
    public interface IWeaponService
    {
        Task<ServiceResponce<GetAllCharactersDto>> AddWeapon(WeaponDto weaponDto);
    }
}
