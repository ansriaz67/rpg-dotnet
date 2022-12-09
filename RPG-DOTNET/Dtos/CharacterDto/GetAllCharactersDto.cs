using RPG_DOTNET.Dtos.SkillDto;
using RPG_DOTNET.Dtos.WeaponDto;
using RPG_DOTNET.Enum;

namespace RPG_DOTNET.Dtos.CharacterDto
{
    public class GetAllCharactersDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoits { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public GetWeaponDto Weapon { get; set; }
        public List<GetSkillDto> SkillDtos { get; set; }
    }
}
