using RPG_DOTNET.Enum;

namespace RPG_DOTNET.Dtos.CharacterDto
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = "Frodo";
        public int HitPoits { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}
