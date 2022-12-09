using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Weapon> Weapon { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<CharacterSkills> CharacterSkills { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkills>().HasKey(cs => new { cs.CharacterId, cs.SkillId });
        }

    }
}
