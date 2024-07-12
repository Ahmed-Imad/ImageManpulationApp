using ImageManpulationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageManpulationApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> appDbContext) : base(appDbContext)
        {

        }

        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot config = builder.Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("DBCS"));
            }

        }
    }
}
