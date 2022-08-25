
using Microsoft.EntityFrameworkCore;

namespace APISkripsi.Function
{
    public class DatabaseAccessLayer : DbContext
    {
        public DatabaseAccessLayer(DbContextOptions<DatabaseAccessLayer> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Server=.\\SQLExpress;Database=APISKRIPSI;Trusted_Connection=true;");
        }

        public DbSet<User> Users => Set<User>();
    }
}
