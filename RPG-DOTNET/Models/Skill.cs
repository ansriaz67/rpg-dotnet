namespace RPG_DOTNET.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public List<CharacterSkills> CharacterSkills { get; set; }
    }
}
