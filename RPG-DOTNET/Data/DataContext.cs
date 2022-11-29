using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }

    }
}
