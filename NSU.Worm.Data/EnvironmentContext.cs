using Microsoft.EntityFrameworkCore;

namespace NSU.Worm.Data
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<FoodGenerationPattern> Patterns { get; set; }

        public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodGenerationPattern>()
                .ToTable("Pattern")
                .HasMany(p => p.Steps)
                .WithOne(s => s.Pattern);

            modelBuilder.Entity<FoodGenerationPatternStep>()
                .ToTable("Step");
        }
    }
}