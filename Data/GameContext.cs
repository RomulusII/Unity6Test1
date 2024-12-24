using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class GameContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
    //=> options.UseSqlServer($"(localdb)\\MSSQLLocalDB;Database=OyunTest2;Trusted_Connection=True;");
    => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OyunTest2;Trusted_Connection=True;");
        public DbSet<Player>? Players { get; set; }
        public DbSet<Unit>? Units { get; set; }

        public void TruncateTable(string tableName)
        {
            Database.ExecuteSqlRaw($"TRUNCATE TABLE [{tableName}];");
        }
    }
}